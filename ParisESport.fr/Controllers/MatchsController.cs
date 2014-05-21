using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using ParisESport.fr.Models;

namespace ParisESport.fr.Controllers
{
    public class MatchsController : Controller
    {
        private paris_e_sportEntities db = new paris_e_sportEntities();

        //
        // GET: /Matchs/

        public ActionResult Index()
        {
            ViewData["equipes"] = from m in db.Equipes
                                  select m;
            Dictionary<String, String> matchAAficher = new Dictionary<String, String>();
            //foreach (Matchs match in db.Matchs.ToList())

            ViewData["equipes"] = matchAAficher;


            var evenement = db.Matchs.Include(e => e.Equipes);
            return View(evenement.ToList());
            //return View(db.Matchs.ToList());
            //return View();
        }

        //
        // GET: /Matchs/Details/5

        public ActionResult Details(int id = 0)
        {
            Matchs matchs = db.Matchs.Find(id);
            if (matchs == null)
            {
                return HttpNotFound();
            }
            return View(matchs);
        }

        public PartialViewResult Parier(int id)
        {
            return PartialView("Parier", db.Matchs.Where(c => c.Id == id).First());
        }

        public ActionResult ParisValide(string valCred, string eqParis, int idmatch)
        {
            int resparie = 0;
            if (db.Matchs.Where(c => c.Id == idmatch).First().Equipes.nom == eqParis)
            {
                resparie = 0;
            }
            else if (db.Matchs.Where(c => c.Id == idmatch).First().Equipes1.nom == eqParis)
            {
                resparie = 2;
            }

            db.paris_user.Add(new paris_user()
            {
                match_id = idmatch,
                user_id = db.UserProfile.Where(c => c.UserName == WebSecurity.CurrentUserName).First().UserId,
                credit_parie = Convert.ToDouble(valCred),
                resultat_parie = resparie
            });
            db.SaveChanges();

            db.UserProfile.Where(c => c.UserName == WebSecurity.CurrentUserName).First().User_infos.First().credits -= Convert.ToDouble(valCred);
            db.SaveChanges();
            return View("Index", db.Matchs.Where(c => c.resultat == 1).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}