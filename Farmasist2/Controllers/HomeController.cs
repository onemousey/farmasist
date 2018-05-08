using Farmasist2.Models;
using Farmasist2.ModulMedic;
using System.Net;
using System.Web.Mvc;

namespace Farmasist2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Farmacist()
        {
            ViewBag.Message = "Farmacist.";

            return View();
        }

        [HttpPost]
        public ActionResult CautaMedicamente(Reteta reteta)
        {
            return Json(CreareReteta.CautaMedicamente(reteta));
        }
        public ActionResult SalvareConsultatie(Reteta reteta)
        {
            bool rezultat = CreareReteta.SalvareConsultatie(reteta);
            if(rezultat)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
        public FileContentResult GenereazaReteta(string numeMedic, string numePacient, string prenumePacient, string diagnostic, string medicament1, string cant1, string medicament2, string cant2, string medicament3, string cant3, string medicament4, string cant4, string medicament5, string cant5, string tipReteta)
        {
            return File(CreareReteta.GenereazaReteta(numeMedic, numePacient, prenumePacient, diagnostic, medicament1, cant1, medicament2, cant2, medicament3, cant3, medicament4, cant4, medicament5, cant5, tipReteta), "application/pdf", "diploma.pdf");
        }
    }
}