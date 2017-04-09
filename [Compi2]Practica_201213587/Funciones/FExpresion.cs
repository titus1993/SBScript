using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Compi2_Practica_201213587.Ejecucion;

namespace _Compi2_Practica_201213587.Funciones
{
    class FExpresion
    {
        public String RutaImagen { get; set; }
        public Double Incerteza { get; set; }
        NodoExpresion Raiz { get; set; }
        public String RutaArchivo { get; set; }

        public FExpresion(ParseTreeNode arbol)
        {
            Raiz = GenerarArbol(arbol);
        }

        public String GenerarArbolGraphics()
        {
            int i = 0;
            String cadena = "digraph G{\n\tgraph[rankir = \"LR\"];\n\tnode[shape = box, fontsize = 16, fontname = \"Arial\", style = filled, fillcolor = grey88];\n\t" + GenerarArbolGraphics(ref i, Raiz) + "\n}";
            return cadena;
        }

        private String GenerarArbolGraphics(ref int pos, NodoExpresion nodo)
        {
            String cadena = "";
            int actual = pos;
            
            cadena = nodo.GenerarNodoGraphviz(actual);
            
            if (nodo.Izquierda != null)
            {
                pos++;
                int actualiz = pos;
                cadena = cadena + GenerarArbolGraphics(ref pos, nodo.Izquierda) + "\t\tnodo" + actual.ToString() + "->" + "nodo"+actualiz.ToString()+"\n";
            }

           
            if (nodo.Derecha != null)
            {
                pos++;
                int actualiz = pos;
                cadena = cadena + GenerarArbolGraphics(ref pos, nodo.Derecha) + "\t\tnodo" + actual.ToString() + "->" + "nodo" + actualiz.ToString() + "\n";
            }
            return cadena;
        }

        private NodoExpresion GenerarArbol(ParseTreeNode Nodo)
        {
            if (Nodo.ChildNodes.Count == 3)
            {
                return new NodoExpresion(Nodo.ChildNodes[1].Token.ValueString, Nodo.ChildNodes[1].Token.ValueString, GenerarArbol(Nodo.ChildNodes[0]), GenerarArbol(Nodo.ChildNodes[2]), null, Nodo.ChildNodes[1].Token.Location.Line + 1, Nodo.ChildNodes[1].Token.Location.Column + 1);
            }
            else if (Nodo.ChildNodes.Count == 2)
            {
                if (Nodo.Term.Name == Constante.LLAMADA_FUNCION)
                {
                    return new NodoExpresion(Nodo.ChildNodes[0].Token.ValueString, Constante.TMetodo, null, null, Nodo, 0, 0);
                        //nombre, tipo, iz, der, valor
                }else
                {
                    return new NodoExpresion(Nodo.ChildNodes[0].Token.ValueString, Nodo.ChildNodes[0].Token.ValueString, GenerarArbol(Nodo.ChildNodes[1]), null, null, Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Line + 1);
                }
                
            }else if (Nodo.ChildNodes.Count ==1)
            {
                return GenerarArbol(Nodo.ChildNodes[0]);
            }
            else
            { 
                return new NodoExpresion(Nodo.Token.ValueString, Nodo.Term.Name, null, null, Nodo,0,0);                      
            }
        }

        
        public NodoExpresion ResolverExpresion(Double incerteza, String rutaarchivo)
        {
            //RutaArchivo = rutaarchivo;
            //Incerteza = incerteza;
            NodoExpresion aux = ResolverExpresion(Raiz);
            if (aux.Tipo == Constante.TError)
            {
                TabError error = new TabError();
                error.InsertarFila(Constante.ErroEjecucion, aux.Cadena, RutaArchivo, aux.Linea.ToString(), aux.Columna.ToString());
                TitusNotifiaciones.setDatosErrores(error);
            }
            return aux;
        }

