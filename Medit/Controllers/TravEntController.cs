using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medit.Controllers
{
    public class TravEntController : Controller
    {
        private MeditEntities db = new MeditEntities();

        //
        // GET: /TravEnt/

        public ActionResult Index()
        {
            return View(db.TravEnts.ToList());
        }

        //
        // GET: /TravEnt/Details/5

        public ActionResult Details(decimal id = 0)
        {
            TravEnt travent = db.TravEnts.Find(id);
            if (travent == null)
            {
                return HttpNotFound();
            }
            return View(travent);
        }

        //
        // GET: /TravEnt/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TravEnt/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TravEnt travent)
        {
            if (ModelState.IsValid)
            {
                db.TravEnts.Add(travent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(travent);
        }

        //
        // GET: /TravEnt/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            TravEnt travent = db.TravEnts.Find(id);
            if (travent == null)
            {
                return HttpNotFound();
            }
            return View(travent);
        }

        //
        // POST: /TravEnt/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TravEnt travent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(travent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(travent);
        }

        //
        // GET: /TravEnt/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            TravEnt travent = db.TravEnts.Find(id);
            if (travent == null)
            {
                return HttpNotFound();
            }
            return View(travent);
        }

        //
        // POST: /TravEnt/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            TravEnt travent = db.TravEnts.Find(id);
            db.TravEnts.Remove(travent);
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