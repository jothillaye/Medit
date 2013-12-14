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
        [HttpGet]
        public JsonResult Translate(int id)
        {
            var professions = (
                from prof in db.Professions
                join langProf in db.LangueProfessions on prof.Code equals langProf.Code
                join lang in db.Langues on langProf.Id_Langue equals lang.Id_Langue
                where lang.Id_Langue == (decimal)id
                select new { prof.Code, langProf.Denomination })
            .ToList();

            return Json(new { professions }, JsonRequestBehavior.AllowGet);
        }

        private MeditEntities db = new MeditEntities();

        //
        // GET: /TravEnt/

        public ActionResult Index()
        {
            //var TravEntList = (
            //    from travent in db.TravEnts
            //    join langProf in db.LangueProfessions on travent.Code_Profession equals langProf.Code
            //    join lang in db.Langues on langProf.Id_Langue equals lang.Id_Langue
            //    where lang.Id_Langue == 0
            //    join travailleur in db.Travailleurs on travent.Id_Travailleur equals travailleur.Id_Travailleur
            //    join entreprise in db.Entreprises on travent.Numero_Entreprise equals entreprise.Numero
            //    select new { Id_Travailleur = (travailleur.Prenom + " " + travailleur.Nom), Numero_Entreprise = entreprise.Denomination, Code_Profession = langProf.Denomination, travent.DateEntree, travent.DateSortie, travent.Interlocuteur})
            //.ToList();

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
            var travailleurs =
                db.Travailleurs.ToArray()
                    .Select(t => new
                    {
                        Id_Travailleur = t.Id_Travailleur,
                        Name = string.Format("{0} {1}", t.Prenom, t.Nom)
                    })
                .ToList();
            ViewBag.Id_Travailleur = new SelectList(travailleurs, "Id_Travailleur", "Name");

            ViewBag.Numero_Entreprise = new SelectList(db.Entreprises, "Numero", "Denomination");
            
            var langues = (
                from lang in db.Langues
                select new { lang.Id_Langue, lang.Libelle })
                .ToList();
            ViewBag.Id_Langue = new SelectList(langues, "Id_Langue", "Libelle");

            decimal idLang = langues.First().Id_Langue;

            var professions = (
                from prof in db.Professions
                join langProf in db.LangueProfessions on prof.Code equals langProf.Code
                join lang in db.Langues on langProf.Id_Langue equals lang.Id_Langue
                where lang.Id_Langue == idLang
                select new { prof.Code, langProf.Denomination })
                .ToList();
            ViewBag.Code_Profession = new SelectList(professions, "Code", "Denomination");
            
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

            var travailleurs =
                db.Travailleurs.ToArray()
                    .Select(t => new
                    {
                        Id_Travailleur = t.Id_Travailleur,
                        Name = string.Format("{0} {1}", t.Prenom, t.Nom)
                    })
                .ToList();
            ViewBag.Id_Travailleur = new SelectList(travailleurs, "Id_Travailleur", "Name");

            ViewBag.Numero_Entreprise = new SelectList(db.Entreprises, "Numero", "Denomination");

            var langues = (
                from lang in db.Langues
                select new { lang.Id_Langue, lang.Libelle })
                .ToList();
            ViewBag.Id_Langue = new SelectList(langues, "Id_Langue", "Libelle");

            decimal idLang = langues.First().Id_Langue;

            var professions = (
                from prof in db.Professions
                join langProf in db.LangueProfessions on prof.Code equals langProf.Code
                join lang in db.Langues on langProf.Id_Langue equals lang.Id_Langue
                where lang.Id_Langue == idLang
                select new { prof.Code, langProf.Denomination })
                .ToList();
            ViewBag.Code_Profession = new SelectList(professions, "Code", "Denomination");

            return View();
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