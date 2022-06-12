using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication14.DataAccessLayer;
using WebApplication14.Models;

namespace WebApplication14.Controllers
{
    /// <summary>
    /// The Roles Controller Contains all logic that belongs to the issues Roles.
    /// </summary>
    public class RolesController : Controller
    {
        private ReviseDataBase db = new ReviseDataBase();


        /// <summary>
        /// the function return a view with a list of all Roles in DataBase.
        /// </summary>
        /// <returns>list of all Roles in DataBase</returns>
        // GET: Roles
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }


        /// <summary>
        /// the HttpPost to search Role name in the list of Roles from the Database.
        /// </summary>
        /// <param name="txtfind">Search term</param>
        /// <returns>result to the search</returns>
        [HttpPost]
        public ActionResult Index(String txtfind)
        {
            //string strName = Request["txtfind"].ToString();
            var roles = from r in db.Roles
                          select r;
            if (!String.IsNullOrEmpty(txtfind))
            {
                roles = roles.Where(r => r.roleName.Contains(txtfind
                    ));
            }
            return View(roles.ToList());
        }



        /// <summary>
        /// The function show a view of details for specific Role.
        /// </summary>
        /// <param name="id">the id Role for show details</param>
        /// <returns>the Role object with all the detials about him.</returns>
        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }



        /// <summary>
        /// The function return the view to the user for create page
        /// </summary>
        /// <returns>view of create page</returns>
        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// the HttpPost function for create new role
        /// </summary>
        /// <param name="role">the new role with all the parameters</param>
        /// <returns>Redirect To index of Roles list</returns>
        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "roleId,roleName,roleDescription")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }


        /// <summary>
        /// the function show the screen to edit Existing role in DataBase.
        /// </summary>
        /// <param name="id">RoleId to edit</param>
        /// <returns>Role object to edit</returns>
        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }



        /// <summary>
        /// The HttpPost function to the edit role and to update the detials.
        /// </summary>
        /// <param name="role">the object Role with all the details that the user update about this Role</param>
        /// <returns>Redirect To the list of Roles</returns>
        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "roleId,roleName,roleDescription")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }


        /// <summary>
        /// the function show the view of confirm delete Member from the Database.
        /// </summary>
        /// <param name="id">RoleId to delete</param>
        /// <returns>the object Role to delete</returns>
        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }


        /// <summary>
        ///  the HttpPost function to delete Role from the DataBase. 
        /// </summary>
        /// <param name="id">RoleId to delete</param>
        /// <returns>Redirect To the list of Roles</returns>
        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
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
