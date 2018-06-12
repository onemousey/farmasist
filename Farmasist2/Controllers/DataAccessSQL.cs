using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Farmasist2.Models;
using System.Data.SqlClient;

namespace Farmasist2.Controllers
{
    public class DataAccessSQL
    {
        private readonly string DB_CON = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=aspnet-Farmasist2-20180324102509;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
        public List<FarmacieRezervariMedicamente> getCereriMedicamente()
        {

            List<FarmacieRezervariMedicamente> ModelMedicament = new List<FarmacieRezervariMedicamente>();
            try
            {

                using (SqlConnection con = new SqlConnection(DB_CON))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT DenumireComerciala,Concentratie FROM [dbo].ListaMedicamente INNER JOIN [dbo].RezervariMedicamente ON [dbo].ListaMedicamente.IDMedicament=[dbo].RezervariMedicamente.IDMedicament WHERE [dbo].RezervariMedicamente.Status=1", con))
                    {

                        con.Open();
                        //  cmd.Connection = con;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                string DenumireComerciala = (string)reader["DenumireComerciala"];
                                double Concentratie = (double)reader["Concentratie"];

                                FarmacieRezervariMedicamente medicament = new FarmacieRezervariMedicamente();
                                medicament.DenumireComerciala = DenumireComerciala;
                                medicament.Concentratie = Concentratie;
                                ModelMedicament.Add(medicament);
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                ModelMedicament.Clear();
                //  throw;
            }
            return ModelMedicament;
        }
    }
}