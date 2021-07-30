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
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;

namespace Btru.Controllers
{
    [Authorize]
    public class ReadingAssignmentsController : Controller
    {

        private readonly ApplicationDbContext db;

        public ReadingAssignmentsController(ApplicationDbContext context)
        {
            db = context;
        }


        public ActionResult Index()
        {
            if (ProfileController.TimeIsUpToDate(db.Users.Where(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault(), db))
            {
                ViewBag.AlredyInFavorites = false;
                ViewBag.FavoriteBooks = db.FavoriteBooks.Where(x => x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Include(x => x.Book).ToList();
                var TopFavorites = (from fb in db.FavoriteBooks group fb.User by fb.Book into g orderby g.Count() select new { Book = g.Key, favorites = g.Count() }).ToList();
                TopFavorites.OrderByDescending(x => x.favorites);
                string TopFavStr = new string("");
                if (TopFavorites != null)
                {
                    for (int i = TopFavorites.Count - 1; i >= Math.Max(0, TopFavorites.Count - 3) ; i--)
                    {
                        TopFavStr += TopFavorites[i].Book.Name + " " + TopFavorites[i].Book.Author + "<br>";
                    }
                }
                ViewBag.TopFavorites = TopFavStr.Split("<br>").ToList();
                return View(db.ReadingAssignments.Where(x => x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && !x.Read).Include(x => x.Book).ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult MarkAsRead(int id)
        {
            if (db.ReadingAssignments.Where(x => x.Book.Id == id && x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault() == null)
            {
                return NotFound();
            }
            db.ReadingAssignments.Where(x => x.Book.Id == id && x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().Reading = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UnMarkAsRead(int id)
        {
            if (db.ReadingAssignments.Where(x => x.Book.Id == id && x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault() == null)
            {
                return NotFound();
            }
            db.ReadingAssignments.Where(x => x.Book.Id == id && x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().Reading = false;
            db.SaveChanges();
            FavoriteBook fb = db.FavoriteBooks.Where(x => x.Book.Id == id).FirstOrDefault();
            if (fb != null)
            {
                db.FavoriteBooks.Remove(fb);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult AddToFavorites(int id)
        {
            if (db.ReadingAssignments.Where(x => x.Book.Id == id && x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault() == null)
            {
                return NotFound();
            }
            FavoriteBook fb = new FavoriteBook();
            fb.User = db.Users.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));
            fb.Book = db.Books.Find(id);
            if (fb != null)
            {
                db.FavoriteBooks.Add(fb);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult RemoveFromFavorites(int id)
        {
            if (db.ReadingAssignments.Where(x => x.Book.Id == id && x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault() == null)
            {
                return NotFound();
            }
            FavoriteBook fb = db.FavoriteBooks.Where(x => x.Book.Id == id && x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            if (fb != null)
            {
                db.FavoriteBooks.Remove(fb);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public static bool UpdateReading(ApplicationUser user, ApplicationDbContext dbContext)
        {
            List<ReadingAssignment> RAss = dbContext.ReadingAssignments.Where(x => x.User.Id == user.Id).ToList();
            for(int i = RAss.Count - 1; i >= 0; i--)
            {
                if (RAss[i].Reading)
                {
                    RAss[i].Reading = false;
                    RAss[i].Read = true;
                    user.UniqueReads++;
                    dbContext.SaveChanges();
                }
                else if(!RAss[i].Read)
                {
                    dbContext.Remove(RAss[i]);
                    dbContext.SaveChanges();
                }
            }
            Random r = new Random();
            int total = dbContext.Books.Count();
            
            Book book;
            bool alreadyAss = false;
            if (dbContext.Books.Count() == user.UniqueReads) return true;
            for (int i = 0; i < Math.Min(5, dbContext.Books.Count() - user.UniqueReads); i++)
            {
                int element = r.Next(0, total);
                book = dbContext.Books.Skip(element).FirstOrDefault();
                if (dbContext.ReadingAssignments.ToList().Count() == 0)
                {
                    ReadingAssignment rass = new ReadingAssignment();
                    rass.Book = book;
                    rass.User = user;
                    dbContext.ReadingAssignments.Add(rass);
                    dbContext.SaveChanges();
                    i++;
                }
                foreach (ReadingAssignment ra in dbContext.ReadingAssignments.Include(x => x.Book).ToList())
                {

                    if (book.Id == ra.Book.Id && user == ra.User)
                    {
                        i--;
                        alreadyAss = true;
                        break;
                    }
                    
                }
                if (!alreadyAss)
                {
                    ReadingAssignment rass = new ReadingAssignment();
                    rass.Book = book;
                    rass.User = user;
                    dbContext.ReadingAssignments.Add(rass);
                    dbContext.SaveChanges();
                }
                alreadyAss = false;
                
            }
            return true;
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
