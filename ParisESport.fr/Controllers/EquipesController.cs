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
    public class EquipesController : Controller
    {
        private paris_e_sportEntities db = new paris_e_sportEntities();

        //
        // GET: /Equipes/

        public ActionResult Index()
        {
            return View(db.Equipes.ToList());
        }

        //
        // GET: /Equipes/Details/5

        public ActionResult Details(int id = 0)
        {
            Equipes equipes = db.Equipes.Find(id);
            if (equipes == null)
            {
                return HttpNotFound();
            }
            return View(equipes);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}