using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication14.DataAccessLayer;

namespace WebApplication14.Controllers
{
    /// <summary>
    /// The Reports Controller Contains all logic that belongs to the issues Reports.
    /// </summary>
    [Authorize]
    [RequireSSL]
    public class ReportsController : Controller
    {
        // The conncection to the server db that in the azure platform.
        public string connectionString = "Data Source=devrevise.database.windows.net;Initial Catalog=ReviseDatabase;Integrated Security=False;User ID=revise;Password=P1m2n3r$;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private ReviseDataBase db = new ReviseDataBase();

        /// <summary>
        /// main menu view of Reports
        /// </summary>
        /// <returns>view</returns>
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// The function return a report that show the Members that participant in project in the web and how much projects the member participanted.  
        /// </summary>
        /// <returns>table report view</returns>
        public ActionResult UserPro()
        {
            List<Object> req = new List<Object>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "select m.userName, count(*) from [dbo].[Members] as m,projectMembers as p where m.MemberId=p.MemberId_MemberId group by m.userName";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                List<Object> r = new List<Object>();
                r.Add(reader.GetString(0));
                r.Add(reader.GetInt32(1));
                req.Add(r);
            }
            reader.Close();

            connection.Close();
            ViewBag.listUsReq = req; // pass the the ReportTable via ViewBag to view
            return View();
        }


        /// <summary>
        /// The function return a report that show the project that contians requirements in the web and how much requirements the project containing.  
        /// </summary>
        /// <returns>table report view</returns>
        public ActionResult ProjectReq()
        {
            List<Object> req = new List<Object>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "select p.projectName, count(*) from Requirements as r, projects as p where p.projectId=r.projectId_projectId group by p.projectName";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                List<Object> r = new List<Object>();
                r.Add(reader.GetString(0));
                r.Add(reader.GetInt32(1));
                req.Add(r);
            }
            reader.Close();

            connection.Close();
            ViewBag.listProReq = req; // pass the the ReportTable via ViewBag to view
            return View();
        }

        /// <summary>
        /// The function return a report that show the Members in system and how much comments they writed p to now.  
        /// </summary>
        /// <returns>table report view</returns>
        public ActionResult MemCom()
        {
            List<Object> req = new List<Object>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "select m.userName, count(*) as count from [dbo].[Members] as m,Comments as c where m.MemberId=c.MemberId_MemberId group by m.userName order by count desc";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                List<Object> r = new List<Object>();
                r.Add(reader.GetString(0));
                r.Add(reader.GetInt32(1));
                req.Add(r);
            }
            reader.Close();

            connection.Close();
            ViewBag.listProReq = req; // pass the the ReportTable via ViewBag to view
            return View();
        }

        public ActionResult ProCom()
        {
            List<Object> req = new List<Object>();
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("", connection);

            command.CommandText = "select p.projectName, count(*) from Comments as c, projects as p where p.projectId=c.projectId_projectId group by p.projectName";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                List<Object> r = new List<Object>();
                r.Add(reader.GetString(0));
                r.Add(reader.GetInt32(1));
                req.Add(r);
            }
            reader.Close();

            connection.Close();
            ViewBag.listProReq = req; // pass the the ReportTable via ViewBag to view
            return View();
        }

    
    }
}