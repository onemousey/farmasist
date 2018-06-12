using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farmasist2.Models
{
    public class Pacient
    {
        public string CNP { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Adresa { get; set; }
        public string Grupa_Sanguina { get; set; }
        public string Alergii { get; set; }

    }
}