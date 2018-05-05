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

            return Content("Bravoooo!!!");
            try
            {
                using (SqlConnection connection2 = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-Farmasist2-20180324102509;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    SqlCommand cmd = new SqlCommand("Select * from ListaMedicamente where DenumireComerciala = @nume_medicament", connection2);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("nume_medicament", reteta.Nume_pacient);
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