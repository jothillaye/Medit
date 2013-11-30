using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medit.Controllers
{
    public class TravailleurController : Controller
    {
        private MeditEntities db = new MeditEntities();

        //
        // GET: /Travailleur/

        public ActionResult Index()
        {
            var travailleurs = db.Travailleurs.Include(t => t.CodePostal);
            return View(travailleurs.ToList());
        }

        //
        // GET: /Travailleur/Details/5

        public ActionResult Details(decimal id = 0)
        {
            Travailleur travailleur = db.Travailleurs.Find(id);
            if (travailleur == null)
            {
                return HttpNotFound();
            }
            return View(travailleur);
        }

        //
        // GET: /Travailleur/Create

        public ActionResult Create()
        {
            ViewBag.Id_CodePostal = new SelectList(db.CodePostals, "Id_CodePostal", "Localite");
            return View();
        }

        //
        // POST: /Travailleur/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Travailleur travailleur)
        {
            if (ModelState.IsValid)
            {
                db.Travailleurs.Add(travailleur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_CodePostal = new SelectList(db.CodePostals, "Id_CodePostal", "Localite", travailleur.Id_CodePostal);
            return View(travailleur);
        }

        //
        // GET: /Travailleur/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            Travailleur travailleur = db.Travailleurs.Find(id);
            if (travailleur == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_CodePostal = new SelectList(db.CodePostals, "Id_CodePostal", "Localite", travailleur.Id_CodePostal);
            return View(travailleur);
        }

        //
        // POST: /Travailleur/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Travailleur travailleur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(travailleur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_CodePostal = new SelectList(db.CodePostals, "Id_CodePostal", "Localite", travailleur.Id_CodePostal);
            return View(travailleur);
        }

        //
        // GET: /Travailleur/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            Travailleur travailleur = db.Travailleurs.Find(id);
            if (travailleur == null)
            {
                return HttpNotFound();
            }
            return View(travailleur);
        }

        //
        // POST: /Travailleur/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Travailleur travailleur = db.Travailleurs.Find(id);
            db.Travailleurs.Remove(travailleur);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}