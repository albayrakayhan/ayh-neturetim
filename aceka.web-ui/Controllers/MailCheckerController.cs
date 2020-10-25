using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using aceka.web_ui.Models;
using System.Configuration;
using System.Collections;

namespace aceka.web_ui.Controllers
{
    public class MailCheckerController : Controller
    {
        // GET: MailChecker
        public ActionResult Index()
        {
            string message = "Mail gönderildi!";
            string errorMessage = "";


            using (var dbContext = new EndofLinEntities())
            {
                var senderList = dbContext.MarsanMailHesaplari.Where(h => h.Aktif == true).ToList();

                var endDate = DateTime.Now.AddDays(5);
                var stardDate = DateTime.Now;

                var terminListe = dbContext.MarsanMail.Where(m => m.TerminTarihi >= stardDate && m.TerminTarihi <= endDate).
                    Select(t => new
                    {
                        t.TerminTarihi,
                        t.Firma,
                        t.SiparisNo,
                        t.SiparisTarihi,
                        t.FirmaSiparisNo,
                        t.Renk,
                        t.StokKodu,
                        t.SiparisMiktari,
                        t.Durum
                    }).OrderBy(t => t.TerminTarihi).ToList();


                if (terminListe != null && terminListe.Count > 0)
                {
                    //Liste oluşturuluyor
                    string mailGovde = "";
                    int counter = 1;
                    foreach (var item in terminListe)
                    {
                        mailGovde += "<tr" + (counter % 2 == 0 ? " style=\"background-color:yellow\"" : null) + " >";
                        mailGovde += "<td>" + (item.TerminTarihi != null ? Convert.ToDateTime(item.TerminTarihi).ToShortDateString() : null) + " </td>";
                        mailGovde += "<td>" + item.Firma + "</td>";
                        mailGovde += "<td>" + item.SiparisNo + "</td>";
                        mailGovde += "<td>" + (item.SiparisTarihi != null ? Convert.ToDateTime(item.SiparisTarihi).ToShortDateString() : null) + "</td>";
                        mailGovde += "<td>" + item.FirmaSiparisNo + "</td> ";
                        mailGovde += "<td>" + item.Renk + "</td> ";
                        mailGovde += "<td>" + item.StokKodu + "</td> ";
                        mailGovde += "<td>" + item.SiparisMiktari + "</td> ";
                        mailGovde += "<td>" + item.Durum + "</td> ";
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
                            hesaplar.TrimEnd(new char[] { ',', ' ' }), "Termine 5 Gün Kalanların Listesi", UITools.ReadToHtml(@"/assets/mail_templates/TerminListe.html", ht), ref errorMessage);
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