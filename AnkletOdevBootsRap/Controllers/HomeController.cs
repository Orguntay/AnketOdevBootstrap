using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Web.Mvc.Ajax;
//using System.Web.Mvc.Html;
namespace AnkletOdevBootsRap.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        AnkletOdevBootsRap.Models.odevDBEntities4 db = new Models.odevDBEntities4();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sorular()
        {
            return View();
        }
        public ActionResult BasariListesi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string kAdi, string sifree)
        {
            var uyem = (from p in db.users
                             where (p.kullaniciadi == kAdi && p.sifre == sifree)
                             select p).SingleOrDefault();
            if ( uyem !=null )
            {
                Session["uyeID"] = uyem.id;
                Session["ad"] = uyem.kullaniciadi;
                Session["yetki"] = uyem.yetki;
                return RedirectToAction("index", "Home");
            }
            return RedirectToAction("Login", "Home");
            
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Cikis()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("index", "Home");
            //return View();
        }
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult UyeKayit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UyeKayit(string email, string sifre, string sifre2)
        {
            if (sifre != sifre2)
            {
                ViewBag.mesaj = 3;
                return View();
            }
            var kontrol = (from p in db.users
                           where email == p.kullaniciadi
                           select p).Count();
            if ( kontrol > 0 )//kaydetme
            {
                ViewBag.mesaj = 1;
                return View();
            }

            AnkletOdevBootsRap.Models.user uye = new Models.user();
            try
            {
                uye.kullaniciadi = email;
                uye.sifre = sifre;
                uye.yetki = "uye";
                db.users.Add(uye);
                db.SaveChanges();

                var uyem = (from p in db.users
                            where (p.kullaniciadi == email && p.sifre == sifre)
                            select p).Single();

                Session["uyeID"] = uyem.id;
                Session["ad"] = uyem.kullaniciadi;
                Session["yetki"] = uyem.yetki;
                return RedirectToAction("Sorular", "Home");
            }
            catch (Exception)
            {
                
            }
            return View();
        }

        [HttpPost]
        public JsonResult SoruSay(int snvId)
        {
            var soruSayisi = (from p in db.sorulars
                              where p.sinavId == snvId
                              select p).Count();
            return Json( soruSayisi , JsonRequestBehavior.AllowGet);
        }
	}
}