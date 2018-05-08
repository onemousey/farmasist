using Farmasist2.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;


namespace Farmasist2.ModulMedic
{
    public class CreareReteta
    {
        public static List<Farmacie> CautaMedicamente(Reteta reteta)
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

                    SqlCommand pac_cmd = new SqlCommand("SELECT TOP 1 * FROM Pacienti WHERE Nume = @nume AND Prenume = @prenume", connection2);
                    pac_cmd.CommandType = CommandType.Text;
                    pac_cmd.Parameters.AddWithValue("nume", reteta.Nume_pacient);
                    pac_cmd.Parameters.AddWithValue("prenume", reteta.Prenume_pacient);
                    SqlDataAdapter pac_adp = new SqlDataAdapter(pac_cmd);
                    DataSet pac_ds = new DataSet();
                    pac_adp.Fill(pac_ds);

                    SqlCommand prog_cmd = new SqlCommand("SELECT TOP 1 * FROM Programari WHERE IDMedic = @id_medic AND CNP = @cnp ORDER BY DATA DESC", connection2);
                    prog_cmd.CommandType = CommandType.Text;
                    prog_cmd.Parameters.AddWithValue("id_medic", ((int)ds_doctor.Tables[0].Rows[0].ItemArray[0]));
                    prog_cmd.Parameters.AddWithValue("cnp", ((string)pac_ds.Tables[0].Rows[0].ItemArray[0]));
                    SqlDataAdapter prog_adp = new SqlDataAdapter(prog_cmd);
                    DataSet prog_ds = new DataSet();
                    prog_adp.Fill(prog_ds);

                    SqlCommand ret_cmd = new SqlCommand("INSERT INTO Retete(CNP, NrProgramare, Medicament1, Medicament2, Medicament3, Medicament4, Medicament5, TipReteta, Status) VALUES(@cnp, @nr_programare, @med1, @med2, @med3, @med4, @med5, @tip_reteta, 'P')", connection2);
                    ret_cmd.CommandType = CommandType.Text;
                    ret_cmd.Parameters.AddWithValue("cnp", ((string)pac_ds.Tables[0].Rows[0].ItemArray[0]));
                    ret_cmd.Parameters.AddWithValue("nr_programare", ((int)prog_ds.Tables[0].Rows[0].ItemArray[0]));
                    ret_cmd.Parameters.AddWithValue("med1", reteta.Medicament1 == null ? "NULL" : reteta.Medicament1);
                    ret_cmd.Parameters.AddWithValue("med2", reteta.Medicament2 == null ? "NULL" : reteta.Medicament2);
                    ret_cmd.Parameters.AddWithValue("med3", reteta.Medicament3 == null ? "NULL" : reteta.Medicament3);
                    ret_cmd.Parameters.AddWithValue("med4", reteta.Medicament4 == null ? "NULL" : reteta.Medicament4);
                    ret_cmd.Parameters.AddWithValue("med5", reteta.Medicament5 == null ? "NULL" : reteta.Medicament5);
                    ret_cmd.Parameters.AddWithValue("tip_reteta", reteta.TipReteta);
                    SqlDataAdapter ret_adp = new SqlDataAdapter(ret_cmd);
                    DataSet ret_ds = new DataSet();
                    ret_adp.Fill(ret_ds);

                    SqlCommand select_ret_cmd = new SqlCommand("SELECT TOP 1 * FROM Retete WHERE NrProgramare = @nr_programare ORDER BY IDTratament DESC", connection2);
                    select_ret_cmd.CommandType = CommandType.Text;
                    select_ret_cmd.Parameters.AddWithValue("nr_programare", ((int)prog_ds.Tables[0].Rows[0].ItemArray[0]));
                    SqlDataAdapter select_ret_adp = new SqlDataAdapter(select_ret_cmd);
                    DataSet select_ret_ds = new DataSet();
                    select_ret_adp.Fill(select_ret_ds);

                    SqlCommand cons_cmd = new SqlCommand("INSERT INTO Consultatii(NrProgramare, IDMedic, CNP, Diagnostic, IDTratament) VALUES(@nr_programare, @id_medic, @cnp, @diagnostic, @id_tratament)", connection2);
                    cons_cmd.CommandType = CommandType.Text;
                    cons_cmd.Parameters.AddWithValue("nr_programare", ((int)prog_ds.Tables[0].Rows[0].ItemArray[0]));
                    cons_cmd.Parameters.AddWithValue("id_medic", ((int)ds_doctor.Tables[0].Rows[0].ItemArray[0]));
                    cons_cmd.Parameters.AddWithValue("cnp", ((string)pac_ds.Tables[0].Rows[0].ItemArray[0]));
                    cons_cmd.Parameters.AddWithValue("diagnostic", reteta.Diagnostic);
                    cons_cmd.Parameters.AddWithValue("id_tratament", ((int)select_ret_ds.Tables[0].Rows[0].ItemArray[0]));
                    SqlDataAdapter cons_adp = new SqlDataAdapter(cons_cmd);
                    DataSet cons_ds = new DataSet();
                    cons_adp.Fill(cons_ds);


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

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var key = ((int)ds.Tables[0].Rows[i].ItemArray[0]);
                        var list = map.FirstOrDefault(x => x.Key == key).Value;
                        if (list == null)
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

