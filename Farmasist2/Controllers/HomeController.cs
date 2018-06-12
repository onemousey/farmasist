using Farmasist2.Models;
using Farmasist2.ModulMedic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Farmasist2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pacient()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Medic()
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
                return Json("status: OK");
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
        [HttpPost]
        public ActionResult CMedicament(string Tip)
        {
            try
            {
                using (SqlConnection connection2 = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=aspnet-Farmasist2-20180324102509;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;"))
                {
                    SqlCommand cmd_medicament = new SqlCommand("SELECT * FROM StocMedicamenteFarmacie WHERE Tip=@tip", connection2);
                    cmd_medicament.CommandType = CommandType.Text;
                    cmd_medicament.Parameters.AddWithValue("Tip", Tip);
                    SqlDataAdapter adp_medicament = new SqlDataAdapter(cmd_medicament);
                    DataSet ds_medicament = new DataSet();
                    adp_medicament.Fill(ds_medicament);

                    if (ds_medicament.Tables[0].Rows.Count == 0)

                    {
                        return Json("Medicamentul căutat nu a fost găsit!");
                    }
                    else
                    {
                        return Json("" + ds_medicament.Tables[0].Rows[0].ItemArray[3] + "           " + ds_medicament.Tables[0].Rows[0].ItemArray[4] + "       " + ds_medicament.Tables[0].Rows[0].ItemArray[5]);
                    }
                }
            }
            catch (SqlException err)
            {

                throw new ApplicationException("Data error. " + err.Message.ToString());
            }
        }

    }
}