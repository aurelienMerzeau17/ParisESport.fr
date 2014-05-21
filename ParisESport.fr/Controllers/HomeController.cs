using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParisESport.fr.Models;

namespace ParisESport.fr.Controllers
{
    public class HomeController : Controller
    {
        private paris_e_sportEntities db = new paris_e_sportEntities();

        public ActionResult Index()
        {
            return View(db.Matchs.Where(c=>c.resultat == 1).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}
