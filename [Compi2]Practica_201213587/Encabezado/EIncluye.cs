using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Practica_201213587.Encabezado
{
    class EIncluye
    {
        public String Ruta { get; set; }
        public String Archivo { get; set; }

        public EIncluye(String archivo, String ruta)
        {
            this.Archivo = archivo;
            this.Ruta = ruta;
        }
        
    }
}
