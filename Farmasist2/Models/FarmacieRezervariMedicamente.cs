using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farmasist2.Models
{
    public class FarmacieRezervariMedicamente
    {
        public int IDFarmacie { set; get; }
        public int IDMedicament { set; get; }
        public double Concentratie { set; get; }
        public string DenumireComerciala { set; get; }
    }
}