using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Practica_201213587.Funciones
{    
    class FFuncion
    {
        public Ambito Ambito { get; set; }
        public List<Simbolo> Parametros { get; set; }
        public String RutaImagen { get; set; }
        public Double Incerteza { get; set; }
        public String RutaArchivo { get; set; }
        public FFuncion()
        {
            Parametros = new List<Simbolo>();
        }
    }
}
