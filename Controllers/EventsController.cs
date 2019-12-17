using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using EventsCenter.Models;

namespace EventsCenter.Controllers
{
    public class EventsController : Controller
    {
        private EventsCenterDBContext _db; // Needs this to be abel to make the DB Queries!!
        public int? _uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }
        //Is logged in function to check if in session
        private bool _isLoggedIn
        {
            get
            {
                int? uid = _uid;

                if (uid != null)
                {
                    User loggedInUser = 
                        _db.Users.FirstOrDefault(u => u.UserId == uid);

                        HttpContext.Session
                            .SetString("FullName", loggedInUser.FullName());
                }
                return uid != null;
            }
        }

        public EventsController(EventsCenterDBContext context)
        {
            _db = context;
        }

        //Return all events
        [HttpGet("events/all")]
        public IActionResult All()
        {   

            int? uid = _uid;

            if (!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            // List<Event> allEvents = _db.Events
            //     .Include(e => e.Registrants) // !!!!!!to be able to acces the author property of our individual events object we must explicitly state to INCLUDE using EF Framework --- joining the users table with the events table
            //     .ToList(); //LINQ Syntax

            List<Event> allEvents = _db.Events.OrderByDescending(x => x.Date)
                .Include(e => e.Registrants)
                .Include(e => e.Coordinator)
                // .ThenInclude(e => e.Guest)
                .ToList();

            User user = _db.Users.FirstOrDefault(u => u.UserId == uid);
            ViewBag.FullName = user.FullName();
            ViewBag.UserId = _uid;

            return View(allEvents);


        }

        [HttpGet("events/new")]
        public IActionResult New()
        {
            return View("New");
        }

        [HttpPost]
        [Route("events/create")]
        public IActionResult Create(Event newEvent)
        {   
            if (ModelState.IsValid)
            {
                if (_uid != null)
                {
                    newEvent.UserId = (int)_uid;
                    _db.Events.Add(newEvent);
                    _db.SaveChanges();

                    return RedirectToAction("All");
                }
                else 
                {
                    // no one in session
                    return RedirectToAction("Index", "Home");
                };
            }

            else
            {
                return View("New");
            }
            
            
        }

        [HttpGet("events/{eventId}")]
        public IActionResult SingleEvent(int eventId)
        {
            var singleEvent = _db.Events
                .Include(e => e.Coordinator)
                .Include(e => e.Registrants)
                .ThenInclude(e => e.Guest)
                .FirstOrDefault(e => e.EventId == eventId);
            return View(singleEvent);
        }

        [HttpPost("events/delete/{eventId}")]
        public IActionResult Delete(int eventId)
        {
            var singleEvent = _db.Events
                .FirstOrDefault(e => e.EventId == eventId);
            _db.Events.Remove(singleEvent);
            _db.SaveChanges();
            return RedirectToAction("All");
        }

        [HttpPost("events/reg/{eventId}")]
        public IActionResult Reg(int eventId)
        {
        var DidJoin = _db.Joins.Where(e => e.EventId == eventId).FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
        if (DidJoin == null)
        {
            Join newJoin = new Join();
            newJoin.UserId = (int)HttpContext.Session.GetInt32("UserId");
            newJoin.EventId = eventId;
            _db.Add(newJoin);
            _db.SaveChanges();
        }
        else
        {
            _db.Remove(DidJoin);
            _db.SaveChanges();
        }
        return RedirectToAction("All");
        }

        ///
    }
}