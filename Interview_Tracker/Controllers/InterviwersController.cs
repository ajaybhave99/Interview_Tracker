using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Interview_Tracker.Models;

namespace Interview_Tracker.Controllers
{
    public class InterviwersController : Controller
    {
        private Interviwer_TrackerEntities db = new Interviwer_TrackerEntities();

        // GET: Interviwers
        public ActionResult Index()
        {
            return View(db.Interviwers.ToList());
        }

        // GET: Interviwers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interviwer interviwer = db.Interviwers.Find(id);
            if (interviwer == null)
            {
                return HttpNotFound();
            }
            return View(interviwer);
        }

        // GET: Interviwers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Interviwers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "E_id,DAS_ID,Interviewer_Name,Phone,Email,Password,GCM_Level,Manager_Name,Role")] Interviwer interviwer)
        {
            if (ModelState.IsValid)
            {
                db.Interviwers.Add(interviwer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(interviwer);
        }

        // GET: Interviwers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interviwer interviwer = db.Interviwers.Find(id);
            if (interviwer == null)
            {
                return HttpNotFound();
            }
            return View(interviwer);
        }

        // POST: Interviwers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "E_id,DAS_ID,Interviewer_Name,Phone,Email,Password,GCM_Level,Manager_Name,Role")] Interviwer interviwer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interviwer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(interviwer);
        }

        // GET: Interviwers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interviwer interviwer = db.Interviwers.Find(id);
            if (interviwer == null)
            {
                return HttpNotFound();
            }
            return View(interviwer);
        }

        // POST: Interviwers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interviwer interviwer = db.Interviwers.Find(id);
            db.Interviwers.Remove(interviwer);
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
