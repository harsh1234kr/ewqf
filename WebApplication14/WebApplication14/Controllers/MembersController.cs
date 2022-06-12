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
    /// The Members Controller Contains all logic that belongs to the issues Members.
    /// </summary>
    //[Authorize]
    public class MembersController : Controller
    {
        private ReviseDataBase db = new ReviseDataBase();
        // The conncection to the server db that in the azure platform.
        public string connectionString = "Data Source=devrevise.database.windows.net;Initial Catalog=ReviseDatabase;Integrated Security=False;User ID=revise;Password=P1m2n3r$;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /// <summary>
        /// The function get a User Name of Member and return the MemberId of the Member.
        /// </summary>
        /// <param name="str">the User Name of Member</param>
        /// <returns>the memberId of the Member</returns>
        public Member GetMember(String str)
        {
            String un = str;
            Member memberId = db.Members.Where(user => user.userName.Contains(un)).First();
            return (memberId);
        }


        // GET: Members
        [Authorize]
        [RequireSSL]
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        /// <summary>
        /// the HttpPost to search user name in the list of Member from the Database.
        /// </summary>
        /// <param name="txtfind">the search type</param>
        /// <returns>the list of members that found in the DataBase according to the Expression search</returns>
        [HttpPost]
        public ActionResult Index(String txtfind)
        {
            var members = from m in db.Members
                          select m;
            //if the Expression search is empty then.... 
            if (!String.IsNullOrEmpty(txtfind))
            {
                members = members.Where(m => m.userName.Contains(txtfind
                    ));
            }
            return View(members.ToList());
        }


        /// <summary>
        /// The function show a view of details for specific Member.
        /// </summary>
        /// <param name="id">the id Member for show details</param>
        /// <returns>the Member object with all the detials about him.</returns>
        // GET: Members/Details/5
        [Authorize]
        [RequireSSL]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        /// <summary>
        /// The function return the view to the user for create page
        /// </summary>
        /// <returns>view of create page</returns>
        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Create1()
        {
            return View();
        }


        /// <summary>
        /// the HttpPost function for create new Member
        /// </summary>
        /// <param name="member">the new member with all the parameters</param>
        /// <returns>Redirect To index of Members list</returns>
        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberId,firstName,lastName,userName,email,phone,password,pic,IsAdmin")] Member member)
        {
            if ((member.pic == "") || (member.pic == null))
            {
                member.pic = "http://www.bitrebels.com/wp-content/uploads/2011/02/Original-Facebook-Geek-Profile-Avatar-1.jpg";
            }
            member.scoreMember = 0;
            member.grade = 0;
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.To.Add("" + member.email + "");
                mail.From = new MailAddress("revisegroup@gmail.com", "ReviseTeam", System.Text.Encoding.UTF8);
                mail.Subject = "Welcome to Revise " + member.firstName + "!";
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = "<h3>Hello, " + member.firstName + ". <br />  We want to congratulate you on your Registration to Revise,  <br /> and wishing you success in your projects. <br /> <br /> Your User name is: " + member.userName + " <br /> Your password is: " + member.password + " <br /> <br /> You Invited to sign in to <a href='https://reviseproject.azurewebsites.net'>Revise site</a> and start to take part of it! <br /> <br /> Always with you and for you<br /> Revise Team:-) </h3>";
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("revisegroup@gmail.com", "p1m2n3r4");
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Send(mail);


                ModelState.AddModelError("CredentialError2", "your sign-up is completed successfully");
                return View("../Authentication/Login");
            }

            return View(member);
        }


        /// <summary>
        /// The function show the screen to edit Existing Member in DataBase.
        /// </summary>
        /// <param name="id">the id of the Member to edit</param>
        /// <returns>The object Member to edit</returns>
        // GET: Members/Edit/5
        [Authorize]
        [RequireSSL]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }


        /// <summary>
        /// The HttpPost function to the edit member and to update the detials.
        /// </summary>
        /// <param name="member">the object Member with all the details that the user update about this member</param>
        /// <returns>Redirect To the list of Members</returns>
        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberId,firstName,lastName,userName,email,phone,password,pic,IsAdmin")] Member member)
        {
            if ((member.pic == "")|| (member.pic == null))
            {
                member.pic = "http://www.bitrebels.com/wp-content/uploads/2011/02/Original-Facebook-Geek-Profile-Avatar-1.jpg";
            }
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                if(Convert.ToBoolean(Session["IsAdmin"]))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Details", new { id = member.MemberId });
                }   
            }
            return View(member);
        }


        /// <summary>
        /// the function show the view of confirm delete Member from the Database.
        /// </summary>
        /// <param name="id">the id Member of the member to delete</param>
        /// <returns>the object Member to delete</returns>
        // GET: Members/Delete/5
        [Authorize]
        [RequireSSL]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }



        /// <summary>
        /// the HttpPost function to delete Member from the DataBase. 
        /// </summary>
        /// <param name="id">MemberId to delete</param>
        /// <returns>Redirect To the list of Members</returns>
        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "DELETE FROM MemberScores WHERE MemberId_MemberId='" + id + "'";

            command.ExecuteNonQuery();

            SqlCommand command2 = new SqlCommand("", connection);

            command2.CommandText = "DELETE FROM Comments WHERE MemberId_MemberId='" + id + "'";

            command2.ExecuteNonQuery();

            SqlCommand command4 = new SqlCommand("", connection);

            command4.CommandText = "DELETE FROM projectMembers WHERE MemberId_MemberId='" + id + "'";

            command4.ExecuteNonQuery();

            
            connection.Close();
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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
        /// view ContactUs to User
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [RequireSSL]
        public ActionResult ContactUs()
        {
            return View(db.Members.Find(Convert.ToInt32(Session["UserId"])));
        }

        /// <summary>
        /// the HttpPost function send a email to the managers about some isuues
        /// </summary>
        /// <param name="lname">The last name from the form web</param>
        /// <param name="fname">The first name from the form web</param>
        /// <param name="email">The email from the form web</param>
        /// <param name="phone">The phone from the form web</param>
        /// <param name="message">The message from the form web</param>
        /// <returns>if the message sent Successfully</returns>
        [HttpPost]
        public ActionResult ContactUs(string lname,string fname,string email, string phone,string message)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add("sap.sapir@hotmail.com,pelegrotem@gmail.com");
            mail.From = new MailAddress("revisegroup@gmail.com", "ReviseTeam", System.Text.Encoding.UTF8);
            mail.Subject = "Contact us - message from " + fname+ " "+ lname;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "name: " + fname + " " + lname+" <br /> Email: " + email + "<br /> phone: " + phone + " <br /> message: <br />" + message + "<br />";
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("revisegroup@gmail.com", "p1m2n3r4");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Send(mail);
            ModelState.AddModelError("CredentialError2", "The message send Successfully!");
            return View("OkSend");
        }

        /// <summary>
        /// the function view to user Ok Message.
        /// </summary>
        /// <returns>the view of the page</returns>
        public ActionResult OkSend()
        {
            return View();
        }


        /// <summary>
        /// view to the user the page AboutUs
        /// </summary>
        /// <returns>The view of About us</returns>
        [Authorize]
        [RequireSSL]
        public ActionResult AboutUs()
        {
            return View();
        }

        /// <summary>
        /// view to the user the page Help
        /// </summary>
        /// <returns>The view of help</returns>
        [Authorize]
        [RequireSSL]
        public ActionResult Help()
        {
            return View();
        }


    }
}
