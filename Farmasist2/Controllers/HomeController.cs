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
            return File(CreareReteta.GenereazaReteta(numeMedic, numePacient, prenumePacient, diagnostic, medicament1, cant1, medicament2, cant2, medicament3, cant3, medicament4, cant4, medicament5, cant5, tipReteta), "application/pdf", "reteta.pdf");
        }

        [HttpPost]
        public ActionResult CautarePacient(string cnp)
        {
            try
            {
                using (SqlConnection connection2 = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-Farmasist2-20180324102509;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    SqlCommand cmd_pacient = new SqlCommand("SELECT * FROM Pacienti WHERE cnp=@cnp", connection2);
                    cmd_pacient.CommandType = CommandType.Text;
                    cmd_pacient.Parameters.AddWithValue("cnp", cnp);
                    SqlDataAdapter adp_pacient = new SqlDataAdapter(cmd_pacient);
                    DataSet ds_pacient = new DataSet();
                    adp_pacient.Fill(ds_pacient);

                    if(ds_pacient.Tables[0].Rows.Count == 0)
                    {
                        return Json("Pacientul cautat nu exista in baza de date!");
                    }
                    else
                    {
                        return Json(" Nume: " + ds_pacient.Tables[0].Rows[0].ItemArray[1] + " " + ds_pacient.Tables[0].Rows[0].ItemArray[2] + "\n" + " Adresa:" + ds_pacient.Tables[0].Rows[0].ItemArray[3] + " \n" + " Grupa Sanguina:" + ds_pacient.Tables[0].Rows[0].ItemArray[4] + " \n" + " Alergii:" + ds_pacient.Tables[0].Rows[0].ItemArray[5]);
                    }
                }
            }
            catch (SqlException err)
            {
                // Replace the error with something less specific.
                // You could also log the error now.
                throw new ApplicationException("Data error. " + err.Message.ToString());
            }
        }

        [HttpPost]
        public ActionResult AdaugarePacient(Pacient pacient)
        {
            try
            {
                using (SqlConnection connection2 = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-Farmasist2-20180324102509;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    SqlCommand cmd_pacient = new SqlCommand("Insert into Pacienti values(@cnp,@nume,@prenume,@adresa,@grupaSanguina,@alergii)", connection2);
                    cmd_pacient.CommandType = CommandType.Text;
                    cmd_pacient.Parameters.AddWithValue("cnp", pacient.CNP);
                    cmd_pacient.Parameters.AddWithValue("nume", pacient.Nume);
                    cmd_pacient.Parameters.AddWithValue("prenume", pacient.Prenume);
                    cmd_pacient.Parameters.AddWithValue("adresa", pacient.Adresa);
                    cmd_pacient.Parameters.AddWithValue("grupaSanguina", pacient.Grupa_Sanguina);
                    cmd_pacient.Parameters.AddWithValue("alergii", pacient.Alergii);
                    SqlDataAdapter adp_pacient = new SqlDataAdapter(cmd_pacient);
                    DataSet ds_pacient = new DataSet();
                    adp_pacient.Fill(ds_pacient);

                    return Json(" ");
                }
            }
            catch (SqlException err)
            {
                // Replace the error with something less specific.
                // You could also log the error now.
                throw new ApplicationException("Data error. " + err.Message.ToString());
            }
        }
    }
}