                    return farmacii;
                }
            }
            catch (SqlException err)
            {
                // Replace the error with something less specific.
                // You could also log the error now.
                throw new ApplicationException("Data error. " + err.Message.ToString());
            }
        }
        public static bool SalvareConsultatie(Reteta reteta)
        {
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

                    SqlCommand pac_cmd = new SqlCommand("SELECT TOP 1 * FROM Pacienti WHERE Nume = @nume AND Prenume = @prenume", connection2);
                    pac_cmd.CommandType = CommandType.Text;
                    pac_cmd.Parameters.AddWithValue("nume", reteta.Nume_pacient);
                    pac_cmd.Parameters.AddWithValue("prenume", reteta.Prenume_pacient);
                    SqlDataAdapter pac_adp = new SqlDataAdapter(pac_cmd);
                    DataSet pac_ds = new DataSet();
                    pac_adp.Fill(pac_ds);

                    SqlCommand prog_cmd = new SqlCommand("SELECT TOP 1 * FROM Programari WHERE IDMedic = @id_medic AND CNP = @cnp ORDER BY DATA DESC", connection2);
                    prog_cmd.CommandType = CommandType.Text;
                    prog_cmd.Parameters.AddWithValue("id_medic", ((int)ds_doctor.Tables[0].Rows[0].ItemArray[0]));
                    prog_cmd.Parameters.AddWithValue("cnp", ((string)pac_ds.Tables[0].Rows[0].ItemArray[0]));
                    SqlDataAdapter prog_adp = new SqlDataAdapter(prog_cmd);
                    DataSet prog_ds = new DataSet();
                    prog_adp.Fill(prog_ds);

                    SqlCommand cons_cmd = new SqlCommand("INSERT INTO Consultatii(NrProgramare, IDMedic, CNP, Diagnostic) VALUES(@nr_programare, @id_medic, @cnp, @diagnostic)", connection2);
                    cons_cmd.CommandType = CommandType.Text;
                    cons_cmd.Parameters.AddWithValue("nr_programare", ((int)prog_ds.Tables[0].Rows[0].ItemArray[0]));
                    cons_cmd.Parameters.AddWithValue("id_medic", ((int)ds_doctor.Tables[0].Rows[0].ItemArray[0]));
                    cons_cmd.Parameters.AddWithValue("cnp", ((string)pac_ds.Tables[0].Rows[0].ItemArray[0]));
                    cons_cmd.Parameters.AddWithValue("diagnostic", reteta.Diagnostic);
                    SqlDataAdapter cons_adp = new SqlDataAdapter(cons_cmd);
                    DataSet cons_ds = new DataSet();
                    cons_adp.Fill(cons_ds);
                }
            }
            catch (SqlException err)
            {
                // Replace the error with something less specific.
                // You could also log the error now.
                throw new ApplicationException("Data error. " + err.Message.ToString());
            }
            return true;
        }
        private static double calcDistance(double latA, double longA, double latB, double longB)
        {

            double theDistance = (Math.Sin(DegreesToRadians(latA)) *
                    Math.Sin(DegreesToRadians(latB)) +
                    Math.Cos(DegreesToRadians(latA)) *
                    Math.Cos(DegreesToRadians(latB)) *
                    Math.Cos(DegreesToRadians(longA - longB)));

            return Convert.ToDouble((RadiansToDegrees(Math.Acos(theDistance)))) * 69.09D * 1.6093D;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        private static double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        public static byte[] GenereazaReteta(string numeMedic, string numePacient, string prenumePacient, string diagnostic, string medicament1, string cant1, string medicament2, string cant2, string medicament3, string cant3, string medicament4, string cant4, string medicament5, string cant5, string tipReteta)
        {
            MemoryStream memoryStream = new MemoryStream();

            Document doc = new Document(new Rectangle(500, 400));

            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

            string med1 = (!string.IsNullOrEmpty(medicament1) && !string.IsNullOrEmpty(cant1)) ? $"{medicament1}({cant1}buc.)" : null;
            string med2 = (!string.IsNullOrEmpty(medicament2) && !string.IsNullOrEmpty(cant2)) ? $"{medicament2}({cant2}buc.)" : null;
            string med3 = (!string.IsNullOrEmpty(medicament3) && !string.IsNullOrEmpty(cant3)) ? $"{medicament3}({cant3}buc.)" : null;
            string med4 = (!string.IsNullOrEmpty(medicament4) && !string.IsNullOrEmpty(cant4)) ? $"{medicament4}({cant4}buc.)" : null;
            string med5 = (!string.IsNullOrEmpty(medicament5) && !string.IsNullOrEmpty(cant5)) ? $"{medicament5}({cant5}buc.)" : null;

            List<string> m = new List<string>();
            if (med1 != null) m.Add(med1);
            if (med2 != null) m.Add(med2);
            if (med3 != null) m.Add(med3);
            if (med4 != null) m.Add(med4);
            if (med5 != null) m.Add(med5);

            Paragraph titlu = new Paragraph("Reteta");
            titlu.Alignment = Element.ALIGN_CENTER;

            doc.Open();
            doc.Add(titlu);
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph("Nume medic: " + numeMedic));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph("Nume pacient: " + numePacient));
            doc.Add(new Paragraph("Prenume pacient: " + prenumePacient));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph("Diagnostic: " + diagnostic));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph("Medicament: " + String.Join(", ", m)));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph("Tip: " + (tipReteta.Equals("C") ? "Compensata" : tipReteta.Equals("G") ? "Gratuita" : "Normala")));
            doc.Close();

            writer.Flush();

            return memoryStream.GetBuffer();
        }
    }
}