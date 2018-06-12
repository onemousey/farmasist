using Farmasist2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Farmasist2.Modul_Farmacie
{
    public class CautareMedicament
    {
        //public static List<Medicament> CautaMedicamentInFarmacie(Medicament med)
        //{
        //    List<Medicament> medicament = new List<Medicament>();
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-Farmasist2-20180324102509;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
        //        {
        //            SqlCommand cmd_medicament = new SqlCommand("SELECT * FROM StocMedicamenteFarmacie WHERE NumeMedicament = @nume_medicament", connection);
        //            cmd_medicament.CommandType = CommandType.Text;
        //            cmd_medicament.Parameters.AddWithValue("nume_medicament", med.NumeMedicament);
        //            SqlDataAdapter adp_doctor = new SqlDataAdapter(cmd_medicament);
        //            DataSet ds_doctor = new DataSet();
        //            adp_doctor.Fill(ds_doctor);
        //        }
        //    }
        //    catch (SqlException error)
        //    {
        //        throw new ApplicationException("Invalid data " + error.Message.ToString());
        //    }
        //}
    }
}