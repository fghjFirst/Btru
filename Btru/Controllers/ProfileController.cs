using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Btru.Data;
using Btru.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Btru.Controllers
{
    public class ProfileController : Controller
    {
        public static bool TimeIsUpToDate(ApplicationUser user, ApplicationDbContext db)
        {
            if (user.LastOnline == DateTime.Now.Date)
            {               
                return true;
            }
            else
            {
                user.LastOnline = DateTime.Now;
                db.SaveChanges();
                UpdateProfile(user, db);
                return false;
            }
        }

        public static bool UpdateProfile(ApplicationUser user, ApplicationDbContext db)
        {
            ReadingAssignmentsController.UpdateReading(user, db);
            SleepSchedulesController.UpdateSleep(user, db);
            return true;
        }

    }
}