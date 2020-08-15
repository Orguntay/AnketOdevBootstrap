using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AnkletOdevBootsRap.Models;
using System.Web.Script.Serialization;
namespace AnkletOdevBootsRap.Controllers
{

    public class EkleController : Controller
    {
        AnkletOdevBootsRap.Models.odevDBEntities4 db = new Models.odevDBEntities4();
        
        public ActionResult TestEkle()
        {
            ViewBag.kat = from p in db.katagorilers
                          orderby p.ustId
                          select p;

            ViewBag.snv = from p in db.sinavlars
                          where p.katId == 1 && p.testmi == true
                          select p;
            return View();
        }
        [HttpPost]
        public ActionResult TestKaydet(int katId, int snvId, string soru, string[] cevaplar, string[] durum, int puan, string ack, string kaydet)
        {

            if (kaydet == "Kaydet")
            {
                sorular srlr = new sorular();
                srlr.userid = 1;// Convert.ToInt32(Session["uyeID"]);
                srlr.katID = Convert.ToInt32(katId);
                srlr.sinavId = snvId;
                srlr.soru = soru;
                //srlr.testmi = false;
                srlr.aciklama = ack;
                srlr.puan = puan;
                db.sorulars.Add(srlr);
                db.SaveChanges();
                //var soruID = from p in db.sorulars
                //             where p.soru==soru
                //             select p.id;

                var soruID = (from p in db.sorulars
                              select p).OrderByDescending(c => c.id).FirstOrDefault() as Models.sorular;
                Models.cevaplar cvplr = new Models.cevaplar();
                for (int i = 0; i < cevaplar.Length; i++)
                {
                    cvplr.soruId = Convert.ToInt32(soruID.id);
                    cvplr.dogrumu = Convert.ToBoolean(durum[i]);
                    cvplr.sira = Convert.ToInt32(i);
                    cvplr.cevap = cevaplar[i].ToString();
                    db.cevaplars.Add(cvplr);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("TestEkle");
        }
        [HttpPost]
        public JsonResult Testler(int katId)
        {
            var sinavSorgu = from p in db.sinavlars
                             where p.katId == katId && p.testmi == true
                             select new SelectListItem
                             {
                                 Value = p.id.ToString(),
                                 Text = p.ad,
                             };
            return Json(sinavSorgu, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SinavAdiEkle()
        {
            var kat = from p in db.katagorilers
                      select p;
            ViewBag.kat = kat;
            return View();
        }
        [HttpPost]
        public ActionResult SinavAdiEkle(int katId, string ad, bool testmi, int puan)
        {
            AnkletOdevBootsRap.Models.sinavlar snv = new Models.sinavlar();
            snv.katId = katId;
            snv.ad = ad;
            snv.testmi = !testmi;
            snv.puan = puan;
            db.sinavlars.Add(snv);
            db.SaveChanges();
            ViewBag.kat = from p in db.katagorilers
                       select p;
            return View();
        }

        public ActionResult AnketEkle()
        {
            ViewBag.kat =  from p in db.katagorilers
                           orderby p.ustId
                           select p;

            ViewBag.snv = from p in db.sinavlars
                          where p.katId == 1 && p.testmi == false
                      select p;
            return View();
        }
        [HttpPost]
        public ActionResult AnketKaydet(int katId, int snvId, string soru, string[] cevaplar, string[] durum,int puan, string ack, string kaydet)
        {

            if (kaydet == "Kaydet")
            {
                sorular srlr = new sorular();
                srlr.userid = 1;// Convert.ToInt32(Session["uyeID"]);
                srlr.katID = Convert.ToInt32(katId);
                srlr.sinavId = snvId;
                srlr.soru = soru;
                //srlr.testmi = false;
                srlr.aciklama = ack;
                srlr.puan = puan;
                db.sorulars.Add(srlr);
                db.SaveChanges();
                //var soruID = from p in db.sorulars
                //             where p.soru==soru
                //             select p.id;

                var soruID = (from p in db.sorulars
                              select p).OrderByDescending(c => c.id).FirstOrDefault() as Models.sorular;
                Models.cevaplar cvplr = new Models.cevaplar();
                for (int i = 0; i < cevaplar.Length; i++)
                {
                    cvplr.soruId = Convert.ToInt32(soruID.id);
                    cvplr.dogrumu = Convert.ToBoolean(durum[i]);
                    cvplr.sira = Convert.ToInt32(i);
                    cvplr.cevap = cevaplar[i].ToString();
                    db.cevaplars.Add(cvplr);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("AnketEkle");
        }
        [HttpPost]
        public JsonResult Sinavlar(int katId)
        {
            var sinavSorgu = from p in db.sinavlars
                             where p.katId == katId && p.testmi == false
                             select new SelectListItem
                             {
                                 Value = p.id.ToString(),
                                 Text = p.ad,
                             };
            return Json(sinavSorgu, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult KatagoriEkle(string id, string girilenKTG, string girilenACK, string ad)
        {
            AnkletOdevBootsRap.Models.katagoriler ktg = new Models.katagoriler();
            ktg.ustId = Convert.ToInt32(id);
            ktg.adi = girilenKTG;
            ktg.tarih = DateTime.Now.ToString();
            ktg.aciklama = girilenACK;

            db.katagorilers.Add(ktg);
            db.SaveChanges();
            ViewBag.kat = from p in db.katagorilers
                          orderby p.ustId
                          select p;
            return View();
        }
        public ActionResult KatagoriEkle()
        {
            ViewBag.kat = from p in db.katagorilers
                          orderby p.ustId
                          select p;
            return View();
        }
	}
}