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
    public class RequirementTypesController : Controller
    {
        private ReviseDataBase db = new ReviseDataBase();

        // GET: RequirementTypes
        public ActionResult Index()
        {
            return View(db.RequirementTypes.ToList());
        }

        [HttpPost]
        public ActionResult Index(String txtfind)
        {
            var Typereq = from TR in db.RequirementTypes
                        select TR;
            if (!String.IsNullOrEmpty(txtfind))
            {
                Typereq = Typereq.Where(r => r.typeName.Contains(txtfind
                    ));
            }
            return View(Typereq.ToList());
        }

        // GET: RequirementTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequirementType requirementType = db.RequirementTypes.Find(id);
            if (requirementType == null)
            {
                return HttpNotFound();
            }
            return View(requirementType);
        }

        // GET: RequirementTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequirementTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "typeId,typeName,typeDiscription")] RequirementType requirementType)
        {
            if (ModelState.IsValid)
            {
                db.RequirementTypes.Add(requirementType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requirementType);
        }

        // GET: RequirementTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequirementType requirementType = db.RequirementTypes.Find(id);
            if (requirementType == null)
            {
                return HttpNotFound();
            }
            return View(requirementType);
        }

        // POST: RequirementTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "typeId,typeName,typeDiscription")] RequirementType requirementType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requirementType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requirementType);
        }

        // GET: RequirementTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequirementType requirementType = db.RequirementTypes.Find(id);
            if (requirementType == null)
            {
                return HttpNotFound();
            }
            return View(requirementType);
        }

        // POST: RequirementTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequirementType requirementType = db.RequirementTypes.Find(id);
            db.RequirementTypes.Remove(requirementType);
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
