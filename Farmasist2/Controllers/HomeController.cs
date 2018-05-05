using Farmasist2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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

        public ActionResult CautaMedicamente(Reteta reteta)
        {

            var nr_med = 0;
            if (reteta.Medicament1 != null)
                nr_med++;
            if (reteta.Medicament2 != null)
                nr_med++;
            if (reteta.Medicament3 != null)
                nr_med++;
            if (reteta.Medicament4 != null)
                nr_med++;
            if (reteta.Medicament5 != null)
                nr_med++;
            try
            {
                using (SqlConnection connection2 = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-Farmasist2-20180324102509;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT S.IDFarmacie, F.Denumire, F.Locatie FROM StocMedicamenteFarmacie AS S" +
                                                    " INNER JOIN ListaMedicamente AS L ON S.IDMedicament = L.IDMedicament" +
                                                    " INNER JOIN ListaFarmacii AS F ON F.IDFarmacie = S.IDFarmacie" +
                                                    " WHERE L.DenumireComerciala IN (@med1, @med2, @med3, @med4, @med5)" +
                                                    " GROUP BY S.IDFarmacie, F.Denumire, F.Locatie" +
                                                    " HAVING Count(L.IDMedicament) = @nr_med", connection2);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("med1", reteta.Medicament1 == null ? "NULL" : reteta.Medicament1);
                    cmd.Parameters.AddWithValue("med2", reteta.Medicament2 == null ? "NULL" : reteta.Medicament2);
                    cmd.Parameters.AddWithValue("med3", reteta.Medicament3 == null ? "NULL" : reteta.Medicament3);
                    cmd.Parameters.AddWithValue("med4", reteta.Medicament4 == null ? "NULL" : reteta.Medicament4);
                    cmd.Parameters.AddWithValue("med5", reteta.Medicament5 == null ? "NULL" : reteta.Medicament5);
                    cmd.Parameters.AddWithValue("nr_med", nr_med);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    return Content("Sql!!!");
                }
            }
            catch (SqlException err)
            {
                // Replace the error with something less specific.
                // You could also log the error now.
                throw new ApplicationException("Data error. " + err.Message.ToString());
            }
            return Content("Bravoooo!!!");
        }
    }
}