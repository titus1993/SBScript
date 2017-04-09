using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Practica_201213587.Funciones
{
    class FCaso
    {
        public Double ValNumero { get; set; }
        public String ValCadena { get; set; }
        public String Tipo { get; set; }
        public Ambito ambito { get; set; }
        public String RutaImagen { get; set; }
        public Double Incerteza { get; set; }
        public String RutaArchivo { get; set; }

        public FCaso(Ambito amb, String tipo, Object valor)
        {
            this.ambito = amb;
            this.Tipo = tipo;

            switch (this.Tipo)
            {
                case Constante.TNumber:
                    this.ValNumero = Double.Parse(valor.ToString());
                    break;

                case Constante.TString:
                    this.ValCadena = valor.ToString();
                    break;
            }
        }
    }
}
