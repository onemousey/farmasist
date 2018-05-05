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

        [HttpPost]
        public ActionResult CautaMedicamente(Reteta reteta)
        {
            List<Farmacie> farmacii = new List<Farmacie>();
            var map = new Dictionary<int, List<object[]>>();
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
                    SqlCommand cmd_doctor = new SqlCommand("SELECT * FROM Medici WHERE Nume = @nume_medic", connection2);
                    cmd_doctor.CommandType = CommandType.Text;
                    cmd_doctor.Parameters.AddWithValue("nume_medic", reteta.Nume_medic);
                    SqlDataAdapter adp_doctor = new SqlDataAdapter(cmd_doctor);
                    DataSet ds_doctor = new DataSet();
                    adp_doctor.Fill(ds_doctor);

                    string[] doc_geo = ((string)ds_doctor.Tables[0].Rows[0].ItemArray[3]).Split('(')[1].Split(',');
                    doc_geo[1] = doc_geo[1].Remove(doc_geo[1].Length - 1);

                    double doc_lat = Double.Parse(doc_geo[0]);
                    double doc_lng = Double.Parse(doc_geo[1]);

                    SqlCommand cmd = new SqlCommand("SELECT T.IDFarmacie, T.Denumire, T.Locatie, L.IDMedicament, L.DenumireComerciala, S.Cantitate FROM StocMedicamenteFarmacie AS S " +
                                                    "INNER JOIN ListaMedicamente AS L ON S.IDMedicament = L.IDMedicament " +
                                                    "INNER JOIN( " +
                                                            "SELECT S.IDFarmacie, F.Denumire, F.Locatie FROM StocMedicamenteFarmacie AS S" +
                                                            " INNER JOIN ListaMedicamente AS L ON S.IDMedicament = L.IDMedicament" +
                                                            " INNER JOIN ListaFarmacii AS F ON F.IDFarmacie = S.IDFarmacie" +
                                                            " WHERE L.DenumireComerciala IN (@med1, @med2, @med3, @med4, @med5)" +
                                                            " GROUP BY S.IDFarmacie, F.Denumire, F.Locatie" +
                                                            " HAVING Count(L.IDMedicament) = @nr_med " +
                                                    ") AS T ON S.IDFarmacie = T.IDFarmacie " +
                                                    "WHERE L.DenumireComerciala IN(@med1, @med2, @med3, @med4, @med5)", connection2);

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

                    for(int i = 0; i< ds.Tables[0].Rows.Count; i++)
                    {
                        var key = ((int)ds.Tables[0].Rows[i].ItemArray[0]);
                        var list = map.FirstOrDefault(x => x.Key == key).Value;
                        if(list == null)
                        {
                            list = new List<object[]>();
                        }
                        list.Add(ds.Tables[0].Rows[i].ItemArray);
                        map[key] = list;
                    }

                    foreach (KeyValuePair<int, List<object[]>> entry in map)
                    {
                        bool adauga = true;
                        foreach (object[] med in entry.Value)
                        {
                            if (reteta.Medicament1 != null && med[4].Equals(reteta.Medicament1) && ((int)med[5]) < reteta.Cant1) { adauga = false; }
                            if (reteta.Medicament2 != null && med[4].Equals(reteta.Medicament2) && ((int)med[5]) < reteta.Cant2) { adauga = false; }
                            if (reteta.Medicament3 != null && med[4].Equals(reteta.Medicament2) && ((int)med[5]) < reteta.Cant3) { adauga = false; }
                            if (reteta.Medicament4 != null && med[4].Equals(reteta.Medicament3) && ((int)med[5]) < reteta.Cant4) { adauga = false; }
                            if (reteta.Medicament5 != null && med[4].Equals(reteta.Medicament4) && ((int)med[5]) < reteta.Cant5) { adauga = false; }
                        }
                        var farm = entry.Value.FirstOrDefault(v => ((int)v[0]) == entry.Key);
                        if (adauga)
                        {
                            string[] geo = ((string)farm[2]).Split('(')[1].Split(',');
                            geo[1] = geo[1].Remove(geo[1].Length - 1);

                            double lat = Double.Parse(geo[0]);
                            double lng = Double.Parse(geo[1]);

                            farmacii.Add(new Farmacie
                            {
                                Nume_farmacie = farm[1].ToString(),
                                Distanta = Math.Round(calcDistance(doc_lat, doc_lng, lat, lng), 2)
                            });
                        }
                    }

                    farmacii.Sort((x, y) => x.Distanta.CompareTo(y.Distanta));
                        
                    return Json(farmacii);
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

        public double calcDistance(double latA, double longA, double latB, double longB)
        {

            double theDistance = (Math.Sin(DegreesToRadians(latA)) *
                    Math.Sin(DegreesToRadians(latB)) +
                    Math.Cos(DegreesToRadians(latA)) *
                    Math.Cos(DegreesToRadians(latB)) *
                    Math.Cos(DegreesToRadians(longA - longB)));

            return Convert.ToDouble((RadiansToDegrees(Math.Acos(theDistance)))) * 69.09D * 1.6093D;
        }

        public double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public double RadiansToDegrees(double radians) {
            return radians * (180 / Math.PI);
        }
    }
}