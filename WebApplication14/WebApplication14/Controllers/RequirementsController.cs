using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication14.DataAccessLayer;
using WebApplication14.Models;
using System.Data.SqlClient;
using System.Globalization;

namespace WebApplication14.Controllers
{
    /// <summary>
    /// The Requirements Controller Contains all logic that belongs to the issues Requirements.
    /// </summary>
   // [Authorize]
    public class RequirementsController : Controller
    {
        // The conncection to the server db that in the azure platform.
        public string connectionString = "Data Source=devrevise.database.windows.net;Initial Catalog=ReviseDatabase;Integrated Security=False;User ID=revise;Password=P1m2n3r$;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private ReviseDataBase db = new ReviseDataBase();

        /// <summary>
        /// the function return the Current id project
        /// </summary>
        /// <returns>project object contians project id</returns>
        public project GetProject()
        {
            String un = Convert.ToString(Session["projectName"]);
            project projectId = db.Projects.Where(user => user.projectName.Contains(un)).First();
            return (projectId);
        }


        /// <summary>
        /// the fuction show list of Requirements That the project id is belong them.
        /// </summary>
        /// <param name="id">id of project that selected by member in projects list</param>
        /// <param name="name">the name of project that selected by member in projects list</param>
        /// <returns>list of Requirements that belong to specipic id project</returns>
        // GET: Requirements
        [Authorize]
        public ActionResult Index(int? id,string name)
        {
            if (id != null) { Session["projectid"] = id; }
            if (name != null) { Session["projectName"] = name; }
            //temp variable for get the number of Members in project.
            int mid = Convert.ToInt32(Session["projectid"]);
            //get the number of members in project to 
            int numberOfMemberOnProject = db.ProjectMembers.Where(x => x.projectId.projectId == mid).Count();

            Session["picsOfMembers"] = db.ProjectMembers.Where(x => x.projectId.projectId == mid).ToList();
            List<Requirement> Reqwhere = new List<Requirement>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);
           
            command.CommandText = "Select * From Requirements Where projectId_projectId='" + Session["projectid"] + "'";

            SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    Requirement r = new Requirement();
                    r.reqId = reader.GetInt32(0);
                    r.title = reader.GetString(1);
                    r.discription = reader.GetString(2);
                    r.Total_Complition_Score = reader.GetInt32(3);
                r.craetedDateTime = reader.GetDateTime(6);

                // check if the chat is still active
                if(DateTime.Compare(reader.GetDateTime(8),DateTime.Now)>0)
                {
                    r.isActive = true;
                }
                else
                {
                    r.isActive = false;
                }
                r.endDatetime = reader.GetDateTime(8);
                r.thresholdScore = reader.GetInt32(9);

                // the calculation of the thresholdScoreValue
                double denominator = numberOfMemberOnProject * 5;
                double numerator = r.Total_Complition_Score;
                double total = Convert.ToDouble((numerator / denominator))*100;
                r.thresholdScoreValue = Convert.ToInt32(total);
                // if the thresholdScoreValue bigger than the thresholdScore so the requirement is aprroved
                if (total>=r.thresholdScore)
                {
                    r.isAprroved = true;
                }
                // update the fields isAprroved and isActive on database.
                db.Entry(r).State = EntityState.Modified;
                db.SaveChanges();

                Reqwhere.Add(r);
                }
            reader.Close();
               
            connection.Close();
            // sace the requirements list for the search in the list.
            Session["listofReqPerUser"] = Reqwhere.ToList();
            Session["projectidForChat"] = GetProject();

            // check if the member that connect to the system is the project manager of the this specific project.
            project p = db.Projects.Find(id);
            if(p!=null)
            { 
            if(p.MemberId.MemberId == Convert.ToInt32(Session["UserId"]))
            {
                Session["IsProjectAdmin"] = true;
            }
            else
            {
                Session["IsProjectAdmin"] = false;
            }
            }


            int mId = Convert.ToInt32(Session["UserId"]);
            Session["CommentlistT"] = db.Comments.Where(x => x.MemberId.MemberId == mId).ToList();

            Reqwhere.ForEach( m => m.calMemberScore(db , db.Members.Find(Convert.ToInt32(Session["userId"]))));

