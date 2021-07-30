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
    public class FavoriteBooksController : Controller
    {
        private readonly ApplicationDbContext db;

        public FavoriteBooksController(ApplicationDbContext context)
        {
            db = context;
        }

        public ActionResult Index()
        {
                return View(db.FavoriteBooks.Where(x => x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Include(x => x.Book).ToList());
        }

        public ActionResult RemoveFromFavorites(int id)
        {
            if (db.FavoriteBooks.Find(id) == null)
            {
                return NotFound();
            }
            FavoriteBook fb = db.FavoriteBooks.Where(x => x.Id == id ).Include(x => x.Book).FirstOrDefault();
            return View(fb);
        }

        [HttpPost,ActionName("RemoveFromFavorites")]
        public ActionResult RemovalConfirmed(int id)
        {
            FavoriteBook fb = db.FavoriteBooks.Where(x => x.Id == id).FirstOrDefault();
            if (fb != null)
            {
                db.FavoriteBooks.Remove(fb);
                db.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
    }
}
