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
    public class ReadBooksController : Controller
    {
        private readonly ApplicationDbContext db;

        public ReadBooksController(ApplicationDbContext context)
        {
            db = context;
        }

        public ActionResult Index()
        {
            ViewBag.alreadyInFavorites = false;
            ViewBag.FavoriteBooks = db.FavoriteBooks.Where(x => x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Include(x => x.Book).ToList();
            return View(db.ReadingAssignments.Where(x => x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && x.Read).Include(x => x.Book).ToList());
        }

        public ActionResult RemoveFromRead(int id)
        {
            if (db.ReadingAssignments.Find(id) == null)
            {
                return NotFound();
            }
            ReadingAssignment ra = db.ReadingAssignments.Where(x => x.Id == id).Include(x => x.Book).FirstOrDefault();
            return View(ra);
        }

        [HttpPost, ActionName("RemoveFromRead")]
        public ActionResult RemovalConfirmed(int id)
        {
            ReadingAssignment ra = db.ReadingAssignments.Where(x => x.Id == id).Include(x => x.Book).FirstOrDefault();
            FavoriteBook fb = db.FavoriteBooks.Where(x => x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && ra.Book == x.Book).FirstOrDefault();
            db.Users.Where(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().UniqueReads--;
            if (ra != null)
            {
                db.ReadingAssignments.Remove(ra);
                db.SaveChanges();
            }
            else
            {
                return NotFound();
            }
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

    }
}