        private NodoExpresion ResolverExpresion(NodoExpresion Nodo)
        {
            NodoExpresion der = new NodoExpresion("Error inesperado", Constante.TError, null, null, null, Nodo.Linea, Nodo.Columna);
            der.Cadena = "Error inesperado";
            der.Tipo = Constante.TError;
            der.Linea = Nodo.Linea;
            der.Columna = Nodo.Columna;

            if (TitusNotifiaciones.ContarErrores() == 0)
            {
                switch (Nodo.Tipo)
                {
                    case Constante.TMetodo:
                        NodoExpresion auxm = new NodoExpresion(Nodo.Nombre, "", null, null, null, 0, 0);
                        Variables metodo = TablaVariables.BuscarMetodo(Nodo.Nombre, Nodo.Llamada);
                        if (metodo != null)
                        {
                            FFuncion funcion = (FFuncion)metodo.Valor;

                            if (metodo.Tipo != Constante.TVoid)
                            {
                                if (funcion.Parametros.Count == Nodo.Llamada.Parametros.Count)
                                {
                                    //metemos el return 
                                    Variables retorno = new Variables(Constante.TRetorno, Constante.TRetorno, Constante.RETORNO, null, null, 0, 0);
                                    TablaVariables.Tabla.Add(retorno);
                                    int cont = 0;
                                    //meter variables de los parametros
                                    while (cont < funcion.Parametros.Count && TitusNotifiaciones.ContarErrores() == 0)
                                    {
                                        NodoExpresion resultadoparametro = Nodo.Llamada.Parametros[cont].ResolverExpresion(funcion.Incerteza, funcion.RutaArchivo);
                                        if (TitusNotifiaciones.ContarErrores() == 0)
                                        {
                                            if (funcion.Parametros[cont].Tipo == resultadoparametro.Tipo)
                                            {
                                                Variables parametro = new Variables(funcion.Parametros[cont].Tipo, funcion.Parametros[cont].Nombre, Constante.TVariable, resultadoparametro, null, Nodo.Linea, Nodo.Columna);
                                                TablaVariables.Tabla.Add(parametro);
                                            }
                                            else
                                            {
                                                //error de asignacion del tipo de parametro
                                                auxm.Cadena = "Se esperaba un tipo: " + funcion.Parametros[cont].Tipo + ", no un tipo: " + resultadoparametro.Tipo;
                                                auxm.Tipo = Constante.TError;
                                                auxm.Linea = resultadoparametro.Linea;
                                                auxm.Columna = resultadoparametro.Columna;
                                            }
                                        }
                                        cont++;
                                    }

                                    //ejecuatamos el metodo
                                    if (TitusNotifiaciones.ContarErrores() == 0)
                                    {
                                        Ejecutar execute = new Ejecutar();
                                        execute.EjecutarInstrucciones(metodo.Ambito.TablaSimbolo);                                       
                                        
                                        //obtenemos el valor del return

                                        if (TablaVariables.IsRetorno())
                                        {
                                            Variables retorno = TablaVariables.ObtenerTope();
                                            if (retorno.Valor != null)
                                            {
                                                NodoExpresion resultado = new NodoExpresion(((NodoExpresion)retorno.Valor));
                                                //comprobamos que el tipo del metodo sea el mismo que el retorno
                                                if (metodo.Tipo == resultado.Tipo)
                                                {
                                                    auxm = new NodoExpresion(resultado);
                                                }
                                                else
                                                {
                                                    auxm.Cadena = "La funcion es de tipo " + metodo.Tipo + " no puede retornar un valor " + resultado.Tipo;
                                                    auxm.Tipo = Constante.TError;
                                                    auxm.Linea = retorno.Linea;
                                                    auxm.Columna = retorno.Columna;
                                                }
                                            }else
                                            {
                                                auxm.Cadena = "Se utiliza retorno pero este no devolvio ningun";
                                                auxm.Tipo = Constante.TError;
                                                auxm.Linea = retorno.Linea;
                                                auxm.Columna = retorno.Columna;
                                            }
                                        }
                                        else
                                        {
                                            //error porque el metodo no devuelve nada
                                            auxm.Cadena = "El metodo no a retornado nada";
                                            auxm.Tipo = Constante.TError;
                                            auxm.Linea = Nodo.Linea;
                                            auxm.Columna = Nodo.Columna;
                                        }
                                        TablaVariables.SacarVariable();
                                        execute.SacarAmbito(metodo.Ambito.TablaSimbolo);
                                        execute.SacarAmbito(funcion.Parametros);
                                    }
                                }
                                else
                                {
                                    //error de cantidad de parametros
                                    auxm.Cadena = "La funcion esperaba " + funcion.Parametros.Count + " parametros";
                                    auxm.Tipo = Constante.TError;
                                    auxm.Linea = Nodo.Linea;
                                    auxm.Columna = Nodo.Columna;
                                }
                            }
                            else
                            {
                                auxm.Cadena = "Llamada a funcion void";
                                auxm.Tipo = Constante.TError;
                                auxm.Linea = Nodo.Linea;
                                auxm.Columna = Nodo.Columna;
                            }

                        }else
                        {
                            auxm.Cadena = "No existe la funcion " + Nodo.Nombre + "()";
                            auxm.Tipo = Constante.TError;
                            auxm.Linea = Nodo.Linea;
                            auxm.Columna = Nodo.Columna;
                        }
                        return auxm;

                    case Constante.Id:
                        Variables aux = TablaVariables.BuscarVariable(Nodo.Nombre);
                        NodoExpresion auxn = new NodoExpresion(Nodo.Nombre, "", null, null, null,0,0);
                        if (aux != null)
                        {
                            NodoExpresion valor = (NodoExpresion)aux.Valor;
                            if (valor != null)
                            {
                                auxn = new NodoExpresion(valor);
                            }
                            else
                            {
                                auxn.Cadena = "NullPointerExeption: " + Nodo.Nombre;
                                auxn.Tipo = Constante.TError;
                                auxn.Linea = Nodo.Linea;
                                auxn.Columna = Nodo.Columna;
                                
                            }
                        }
                        else
                        {
                            auxn.Cadena = "Variable no declarada: " + Nodo.Nombre;
                            auxn.Tipo = Constante.TError;
                            auxn.Linea = Nodo.Linea;
                            auxn.Columna = Nodo.Columna;
                            
                        }
                        return auxn;

                    case Constante.TNumber:
                        return new NodoExpresion(Nodo.Nombre, Nodo.Tipo, null, null, Nodo, Nodo.Linea, Nodo.Columna);

                    case Constante.TString:
                        return new NodoExpresion(Nodo.Nombre, Nodo.Tipo, null, null, Nodo, Nodo.Linea, Nodo.Columna);

                    case Constante.TBool:
                        return new NodoExpresion(Nodo.Nombre, Nodo.Tipo, null, null, Nodo, Nodo.Linea, Nodo.Columna);

                    case Constante.TMas:
                        return Suma(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha));

                    case Constante.TMenos:
                        if (Nodo.Derecha != null)
                        {
                            return Resta(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);
                        }
                        else
                        {
                            return Resta(null, ResolverExpresion(Nodo.Izquierda), Nodo.Linea, Nodo.Columna);
                        }

                    case Constante.TPor:
                        return Multiplicacion(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TDivision:
                        return Division(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TModulo:
                        return Modulo(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TPotencia:
                        return Potencia(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TIgualacion:
                        return Igual(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TDiferente:
                        return Diferente(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TMenor:
                        return Menor(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TMayor:
                        return Mayor(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TMenorIgual:
                        return MenorIgual(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TMayorIgual:
                        return MayorIgual(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TSemejante:
                        return Semejante(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TAnd:
                        return And(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TOr:
                        return Or(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TXor:
                        return Xor(ResolverExpresion(Nodo.Izquierda), ResolverExpresion(Nodo.Derecha), Nodo.Linea, Nodo.Columna);

                    case Constante.TNot:
                        return Not(ResolverExpresion(Nodo.Izquierda), Nodo.Linea, Nodo.Columna);

                    default:
                        
                        return der;
                }
            }else
            {
                return der;
            }
            
        }

        private NodoExpresion Suma(NodoExpresion iz, NodoExpresion der)
        {
            if (iz.Tipo == Constante.TBool)
            {
                if (der.Tipo == Constante.TBool)
                {
                    der.Booleano = iz.Booleano || der.Booleano;
                    if (der.Booleano)
                    {
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TBool;
                    der.Nombre = Constante.TBool;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    der.Numero = iz.Numero + der.Numero;
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TNumber;
                    der.Nombre = Constante.TNumber;
                }
                else if (der.Tipo == Constante.TString)
                {
                    der.Cadena = iz.Cadena + der.Cadena;
                    der.Tipo = Constante.TString;
                    der.Nombre = Constante.TString;
                }
            }
            else if (iz.Tipo == Constante.TNumber)
            {
                if (der.Tipo == Constante.TBool)
                {
                    der.Numero = iz.Numero + der.Numero;
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TNumber;
                    der.Nombre = Constante.TNumber;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    der.Numero = iz.Numero + der.Numero;
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TNumber;
                    der.Nombre = Constante.TNumber;
                }
                else if (der.Tipo == Constante.TString)
                {
                    der.Cadena = iz.Cadena + der.Cadena;
                    der.Tipo = Constante.TString;
                    der.Nombre = Constante.TString;
                }
            }
            else if (iz.Tipo == Constante.TString)
            {
                if (der.Tipo == Constante.TBool)
                {
                    der.Cadena = iz.Cadena + der.Cadena;
                    der.Tipo = Constante.TString;
                    der.Nombre = Constante.TString;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    der.Cadena = iz.Cadena + der.Cadena;
                    der.Tipo = Constante.TString;
                    der.Nombre = Constante.TString;
                }
                else if (der.Tipo == Constante.TString)
                {
                    der.Cadena = iz.Cadena + der.Cadena;
                    der.Tipo = Constante.TString;
                    der.Nombre = Constante.TString;
                }
            }
            else//error
            {
                der.Cadena = iz.Cadena;
                der.Tipo = iz.Tipo;
                der.Nombre = iz.Nombre;
                der.Linea = iz.Linea;
                der.Columna = der.Columna;
            }
            return der;
        }

        private NodoExpresion Resta(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            if (iz != null)
            {
                if (iz.Tipo == Constante.TBool)
                {
                    if (der.Tipo == Constante.TBool)
                    {
                        der.Cadena = "No se puede restar " + iz.Tipo + " - " + der.Tipo;
                        der.Nombre = Constante.TError;
                        der.Tipo = Constante.TError;
                        der.Linea = linea;
                        der.Columna = columna;
                    }
                    else if (der.Tipo == Constante.TNumber)
                    {
                        der.Numero = iz.Numero - der.Numero;
                        der.Cadena = der.Numero.ToString();
                        der.Tipo = Constante.TNumber;
                        der.Nombre = Constante.TNumber;
                    }
                    else if (der.Tipo == Constante.TString)
                    {
                        der.Cadena = "No se puede restar " + iz.Tipo + " - " + der.Tipo;
                        der.Nombre = Constante.TError;
                        der.Tipo = Constante.TError;
                        der.Linea = linea;
                        der.Columna = columna;
                    }
                }
                else if (iz.Tipo == Constante.TNumber)
                {
                    if (der.Tipo == Constante.TBool)
                    {
                        der.Numero = iz.Numero - der.Numero;
                        der.Cadena = der.Numero.ToString();
                        der.Tipo = Constante.TNumber;
                        der.Nombre = Constante.TNumber;
                    }
                    else if (der.Tipo == Constante.TNumber)
                    {
                        der.Numero = iz.Numero - der.Numero;
                        der.Cadena = der.Numero.ToString();
                        der.Tipo = Constante.TNumber;
                        der.Nombre = Constante.TNumber;
                    }
                    else if (der.Tipo == Constante.TString)
                    {
                        der.Cadena = "No se puede restar " + iz.Tipo + " - " + der.Tipo;
                        der.Nombre = Constante.TError;
                        der.Tipo = Constante.TError;
                        der.Linea = linea;
                        der.Columna = columna;
                    }
                }
                else if (iz.Tipo == Constante.TString)
                {
                    if (der.Tipo == Constante.TBool)
                    {
                        der.Cadena = "No se puede restar " + iz.Tipo + " - " + der.Tipo;
                        der.Nombre = Constante.TError;
                        der.Tipo = Constante.TError;
                        der.Linea = linea;
                        der.Columna = columna;
                    }
                    else if (der.Tipo == Constante.TNumber)
                    {
                        der.Cadena = "No se puede restar " + iz.Tipo + " - " + der.Tipo;
                        der.Nombre = Constante.TError;
                        der.Tipo = Constante.TError;
                        der.Linea = linea;
                        der.Columna = columna;
                    }
                    else if (der.Tipo == Constante.TString)
                    {
                        der.Cadena = "No se puede restar " + iz.Tipo + " - " + der.Tipo;
                        der.Nombre = Constante.TError;
                        der.Tipo = Constante.TError;
                        der.Linea = linea;
                        der.Columna = columna;
                    }
                }
                else//error
                {
                    der.Cadena = iz.Cadena;
                    der.Tipo = iz.Tipo;
                    der.Nombre = iz.Nombre;
                    der.Linea = iz.Linea;
                    der.Columna = der.Columna;
                }
            }
            else//cuando es el operador unario
            {
                if (der.Tipo == Constante.TNumber)
                {
                    der.Numero = -der.Numero;
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TNumber;
                    der.Nombre = Constante.TNumber;
                }
                else
                {
                    der.Cadena = "No se puede usar el operador - con: " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }

            return der;
        }

        private NodoExpresion Multiplicacion(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            if (iz.Tipo == Constante.TBool)
            {
                if (der.Tipo == Constante.TBool)
                {
                    der.Booleano = iz.Booleano && der.Booleano;
                    if (der.Booleano)
                    {
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TBool;
                    der.Nombre = Constante.TBool;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    der.Numero = iz.Numero * der.Numero;
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TNumber;
                    der.Nombre = Constante.TNumber;
                }
                else if (der.Tipo == Constante.TString)
                {
                    der.Cadena = "No se puede multiplicar " + iz.Tipo + " * " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TNumber)
            {
                if (der.Tipo == Constante.TBool)
                {
                    der.Numero = iz.Numero * der.Numero;
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TNumber;
                    der.Nombre = Constante.TNumber;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    der.Numero = iz.Numero * der.Numero;
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TNumber;
                    der.Nombre = Constante.TNumber;
                }
                else if (der.Tipo == Constante.TString)
                {
                    der.Cadena = "No se puede multiplicar " + iz.Tipo + " * " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TString)
            {
                der.Cadena = "No se puede multiplicar " + iz.Tipo + " * " + der.Tipo;
                der.Nombre = Constante.TError;
                der.Tipo = Constante.TError;
                der.Linea = linea;
                der.Columna = columna;
            }
            else//error
            {
                der.Cadena = iz.Cadena;
                der.Tipo = iz.Tipo;
                der.Nombre = iz.Nombre;
                der.Linea = iz.Linea;
                der.Columna = der.Columna;
            }
            return der;
        }

        private NodoExpresion Division(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            if (iz.Tipo == Constante.TBool)
            {
                if (der.Tipo == Constante.TBool)
                {
                    der.Cadena = "No se puede dividir " + iz.Tipo + " / " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    if (der.Numero != 0)
                    {
                        der.Numero = iz.Numero / der.Numero;
                        der.Cadena = der.Numero.ToString();
                        der.Tipo = Constante.TNumber;
                        der.Nombre = Constante.TNumber;
                    }
                    else
                    {
                        der.Cadena = "Division por 0: " + iz.Tipo + " / " + der.Tipo;
                        der.Nombre = Constante.TError;
                        der.Tipo = Constante.TError;
                        der.Linea = linea;
                        der.Columna = columna;
                    }
                }
                else if (der.Tipo == Constante.TString)
                {
                    der.Cadena = "No se puede dividir " + iz.Tipo + " / " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TNumber)
            {
                if (der.Tipo == Constante.TBool)
                {
                    if (der.Numero != 0)
                    {
                        der.Numero = iz.Numero / der.Numero;
                        der.Cadena = der.Numero.ToString();
                        der.Tipo = Constante.TNumber;
                        der.Nombre = Constante.TNumber;
                    }
                    else
                    {
                        der.Cadena = "Division por 0: " + iz.Tipo + " / " + der.Tipo;
                        der.Nombre = Constante.TError;
                        der.Tipo = Constante.TError;
                        der.Linea = linea;
                        der.Columna = columna;
                    }
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    if (der.Numero != 0)
                    {
                        der.Numero = iz.Numero / der.Numero;
                        der.Cadena = der.Numero.ToString();
                        der.Tipo = Constante.TNumber;
                        der.Nombre = Constante.TNumber;
                    }
                    else
                    {
                        der.Cadena = "Division por 0: " + iz.Tipo + " / " + der.Tipo;
                        der.Nombre = Constante.TError;
                        der.Tipo = Constante.TError;
                        der.Linea = linea;
                        der.Columna = columna;
                    }
                }
                else if (der.Tipo == Constante.TString)
                {
                    der.Cadena = "No se puede dividir " + iz.Tipo + " / " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TString)
            {
                der.Cadena = "No se puede dividir " + iz.Tipo + " / " + der.Tipo;
                der.Nombre = Constante.TError;
                der.Tipo = Constante.TError;
                der.Linea = linea;
                der.Columna = columna;
            }
            else//error
            {
                der.Cadena = iz.Cadena;
                der.Tipo = iz.Tipo;
                der.Nombre = iz.Nombre;
                der.Linea = iz.Linea;
                der.Columna = der.Columna;
            }
            return der;
        }

        private NodoExpresion Modulo(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            if (iz.Tipo == Constante.TBool)
            {
                if (der.Tipo == Constante.TBool)
                {
                    der.Cadena = "No se puede dividir " + iz.Tipo + " / " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    if (der.Numero != 0)
                    {
                        der.Numero = iz.Numero % der.Numero;
                        der.Cadena = der.Numero.ToString();
                        der.Tipo = Constante.TNumber;
                        der.Nombre = Constante.TNumber;
                    }
                    else
                    {
                        der.Cadena = "Division por 0: " + iz.Tipo + " / " + der.Tipo;
                        der.Nombre = Constante.TError;
                        der.Tipo = Constante.TError;
                        der.Linea = linea;
                        der.Columna = columna;
                    }
                }
                else if (der.Tipo == Constante.TString)
                {
                    der.Cadena = "No se puede dividir " + iz.Tipo + " / " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TNumber)
            {
                if (der.Tipo == Constante.TBool)
                {
                    if (der.Numero != 0)
                    {
                        der.Numero = iz.Numero % der.Numero;
                        der.Cadena = der.Numero.ToString();
                        der.Tipo = Constante.TNumber;
                        der.Nombre = Constante.TNumber;
                    }
                    else
                    {
                        der.Cadena = "Division por 0: " + iz.Tipo + " / " + der.Tipo;
                        der.Nombre = Constante.TError;
                        der.Tipo = Constante.TError;
                        der.Linea = linea;
                        der.Columna = columna;
                    }
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    der.Numero = iz.Numero % der.Numero;
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TNumber;
                    der.Nombre = Constante.TNumber;
                }
                else if (der.Tipo == Constante.TString)
                {
                    der.Cadena = "No se puede dividir " + iz.Tipo + " / " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TString)
            {
                der.Cadena = "No se puede dividir " + iz.Tipo + " / " + der.Tipo;
                der.Nombre = Constante.TError;
                der.Tipo = Constante.TError;
                der.Linea = linea;
                der.Columna = columna;
            }
            else//error
            {
                der.Cadena = iz.Cadena;
                der.Tipo = iz.Tipo;
                der.Nombre = iz.Nombre;
                der.Linea = iz.Linea;
                der.Columna = der.Columna;
            }
            return der;
        }

        private NodoExpresion Potencia(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            if (iz.Tipo == Constante.TBool)
            {
                if (der.Tipo == Constante.TBool)
                {
                    der.Cadena = "No se puede elevar " + iz.Tipo + " ^ " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    der.Numero = Math.Pow(iz.Numero, der.Numero);
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TNumber;
                    der.Nombre = Constante.TNumber;
                }
                else if (der.Tipo == Constante.TString)
                {
                    der.Cadena = "No se puede elevar " + iz.Tipo + " ^ " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TNumber)
            {
                if (der.Tipo == Constante.TBool)
                {
                    der.Numero = Math.Pow(iz.Numero, der.Numero);
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TNumber;
                    der.Nombre = Constante.TNumber;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    der.Numero = Math.Pow(iz.Numero, der.Numero);
                    der.Cadena = der.Numero.ToString();
                    der.Tipo = Constante.TNumber;
                    der.Nombre = Constante.TNumber;
                }
                else if (der.Tipo == Constante.TString)
                {
                    der.Cadena = "No se puede elevar " + iz.Tipo + " ^ " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TString)
            {
                der.Cadena = "No se puede elevar " + iz.Tipo + " ^ " + der.Tipo;
                der.Nombre = Constante.TError;
                der.Tipo = Constante.TError;
                der.Linea = linea;
                der.Columna = columna;
            }
            else//error
            {
                der.Cadena = iz.Cadena;
                der.Tipo = iz.Tipo;
                der.Nombre = iz.Nombre;
                der.Linea = iz.Linea;
                der.Columna = der.Columna;
            }
            return der;
        }

        private NodoExpresion Igual(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            if (iz.Tipo == Constante.TBool)
            {
                if (der.Tipo == Constante.TBool)
                {
                    if (iz.Booleano == der.Booleano)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    if (iz.Numero == der.Numero)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " == " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TNumber)
            {
                if (der.Tipo == Constante.TBool)
                {
                    if (iz.Numero == der.Numero)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    if (iz.Numero == der.Numero)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " == " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TString)
            {
                if (der.Tipo == Constante.TString)
                {
                    if (iz.Cadena == der.Cadena)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " == " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else
            {
                der.Cadena = iz.Cadena;
                der.Tipo = iz.Tipo;
                der.Nombre = iz.Nombre;
                der.Linea = iz.Linea;
                der.Columna = der.Columna;
            }

            return der;
        }

        private NodoExpresion Diferente(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            if (iz.Tipo == Constante.TBool)
            {
                if (der.Tipo == Constante.TBool)
                {
                    if (iz.Booleano != der.Booleano)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    if (iz.Numero != der.Numero)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " != " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TNumber)
            {
                if (der.Tipo == Constante.TBool)
                {
                    if (iz.Numero != der.Numero)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    if (iz.Numero != der.Numero)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " != " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TString)
            {
                if (der.Tipo == Constante.TString)
                {
                    if (iz.Cadena != der.Cadena)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " != " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else
            {
                der.Cadena = iz.Cadena;
                der.Tipo = iz.Tipo;
                der.Nombre = iz.Nombre;
                der.Linea = iz.Linea;
                der.Columna = der.Columna;
            }

            return der;
        }

        private NodoExpresion Menor(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            if (iz.Tipo == Constante.TBool)
            {
                if (der.Tipo == Constante.TBool)
                {
                    if (iz.Numero < der.Numero)
                    {

                        der.Booleano = true;
                        der.Numero = 1;
                    }else
                    {

                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    if (iz.Numero < der.Numero)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " < " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TNumber)
            {
                if (der.Tipo == Constante.TBool)
                {
                    if (iz.Numero < der.Numero)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    if (iz.Numero < der.Numero)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " < " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TString)
            {
                if (der.Tipo == Constante.TString)
                {
                    der.Booleano = false;
                    der.Numero = 0;
                    der.Tipo = Constante.TBool;
                    if (iz.Cadena.Length < der.Cadena.Length)//no se cambia
                    {
                        int i = 0;
                        bool detener = false;
                        while (i < iz.Cadena.Length && detener == false)
                        {
                            if (!(iz.Cadena[i] == der.Cadena[i]))
                            {
                                if (iz.Cadena[i] < der.Cadena[i])
                                {
                                    der.Booleano = true;
                                    der.Numero = 1;                                    
                                }
                                detener = true;
                            }
                            i++;
                        }
                        if ( i == iz.Cadena.Length && detener == false)
                        {
                            if (iz.Cadena.Length < der.Cadena.Length)
                            {
                                der.Booleano = true;
                                der.Numero = 1;
                            }
                        }
                    }
                    else
                    {
                        int i = 0;
                        bool detener = false;
                        while (i < der.Cadena.Length && detener == false)
                        {
                            if (!(iz.Cadena[i] == der.Cadena[i]))
                            {
                                if (iz.Cadena[i] < der.Cadena[i])
                                {
                                    der.Booleano = true;
                                    der.Numero = 1;
                                }
                                detener = true;
                            }
                            i++;
                        }
                        if (i == der.Cadena.Length && detener == false)
                        {
                            if (iz.Cadena.Length < der.Cadena.Length)
                            {
                                der.Booleano = true;
                                der.Numero = 1;
                            }
                        }
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " < " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else
            {
                der.Cadena = iz.Cadena;
                der.Tipo = iz.Tipo;
                der.Nombre = iz.Nombre;
                der.Linea = iz.Linea;
                der.Columna = der.Columna;
            }

            return der;
        }

        private NodoExpresion Mayor(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            if (iz.Tipo == Constante.TBool)
            {
                if (der.Tipo == Constante.TBool)
                {
                    if (iz.Numero > der.Numero)
                    {

                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {

                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    if (iz.Numero > der.Numero)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " > " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TNumber)
            {
                if (der.Tipo == Constante.TBool)
                {
                    if (iz.Numero > der.Numero)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TNumber)
                {
                    if (iz.Numero > der.Numero)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " > " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TString)
            {
                if (der.Tipo == Constante.TString)
                {
                    der.Booleano = false;
                    der.Numero = 0;
                    der.Tipo = Constante.TBool;
                    if (iz.Cadena.Length < der.Cadena.Length)//no se cambia
                    {
                        int i = 0;
                        bool detener = false;
                        while (i < iz.Cadena.Length && detener == false)
                        {
                            if (!(iz.Cadena[i] == der.Cadena[i]))
                            {
                                if (iz.Cadena[i] > der.Cadena[i])
                                {
                                    der.Booleano = true;
                                    der.Numero = 1;
                                }
                                detener = true;
                            }
                            i++;
                        }
                        if (i == iz.Cadena.Length && detener == false)
                        {
                            if (iz.Cadena.Length > der.Cadena.Length)
                            {
                                der.Booleano = true;
                                der.Numero = 1;
                            }
                        }
                    }
                    else
                    {
                        int i = 0;
                        bool detener = false;
                        while (i < der.Cadena.Length && detener == false)
                        {
                            if (!(iz.Cadena[i] == der.Cadena[i]))
                            {
                                if (iz.Cadena[i] > der.Cadena[i])
                                {
                                    der.Booleano = true;
                                    der.Numero = 1;
                                }
                                detener = true;
                            }
                            i++;
                        }
                        if (i == der.Cadena.Length && detener == false)
                        {
                            if (iz.Cadena.Length > der.Cadena.Length)
                            {
                                der.Booleano = true;
                                der.Numero = 1;
                            }
                        }
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " > " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else
            {
                der.Cadena = iz.Cadena;
                der.Tipo = iz.Tipo;
                der.Nombre = iz.Nombre;
                der.Linea = iz.Linea;
                der.Columna = der.Columna;
            }

            return der;
        }

        private NodoExpresion MenorIgual(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            
            NodoExpresion menor = Menor(new NodoExpresion(iz), new NodoExpresion(der), linea, columna);
            NodoExpresion igual = Igual(new NodoExpresion(iz), new NodoExpresion(der), linea, columna);

            if (menor.Tipo != Constante.TError && igual.Tipo != Constante.TError)
            {
                if (menor.Booleano || igual.Booleano)
                {
                    der.Booleano = true;
                    der.Numero = 1;
                }
                else
                {
                    der.Booleano = false;
                    der.Numero = 0;
                }

                der.Cadena = der.Numero.ToString();
                der.Nombre = Constante.TBool;
                der.Tipo = Constante.TBool;
            }
            else if (der.Tipo == Constante.TError)
            {

            }
            else
            {
                der.Cadena = "No se puede operar: " + iz.Tipo + " <= " + der.Tipo;
                der.Nombre = Constante.TError;
                der.Tipo = Constante.TError;
                der.Linea = linea;
                der.Columna = columna;
            }

            return der;
        }

        private NodoExpresion MayorIgual(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            NodoExpresion mayor = Mayor(new NodoExpresion(iz), new NodoExpresion(der), linea, columna);
            NodoExpresion igual = Igual(new NodoExpresion(iz), new NodoExpresion(der), linea, columna);

            if (mayor.Tipo != Constante.TError && igual.Tipo != Constante.TError)
            {
                if (mayor.Booleano || igual.Booleano)
                {
                    der.Booleano = true;
                    der.Numero = 1;
                }
                else
                {
                    der.Booleano = false;
                    der.Numero = 0;
                }

                der.Cadena = der.Numero.ToString();
                der.Nombre = Constante.TBool;
                der.Tipo = Constante.TBool;
            }
            else if (der.Tipo == Constante.TError)
            {

            }
            else
            {
                der.Cadena = "No se puede operar: " + iz.Tipo + " >= " + der.Tipo;
                der.Nombre = Constante.TError;
                der.Tipo = Constante.TError;
                der.Linea = linea;
                der.Columna = columna;
            }

            return der;
        }

        int ContarDecimal(double value)
        {
            bool start = false;
            int count = 0;
            foreach (var s in value.ToString())
            {
                if (s == '.')
                {
                    start = true;
                }
                else if (start)
                {
                    count++;
                }
            }

            return count;
        }

        private NodoExpresion Semejante(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            Double a = Double.Parse(Decimal.Round(new decimal(5.5),5).ToString());
            if (iz.Tipo == Constante.TNumber)
            {
                if (der.Tipo == Constante.TNumber)
                {
                    if (Math.Round(Math.Abs(iz.Numero - der.Numero), ContarDecimal(Incerteza)) <= Incerteza)
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " ~ " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else if (iz.Tipo == Constante.TString)
            {
                if (der.Tipo == Constante.TString)
                {
                    if (iz.Cadena.Trim().ToLower() == der.Cadena.Trim().ToLower())
                    {
                        der.Booleano = true;
                        der.Numero = 1;
                    }
                    else
                    {
                        der.Booleano = false;
                        der.Numero = 0;
                    }
                    der.Cadena = der.Numero.ToString();
                    der.Nombre = Constante.TBool;
                    der.Tipo = Constante.TBool;
                }
                else if (der.Tipo == Constante.TError)
                {

                }
                else
                {
                    der.Cadena = "No se puede operar: " + iz.Tipo + " ~ " + der.Tipo;
                    der.Nombre = Constante.TError;
                    der.Tipo = Constante.TError;
                    der.Linea = linea;
                    der.Columna = columna;
                }
            }
            else
            {
                der.Cadena = iz.Cadena;
                der.Tipo = iz.Tipo;
                der.Nombre = iz.Nombre;
                der.Linea = iz.Linea;
                der.Columna = der.Columna;
            }

            return der;
        }

        private NodoExpresion And(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            if (iz.Tipo == Constante.TBool && der.Tipo == Constante.TBool)
            {
                der.Booleano = iz.Booleano && der.Booleano;
                if(der.Booleano)
                {
                    der.Numero = 1;
                }else
                {
                    der.Numero = 0;
                }
                der.Cadena = der.Numero.ToString();
                der.Nombre = Constante.TBool;
                der.Tipo = Constante.TBool;

            }
            else if (der.Tipo == Constante.TError)
            {

            }
            else
            {
                der.Cadena = "No se puede operar: " + iz.Tipo + " && " + der.Tipo;
                der.Nombre = Constante.TError;
                der.Tipo = Constante.TError;
                der.Linea = linea;
                der.Columna = columna;
            }
            return der;
        }

        private NodoExpresion Or(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            if (iz.Tipo == Constante.TBool && der.Tipo == Constante.TBool)
            {
                der.Booleano = iz.Booleano || der.Booleano;
                if (der.Booleano)
                {
                    der.Numero = 1;
                }
                else
                {
                    der.Numero = 0;
                }
                der.Cadena = der.Numero.ToString();
                der.Nombre = Constante.TBool;
                der.Tipo = Constante.TBool;

            }
            else if (der.Tipo == Constante.TError)
            {

            }
            else
            {
                der.Cadena = "No se puede operar: " + iz.Tipo + " || " + der.Tipo;
                der.Nombre = Constante.TError;
                der.Tipo = Constante.TError;
                der.Linea = linea;
                der.Columna = columna;
            }
            return der;
        }

        private NodoExpresion Xor(NodoExpresion iz, NodoExpresion der, int linea, int columna)
        {
            if (iz.Tipo == Constante.TBool && der.Tipo == Constante.TBool)
            {
                der.Booleano = (!iz.Booleano && der.Booleano) || (iz.Booleano && !der.Booleano);
                if (der.Booleano)
                {
                    der.Numero = 1;
                }
                else
                {
                    der.Numero = 0;
                }
                der.Cadena = der.Numero.ToString();
                der.Nombre = Constante.TBool;
                der.Tipo = Constante.TBool;

            }
            else if (der.Tipo == Constante.TError)
            {

            }
            else
            {
                der.Cadena = "No se puede operar: " + iz.Tipo + " |& " + der.Tipo;
                der.Nombre = Constante.TError;
                der.Tipo = Constante.TError;
                der.Linea = linea;
                der.Columna = columna;
            }
            return der;
        }

        private NodoExpresion Not(NodoExpresion der, int linea, int columna)
        {
            if (der.Tipo == Constante.TBool)
            {
                der.Booleano = !der.Booleano;
                if (der.Booleano)
                {
                    der.Numero = 1;
                }
                else
                {
                    der.Numero = 0;
                }
                der.Cadena = der.Numero.ToString();
                der.Nombre = Constante.TBool;
                der.Tipo = Constante.TBool;

            }
            else if (der.Tipo == Constante.TError)
            {

            }
            else
            {
                der.Cadena = "No se puede operar:  ! " + der.Tipo;
                der.Nombre = Constante.TError;
                der.Tipo = Constante.TError;
                der.Linea = linea;
                der.Columna = columna;
            }
            return der;
        }

    }
}
