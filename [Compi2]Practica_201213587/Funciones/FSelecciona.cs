using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Practica_201213587.Funciones
{
    class FSelecciona
    {
        public String RutaArchivo { get; set; }
        public String RutaImagen { get; set; }
        public Double Incerteza { get; set; }
        public Ambito Ambito { get; set; }
        public List<FCaso> Casos { get; set; }
        public FCaso Defecto { get; set; }
        public FExpresion Expresion { get; set; }
    }
}
