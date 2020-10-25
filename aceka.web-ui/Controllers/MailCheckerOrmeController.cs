using aceka.web_ui.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aceka.web_ui.Controllers
{
    public class MailCheckerOrmeController : Controller
    {
        // GET: MailCheckerOrme
        public ActionResult Index()
        {
            string message = "Mail gönderildi!";
            string errorMessage = "";


            using (var dbContext = new EndofLinEntities())
            {
                var senderList = dbContext.MarsanMailHesaplari.Where(h => h.OrmeAktif == true).ToList();

                var endDate = DateTime.Now.AddDays(5);
                var stardDate = DateTime.Now;

                var terminListe = dbContext.MarsanMailOrme.Where(m => m.OrmeTerminTarihi >= stardDate && m.OrmeTerminTarihi <= endDate).
                    Select(t => new
                    {
                        t.TerminTarihi,
                        t.Firma,
                        t.SiparisNo,
                        t.FirmaSiparisNo,
                        t.Cinsi,
                        t.SiparisMiktari,
                        t.Aciklama,
                        t.Birim,
                        t.Fayn,
                        t.IsEmriBakiye,
                        t.IsEmriMiktari,
                        t.OrmeTerminTarihi,
                        t.Pus,
                        t.TüpMay,
                        t.UretimBakiye,
                        t.UretimMiktari
                    }).OrderBy(t => t.OrmeTerminTarihi).ToList();


                if (terminListe != null && terminListe.Count > 0)
                {
                    //Liste oluşturuluyor
                    string mailGovde = "";
                    int counter = 1;
                    foreach (var item in terminListe)
                    {
                        mailGovde += "<tr" + (counter % 2 == 0 ? " style=\"background-color:yellow\"" : null) + " >";
                        mailGovde += "<td>" + (item.OrmeTerminTarihi != null ? Convert.ToDateTime(item.OrmeTerminTarihi).ToShortDateString() : null) + " </td>";
                        mailGovde += "<td>" + (item.TerminTarihi != null ? Convert.ToDateTime(item.TerminTarihi).ToShortDateString() : null) + "</td>";
                        mailGovde += "<td>" + item.Firma + "</td>";
                        mailGovde += "<td>" + item.SiparisNo + "</td>";
                        mailGovde += "<td>" + item.FirmaSiparisNo + "</td> ";
                        mailGovde += "<td>" + item.Cinsi + "</td>";
                        mailGovde += "<td>" + item.SiparisMiktari + "</td> ";
                        mailGovde += "<td>" + item.Birim + "</td>";
                        mailGovde += "<td>" + item.IsEmriMiktari + "</td>";
                        mailGovde += "<td>" + item.IsEmriBakiye + "</td>";
                        mailGovde += "<td>" + item.UretimMiktari + "</td>";
                        mailGovde += "<td>" + item.UretimBakiye + "</td>";
                        mailGovde += "<td>" + item.Pus + "</td> ";
                        mailGovde += "<td>" + item.Fayn + "</td> ";
                        mailGovde += "<td>" + item.TüpMay + "</td> ";
                        mailGovde += "<td>" + item.Aciklama + "</td> ";
                        mailGovde += "</tr>";
                        counter++;
                    }

                    Hashtable ht = new Hashtable();
                    ht.Add("<@liste@>", mailGovde);

                    //Tüm kullanıcılara mail gönderiliyor.
                    string hesaplar = "";
                    for (int i = 0; i < senderList.Count; i++)
                    {
                        hesaplar += senderList[i].EPosta.ToString() + ",";
                    }


                    //Mail Atılıyor
                    //foreach (var item in senderList)
                    //{
                    var retVal = UITools.SendMail(
                            ConfigurationManager.AppSettings["acekaSenderAccount"],
                            hesaplar.TrimEnd(new char[] { ',', ' ' }), "Örgü Terminine 5 Gün Kalanların Listesi", UITools.ReadToHtml(@"/assets/mail_templates/TerminListeOrme.html", ht), ref errorMessage);
                    if (!retVal)
                    {
                        message = errorMessage;
                        // break;
                    }
                    //}
                }
                else
                {
                    message = "Listelenecek kayıt yok!";
                }

            }
            return Content(message);
        }
    }
}