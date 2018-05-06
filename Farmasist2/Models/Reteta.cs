using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farmasist2.Models
{
    public class Reteta
    {
        public string Nume_medic { get; set; }
        public string Nume_pacient { get; set; }
        public string Prenume_pacient { get; set; }
        public string Diagnostic { get; set; }
        public string Medicament1 { get; set; }
        public int Cant1 { get; set; }
        public string Medicament2 { get; set; }
        public int Cant2 { get; set; }
        public string Medicament3 { get; set; }
        public int Cant3 { get; set; }
        public string Medicament4 { get; set; }
        public int Cant4 { get; set; }
        public string Medicament5 { get; set; }
        public int Cant5 { get; set; }
        public string TipReteta { get; set; }
        public string Observatie { get; set; }
    }
}