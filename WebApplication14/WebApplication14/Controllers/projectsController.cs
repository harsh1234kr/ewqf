using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication14.DataAccessLayer;
using WebApplication14.Models;

namespace WebApplication14.Controllers
{
    /// <summary>
    /// The Projects Controller Contains all logic that belongs to the issues Projects.
    /// </summary>
    [Authorize]
    public class projectsController : Controller
    {
        private ReviseDataBase db = new ReviseDataBase();
        // The conncection to the server db that in the azure platform.
        public string connectionString = "Data Source=devrevise.database.windows.net;Initial Catalog=ReviseDatabase;Integrated Security=False;User ID=revise;Password=P1m2n3r$;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        /// <summary>
        /// not in use? 
        /// </summary>
        /// <param name="selectedValues"></param>
        /// <returns></returns>
        private MultiSelectList GetCountries(string[] selectedValues)
        {

            List<Member> memberlist = new List<Member>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "Select * From Members";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Member m = new Member();
                m.MemberId = reader.GetInt32(0);
                m.userName = reader.GetString(4);

                memberlist.Add(m);
            }
            reader.Close();
            connection.Close();
            return new MultiSelectList(memberlist, "ID", "Name", selectedValues);

        }

        /// <summary>
        /// The function return Member Object by the UserName seassion
        /// </summary>
        /// <returns>the id member of the user that connection now to the web</returns>
        public Member GetMember()
        {
            String un = Convert.ToString(Session["UserName"]);
            Member memberId = db.Members.Where(user => user.userName.Contains(un)).First();
            return (memberId);
        }

        /// <summary>
        /// The function get Id member as integer parameter and return a Member object by the Id that input.
        /// </summary>
        /// <param name="memberIdint">Id member as integer</param>
        /// <returns>Member object by the Id</returns>
        public Member GetMember(int memberIdint)
        {
            Member memberId = db.Members.Where(memId => memId.MemberId == memberIdint).First();
            return (memberId);
        }

        /// <summary>
        /// The function get Id Role as integer parameter and return a Role object by the Id that input.
        /// </summary>
        /// <param name="RoleIdint">Id Role as integer</param>
        /// <returns>Role object by the Id</returns>
        public Role GetRole(int RoleIdint)
        {
            Role RoleId = db.Roles.Where(rolId => rolId.roleId == RoleIdint).First();
            return (RoleId);
        }


        /// <summary>
        /// the fuction show list of Projects That the Member is belong them.
        /// </summary>
        /// <returns>list of Projects That the Member is belong them</returns>
        // GET: projects
        public ActionResult Index()
        {
            List<project> ProjectsList = new List<project>();
            List<IEnumerable<WebApplication14.Models.projectMember>> MembersList = new List<IEnumerable<WebApplication14.Models.projectMember>>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "Select * From projects where ProjectId in (Select projectId_projectId from projectMembers where MemberId_MemberId=" + GetMember().MemberId + ")";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                project p = new project();
                
                p.projectId = reader.GetInt32(0);
                MembersList.Add(db.ProjectMembers.Where(x => x.projectId.projectId == p.projectId).ToList()); // to show the picture of the Participants.
                p.projectName = reader.GetString(1);
                p.discription = reader.GetString(2);
                p.MemberId = new Member { MemberId = Convert.ToInt32(reader[3]) };
                ProjectsList.Add(p);
            }
            reader.Close();

            connection.Close();
            Session["listofProjectPerUser"] = ProjectsList.ToList(); // save to use in the search.
            Session["IdUserConncet"] = GetMember().MemberId;
            Session["ListOfMembersToProject"] = MembersList; // to show the picture of the Participants


            //var p = db.ProjectMembers.Where(pm => pm.MemberId.MemberId.Equals(GetMember().MemberId)).Select(u => u.projectId).ToList();

            //db.Projects.Include;

