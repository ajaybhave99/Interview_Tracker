using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Interview_Tracker.Models;


namespace Interview_Tracker.Controllers
{
    public class HomeController : Controller
    {
        Interviwer_TrackerEntities db = new Interviwer_TrackerEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpPost]
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user from the database based on the provided email
                var user = db.Interviwers.FirstOrDefault(u => u.Email == model.Email);

                if (user != null)
                {
                    // Verify the password using your preferred password hashing mechanism
                    bool isPasswordValid = VerifyPassword(model.Password, user.Password);

                    if (isPasswordValid)
                    {
                        // Check the role of the user
                        if (user.Role == "Interviewer")
                        {
                            // Perform actions specific to the Interviewer role
                            // Redirect to the Index view of Candidates controller
                            return RedirectToAction("Index", "Candidates");
                        }
                        else if (user.Role == "Recruiter")
                        {
                            // Perform actions specific to the Recruiter role
                            // Redirect to the Index view of Feedback controller
                            return RedirectToAction("Index", "Feedbacks");
                        }
                    }
                }

                // If the email or password is invalid, add a model error
                ModelState.AddModelError("", "Invalid email or password.");
            }

            // If there is an error, return the login view with the model
            return View(model);
        }
        private bool VerifyPassword(string password1, string password2)
        {
            return password1 == password2;
        }
        public ActionResult Submit()
        {
            return View();
        }
        public ActionResult Logout()
        {
            // Perform any necessary logout operations, such as clearing session data or authentication tokens

            // Redirect to the Login view
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();

            // Disable caching
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetNoStore();

            // Set a response header to prevent caching in browsers
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");

            return RedirectToAction("Index", "Home");
        }

        public ActionResult PasswordRecovery()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordRecovery(string email)
        {
            var user = db.Interviwers.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                string newPassword = GenerateNewPassword();

                user.Password = newPassword;
                db.SaveChanges();

                // Send the new password to the user via email or other communication method

                return RedirectToAction("PasswordResetConfirmation");
            }

            ModelState.AddModelError("", "User with the provided email does not exist.");

            return View();
        }

        public ActionResult PasswordResetConfirmation()
        {
            return View();
        }

        private string GenerateNewPassword()
        {
            // Define the characters that can be used in the password
            string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";

            // Set the desired length of the password
            int passwordLength = 10;

            // Create a random number generator
            Random random = new Random();

            // Generate the password by randomly selecting characters from the valid character set
            string password = new string(Enumerable.Repeat(validChars, passwordLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return password;
        }

    }
}