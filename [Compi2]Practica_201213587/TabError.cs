using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace _Compi2_Practica_201213587
{
    class TabError : DataTable
    {
        private int contador;

        public TabError()
        {
            this.contador = 1;
            this.Columns.Add("No.");
            this.Columns.Add("Tipo");
            this.Columns.Add("Descripcion");
            this.Columns.Add("Archivo");
            this.Columns.Add("Linea");
            this.Columns.Add("Columna");
        }

        public void InsertarFila(String Tipo, String Descripcion, String Archivo, String Linea, String Columna)
        {
            this.Rows.Add(contador.ToString(), Tipo, Descripcion, Archivo, Linea, Columna);
            contador++;
        }

        public DataTable getTabla()
        {
            return this;
        }
    }


}
