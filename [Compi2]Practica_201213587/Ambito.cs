using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Practica_201213587
{
    class Ambito
    {
        public List<Simbolo> TablaSimbolo { get; set; }
        public String Nombre { get; set; }
        public Ambito Padre { get; set; }
        public Double Incerteza { get; set; }
        public String RutaImagenes { get; set; }
        public String RutaArchivo { get; set; }
        public Ambito(String nombre)
        {
            this.Nombre = nombre;
            this.Padre = null;
            this.TablaSimbolo = new List<Simbolo>();
            this.Incerteza = Constante.DefaultDefineNumber;
            this.RutaImagenes = Constante.DefaultDefineRuta;
        }
    }
}
