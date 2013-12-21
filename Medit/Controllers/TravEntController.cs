using System;
using System.Collections;
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

        public List<Travailleur> ListTravailleurs(string filter = null)
        {
            List<Travailleur> listTrav = db.Travailleurs.ToArray()
                .Select(t => new Travailleur
                {
                    Id_Travailleur = t.Id_Travailleur,
                    Nom = t.Nom,
                    Prenom = t.Prenom
                })
                .OrderBy(t => t.Nom)
                .ToList();
            
            if(filter != null)
            {
                listTrav = listTrav.Where(trav => trav.Nom.ToLower().Contains(filter.ToLower()) || trav.Prenom.ToLower().Contains(filter.ToLower())).ToList(); 
            }

            return listTrav;
        }

        public List<Langue> ListLangues()
        {
            return db.Langues.ToArray()
                .Select(l => new Langue
                    {
                        Id_Langue = l.Id_Langue,
                        Libelle = l.Libelle
                    })
                .OrderBy(l => l.Libelle)
                .ToList();
        }

        public List<LangueProfession> ListProfessions(decimal id_langue)
        {
            return db.LangueProfessions.ToArray()
                .Select(lP => new LangueProfession
                    {
                        Id_Langue = lP.Id_Langue,
                        Code = lP.Code,
                        Denomination = lP.Denomination
                    })
                .Where(lP => lP.Id_Langue == id_langue)
                .OrderBy(lP => lP.Denomination)
                .ToList();
        }

        public List<Entreprise> ListEntreprises(string filter = null)
        {
            List<Entreprise> listEntreprises = db.Entreprises.ToArray()
                .Select(e => new Entreprise
                {
                    Numero = e.Numero,
                    Denomination = e.Denomination
                })
                .OrderBy(e => e.Denomination)
                .ToList();

            if (filter != null) 
            { 
                listEntreprises = listEntreprises.Where(ent => ent.Denomination.ToLower().Contains(filter.ToLower())).ToList();
            }

            return listEntreprises;
        }

        // Get list profession in a specific Langage
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

            IList listProfessions = ListProfessions(id_langue);
            
            return Json(new { langue, listProfessions }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FilterEntreprises(String filter) 
        {             
            return Json(new {filteredEntreprises = ListEntreprises(filter)}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FilterTravailleurs(String filter)
        {
            return Json(new { filteredTravailleurs = ListTravailleurs(filter)}, JsonRequestBehavior.AllowGet);
        }

        // Set lists without selected values
        public void setLists()
        {
            ViewBag.Id_Travailleur = new SelectList(ListTravailleurs(), "Id_Travailleur", "NomPre");

            ViewBag.Numero_Entreprise = new SelectList(ListEntreprises(), "Numero", "Denomination");

            ViewBag.Id_Langue = new SelectList(ListLangues(), "Id_Langue", "Libelle");

            decimal id_lang = ListLangues().First().Id_Langue;

            ViewBag.Code_Profession = new SelectList(ListProfessions(id_lang), "Code", "Denomination");
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
                ViewBag.Id_Travailleur = new SelectList(ListTravailleurs(), "Id_Travailleur", "NomPre", travent.Id_Travailleur);

                ViewBag.Numero_Entreprise = new SelectList(ListEntreprises(), "Numero", "Denomination", travent.Numero_Entreprise);

                decimal id_lang = travent.Entreprise.Id_Langue;
                
                ViewBag.Id_Langue = new SelectList(ListLangues(), "Id_Langue", "Libelle", id_lang);
                
                ViewBag.Code_Profession = new SelectList(ListProfessions(id_lang), "Code", "Denomination", travent.Code_Profession);
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