using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FilmKupriyanov.Models;
using Microsoft.AspNet.Identity;
using X.PagedList;

namespace FilmKupriyanov.Controllers
{
    public class FilmsController : Controller
    {
        private static readonly HashSet<String> AllowedExtensions = new HashSet<String> { ".jpg", ".jpeg", ".png", ".gif" };
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Films
        public async Task<ActionResult> Index(int? page)
        {
            int pageSize = 3;
            int pageNumber = page ?? 1;
            var films = db.Films.Include(f => f.CreatorUser).OrderBy(f => f.Year).ToPagedList(pageNumber, pageSize);
            return View(films);
        }

        // GET: Films/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = await db.Films.FindAsync(id);
            film.CreatorUser = db.Users.Find(film.CreatorUserId);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // GET: Films/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new FilmView());
        }

        // POST: Films/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FilmView filmView)
        {
            if (ModelState.IsValid && User.Identity.GetUserId() != null)
            {
                var Film = new Film
                {
                    Id = Guid.NewGuid(),
                    CreatorUserId = User.Identity.GetUserId(),
                    Description = filmView.Description,
                    Director = filmView.Director,
                    Name = filmView.Name,
                    Year = filmView.Year
                };
                if (filmView.Poster != null && filmView.Poster.ContentLength > 0)
                {
                    string FileName = Path.GetFileNameWithoutExtension(filmView.Poster.FileName);
                    string FileExtension = Path.GetExtension(filmView.Poster.FileName);
                    if (!AllowedExtensions.Contains(FileExtension))
                    {
                        ViewBag.CreatorUserId = User.Identity.GetUserId();
                        return View(filmView);
                    }
                    FileName = DateTime.Now.ToString("yyyy-MM-dd") + Guid.NewGuid() + FileExtension;
                    string UploadPath = Path.Combine(Server.MapPath("~/Images"), FileName);
                    filmView.Poster.SaveAs(UploadPath);
                    Film.Poster = $"/Images/{FileName}";
                }

                db.Films.Add(Film);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(filmView);
        }

        // GET: Films/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = await db.Films.FindAsync(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            if (film.CreatorUserId != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilmView filmView = new FilmView
            {
                Name = film.Name,
                Description = film.Description,
                Year = film.Year,
                Director = film.Director,
                ImagePath = film.Poster
            };
            ViewData["FilmId"] = film.Id;
            return View(filmView);
        }

        // POST: Films/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FilmView filmView, Guid id)
        {
            if (ModelState.IsValid)
            {
                Film film = await db.Films.FindAsync(id);
                if (film.CreatorUserId != User.Identity.GetUserId())
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (filmView.Poster != null && filmView.Poster.ContentLength > 0 && film.Poster == null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(filmView.Poster.FileName);
                    string FileExtension = Path.GetExtension(filmView.Poster.FileName);
                    if (!AllowedExtensions.Contains(FileExtension))
                    {
                        ViewBag.CreatorUserId = User.Identity.GetUserId();
                        return View(filmView);
                    }
                    FileName = DateTime.Now.ToString("yyyy-MM-dd") + Guid.NewGuid() + FileExtension;
                    string UploadPath = Path.Combine(Server.MapPath("~/Images"), FileName);
                    filmView.Poster.SaveAs(UploadPath);
                    filmView.ImagePath = $"/Images/{FileName}";
                }
                film.Name = filmView.Name;
                film.Description = filmView.Description;
                film.Director = filmView.Director;
                film.Year = filmView.Year;
                if (filmView.ImagePath != null)
                {
                    film.Poster = filmView.ImagePath;
                }
                db.Entry(film).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(filmView);
        }

        // GET: Films/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = await db.Films.FindAsync(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            if (film.CreatorUserId != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Film film = await db.Films.FindAsync(id);
            if (film.CreatorUserId != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Films.Remove(film);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Films/PosterDelete/5
        public async Task<ActionResult> PosterDelete(Guid? filmId)
        {
            if (filmId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = await db.Films.FindAsync(filmId);
            if (film == null)
            {
                return HttpNotFound();
            }
            if (film.CreatorUserId != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            film.CreatorUser = db.Users.Find(film.CreatorUserId);
            return View(film);
        }

        // POST: Films/PosterDelete/5
        [HttpPost, ActionName("PosterDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePosterConfirmed(Guid filmId)
        {
            Film film = await db.Films.FindAsync(filmId);
            if (film.CreatorUserId != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (film.Poster == null)
            {
                return RedirectToAction("Details", new { id = film.Id });
            }
            var strFile = Server.MapPath("~" + film.Poster);
            if (System.IO.File.Exists(strFile))
            {
                System.IO.File.Delete(strFile);
            }

            film.Poster = null;
            db.Entry(film).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new{id = film.Id});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
