using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Interview_Tracker.Models;

namespace Interview_Tracker.Controllers
{
    public class CandidatesController : Controller
    {
        private Interviwer_TrackerEntities db = new Interviwer_TrackerEntities();

        // GET: Candidates
        public ActionResult Index()
        {
            return View(db.Candidates.ToList());
        }

        // GET: Candidates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidate candidate = db.Candidates.Find(id);
            if (candidate == null)
            {
                return HttpNotFound();
            }
            return View(candidate);
        }

        // GET: Candidates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Candidates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Candidate document, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(fileName).ToLower();

                if (extension != ".pdf" && extension != ".docx")
                {
                    ModelState.AddModelError("", "Only PDF and DOCX files are allowed.");
                    return View(document);
                }

                using (var reader = new BinaryReader(file.InputStream))
                {
                    var fileContent = reader.ReadBytes(file.ContentLength);

                    document.FileName = fileName;
                    document.ResumeFile = fileContent;
                    // Set other properties of the Document model based on user input
                    document.C_Name = Request.Form["C_Name"];
                    document.Phone = Request.Form["Phone"];
                    document.Email = Request.Form["Email"];
                    document.College_Name = Request.Form["College_Name"];
                    document.Date = DateTime.Parse(Request.Form["Date"]);
                    document.Higher_Qualification = Request.Form["Higher_Qualification"];
                    document.YearOfPassing = Request.Form["YearOfPassing"];
                    document.Percentage = Request.Form["Percentage"];
                   


                    db.Candidates.Add(document);
                    db.SaveChanges();

                    return RedirectToAction("Submit", "Home");
                }
            }

            return View(document);
        }


        // GET: Candidates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidate candidate = db.Candidates.Find(id);
            if (candidate == null)
            {
                return HttpNotFound();
            }
            return View(candidate);
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "C_Id,C_Name,Phone,Email,Date,College_Name,Higher_Qualification,Stream,YearOfPassing,Percentage,ResumeFile,FileName")] Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(candidate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(candidate);
        }

        // GET: Candidates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidate candidate = db.Candidates.Find(id);
            if (candidate == null)
            {
                return HttpNotFound();
            }
            return View(candidate);
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Candidate candidate = db.Candidates.Find(id);
            db.Candidates.Remove(candidate);
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
        public ActionResult OpenFile(int id)
        {
            // Retrieve the Document entity from the database based on the provided id
            var document = db.Candidates.Find(id);

            if (document != null)
            {
                string contentType;
                if (document.FileName.EndsWith(".pdf"))
                {
                    contentType = "application/pdf";
                }
                else if (document.FileName.EndsWith(".docx"))
                {
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                }
                else
                {
                    // Handle unsupported file types or set a default content type
                    contentType = "application/octet-stream";
                }

                // Return the file content as a FileContentResult with the appropriate content type
                return File(document.ResumeFile, contentType, document.FileName);
            }

            // If the document is not found, return a HttpNotFound response or handle the scenario accordingly
            return HttpNotFound();
        }
    }
}
