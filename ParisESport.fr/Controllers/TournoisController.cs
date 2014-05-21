using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParisESport.fr.Models;

namespace ParisESport.fr.Controllers
{
    public class TournoisController : Controller
    {
        private paris_e_sportEntities db = new paris_e_sportEntities();

        //
        // GET: /Tournois/

        public ActionResult Index()
        {
            var tournois = db.Tournois.Include(t => t.Jeux).Include(t => t.Matchs);
            return View(tournois.ToList());
        }

        //
        // GET: /Tournois/Details/5

        public ActionResult Details(int id = 0)
        {
            Tournois tournois = db.Tournois.Find(id);
            if (tournois == null)
            {
                return HttpNotFound();
            }
            return View(tournois);
        }

       
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}