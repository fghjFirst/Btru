using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Btru.Data;
using Btru.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Btru.Controllers
{
    [Authorize]
    public class SleepSchedulesController : Controller
    {
        private readonly ApplicationDbContext db;

        public SleepSchedulesController(ApplicationDbContext context)
        {
            db = context;
        }


        public ActionResult Index()
        {
            if (ProfileController.TimeIsUpToDate(db.Users.Where(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault(), db))
            {
                return View(db.SleepSchedules.Where(x => x.Date == DateTime.Now.Date && x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault());
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditWokeUp(int id)
        {
            var ss = db.SleepSchedules.Find(id);
            if (ss == null)
            {
                return NotFound();
            }
            return View(ss);
        }

        [HttpPost]
        public ActionResult EditWokeUp(TimeSpan time)
        {
            if (validTime(time, "WokeUp"))
            {
                db.SleepSchedules.Where(x => x.Date == DateTime.Now.Date && x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().WokeUp = time;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditWentToSleep(int id)
        {
            var ss = db.SleepSchedules.Find(id);
            if (ss == null)
            {
                return NotFound();
            }
            return View(ss);
        }

        [HttpPost]
        public ActionResult EditWentToSleep(TimeSpan time)
        {
            if (validTime(time, "WentToSleep"))
            {
                db.SleepSchedules.Where(x => x.Date == DateTime.Now.Date && x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().WentToSleep = time;
                db.SaveChanges();   
            }
            return RedirectToAction("Index");
        }

        public static bool UpdateSleep(ApplicationUser user, ApplicationDbContext dbContext)
        {
            SleepSchedule ss = new SleepSchedule();
            ss.User = user;
            ss.Date = DateTime.Now;
            if (dbContext.SleepSchedules.Where(x => x.Date == DateTime.Now.Date && x.User == user).FirstOrDefault() != null)
            {
                dbContext.SleepSchedules.Remove(dbContext.SleepSchedules.Where(x => x.Date == DateTime.Now.Date && x.User == user).FirstOrDefault());
                dbContext.SaveChanges();
            }
            dbContext.SleepSchedules.Add(ss);
            dbContext.SaveChanges();
            return true;
        }

        private bool validTime(TimeSpan time, string Case)
        {
            SleepSchedule Today = db.SleepSchedules.Where(x => x.Date == DateTime.Now.Date && x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            SleepSchedule Yesterday = db.SleepSchedules.Where(x => x.Date == DateTime.Now.Date.AddDays(-1) && x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            if (time >= new TimeSpan(1, 0, 0, 0, 0)) return false;
            if (Yesterday == null) return true;
            if ((Today.WentToSleep > time ||  Today.WentToSleep == null) && Case == "WokeUp")
            {
                return NoWakeUpInconsistency(Yesterday);
            }
            else if(Case == "WokeUp")
            {
                return NoWentToSleepInconsistency(Yesterday);
            }
            if ((time < Today.WokeUp || Today.WokeUp == null ) && Case == "WentToSleep")
            {
                return NoWentToSleepInconsistency(Yesterday);
            }
            else if(Case == "WentToSleep")
            {
                return NoWakeUpInconsistency(Yesterday);
            }
            return true;
        }

        private bool NoWakeUpInconsistency(SleepSchedule Y)
        {
            if ((Y.WentToSleep == null && Y.WokeUp != null) || (Y.WokeUp > Y.WentToSleep))
            {
                return false;
            }
            return true;
        }

        private bool NoWentToSleepInconsistency(SleepSchedule Y)
        {
            if ((Y.WokeUp == null && Y.WentToSleep != null) || (Y.WokeUp < Y.WentToSleep))
            {
                return false;
            }
            return true;
        }

        public static List<TimeSpan?> GetSleepLog(ApplicationUser user, ApplicationDbContext dbContext, List<DateTime> dates)
        {
            List<TimeSpan?> log = new List<TimeSpan?>();
            SleepSchedule sleep;
            for (int i = 0; i < 7; i++)
            {
                sleep = dbContext.SleepSchedules.Where(x => x.User == user && x.Date == dates[i]).FirstOrDefault();
                log.Add(GetSleepTime(sleep));
            }
            return log;
        }

        public static TimeSpan? GetSleepTime(SleepSchedule sleepDay)
        {
            if (sleepDay == null || (sleepDay.WokeUp == null && sleepDay.WentToSleep == null)) return null;
            if (sleepDay.WentToSleep == null) return sleepDay.WokeUp;
            if (sleepDay.WokeUp == null) return new TimeSpan(1,0,0,0,0) - sleepDay.WentToSleep;
            if (sleepDay.WentToSleep < sleepDay.WokeUp) return sleepDay.WokeUp - sleepDay.WentToSleep;
            if (sleepDay.WentToSleep > sleepDay.WokeUp) return sleepDay.WokeUp + new TimeSpan(1, 0, 0, 0, 0) - sleepDay.WentToSleep;
            return null;
        }

        public ActionResult ChangeDay()
        {
            DateTime now = DateTime.Now;
            DateTime wrong = now.AddDays(2);
            db.Users.Where(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().LastOnline = wrong;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
