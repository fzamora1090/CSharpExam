using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using EventsCenter.Models;

namespace EventsCenter.Controllers
{
    public class HomeController : Controller
    {
        // DB Connection!!
        private EventsCenterDBContext _db;
        public HomeController(EventsCenterDBContext context)
        {
            _db = context;
        }


        public IActionResult Index()
        {   
            int? _uid = HttpContext.Session.GetInt32("UserId");

            if (_uid != null)
            {
                // need to specify the controller if youa re redirecting to a different controller function
                return RedirectToAction("All", "Events");
            }
            return View();
        }

        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                bool isEmailTaken = _db.Users.Any(user => newUser.Email == user.Email);

                if (isEmailTaken)
                {
                    ModelState.AddModelError("Email", "Email taken");
                }
            }

            if (ModelState.IsValid == false)
            {
                return View("Index");
            }

            //No errors! yay

            //HashPassword 
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            _db.Users.Add(newUser);
            _db.SaveChanges();
            //adding user id to session
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            return RedirectToAction("All", "Events");
        }

        public IActionResult Login(Login loginUser)
        {
            if (ModelState.IsValid == false)
            {
                return View("Index");
            }
            else
            {
                User dbUser = _db.Users.FirstOrDefault(user => loginUser.LoginEmail == user.Email);

                if (dbUser == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Credentials, ya dummy!");
                }
                else 
                {
                    User viewUser = new User {
                        Email = loginUser.LoginEmail,
                        Password = loginUser.LoginPassword
                    };
                    
                    PasswordHasher<User> hasher = new PasswordHasher<User>();

                    PasswordVerificationResult result = hasher.VerifyHashedPassword(viewUser, dbUser.Password, viewUser.Password);
                    
                    //check if it failed
                    if (result == 0)
                    {
                        ModelState.AddModelError("LoginPassword", "Invalid credentials");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("UserId", dbUser.UserId);
                    }
                }
            }
            if (ModelState.IsValid == false)
            {
                // display error messages
                // on the form they came from
                return View("Index");
            }

            return RedirectToAction("All", "Events");

            }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
