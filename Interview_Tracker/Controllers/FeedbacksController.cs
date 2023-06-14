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
    public class FeedbacksController : Controller
    {
        private Interviwer_TrackerEntities db = new Interviwer_TrackerEntities();

        // GET: Feedbacks
        public ActionResult Index()
        {
            var feedbacks = db.Feedbacks.Include(f => f.Candidate).Include(f => f.Interviwer);
            return View(feedbacks.ToList());
        }

        // GET: Feedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: Feedbacks/Create
        public ActionResult Create()
        {
            ViewBag.Candidate_Id = new SelectList(db.Candidates, "C_Id", "C_Name");
            ViewBag.Interviwer_Id = new SelectList(db.Interviwers, "E_id", "Interviewer_Name");
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "F_Id,Date,Screening_Level,Candidate_Id,Interviwer_Id,Feedback1,Result")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index","Candidates");
            }

            ViewBag.Candidate_Id = new SelectList(db.Candidates, "C_Id", "C_Name", feedback.Candidate_Id);
            ViewBag.Interviwer_Id = new SelectList(db.Interviwers, "E_id", "Interviewer_Name", feedback.Interviwer_Id);
            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.Candidate_Id = new SelectList(db.Candidates, "C_Id", "C_Name", feedback.Candidate_Id);
            ViewBag.Interviwer_Id = new SelectList(db.Interviwers, "E_id", "DAS_ID", feedback.Interviwer_Id);
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "F_Id,Date,Screening_Level,Candidate_Id,Interviwer_Id,Feedback1,Result")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Candidate_Id = new SelectList(db.Candidates, "C_Id", "C_Name", feedback.Candidate_Id);
            ViewBag.Interviwer_Id = new SelectList(db.Interviwers, "E_id", "DAS_ID", feedback.Interviwer_Id);
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
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
        [HttpGet]
        public ActionResult Report()
        {
            // Display a form to search for a candidate by name

            return View();
        }

        // POST: Feedback/Report
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(string candidateName)
        {
            // Retrieve feedback data for the specified candidate
            List<Feedback> feedbacks = GetFeedbacksByCandidate(candidateName);

            // Pass the feedbacks to the view for display
            return View(feedbacks);
        }

        //private List<Feedback> GetFeedbacksByCandidate(string candidateName)
        //{
        //    using (practiceEntities dbContext = new practiceEntities())
        //    {
        //        // Retrieve feedbacks for the specified candidate from the database
        //        List<Feedback> feedbacks = dbContext.Feedbacks
        //            .Where(f => f.Candidate.C_Name == candidateName)
        //            .ToList();

        //        return feedbacks;
        //    }
        private List<Feedback> GetFeedbacksByCandidate(string candidateName)
        {
            using (Interviwer_TrackerEntities dbContext = new Interviwer_TrackerEntities())
            {
                // Retrieve feedbacks for the specified candidate from the database
                List<Feedback> feedbacks = dbContext.Feedbacks
                    .Include("Candidate") // Include the Candidate navigation property
                    .Include("Interviwer") // Include the Interviwer navigation property
                    .Where(f => f.Candidate.C_Name == candidateName)
                    .ToList();

                // Set the Interviwer_Name property using the related Interviwer entity
                foreach (var feedback in feedbacks)
                {
                    feedback.Interviwer_Name = feedback.Interviwer?.Interviewer_Name;
                }

                return feedbacks;
            }
        }
        [HttpGet]
        public ActionResult CandidateSummary()
        {
            Class1 viewModel = new Class1(); // Create an instance of the model
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CandidateSummary(Class1 viewModel)
        {
            if (ModelState.IsValid)
            {
                // Retrieve data and update the model properties
                viewModel.TotalCandidates = db.Candidates.Count();
                viewModel.SelectedCandidates = db.Feedbacks.Count(f => f.Result == "Selected");
                viewModel.NotSelectedCandidates = db.Feedbacks.Count(f => f.Result == "Not Selected");
                viewModel.CandidatesSelectedInL1 = db.Feedbacks.Count(f => f.Screening_Level == "L1" && f.Result == "Selected");
                viewModel.CandidatesSelectedInL2 = db.Feedbacks.Count(f => f.Screening_Level == "L2" && f.Result == "Selected");
                viewModel.CandidatesSelectedInHR = db.Feedbacks.Count(f => f.Screening_Level == "HR" && f.Result == "Selected");

                return View(viewModel);
            }

            return View(viewModel);
        }
        public ActionResult DetailReport()
        {
            var feedbacks = db.Feedbacks.Include(f => f.Candidate).Include(f => f.Interviwer);
            return View(feedbacks.ToList());
            

        }
    }
}
