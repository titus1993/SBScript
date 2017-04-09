using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Practica_201213587.Ejecucion
{
    class Variables
    {
        public String Tipo { get; set; }
        public String Nombre { get; set; }

        public String Rol { get; set; }

        public int Linea { get; set; }

        public int Columna { get; set; }

        public Object Valor { get; set; }

        public Ambito Ambito {get; set; }
        public Variables()
        {
            Tipo = "";
            Nombre = "";
            Rol = "";
            Valor = null;
            Linea = 0;
            Columna = 0;
        }

        public Variables(String tipo, String nombre, string rol, object valor, Ambito ambito, int linea, int columna)
        {
            Tipo = tipo;
            Nombre = nombre;
            Rol = rol;
            Valor = valor;
            Ambito = ambito;
            Linea = linea;
            Columna = columna;
        }
    }
}
