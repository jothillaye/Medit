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

        // Get list profession in a specific Id_Langue
        // GET: /TravEnt/Translate/5
        [HttpGet]
        public JsonResult TranslateProfessions(int id)
        {
            decimal id_entreprise = id;

            var langue = (
                from entreprise in db.Entreprises
                join lang in db.Langues on entreprise.Id_Langue equals lang.Id_Langue
                where entreprise.Numero == id_entreprise
                select new { lang.Id_Langue, lang.Libelle })
            .ToList();
            
            decimal id_langue;
            if (langue.Count > 0)
                id_langue = langue.First().Id_Langue;
            else  //TODO better way to do this ?
                id_langue = 0;

            var professions = (
                from langProf in db.LangueProfessions
                where langProf.Id_Langue == id_langue
                select new { langProf.Code, langProf.Denomination })
            .ToList();

            return Json(new { langue, professions }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FilterEntreprises(String filter) { 
            var filteredEntreprises = db.Entreprises.ToArray()
                .Where(ent => ent.Denomination.ToLower().Contains(filter.ToLower()))
                .Select(ent => new 
                {
                    Numero_Entreprise = ent.Numero,
                    Denomination = ent.Denomination
                })
            .ToList();
            return Json(new {filteredEntreprises}, JsonRequestBehavior.AllowGet);
        }

        // Set lists without selected values
        public void setLists() 
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

            decimal id_lang = langues.First().Id_Langue;

            var professions = (
                from prof in db.Professions
                join langProf in db.LangueProfessions on prof.Code equals langProf.Code
                join lang in db.Langues on langProf.Id_Langue equals lang.Id_Langue
                where lang.Id_Langue == id_lang
                select new { prof.Code, langProf.Denomination })
            .ToList();
            ViewBag.Code_Profession = new SelectList(professions, "Code", "Denomination");
        }

        // Set lists with selected values
        public void setLists(decimal idTravEnt) 
        {
            TravEnt travent = db.TravEnts.Find(idTravEnt);
            if (travent == null)
            {
                setLists();
            }
            else
            {
                var travailleurs =
                db.Travailleurs.ToArray()
                    .Select(t => new
                    {
                        Id_Travailleur = t.Id_Travailleur,
                        Name = string.Format("{0} {1}", t.Prenom, t.Nom)
                    })
                .ToList();
                ViewBag.Id_Travailleur = new SelectList(travailleurs, "Id_Travailleur", "Name", travent.Id_Travailleur);

                ViewBag.Numero_Entreprise = new SelectList(db.Entreprises, "Numero", "Denomination", travent.Numero_Entreprise);

                decimal id_lang = travent.Entreprise.Id_Langue;

                var langues = (
                    from lang in db.Langues
                    select new { lang.Id_Langue, lang.Libelle })
                .ToList();
                ViewBag.Id_Langue = new SelectList(langues, "Id_Langue", "Libelle", id_lang);

                var professions = (
                    from prof in db.Professions
                    join langProf in db.LangueProfessions on prof.Code equals langProf.Code
                    join lang in db.Langues on langProf.Id_Langue equals lang.Id_Langue
                    where lang.Id_Langue == id_lang
                    select new { prof.Code, langProf.Denomination })
                .ToList();
                ViewBag.Code_Profession = new SelectList(professions, "Code", "Denomination", travent.Code_Profession);
            }
        }


        // GET: /TravEnt/
        public ActionResult Index()
        {
            return View(db.TravEnts.ToList());
        }

        // GET: /TravEnt/Details/5
        public ActionResult Details(decimal idTravEnt = 0)
        {
            TravEnt travent = db.TravEnts.Find(idTravEnt);
            if (travent == null)
            {
                return HttpNotFound();
            }
            return View(travent);
        }

        // GET: /TravEnt/Create
        public ActionResult Create()
        {
            setLists();
            return View();
        }

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

            setLists();
            return View();
        }

        // GET: /TravEnt/Edit/5
        public ActionResult Edit(decimal idTravEnt = 0)
        {
            TravEnt travent = db.TravEnts.Find(idTravEnt);
            if (travent == null)
            {
                return HttpNotFound();
            }

            setLists(idTravEnt);
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

        public ActionResult Delete(decimal idTravEnt = 0)
        {
            TravEnt travent = db.TravEnts.Find(idTravEnt);
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
        public ActionResult DeleteConfirmed(decimal idTravEnt)
        {
            TravEnt travent = db.TravEnts.Find(idTravEnt);
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