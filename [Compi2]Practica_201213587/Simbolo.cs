using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Practica_201213587
{
    class Simbolo
    {
        public String Nombre { get; set; }
        public String Rol { get; set; }
        public String Tipo { get; set; }
        public String RutaArchivo { get; set; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        public Ambito Ambito { get; set; }
        public Object Objeto { get; set; }

        public Simbolo(String nombre, String rol, String tipo, int fila, int columna, Ambito ambito, Object objeto)
        {
            this.Nombre = nombre;
            this.Rol = rol;
            this.Tipo = tipo;
            this.Fila = fila;
            this.Columna = columna;
            this.Ambito = ambito;
            this.Objeto = objeto;
            this.RutaArchivo = "";
        }
    }
}
