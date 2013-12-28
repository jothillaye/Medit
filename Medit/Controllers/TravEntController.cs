using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
            var langue = (
                from entreprise in db.Entreprises
                join lang in db.Langues on entreprise.Id_Langue equals lang.Id_Langue
                where entreprise.Numero == id
                select new { lang.Id_Langue, lang.Libelle })
            .ToList();
            
            decimal id_langue;
            if (langue.Count > 0)
                id_langue = langue.First().Id_Langue;
            else  //TODO better way to do this ?
                id_langue = 0;

            List<LangueProfession> listProfessions = ListProfessions(id_langue);
            
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

        public string setSoumis(TravEnt travent)
        {
            if (travent.Travailleur_Soumis != null)
                return "Oui";
            else
                return "Non";
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

        
        //
        // GET: /TravEnt/
        public ActionResult Index()
        {
            var listTravEnt = db.TravEnts.ToList();
            foreach(var travent in listTravEnt)
            {
                travent.isSoumis = setSoumis(travent);
            }
            return View(listTravEnt);
        }

        // GET: /TravEnt/Details/5
        public ActionResult Details(decimal id = 0)
        {
            TravEnt travent = db.TravEnts.Find(id);
            if (travent == null)
            {
                return HttpNotFound();
            }
            travent.isSoumis = setSoumis(travent);
            return View(travent);
        }

        // GET: /TravEnt/Create
        public ActionResult Create()
        {
            setLists();
            return View();
        }

        public List<TravEnt> TravEntAlreadyExist(TravEnt travent)
        {
            List<TravEnt> listTravEnt = db.TravEnts.ToArray()
                .Where(te => 
                    te.Id_Travailleur == travent.Id_Travailleur &&
                    te.DateEntree == travent.DateEntree
                )
                .Select(te => new TravEnt
                {
                    Id_TravEnt = te.Id_TravEnt
                })
                .ToList();
            return listTravEnt;
        }

        // POST: /TravEnt/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TravEnt travent)
        {
            if (ModelState.IsValid)
            {
                List<TravEnt> listTravEnt = TravEntAlreadyExist(travent);
                if (listTravEnt.Count() > 0)
                {
                    ViewBag.ErrorField = "Lien déjà existant pour cette date.";     
                }
                else 
                {
                    if (travent.DateSortie != null && (travent.DateEntree.Date > travent.DateSortie.Value.Date))
                    {
                        ViewBag.ErrorField = "La date de sortie doit être plus grande que la date d'entrée.";
                    }
                    else
                    {
                        db.TravEnts.Add(travent);
                        db.SaveChanges();

                        if (travent.isSoumis.CompareTo("Oui") == 0)
                        {
                            Travailleur_Soumis travSoum = new Travailleur_Soumis();
                            travSoum.Id_TravEnt = travent.Id_TravEnt;
                            db.Travailleur_Soumis.Add(travSoum);
                        }
                        else
                        {
                            Travailleur_NonSoumis travNonSoum = new Travailleur_NonSoumis();
                            travNonSoum.Id_TravEnt = travent.Id_TravEnt;
                            db.Travailleur_NonSoumis.Add(travNonSoum);
                        }
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
            }

            setLists();
            return View();
        }

        // GET: /TravEnt/Edit/5
        public ActionResult Edit(decimal id = 0)
        {
            TravEnt travent = db.TravEnts.Find(id);
            if (travent == null)
            {
                return HttpNotFound();
            }
            travent.isSoumis = setSoumis(travent);
            setLists(id);
            return View(travent);
        }

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
            setLists();
            return View(travent);
        }

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

        // POST: /TravEnt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            TravEnt travent = db.TravEnts.Find(id);
            
            if (travent.Travailleur_Soumis != null)
            {
                Travailleur_Soumis travSoum = db.Travailleur_Soumis.Find(id);
                db.Travailleur_Soumis.Remove(travSoum);
            }
            else if(travent.Travailleur_NonSoumis != null)
            {
                Travailleur_NonSoumis travNonSoum = db.Travailleur_NonSoumis.Find(id);
                db.Travailleur_NonSoumis.Remove(travNonSoum);
            }

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