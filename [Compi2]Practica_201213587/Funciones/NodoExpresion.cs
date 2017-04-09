using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Compi2_Practica_201213587.Funciones;
using Irony.Parsing;

namespace _Compi2_Practica_201213587.Funciones
{
    class NodoExpresion
    {
        public NodoExpresion Derecha { get; set; }
        public NodoExpresion Izquierda { get; set; }
        public String Tipo { get; set; }
        public String Nombre { get; set; }
        public Double Numero { get; set; }
        public String Cadena { get; set; }
        public FLlamada Llamada { get; set; }
        public Boolean Booleano { get; set; }
        public int Linea { get; set; }
        public int Columna { get; set; }

        public NodoExpresion(NodoExpresion Nodo)
        {
            this.Booleano = Nodo.Booleano;
            this.Cadena = Nodo.Cadena;
            this.Columna = Nodo.Columna;
            this.Derecha = Nodo.Derecha;
            this.Izquierda = Nodo.Izquierda;
            this.Linea = Nodo.Linea;
            this.Llamada = Nodo.Llamada;
            this.Nombre = Nodo.Nombre;
            this.Numero = Nodo.Numero;
            this.Tipo = Nodo.Tipo;
        }

        public String GenerarNodoGraphviz(int pos)
        {
            String cadena = "";
            if (Tipo == Constante.TNumber)
            {
                cadena = Tipo + ":   " + Numero.ToString();
            }else if(Tipo == Constante.TString)
            {
                cadena = Tipo + ":   " + Cadena;
            }
            else if (Tipo == Constante.TBool)
            {
                cadena = Tipo + ":   " + Booleano.ToString();
            }
            else if (Tipo == Constante.Id)
            {
                cadena = Tipo + ":   " + Nombre;
            }
            else if(Tipo == Constante.TMetodo)
            {
                cadena = Tipo + ":   " + Nombre;
            }else if(Tipo == Constante.TMas || Tipo == Constante.TMenos  || Tipo == Constante.TPor || Tipo == Constante.TDivision || Tipo == Constante.TModulo || Tipo == Constante.TPotencia)
            {
                cadena = "Aritm:   " + Tipo;
            }else if (Tipo == Constante.TMenor || Tipo == Constante.TMayor || Tipo == Constante.TMenorIgual || Tipo == Constante.TMayorIgual || Tipo == Constante.TIgualacion || Tipo == Constante.TDiferente || Tipo == Constante.TSemejante)
            {
                cadena = "Rela:   " + Tipo;
            }else if ( Tipo == Constante.TOr || Tipo == Constante.TXor || Tipo == Constante.TAnd || Tipo == Constante.TNot)
            {
                cadena = "Log:   " + Tipo;
            }
            return "\t\tnodo"+pos.ToString() + "[label=\"" + cadena + "\"]\n";
        }
        public NodoExpresion(String nombre, String tipo, NodoExpresion izq, NodoExpresion der, Object valor, int linea, int columna)
        {
            this.Nombre = nombre;
            this.Derecha = der;
            this.Izquierda = izq;
            this.Tipo = tipo;
            this.Linea = linea;
            this.Columna = columna;
            

            switch (Tipo)
            {
                case Constante.TNumber:
                    NodoExpresion auxn = (NodoExpresion)valor;

                    this.Booleano = auxn.Booleano;
                    this.Cadena = auxn.Cadena;
                    this.Columna = auxn.Columna;
                    this.Derecha = auxn.Derecha;
                    this.Izquierda = auxn.Izquierda;
                    this.Linea = auxn.Linea;
                    this.Llamada = auxn.Llamada;
                    this.Nombre = auxn.Nombre;
                    this.Numero = auxn.Numero;
                    this.Tipo = auxn.Tipo;
                    break;

                case Constante.Numero:
                    Numero = Double.Parse(((ParseTreeNode)valor).Token.ValueString);
                    Tipo = Constante.TNumber;
                    Cadena = ((ParseTreeNode)valor).Token.ValueString;
                    this.Linea = ((ParseTreeNode)valor).Token.Location.Line + 1;
                    this.Columna = ((ParseTreeNode)valor).Token.Location.Column + 1;
                    break;

                case Constante.TString:
                    NodoExpresion auxs = (NodoExpresion)valor;

                    this.Booleano = auxs.Booleano;
                    this.Cadena = auxs.Cadena;
                    this.Columna = auxs.Columna;
                    this.Derecha = auxs.Derecha;
                    this.Izquierda = auxs.Izquierda;
                    this.Linea = auxs.Linea;
                    this.Llamada = auxs.Llamada;
                    this.Nombre = auxs.Nombre;
                    this.Numero = auxs.Numero;
                    this.Tipo = auxs.Tipo;
                    break;

                case Constante.Cadena:
                    this.Cadena = ((ParseTreeNode)valor).Token.ValueString;
                    Tipo = Constante.TString;
                    this.Linea = ((ParseTreeNode)valor).Token.Location.Line + 1;
                    this.Columna = ((ParseTreeNode)valor).Token.Location.Column + 1;
                    break;

                case Constante.TBool:
                    NodoExpresion auxb = (NodoExpresion)valor;

                    this.Booleano = auxb.Booleano;
                    this.Cadena = auxb.Cadena;
                    this.Columna = auxb.Columna;
                    this.Derecha = auxb.Derecha;
                    this.Izquierda = auxb.Izquierda;
                    this.Linea = auxb.Linea;
                    this.Llamada = auxb.Llamada;
                    this.Nombre = auxb.Nombre;
                    this.Numero = auxb.Numero;
                    this.Tipo = auxb.Tipo;
                    break;

                case Constante.TVerdadero:
                    Booleano = true;
                    Tipo = Constante.TBool;
                    Numero = 1;
                    Cadena = "1";
                    this.Linea = ((ParseTreeNode)valor).Token.Location.Line + 1;
                    this.Columna = ((ParseTreeNode)valor).Token.Location.Column + 1;
                    break;

                case Constante.TFalso:
                    Booleano = false;
                    Tipo = Constante.TBool;
                    Numero = 0;
                    Cadena = "0";
                    this.Linea = ((ParseTreeNode)valor).Token.Location.Line + 1;
                    this.Columna = ((ParseTreeNode)valor).Token.Location.Column + 1;
                    break;

                case Constante.Id:
                    Cadena = valor.ToString();
                    Tipo = Constante.Id;
                    this.Linea = ((ParseTreeNode)valor).Token.Location.Line + 1;
                    this.Columna = ((ParseTreeNode)valor).Token.Location.Column + 1;
                    break;

                case Constante.TMetodo:
                    FLlamada fllamadafuncion = new FLlamada();

                    ParseTreeNode Nodo = (ParseTreeNode)valor;
                    //enviamos a recorrer si tiene parametros
                    if (Nodo.ChildNodes[1].ChildNodes.Count > 0)
                    {
                        foreach (ParseTreeNode hijo in Nodo.ChildNodes[1].ChildNodes[0].ChildNodes)
                        {
                            fllamadafuncion.Parametros.Add(new FExpresion(hijo));
                        }
                    }
                    Llamada = fllamadafuncion;
                    this.Linea = Nodo.ChildNodes[0].Token.Location.Line + 1;
                    this.Columna = Nodo.ChildNodes[0].Token.Location.Column + 1;
                    break;
            }

        }
    }
}
