using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Farmasist2.Models;

namespace Farmasist2.ModulMedic
{
    public class Programarecs
    {

        public static List<Programare> getProgramare()
        {
            List<Programare> farmacii = new List<Programare>();
            try
            {
                using (SqlConnection connection2 = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-Farmasist2-20180324102509;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    SqlCommand prog_cmd = new SqlCommand("SELECT * FROM Programari", connection2);
                    prog_cmd.CommandType = CommandType.Text;
                    SqlDataAdapter prog_adp = new SqlDataAdapter(prog_cmd);
                    DataSet prog_ds = new DataSet();
                    prog_adp.Fill(prog_ds);

                    SqlCommand pac_cmd = new SqlCommand("select Data,Nume, Prenume from Programari as prog inner join Pacienti as pac on prog.CNP = pac.CNP", connection2);
                    pac_cmd.CommandType = CommandType.Text;
                    SqlDataAdapter pac_adp = new SqlDataAdapter(pac_cmd);
                    DataSet pac_ds = new DataSet();
                    pac_adp.Fill(pac_ds);

                    for (int i = 0; i < pac_ds.Tables[0].Rows.Count; i++)
                    {
                        farmacii.Add(new Programare { Data = pac_ds.Tables[0].Rows[i].ItemArray[0].ToString(), Nume = pac_ds.Tables[0].Rows[i].ItemArray[1] + " " + pac_ds.Tables[0].Rows[i].ItemArray[2] });
                    }

                    return farmacii;
                }
            }
            catch (SqlException err)
            {
                throw new ApplicationException("Data error. " + err.Message.ToString());
            }
        }
    }
}