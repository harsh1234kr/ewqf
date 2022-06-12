using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication14.Models;
using WebApplication14.DataAccessLayer;
using System.Net.Mail;
using System.Data.SqlClient;

namespace WebApplication14.Controllers
{
    /// <summary>
    /// Class that deals with everything related to security at SSL HTTPS
    /// </summary>
    public class RequireSSLAttribute : FilterAttribute, IAuthorizationFilter
    {
       
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (!filterContext.HttpContext.Request.IsSecureConnection)
            {
                HandleNonHttpsRequest(filterContext);
            }
        }

        protected virtual void HandleNonHttpsRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.Url.Host.Contains("localhost")) return;

            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("The requested resource can only be accessed via SSL");
            }

            string url = "https://" + filterContext.HttpContext.Request.Url.Host + filterContext.HttpContext.Request.RawUrl;
            filterContext.Result = new RedirectResult(url);
        }
    }

    /// <summary>
    /// The Authentication Controller Contains all logic that belongs to the issues checking the user's privileges identity.
    /// </summary>
    public class AuthenticationController : Controller
    {
        private static ReviseDataBase db = new ReviseDataBase();
        // The conncection to the server db that in the azure platform.
        public string connectionString = "Data Source=devrevise.database.windows.net;Initial Catalog=ReviseDatabase;Integrated Security=False;User ID=revise;Password=P1m2n3r$;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        // GET: Authentication
        [RequireSSL]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// The function Taking the logged user and returns the IDMember of the user.
        /// </summary>
        /// <returns>the ID Member of the user that connect now to the system</returns>
        public Member GetMember()
        {
            String un = Convert.ToString(Session["UserName"]);
            Member memberId = db.Members.Where(user => user.userName.Contains(un)).First();
            return (memberId);
        }

        /// <summary>
        /// The function do the log in to the web and check the detials that the user type and send in the Log-in screen.
        /// </summary>
        /// <param name="u">contains the user name and the password that the user type on the screen</param>
        /// <returns>Redirect to the right page.. if the details its true to the web site, if no return again to the log-in screen.</returns>
        [HttpPost]
        public ActionResult DoLogin(UserDetails u)
        {
            if (ModelState.IsValid)
            {
                MemberBusinessLayer bal = new MemberBusinessLayer();
                UserStatus status = bal.GetUserValidity(u); // the function GetUserValidity check in the DataBase if the detials that the the user type its true or not. and return the status. 
                bool IsAdmin = false; // is Admin or not
                // if the user is Adminstrtor then...
                if (status == UserStatus.AuthenticatedAdmin)
                {
                    IsAdmin = true;
                }
                // if the user is Member in the system then...
                else if (status == UserStatus.AuthentucatedUser)
                {
                    IsAdmin = false;
                }
                // if the user is not in registered in the DataBase...
                else
                {
                    ModelState.AddModelError("CredentialError", "Invalid Username or Password");
                    return View("Login");
                }
                // creates an authentication tickat fo the supplied user name and adds it to the cookies collection of the response. 
                FormsAuthentication.SetAuthCookie(u.UserName, false);
                //insert to Sessions importent informaion to the Check permissions within the web site.
                Session["IsAdmin"] = IsAdmin;
                Session["UserName"] = u.UserName;
                Session["UserId"] = GetMember().MemberId;
                Session["ScoreMember"] = db.Members.Find(GetMember().MemberId).scoreMember;
                Session["gradeMember"] = GetMember().grade;

                int mId = GetMember().MemberId;
                Session["CommentlistT"] = db.Comments.Where(x => x.MemberId.MemberId == mId ).ToList();

                




                return RedirectToAction("Index", "Projects");
                //New Code End
            }
            else
            {
                return View("Login");
            }
        }

        /// <summary>
        /// The function executes logout from the system
        /// </summary>
        /// <returns>Redirect To log in screen</returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        /// <summary>
        /// The funcion show the view of the forgot a password.
        /// </summary>
        /// <returns></returns>
        [RequireSSL]
        public ActionResult ForgotPass()
        {
            return View();
        }


        /// <summary>
        /// the HttpPost after the user type your yser name and get his password to the e-mail.
        /// </summary>
        /// <param name="UserName">the username that the the user type from the screen</param>
        /// <returns>return to the right screen if the detials true or false</returns>
        [HttpPost]
        public ActionResult ForgotPass(string UserName)
        {
            string em = "";
            string pass = "";
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "Select * From Members where userName='" + UserName + "';";

            SqlDataReader reader = command.ExecuteReader();
        // if the query returned  any value then...
        if(reader.Read())
        { 
                em = reader.GetString(4);
                pass = reader.GetString(6);
        }
        else{
                ModelState.AddModelError("CredentialError", "Invalid Username");
                return View("ForgotPass"); }

            reader.Close();
            connection.Close(); // close the connection to the DataBase.


            // if the query returned  any value then send to the user email with his password.
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(em);
            mail.From = new MailAddress("revisegroup@gmail.com", "ReviseTeam", System.Text.Encoding.UTF8);
            mail.Subject = "password recovery for "+ UserName;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Hello, " + UserName + "! <br /> your password is " + pass+ "<br /><br /><br /><br /> Thanks,<br /> Revise Team";
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("revisegroup@gmail.com", "p1m2n3r4");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Send(mail);
            ModelState.AddModelError("CredentialError2", "The Password sent to your member email!");
            return View("Login");
        } 
    }
}