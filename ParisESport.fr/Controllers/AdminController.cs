using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using WebMatrix.WebData;

using ParisESport.fr.Models;

namespace ParisESport.fr.Controllers
{
    public class AdminController : Controller
    {
        private paris_e_sportEntities db = new paris_e_sportEntities();
        public List<Equipes> listeEquipes = new List<Equipes>();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                bool isAdmin = (from i in db.UserProfile
                                where i.UserName == WebSecurity.CurrentUserName
                                select i.User_infos.FirstOrDefault().admin).FirstOrDefault(); ;


                if (isAdmin == true && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                {
                    return RedirectToLocal("Admin/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Vous n'êtes pas Administrateur.");
                    return View(model);
                }
            }

            // Si nous sommes arrivés là, quelque chose a échoué, réafficher le formulaire
            ModelState.AddModelError("", "Le nom d'utilisateur ou mot de passe fourni est incorrect.");
            return View(model);
        }

        #region Applications auxiliaires
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult IndexEquipes()
        {
            return View("IndexEquipes", db.Equipes.ToList());
        }


        public ActionResult DetailsEquipes(int id = 0)
        {
            Equipes equipes = db.Equipes.Find(id);
            if (equipes == null)
            {
                return HttpNotFound();
            }
            return View("DetailsEquipes", equipes);
        }


