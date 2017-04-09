using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _Compi2_Practica_201213587.Funciones
{
    class FLlamada
    {
        public Ambito Ambito { get; set; }
        public String RutaImagen { get; set; }
        public Double Incerteza { get; set; }
        public String RutaArchivo { get; set; }
        public List<FExpresion> Parametros { get; set; }
        public FLlamada()
        {
            Parametros = new List<FExpresion>();
        }
    }
}
