using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication14.DataAccessLayer;
using WebApplication14.Models;

namespace WebApplication14.Controllers
{
    /// <summary>
    /// The Comments Controller Contains all logic that belongs to the issues comments.
    /// </summary>
    public class CommentsController : Controller
    {
        private ReviseDataBase db = new ReviseDataBase();
        // The conncection to the server db that in the azure platform.
        public string connectionString = "Data Source=devrevise.database.windows.net;Initial Catalog=ReviseDatabase;Integrated Security=False;User ID=revise;Password=P1m2n3r$;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        /// <summary>
        /// the function return a view with a list of all Comments in DataBase.
        /// </summary>
        /// <returns>list of all Roles in DataBase</returns>
        // GET: Comments
        public ActionResult Index()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "content", Value = "1" });
            items.Add(new SelectListItem { Text = "user name", Value = "2" });
            items.Add(new SelectListItem { Text = "project", Value = "3" });
            items.Add(new SelectListItem { Text = "requirement", Value = "4" });
            ViewBag.FilterList = items;
            return View(db.Comments.ToList().Reverse<Comment>());
        }


        /// <summary>
        /// the HttpPost to search Role name in the list of Comments from the Database.
        /// </summary>
        /// <param name="txtfind">Search term</param>
        /// <returns>result to the search</returns>
        [HttpPost]
        public ActionResult Index(String txtfind)
        {
            string ChooseTofilter = Request.Form["FilterList"].ToString();
            //string strName = Request["txtfind"].ToString();
            var comments = from c in db.Comments
                        select c;
            if (!String.IsNullOrEmpty(txtfind))
            {
                if (ChooseTofilter == "1") {
                comments = comments.Where(r => r.Content.Contains(txtfind
                    ));
                }
                else if(ChooseTofilter == "2")
                {
                    comments = comments.Where(r => r.MemberId.userName.Contains(txtfind
                    ));
                }
                else if (ChooseTofilter == "3")
                {
                    comments = comments.Where(r => r.projectId.projectName.Contains(txtfind
                    ));
                }
                else if (ChooseTofilter == "4")
                {
                    comments = comments.Where(r => r.reqId.title.Contains(txtfind
                    ));
                }
            }
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "content", Value = "1" });
            items.Add(new SelectListItem { Text = "user name", Value = "2" });
            items.Add(new SelectListItem { Text = "project", Value = "3" });
            items.Add(new SelectListItem { Text = "requirement", Value = "4" });
            ViewBag.FilterList = items;
            return View(comments.ToList().Reverse<Comment>());
        }



        // GET: Comments/Details/5
        /// <summary>
        /// The function show a view of details for specific Comment.
        /// </summary>
        /// <param name="id">the id Comment for show details</param>
        /// <returns>the Comment object with all the detials about him.</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        /// <summary>
        /// The function return the view to the user for create page
        /// </summary>
        /// <returns>view of create page</returns>
        public ActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// the HttpPost function for create new Comment
        /// </summary>
        /// <param name="comment">the new comment with all the parameters</param>
        /// <returns>Redirect To index of Comments list</returns>
        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "commentId,CreateDatetime,Content")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comment);
        }


        /// <summary>
        /// the function show the screen to edit Existing comments in DataBase.
        /// </summary>
        /// <param name="id">CommentId to edit</param>
        /// <returns>Comment object to edit</returns>
        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }




        /// <summary>
        /// The HttpPost function to the edit comment and to update the detials.
        /// </summary>
        /// <param name="comment">the object Comment with all the details that the user update about this Role</param>
        /// <returns>Redirect To the list of Comment</returns>
        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "commentId,CreateDatetime,Content")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }



        /// <summary>
        /// the function show the view of confirm delete Member from the Database.
        /// </summary>
        /// <param name="id">CommentId to delete</param>
        /// <returns>the object Comment to delete</returns>
        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }


        /// <summary>
        ///  the HttpPost function to delete Comment from the DataBase. 
        /// </summary>
        /// <param name="id">CommentId to delete</param>
        /// <returns>Redirect To the list of Comments</returns>
        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
    }
}