            return View(Reqwhere.ToList());
        }


        /// <summary>
        /// The HttpPost to the list of Requirements, search in the list
        /// </summary>
        /// <param name="txtfind">Search term</param>
        /// <returns>result to the search</returns>
        [HttpPost]
        public ActionResult Index(String txtfind)
        {
            var listreq = Session["listofReqPerUser"] as List<Requirement>;
            var requirements = (listreq.Where(r => r.title.Contains(txtfind
                   )));
            return View(requirements.ToList());
        }


        /// <summary>
        /// The function show a view of detiails for specific Requirement.
        /// </summary>
        /// <param name="id">the id Requirement for show details</param>
        /// <returns>the Requirement object with all the detials about him.</returns>
        // GET: Requirements/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirement requirement = await db.Requirements.FindAsync(id);
            if (requirement == null)
            {
                return HttpNotFound();
            }

            List<Member> memberslist = new List<Member>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "select * from Members where MemberId in (select MemberId_MemberId from projectMembers where projectId_projectId='"+ Session["projectid"] + "');";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Member m = new Member();
                m.MemberId = reader.GetInt32(0);
                m.userName = reader.GetString(3);

                memberslist.Add(m);
            }
            
            //Member r = db.Projects.Find(Session["projectid"]).MemberId;
            //Member r2 = db.Members.Find(r);
            
            //ViewBag.projectmeneger = r2.userName;
            reader.Close();
            connection.Close();
            
            ViewBag.memberslist = memberslist; // the list of members that participant in this project pass via ViewBag to the view.
            ViewBag.reqId = id;
            ViewBag.endDate = requirement.endDatetime.ToString("MMMM d, yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
            return View(requirement);
        }


        /// <summary>
        /// the function show the view of Create Requirement
        /// </summary>
        /// <returns>view</returns>
        // GET: Requirements/Create
        [Authorize]
        public ActionResult Create()
        {
            List<SelectListItem> projList = new List<SelectListItem>();
            projList.Add(new SelectListItem
            {
                Text = "",
                Value = ""
            });
            List<project> p = new List<project>();
            p = db.Projects.ToList();
            p.ForEach(delegate (project pr) {
                projList.Add(new SelectListItem
                {
                    Text = pr.projectName,
                    Value = pr.projectId + ""
                });
            });
            ViewBag.projectId_projectId = projList;

            //list that contians all the RequirementType options in database.
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "", Value = "" });
            List<RequirementType> RequirementTypelist = new List<RequirementType>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "Select * From RequirementTypes";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new SelectListItem { Text = reader.GetString(1), Value = Convert.ToString(reader.GetInt32(0)) });
            }
            reader.Close();
            connection.Close();
            ViewBag.ReqTypelist = items; // pass the list of RequirementTypes to view via ViewBag.

            return View();
        }



        /// <summary>
        /// the HttpPost function for create new requirement
        /// </summary>
        /// <param name="requirement">the new requirement with all the parameters</param>
        /// <returns>Redirect To the index of Requirements</returns>
        // POST: Requirements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "reqId,title,discription,Total_Complition_Score,projectId_projectId,endDatetime,thresholdScore")] Requirement requirement)
        {
            //list that contians all the RequirementType options in database.
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "", Value = "" });
            List<RequirementType> RequirementTypelist = new List<RequirementType>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "Select * From RequirementTypes";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new SelectListItem { Text = reader.GetString(1), Value = Convert.ToString(reader.GetInt32(0)) });
            }
            reader.Close();
            connection.Close();
            ViewBag.ReqTypelist = items; // pass the list of RequirementTypes to view via ViewBag.

            string ChooseOfRequirementType = Request.Form["ReqTypelist"].ToString();
            if (ChooseOfRequirementType == "")
            {
                ModelState.AddModelError("CredentialError", "You did not fill Requirement Type");

                return View(requirement);
            }

            project p = new project();
            //p = db.Projects.Find(Convert.ToInt32(Request["projectId_projectId"].ToString()));
            p = db.Projects.Find(GetProject().projectId);
            requirement.projectId = p;
            requirement.isActive = true;
            requirement.isAprroved = false;
            requirement.craetedDateTime = DateTime.Now;
            requirement.thresholdScoreValue = 0;
            requirement.typeId = db.RequirementTypes.Find(Convert.ToInt32(ChooseOfRequirementType));
            if (ModelState.IsValid)
            {
                db.Requirements.Add(requirement);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(requirement);
        }



        /// <summary>
        /// The function view to the user the option to edit a Requirement.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Requirements/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirement requirement = await db.Requirements.FindAsync(id);
            if (requirement == null)
            {
                return HttpNotFound();
            }
            return View(requirement);
        }


        /// <summary>
        /// The HttpPost function update the detials that the user insert in the form of Edit Page.
        /// </summary>
        /// <param name="requirement">the update Requirement with all the parameters</param>
        /// <returns>Redirect To the index of Requirements list</returns>
        // POST: Requirements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "reqId,title,discription,Total_Complition_Score,thresholdScore,isActive")] Requirement requirement,DateTime endDatetime)
        {
            requirement.endDatetime = endDatetime; 
            if (ModelState.IsValid)
            {
                db.Entry(requirement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(requirement);
        }


        /// <summary>
        /// the function show the view of confirm delete Member from the Database.
        /// </summary>
        /// <param name="id">id Requirement to delete </param>
        /// <returns>the object Requirement to delete</returns>
        // GET: Requirements/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirement requirement = await db.Requirements.FindAsync(id);
            if (requirement == null)
            {
                return HttpNotFound();
            }
            return View(requirement);
        }



        /// <summary>
        /// the HttpPost function to delete Requirement from the DataBase. 
        /// </summary>
        /// <param name="id">id Requirement to delete</param>
        /// <returns>Redirect To the list of Requirements</returns>
        // POST: Requirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "DELETE FROM ComplitionScores WHERE reqId_reqId='" + id + "'";

            command.ExecuteNonQuery();

            SqlCommand command2 = new SqlCommand("", connection);

            command2.CommandText = "DELETE FROM Comments WHERE reqId_reqId='" + id + "'";

            command2.ExecuteNonQuery();

            SqlCommand command3 = new SqlCommand("", connection);

            command3.CommandText = "DELETE FROM MemberScores WHERE reqId_reqId='" + id + "'";

            command3.ExecuteNonQuery();
            connection.Close();
            Requirement requirement = await db.Requirements.FindAsync(id);
            db.Requirements.Remove(requirement);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// the function view to the user the vote page to the project. 
        /// </summary>
        /// <param name="id">the id of the project</param>
        /// <returns>the view of the page</returns>
        [Authorize]
        public ActionResult VoteToReq(int id)
        {
            Session["ReqId"] = id;
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "", Value = "" });
            items.Add(new SelectListItem { Text = "1", Value = "1" });
            items.Add(new SelectListItem { Text = "2", Value = "2" });
            items.Add(new SelectListItem { Text = "3", Value = "3" });
            items.Add(new SelectListItem { Text = "4", Value = "4" });
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            ViewBag.Votelist = items;
            return View();
        }

        /// <summary>
        /// the httpost function to vote to the project
        /// </summary>
        /// <returns>Redirect To index of requirements list</returns>
        [HttpPost]
        public ActionResult VoteToReq()
        {
            int ChooseVote = Convert.ToInt32(Request.Form["Votelist"]);
            // add to the ComplitionScore table a record of the vote of the user
            ComplitionScore cs = new ComplitionScore();
            cs.scoreValue = ChooseVote;
            cs.MemberId = db.Members.Find(Convert.ToInt32(Session["UserId"]));
            cs.reqId = db.Requirements.Find(Convert.ToInt32(Session["ReqId"]));
            db.ComplitionScores.Add(cs);

            // update the Total_Complition_Score of the requirement.
            Requirement r = db.Requirements.Find(Convert.ToInt32(Session["ReqId"]));
            r.Total_Complition_Score = r.Total_Complition_Score + ChooseVote;
            db.Entry(r).State = EntityState.Modified;
            db.SaveChanges();
            ModelState.AddModelError("CredentialError", "Your vote was received on the system. Thanks");
            //return View("Index/"+ Convert.ToInt32(Session["projectid"]));
            return RedirectToAction("Index", new { id= Convert.ToInt32(Session["projectid"]) });
        }

        /// <summary>
        /// the function view to the user the english document that export to SRS format
        /// </summary>
        /// <param name="id">the id of the project</param>
        /// <returns>the view of the page with the list of the requirements of project</returns>
        public ActionResult ExportDucSRS(int id)
        {
            int ProjectId = id;
            ViewBag.MemberProjectList = db.ProjectMembers.Where(x => x.projectId.projectId == ProjectId).ToList();
            return View(db.Requirements.Where(x => x.projectId.projectId == ProjectId).ToList());
        }

        /// <summary>
        /// the function view to the user the document that export to SRS format but in hebrew lang.
        /// </summary>
        /// <param name="id">the if of the project</param>
        /// <returns>the view of the page with the list of the requirements of project</returns>
        public ActionResult ExportDucSRSHeb(int id)
        {
            int ProjectId = id;
            ViewBag.MemberProjectList = db.ProjectMembers.Where(x => x.projectId.projectId == ProjectId).ToList();
            return View(db.Requirements.Where(x => x.projectId.projectId == ProjectId).ToList());
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
