using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medit.Controllers
{
    public class EntrepriseController : Controller
    {
        private MeditEntities db = new MeditEntities();

        //
        // GET: /Entreprise/

        public ActionResult Index()
        {
            var entreprises = db.Entreprises.Include(e => e.CodePostal).Include(e => e.Langue);
            return View(entreprises.ToList());
        }
    }
}