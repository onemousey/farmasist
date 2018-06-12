using Farmasist2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace Farmasist2.Controllers
{
    public class FarmacieController : Controller
    {
        [HttpGet]
        public JsonResult GetMedicamente()
        {
            try
            {
                var db = new DataAccessSQL();
                var v = db.getCereriMedicamente();

                //System.Threading.Thread.Sleep(5000);

                return Json(new { IsError = false, Message = " ", CereriMedicamenteAcceptate = v }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(new { IsError = true, Message = exc.Message, CereriMedicamenteAcceptate = new List<DataAccessSQL>() }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}