            return View(ProjectsList.ToList());
        }


        /// <summary>
        /// The HttpPost to the list of Projects, search in the list
        /// </summary>
        /// <param name="txtfind">Search term</param>
        /// <returns>result to the search</returns>
        [HttpPost]
        public ActionResult Index(String txtfind)
        {
            var listpro = Session["listofProjectPerUser"] as List<project>;
            var projects = (listpro.Where(p => p.projectName.Contains(txtfind
                   )));
            return View(projects.ToList());
        }


        /// <summary>
        /// the function show the all project in the DataBase. for Admin only. 
        /// </summary>
        /// <returns>list of all project in the DataBase</returns>
        // GET: projects
        public ActionResult IndexM()
        {
            return View(db.Projects.ToList());
        }


        /// <summary>
        /// The HttpPost to the list of Project, search in the list. for admin only.
        /// </summary>
        /// <param name="txtfind">Search term</param>
        /// <returns>>result to the search</returns>
        [HttpPost]
        public ActionResult IndexM(String txtfind)
        {
            var projects = from p in db.Projects
                           select p;
            if (!String.IsNullOrEmpty(txtfind))
            {
                projects = projects.Where(m => m.projectName.Contains(txtfind
                    ));
            }
            return View(projects.ToList());
        }


        /// <summary>
        /// The function show a view of detiails for specific Project.
        /// </summary>
        /// <param name="id">the id Project for show details</param>
        /// <returns>the Project object with all the detials about him.</returns>
        // GET: projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            List<Object> req = new List<Object>(); // the list of the members that participant in this project and his Role.
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "select * from projectMembers where projectId_projectId='"+ id +"'";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                List<Object> r = new List<Object>();
                r.Add(GetMember(reader.GetInt32(1)).pic);
                r.Add(GetMember(reader.GetInt32(1)).userName);
                r.Add(GetRole(reader.GetInt32(3)).roleName);
                req.Add(r);
                
            }
            reader.Close();
            connection.Close();

            
            ViewBag.listMR = req; // the list of the members Delivered to the view part.
            return View(project);
        }


        /// <summary>
        /// the function show the view of Create Project
        /// </summary>
        /// <returns>view</returns>
        // GET: projects/Create
        public ActionResult Create()
        {
            List<Member> memberlist = new List<Member>(); // the list contians all the Members in the system apart from the Member that create the project.

            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "Select * From Members where userName <> '" + Convert.ToString(Session["UserName"]) + "'";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Member m = new Member();
                m.MemberId = reader.GetInt32(0);
                m.userName = reader.GetString(3);

                memberlist.Add(m);
            }
            reader.Close();
            connection.Close();

            // the list of Members Becomes a object of Multi Select List and sent to the view with viewBag.
            ViewBag.memberlist = new MultiSelectList(memberlist, "MemberId", "userName");
            return View();
        }



        /// <summary>
        /// the HttpPost function for create new Project
        /// </summary>
        /// <param name="project">>the new Project with all the parameters</param>
        /// <param name="MembersId">the Integer array with all the id Members that the user assign them to the project. </param>
        /// <returns>Redirect To the assign Roles to the Members in project.</returns>
        // POST: projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "projectId,projectName,discription")] project project, int[] MembersId)
        {
            if((project.discription==null) || (project.projectName==null)||(MembersId==null))
            {
                List<Member> memberlist = new List<Member>(); // the list contians all the Members in the system apart from the Member that create the project.

                // open connection to the DataBase using the connectionString.
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand("", connection);

                command.CommandText = "Select * From Members where userName <> '" + Convert.ToString(Session["UserName"]) + "'";

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Member m = new Member();
                    m.MemberId = reader.GetInt32(0);
                    m.userName = reader.GetString(3);

                    memberlist.Add(m);
                }
                reader.Close();
                connection.Close();

                // the list of Members Becomes a object of Multi Select List and sent to the view with viewBag.
                ViewBag.memberlist = new MultiSelectList(memberlist, "MemberId", "userName");

                ModelState.AddModelError("CredentialError", "You did not fill one mandatory fields");
                return View("Create");
            }
            if (ModelState.IsValid)
            {
                // add the details of project to DataBase.
                project.MemberId = GetMember();
                project prj = db.Projects.Add(project);
                db.SaveChanges();

                // add the detais of the members in this project to the DataBase.
                db.ProjectMembers.Add(new projectMember
                {
                    joinDate = DateTime.Now,
                    MemberId = GetMember(),
                    projectId = prj,
                    roleId = GetRole(4)
                });
                db.SaveChanges();
                for (int i = 0; i < MembersId.Length; i++)
                {
                    db.ProjectMembers.Add(new projectMember
                    {
                        joinDate = DateTime.Now,
                        MemberId = GetMember(MembersId[i]),
                        projectId = prj,
                    });
                    db.SaveChanges();

                    // send a mail for all the Participants in this project.
                    Member Membermail = db.Members.Find(MembersId[i]);
                    string mailMember= Membermail.email;
                    string fname = Membermail.firstName;
                    Member manager = db.Members.Find(Convert.ToInt32(Session["UserId"]));
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.To.Add(""+ mailMember + "");
                    mail.From = new MailAddress("revisegroup@gmail.com", "ReviseTeam", System.Text.Encoding.UTF8);
                    mail.Subject = "Revise - Assign to project " + project.projectName;
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.Body = "<h3>Hello, " + fname + ". <br />  We want to tell you that "+manager.firstName+" "+manager.lastName+" opened a new project named '"+project.projectName+ "' <br /> and you are part of it:) <br /> You Invited to sign in to <a href='https://reviseproject.azurewebsites.net'>Revise site</a> and start to take part of it! <br /> <br /> good luck!<br /> Revise Team:-) </h3>";
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.High;
                    SmtpClient client = new SmtpClient();
                    client.Credentials = new System.Net.NetworkCredential("revisegroup@gmail.com", "p1m2n3r4");
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    client.Send(mail);
                }

                // the new id of project that create
                Session["IdProjectToAssign"] = prj.projectId;

                // now the user Redirect To assign role every member in project. 
                return RedirectToAction("AssignMemToPro", new { id = prj.projectId });
            }

            return View(project);
        }


        /// <summary>
        /// the function view to the user the option to edit a project.
        /// </summary>
        /// <param name="id">the id project to edit.</param>
        /// <returns>object of project to edit</returns>
        // GET: projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.Projects.Find(id);

            Session["IdProjectToDel"] = id; // the id project to edit save in Session object to use this id to delete from this project Members there were assign to this project. 
            if (project == null)
            {
                return HttpNotFound();
            }

            List<Member> memberslist = new List<Member>();

            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);
            // select all the members that assigned to this project and return them in list to view in viewBag.
            command.CommandText = "select * from Members where MemberId in (select MemberId_MemberId from projectMembers where projectId_projectId='" + id + "');";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Member m = new Member();
                m.MemberId = reader.GetInt32(0);
                m.userName = reader.GetString(3);
                m.pic = reader.GetString(8);
                memberslist.Add(m);
            }

            reader.Close();
            connection.Close();
            ViewBag.memberslistT = memberslist;

            /// now create a list of all the members that not assign yet to this project
            List<Member> memberlist2 = new List<Member>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection2 = new SqlConnection(connectionString);
            connection2.Open();
            SqlCommand command2 = new SqlCommand("", connection2);

            command2.CommandText = "Select * From Members";

            SqlDataReader reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {

                Member m = new Member();
                m.MemberId = reader2.GetInt32(0);
                m.userName = reader2.GetString(3);

                // check if the member already assign to this project.. if not
                if (memberslist.Find(x => x.MemberId == m.MemberId) == null)
                { memberlist2.Add(m); } // add to list

            }
            reader2.Close();
            connection2.Close();
            // pass the list of not assign members in Multi Select List object to the view via ViewBag.
            ViewBag.memberlist2 = new MultiSelectList(memberlist2, "MemberId", "userName");
            return View(project);
        }



        /// <summary>
        /// The HttpPost function update the detials that the user insert in the form of Edit Page.
        /// </summary>
        /// <param name="project">the update Project with all the parameters</param>
        /// <param name="MembersId">the Integer array with all the id Members that the user assign them to the project.</param>
        /// <returns>if there an additional Members assign to this project, Redirect To view of edit role to member in project else pass the user to the index of projects list</returns>
        // POST: projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "projectId,projectName,discription")] project project, int[] MembersId)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                project prj = project;
                db.SaveChanges();

                //db.ProjectMembers.Add(new projectMember
                //{
                //    joinDate = DateTime.Now,
                //    MemberId = GetMember(),
                //    projectId = prj,
                //});
                //db.SaveChanges();
                if (MembersId == null)
                {
                    return RedirectToAction("Index");
                }
                for (int i = 0; i < MembersId.Length; i++)
                {
                    db.ProjectMembers.Add(new projectMember
                    {
                        joinDate = DateTime.Now,
                        MemberId = GetMember(MembersId[i]),
                        projectId = prj,
                    });
                    db.SaveChanges();


                }
                List<Member> membertemp = new List<Member>();
                for (int i = 0; i < MembersId.Length; i++)
                {
                    membertemp.Add(db.Members.Find(MembersId[i]));
                }
                ViewBag.memberslist = membertemp;
                Session["IdProjectToAssign"] = prj.projectId;

                return RedirectToAction("AssignMemToPro", new { id = prj.projectId });
            }
            return View(project);
        }



        /// <summary>
        /// the function delete a member from project assignment
        /// </summary>
        /// <param name="id">the id member to delete from the project</param>
        /// <returns>Redirect To view of edit project </returns>
        public ActionResult deleteAssign(int? id)
        {
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "DELETE FROM projectMembers WHERE MemberId_MemberId='" + id + "' and projectId_projectId='" + Convert.ToInt32(Session["IdProjectToDel"]) + "'";

            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Edit/" + Convert.ToInt32(Session["IdProjectToDel"]) + "");
        }


        /// <summary>
        /// the view confirm that you want to delete project
        /// </summary>
        /// <param name="id">the id project to delete</param>
        /// <returns>delete the project</returns>
        // GET: projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project  project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        /// <summary>
        /// the HttpPost function to delete a project
        /// </summary>
        /// <param name="id">id project to delete</param>
        /// <returns>Redirect To index of project list.</returns>
        // POST: projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

           
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "DELETE FROM projectMembers WHERE projectId_projectId='" + id + "'";

            command.ExecuteNonQuery();

            SqlCommand command2 = new SqlCommand("", connection);

            command2.CommandText = "DELETE FROM Comments WHERE projectId_projectId='" + id + "'";

            command2.ExecuteNonQuery();


            SqlCommand command4 = new SqlCommand("", connection);

            command4.CommandText = "DELETE FROM Requirements WHERE projectId_projectId='" + id + "'";

            command4.ExecuteNonQuery();



            connection.Close();
            project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// The function show the view that deal with assign per member to specific role after create a project or edit
        /// </summary>
        /// <param name="id">the id project to assign members to a roles</param>
        /// <returns>view with the list of members in the project and the optionals roles to each member.</returns>
        public ActionResult AssignMemToPro(int? id)
        {
            List<Member> memberslist = new List<Member>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);
            // select all the member that assign to id project.
            command.CommandText = "select * from Members where MemberId in (select MemberId_MemberId from projectMembers where projectId_projectId='" + id + "');";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Member m = new Member();
                m.MemberId = reader.GetInt32(0);
                m.userName = reader.GetString(3);
                m.pic = reader.GetString(8);

                memberslist.Add(m);
            }

            reader.Close();
            connection.Close();
            ViewBag.memberslist = memberslist; // pass the list of members in project in ViewBag to view
            Session["ListForAssign"] = memberslist; // save the list in Session object to use this list again in Further

            //list that contians all the role options in database.
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "", Value = "4" });
            List<Role> Rolelist = new List<Role>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection2 = new SqlConnection(connectionString);
            connection2.Open();
            SqlCommand command2 = new SqlCommand("", connection2);

            command2.CommandText = "Select * From Roles";

            SqlDataReader reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {
                items.Add(new SelectListItem { Text = reader2.GetString(1), Value = Convert.ToString(reader2.GetInt32(0)) });
            }
            reader2.Close();
            connection2.Close();
            ViewBag.Rolelist = items; // pass the list of Roles to view via ViewBag.

            return View();
        }

        /// <summary>
        /// The HttpPost to the Member assign Role.
        /// </summary>
        /// <returns>Redirect To the details of the Project to see if the project update or created.</returns>
        [HttpPost]
        public ActionResult AssignMemToPro()
        {
            string ChooseAssigns = Request.Form["Rolelist"].ToString();
            String[] CC = ChooseAssigns.Split(',');
            List<Member> listOfMemberToAssign = (List<Member>)Session["ListForAssign"];
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            for (int i = 0; i < listOfMemberToAssign.Count; i++)
            {
                if(CC[i]=="")
                { }
                else
                { 
                SqlCommand command = new SqlCommand("", connection);

                command.CommandText = "UPDATE projectMembers SET roleId_roleId = '"+ CC[i] + "' WHERE MemberId_MemberId='"+ listOfMemberToAssign[i].MemberId + "' and [projectId_projectId] = '"+ Convert.ToInt32(Session["IdProjectToAssign"]) + "'";

                command.ExecuteNonQuery();
                }
            }
            connection.Close();
            return RedirectToAction("Details/" + Convert.ToInt32(Session["IdProjectToAssign"]) + "");
        }
    }
}
