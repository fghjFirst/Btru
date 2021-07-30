using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Btru.Models;
using Microsoft.AspNetCore.Authorization;
using Btru.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Btru.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext context)
        {
            db = context;
        }

        public ActionResult Index()
        {
            List<DateTime> dates = new List<DateTime>();
            List<string> datesOnly = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                dates.Add(DateTime.Now.Date.AddDays(-i));
                datesOnly.Add(dates[i].ToString().Split(' ').ToList()[0]);
            }
            ApplicationUser user = db.Users.Where(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            List<TimeSpan?> sleep = SleepSchedulesController.GetSleepLog(user, db, dates);
            ViewBag.Sleep = sleep;
            ViewBag.Dates = datesOnly;
            ViewBag.alreadyInFavorites = false;
            ViewBag.FavoriteBooks = db.FavoriteBooks.Where(x => x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Include(x => x.Book).ToList();
            ViewBag.ReadBooks = db.ReadingAssignments.Where(x => x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && x.Read == true).Include(x => x.Book).ToList();
            return View();
        }
    
        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
