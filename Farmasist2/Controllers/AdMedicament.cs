//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace Farmasist2.Controllers
//{
//    public class AdMedicament
//    {
//    }
//    public partial class Admedicament : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {

//        }
//        public static bool SalveazaMedicament(bool cb1nurofen, bool cb2colebil, bool cb3triferment, bool cb4albocalmin)
//        {
//            bool status;
//            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand("RezervareMedicament", con))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@CheckboxOne", cb1nurofen);
//                    cmd.Parameters.AddWithValue("@CheckboxTwo", cb2colebil);
//                    cmd.Parameters.AddWithValue("@CheckboxThree", cb3triferment);
//                    cmd.Parameters.AddWithValue("@CheckboxFour", cb4albocalmin);
//                    if (con.State == ConnectionState.Closed)
//                    {
//                        con.Open();
//                    }
//                    Int32 cantitate = cmd.ExecuteNonQuery();
//                    if (cantitate > 0)
//                    {
//                        status = true;
//                    }
//                    else
//                    {
//                        status = false;
//                    }
//                    return status;
//                }
//            }
//        }
//    }
//}