        public ActionResult CreateEquipes()
        {
            return View("CreateEquipes");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEquipes(Equipes equipes)
        {
            if (ModelState.IsValid)
            {
                db.Equipes.Add(equipes);
                db.SaveChanges();
                return RedirectToAction("IndexEquipes");
            }

            return View("CreateEquipes", equipes);
        }


        public ActionResult EditEquipes(int id = 0)
        {
            Equipes equipes = db.Equipes.Find(id);
            if (equipes == null)
            {
                return HttpNotFound();
            }
            return View("EditEquipes", equipes);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEquipes(Equipes equipes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexEquipes");
            }
            return View("EditEquipes", equipes);
        }


        public ActionResult DeleteEquipes(int id = 0)
        {
            Equipes equipes = db.Equipes.Find(id);
            if (equipes == null)
            {
                return HttpNotFound();
            }
            return View("DeleteEquipes", equipes);
        }


        [HttpPost, ActionName("DeleteEquipes")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEquipesConfirmed(int id)
        {
            Equipes equipes = db.Equipes.Find(id);
            db.Equipes.Remove(equipes);
            db.SaveChanges();
            return RedirectToAction("IndexEquipes");
        }






        public ActionResult IndexJeux()
        {
            return View("IndexJeux", db.Jeux.ToList());
        }


        public ActionResult DetailsJeux(int id = 0)
        {
            Jeux jeux = db.Jeux.Find(id);
            if (jeux == null)
            {
                return HttpNotFound();
            }
            return View("IndexJeux", jeux);
        }


        public ActionResult CreateJeux()
        {
            return View("CreateJeux");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateJeux(Jeux jeux)
        {
            if (ModelState.IsValid)
            {
                db.Jeux.Add(jeux);
                db.SaveChanges();
                return RedirectToAction("IndexJeux");
            }

            return View("CreateJeux", jeux);
        }


        public ActionResult EditJeux(int id = 0)
        {
            Jeux jeux = db.Jeux.Find(id);
            if (jeux == null)
            {
                return HttpNotFound();
            }
            return View("EditJeux", jeux);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditJeux(Jeux jeux)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jeux).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexJeux");
            }
            return View("EditJeux", jeux);
        }


        public ActionResult DeleteJeux(int id = 0)
        {
            Jeux jeux = db.Jeux.Find(id);
            if (jeux == null)
            {
                return HttpNotFound();
            }
            return View("DeleteJeux", jeux);
        }


        [HttpPost, ActionName("DeleteJeux")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteJeuxConfirmed(int id)
        {
            Jeux jeux = db.Jeux.Find(id);
            db.Jeux.Remove(jeux);
            db.SaveChanges();
            return RedirectToAction("IndexJeux");
        }



        public ActionResult IndexTournois()
        {
            var tournois = db.Tournois.Include(t => t.Jeux).Include(t => t.Matchs);
            return View("IndexTournois", tournois.ToList());
        }

        //
        // GET: /admTounois/Details/5

        public ActionResult DetailsTournois(int id = 0)
        {
            Tournois tournois = db.Tournois.Find(id);
            if (tournois == null)
            {
                return HttpNotFound();
            }
            return View("DetailsTournois", tournois);
        }

        //
        // GET: /admTounois/Create

        public ActionResult CreateTournois()
        {
            ViewBag.jeu_id = new SelectList(db.Jeux, "Id", "nom");
            ViewBag.match_id = new SelectList(db.Matchs, "Id", "Id");
            ViewBag.lstEquipes = null;
            Session["SessionEquipes"] = null;
            return View("CreateTournois");
        }

        //
        // POST: /admTounois/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTournois(Tournois tournois)
        {            
            ViewBag.lstEquipes = Session["SessionEquipes"];
            foreach (Equipes e in ViewBag.lstEquipes)
            {
                listeEquipes.Add(e);
            }           
            Equipes equ = new Equipes();
            foreach (Equipes idr in listeEquipes)
            {
                equ = (from r in db.Equipes
                       where idr.Id == r.Id
                       select r).FirstOrDefault();

                tournois.Equipes.Add(equ);
            }

            if (ModelState.IsValid)
            {
                db.Tournois.Add(tournois);
                db.SaveChanges();
                return RedirectToAction("IndexTournois");
            }

            ViewBag.jeu_id = new SelectList(db.Jeux, "Id", "nom", tournois.jeu_id);
            ViewBag.match_id = new SelectList(db.Matchs, "Id", "Id", tournois.Matchs);
            return View("CreateTournois", tournois);
        }

        //
        // GET: /admTounois/Edit/5

        public ActionResult EditTournois(int id = 0)
        {
            Tournois tournois = db.Tournois.Find(id);
            if (tournois == null)
            {
                return HttpNotFound();
            }
            //ViewBag.lstEquipes = null;
            //Session["SessionEquipes"] = null;
            ViewBag.jeu_id = new SelectList(db.Jeux, "Id", "nom", tournois.jeu_id);
            ViewBag.match_id = new SelectList(db.Matchs, "Id", "Id", tournois.Matchs);
            return View("EditTournois", tournois);
        }

        //
        // POST: /admTounois/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTournois(Tournois tournois)
        {
            tournois.Equipes.Clear();
            ViewBag.lstEquipes = Session["SessionEquipes"];

            foreach(Equipes e in ViewBag.lstEquipes)
            {
                listeEquipes.Add(e);
            }
            Equipes equ = new Equipes();
            foreach (Equipes idr in listeEquipes)
            {
                equ = (from r in db.Equipes
                       where idr.Id == r.Id
                       select r).FirstOrDefault();

                tournois.Equipes.Add(equ);
            }

            if (ModelState.IsValid)
            {
                db.Entry(tournois).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexTournois");
            }
            ViewBag.jeu_id = new SelectList(db.Jeux, "Id", "nom", tournois.jeu_id);
            ViewBag.match_id = new SelectList(db.Matchs, "Id", "Id", tournois.Matchs);
            return View("EditTournois", tournois);
        }

        //
        // GET: /admTounois/Delete/5

        public ActionResult DeleteTournois(int id = 0)
        {
            Tournois tournois = db.Tournois.Find(id);
            if (tournois == null)
            {
                return HttpNotFound();
            }
            return View("DeleteTournois", tournois);
        }

        //
        // POST: /admTounois/Delete/5

        [HttpPost, ActionName("DeleteTournois")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTournoisConfirmed(int id)
        {
            Tournois tournois = db.Tournois.Find(id);
            db.Tournois.Remove(tournois);
            db.SaveChanges();
            return RedirectToAction("IndexTournois");
        }




        public ActionResult IndexMatchs()
        {
            var matchs = db.Matchs.Include(m => m.Equipes).Include(m => m.Equipes1);
            return View("IndexMatchs", matchs.ToList());
        }

        //
        // GET: /admMatchs/Details/5

        public ActionResult DetailsMatchs(int id = 0)
        {
            Matchs matchs = db.Matchs.Find(id);
            if (matchs == null)
            {
                return HttpNotFound();
            }
            return View("DetailsMatchs", matchs);
        }

        //
        // GET: /admMatchs/Create

        public ActionResult CreateMatchs(int id)
        {
            ViewBag.id_Eq1 = new SelectList(db.Tournois.Where(c => c.Id == id).First().Equipes.ToList(), "Id", "nom");
            ViewBag.id_Eq2 = new SelectList(db.Tournois.Where(c => c.Id == id).First().Equipes.ToList(), "Id", "nom");

            Session["idTournoi"] = id;
            return View("CreateMatchs");
        }

        //
        // POST: /admMatchs/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMatchs(Matchs matchs, int idtournoi)
        {
            matchs.tournoi_id = idtournoi;
            matchs.resultat = 1;
            if (ModelState.IsValid)
            {

                db.Matchs.Add(matchs);
                db.SaveChanges();
                return RedirectToAction("IndexMatchs");
            }

            ViewBag.id_Eq1 = new SelectList(db.Tournois.Where(c => c.Id == idtournoi).First().Equipes.ToList(), "Id", "nom", matchs.id_Eq1);
            ViewBag.id_Eq2 = new SelectList(db.Tournois.Where(c => c.Id == idtournoi).First().Equipes.ToList(), "Id", "nom", matchs.id_Eq2);
            return View("CreateMatchs", matchs);
        }

        //
        // GET: /admMatchs/Edit/5

        public ActionResult EditMatchs(int id = 0)
        {
            Matchs matchs = db.Matchs.Find(id);
            if (matchs == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_Eq1 = new SelectList(db.Tournois.Where(c => c.Id == matchs.tournoi_id).First().Equipes.ToList(), "Id", "nom", matchs.id_Eq1);
            ViewBag.id_Eq2 = new SelectList(db.Tournois.Where(c => c.Id == matchs.tournoi_id).First().Equipes.ToList(), "Id", "nom", matchs.id_Eq2);

            return View("EditMatchs", matchs);
        }

        //
        // POST: /admMatchs/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMatchs(Matchs matchs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matchs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexMatchs");
            }
            ViewBag.id_Eq1 = new SelectList(db.Tournois.Where(c => c.Id == matchs.tournoi_id).First().Equipes.ToList(), "Id", "nom", matchs.id_Eq1);
            ViewBag.id_Eq2 = new SelectList(db.Tournois.Where(c => c.Id == matchs.tournoi_id).First().Equipes.ToList(), "Id", "nom", matchs.id_Eq2);

            return View("EditMatchs", matchs);
        }


        public ActionResult CloturerMatchs(int id = 0)
        {
            Matchs matchs = db.Matchs.Find(id);
            if (matchs == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_Eq1 = new SelectList(db.Tournois.Where(c => c.Id == matchs.tournoi_id).First().Equipes.ToList(), "Id", "nom", matchs.id_Eq1);
            ViewBag.id_Eq2 = new SelectList(db.Tournois.Where(c => c.Id == matchs.tournoi_id).First().Equipes.ToList(), "Id", "nom", matchs.id_Eq2);

            return View("CloturerMatchs", matchs);
        }


        //
        // POST: /admMatchs/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CloturerMatchs(Matchs matchs)
        {
            List<UserProfile> gagnants = new List<UserProfile>();
            List<UserProfile> perdants = new List<UserProfile>();

            if (ModelState.IsValid)
            {
                switch (matchs.resultat)
                {
                    case 0:
                        gagnants = db.UserProfile.Where(c => c.paris_user.Where(i => i.match_id == matchs.Id).FirstOrDefault().resultat_parie == 0).ToList();
                        perdants = db.UserProfile.Where(c => c.paris_user.Where(i => i.match_id == matchs.Id).FirstOrDefault().resultat_parie == 2).ToList();
                        break;
                    case 2:
                        gagnants = db.UserProfile.Where(c => c.paris_user.Where(i => i.match_id == matchs.Id).FirstOrDefault().resultat_parie == 2).ToList();
                        perdants = db.UserProfile.Where(c => c.paris_user.Where(i => i.match_id == matchs.Id).FirstOrDefault().resultat_parie == 0).ToList();
                        break;
                }
                double sommeP = 0;
                double sommeG = 0;
                foreach(UserProfile p in perdants)
                {
                    sommeP += p.paris_user.Where(c => c.match_id == matchs.Id).FirstOrDefault().credit_parie;
                }
                foreach (UserProfile p in gagnants)
                {
                    sommeG += p.paris_user.Where(c => c.match_id == matchs.Id).FirstOrDefault().credit_parie;
                }

                db.User_infos.Where(c => c.admin == true).FirstOrDefault().credits += sommeP / 10;

                sommeP -= sommeP / 10;

                double credGagne = 0;
                double credParie = 0;
                double pourcentage = 0;
                foreach(UserProfile p in gagnants)
                {
                    credParie = p.paris_user.Where(c => c.match_id == matchs.Id).FirstOrDefault().credit_parie;
                    pourcentage = (credParie * 100) / sommeG;
                    credGagne = credParie + (sommeP * pourcentage) / 100;

                    db.User_infos.Where(c => c.UserId == p.UserId).FirstOrDefault().credits += credGagne;
                }

                db.Entry(matchs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexMatchs");
            }
            ViewBag.id_Eq1 = new SelectList(db.Tournois.Where(c => c.Id == matchs.tournoi_id).First().Equipes.ToList(), "Id", "nom", matchs.id_Eq1);
            ViewBag.id_Eq2 = new SelectList(db.Tournois.Where(c => c.Id == matchs.tournoi_id).First().Equipes.ToList(), "Id", "nom", matchs.id_Eq2);

            return View("CloturerMatchs", matchs);
        }
        //
        // GET: /admMatchs/Delete/5

        public ActionResult DeleteMatchs(int id = 0)
        {
            Matchs matchs = db.Matchs.Find(id);
            if (matchs == null)
            {
                return HttpNotFound();
            }
            return View("DeleteMatchs", matchs);
        }

        //
        // POST: /admMatchs/Delete/5

        [HttpPost, ActionName("DeleteMatchs")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMatchsConfirmed(int id)
        {
            Matchs matchs = db.Matchs.Find(id);
            db.Matchs.Remove(matchs);
            db.SaveChanges();
            return RedirectToAction("IndexMatchs");
        }

        public ActionResult Crediter(int id = 0)
        {
            foreach(User_infos ui in db.User_infos.Where(c=>c.admin == false))
            {
                ui.credits += 10;
            }
            db.SaveChanges();
            return View("Index");
        }
        


        [HttpPost]
        public String getListEquipesAutoComplete(String searchEqu)
        {
            List<Equipes> listE = new List<Equipes>();

            foreach (Equipes e in db.Equipes.Where(c => c.nom.StartsWith(searchEqu)).ToList())
            {
                listE.Add(new Equipes { Id = e.Id, description = e.description, nom = e.nom, pays = e.pays });
            }



            JavaScriptSerializer jss = new JavaScriptSerializer();

            string listJSON = jss.Serialize(listE);

            return listJSON;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult AutoCompleteEquipes(string tagsEquipes)
        {
            if (Session["SessionEquipes"] != null)
            {
                ViewBag.lstEquipes = Session["SessionEquipes"];
                foreach (Equipes e in ViewBag.lstEquipes)
                {
                    listeEquipes.Add(e);
                }
            }

            listeEquipes.Add(db.Equipes.Where(c => c.nom == tagsEquipes).First());

            Session["SessionEquipes"] = listeEquipes;
            //lstResp.Clear();
            return PartialView("PartialListEquipesSelected");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult SupprimerEquipeSelected(int suptagsEqu)
        {
            ViewBag.lstEquipes = Session["SessionEquipes"];
            listeEquipes = ViewBag.lstEquipes;

            listeEquipes.RemoveAt(suptagsEqu);

            Session["SessionEquipess"] = listeEquipes;
            return PartialView("PartialListEquipesSelected");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}