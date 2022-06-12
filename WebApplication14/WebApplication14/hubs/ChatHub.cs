using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using WebApplication14.Models;
using WebApplication14.DataAccessLayer;

using System.Threading.Tasks;

namespace WebApplication14
{
    /// <summary>
    /// Hub Class which resposible to deal with event from the Client side and prompt functions in the Client side by by the Server side.
    /// </summary>
    public class ChatHub : Hub
    {

        /// <summary>
        /// Class's attributes which store details and conections to the Data base.
        /// </summary>

        // DB Database class wich enable connection to the Revise Database.
        static ReviseDataBase db = new ReviseDataBase();

        //List which contains information about Users that connected to the hub.
        static List<ChatUserDetails> ConnectedUsers = new List<ChatUserDetails>();

        //List which contains messeages from the Revise DB.
        static List<Comment> CurrentMessages;

        //List which contains object of Roles - Project Members Roles.
        static List<Role> rolesList;


        /// <summary>
        /// Function which responsible to deal with an User (Client) that connect to the Hub.
        /// </summary>
        public void join()
        {

            CurrentMessages = db.Comments.ToList();

            var id = Context.ConnectionId;
            var userName = Context.User.Identity.Name.ToString();
            int reqId = Convert.ToInt32(Clients.Caller.reqId);

            int projId = db.Requirements.Where(r => r.reqId == reqId).SingleOrDefault().projectId.projectId;

            rolesList = db.Roles.ToList();

            //Query - Join Query which returns Records of Members Details and theirs Roles.

            var projectMembersDetails = from pm in db.ProjectMembers.AsQueryable()
                                        join md in db.Members.AsQueryable()
                                        on pm.MemberId.MemberId equals md.MemberId
                                        where pm.projectId.projectId == projId
                                        select new
                                        {
                                            memberId = md.MemberId,
                                            userName = md.userName.ToString(),
                                            firstName = md.firstName,
                                            lastName = md.lastName,
                                            picURL = md.pic,
                                            roleId = pm.roleId.roleId,
                                            roleName = pm.roleId.roleName,
                                            grade = md.grade
                                        };

            int memberId = Convert.ToInt32(Clients.Caller.userId);

            var currentMemberDetails = projectMembersDetails.Where(m => m.memberId == memberId).First();

            var projAttached = db.ProjectMembers.Where(p => p.MemberId.MemberId == currentMemberDetails.memberId);

            var reqAttached = db.Requirements;

            //Query - Join Query which return a list of Rooms that to User attached to.
            var roomList = from sc in projAttached
                           join soc in reqAttached
                           on sc.projectId equals soc.projectId
                           select new { roomName = soc.reqId.ToString() };

            joinChatRoom(reqId.ToString());

            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                ConnectedUsers.Add(new ChatUserDetails { ReqId = reqId, MemberId = currentMemberDetails.memberId, ConnectionId = id, UserName = userName });

                // send to caller
                var messages = CurrentMessages.Where(r => r.reqId.reqId.Equals(reqId)).Select(m => new MessageDetail { UserName = m.MemberId.userName, Message = m.Content, Date = m.CreateDatetime.ToString() });
                Clients.Caller.onConnected(id, userName, ConnectedUsers.Where(x => x.ReqId == reqId), messages, projectMembersDetails);

                // send to all except caller client
                Clients.Group(reqId.ToString(), id).onNewUserConnected(id, userName);
            }
        }

        /// <summary>
        /// Function which resposible to add the current connected user to a Requirment ChatRoom.
        /// </summary>
        /// <param name="roomName">The Room Name String = Requirment ID key </param>
        /// <returns>Tasks of joinChatRoom event</returns>
        private Task joinChatRoom(string roomName)
        {
            return Groups.Add(Context.ConnectionId, roomName);
        }

        /// <summary>
        /// Function which creates ChatRoom group for a Requirment.
        /// </summary>
        /// <param name="reqId">Requirment ID key </param>
        /// <param name="projId">Project ID key </param>
        /// <returns>Task of startChatRoom</returns>
        public Task startChatRoom(string reqId, string projId)
        {

            return Groups.Add(Context.ConnectionId, reqId);


        }

        /// <summary>
        /// Function which close a Requirment ChatRoom Group.
        /// </summary>
        /// <param name="reqId">Requirment ID key </param>
        /// <param name="projId">Project ID key</param>
        /// <returns>>Task of disconChatRoom</returns>
        public Task disconChatRoom(string reqId, string projId)
        {
            return Groups.Remove(Context.ConnectionId, reqId);

        }

        /// <summary>
        /// Function which send a message (string) form Client Side to the other Clients (Users) that connected to the same Requirment ChatRoom.
        /// </summary>
        /// <param name="userName">Member User Name</param>
        /// <param name="message">A texct message </param>
        public void SendMessageToAll(string userName, string message)
        {

            //Connection ID key to be uniquely recognize.
            var conId = Context.ConnectionId;
            //Contains Details about the specific User that call this function.
            var userContext = ConnectedUsers.Where(cId => cId.ConnectionId.Equals(conId)).First();


            Comment comment = new Comment();
            comment.Content = message;
            comment.CreateDatetime = DateTime.Now;
            var idd = Context.User.Identity.ToString();

            comment.MemberId = db.Members.Find(userContext.MemberId);
            comment.reqId = db.Requirements.Find(userContext.ReqId);
            comment.projectId = db.Projects.Find(comment.reqId.projectId.projectId);
            db.Comments.Add(comment);
            db.SaveChanges();

            // Broad cast message
            string a = userContext.ReqId.ToString();
            Clients.Group(userContext.ReqId.ToString()).messageReceived(userName, message, comment.CreateDatetime.ToString());
        }

        /// <summary>
        /// Override Function which responsible to deal with an event of user disconnecting the Hub.
        /// </summary>
        /// <param name="stopCalled">Bool para indicate if the disconnecting is valid or not</param>
        /// <returns>Task of OnDisconnected </returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                ChatUserDetails item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId.Equals(Context.ConnectionId));
                if (item != null)
                {
                    ConnectedUsers.Remove(item);
                    Clients.Group(item.ReqId.ToString()).onUserDisconnected(item.ConnectionId, item.UserName);

                }
            }
            else
            {
                // This server hasn't heard from the client in the last ~35 seconds.
                // If SignalR is behind a load balancer with scaleout configured, 
                // the client may still be connected to another SignalR server.
            }

            return base.OnDisconnected(stopCalled);
        }


    }
}