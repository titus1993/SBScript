﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Practica_201213587.Funciones
{
    class FPara
    {
        public String RutaArchivo { get; set; }
        public String RutaImagen { get; set; }
        public Double Incerteza { get; set; }
        public FExpresion Condicion { get; set; }
        public Simbolo Declaracion { get; set; }
        public String Operacion { get; set; }
        public Ambito ambito { get; set; }


    }
}
