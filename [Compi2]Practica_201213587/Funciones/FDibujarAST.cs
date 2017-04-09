using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Compi2_Practica_201213587.Ejecucion;
using System.IO;
using System.Diagnostics;

namespace _Compi2_Practica_201213587.Funciones
{
    class FDibujarAST
    {
        Ambito Ambito { get; set; }
        public String Id { get; set; }
        public String RutaImagen { get; set; }
        public Double Incerteza { get; set; }
        public String RutaArchivo { get; set; }

        public void DibujarAst(int linea, int columna)
        {
            String a = DibujarAST(linea,columna);
            
        }

        private String DibujarAST(int linea, int columna)
        {
            String cadena = "";
            List<Variables> lista = TablaVariables.BuscarMetodo(Id);
            if (lista.Count > 0)
            {
                foreach (Variables metodo in lista)
                {
                    int i = 0;
                    int actual = i;
                    cadena = "digraph G{\n\tgraph[rankir = \"LR\"];\n\tnode[shape = box, fontsize = 16, fontname = \"Arial\", style = filled, fillcolor = grey88];\n\t";
                    cadena = cadena + "\t\tnodo" + actual.ToString() + "[label=\"" + metodo.Tipo + ": " + metodo.Nombre + "(";
                    FFuncion parametros = (FFuncion)metodo.Valor;
                    String aux = "";
                    String para = "";
                    foreach (Simbolo p in parametros.Parametros)
                    {
                        para = aux + p.Tipo;
                        aux = para + ", ";
                    }

                    cadena = cadena + para + ")\"]\n";

                    if (metodo.Ambito != null)
                    {
                        foreach (Simbolo simbolo in metodo.Ambito.TablaSimbolo)
                        {
                            i++;
                            int actualambito = i;
                            cadena = cadena + GenerarArbolGraphics(ref i, simbolo);
                            cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + actualambito.ToString() + "\n";
                        }
                    }
                    cadena = cadena + "\n}";

                    try
                    {
                        String ruta = RutaImagen + "\\";
                        String rutatxt = ruta + "AST_" + metodo.Nombre + "_" + (TitusNotifiaciones.ListaImagenes.Images.Count + 1).ToString() + ".txt";
                        String rutaimg = ruta + "AST_" + metodo.Nombre + "_" + (TitusNotifiaciones.ListaImagenes.Images.Count + 1).ToString() + ".png";
                        String nombreimg = "AST_" + metodo.Nombre + "_" + (TitusNotifiaciones.ListaImagenes.Images.Count + 1).ToString() + ".png";
                        /*if (File.Exists(rutaimg))
                        {
                            File.Delete(rutaimg);
                        }*/
                        File.WriteAllText(rutatxt, cadena);
                        
                        ProcessStartInfo startInfo = new ProcessStartInfo("dot.exe");
                        startInfo.Arguments = "-Tjpg  " + @rutatxt + " -o " + @rutaimg;
                        Process.Start(startInfo);

                        System.Threading.Thread.Sleep(3000);

                        TitusNotifiaciones.MeterImagen(nombreimg, rutaimg);
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                TabError error = new TabError();
                error.InsertarFila(Constante.ErroEjecucion, "No se encontro ningun metodo con el nombre " + Id,RutaArchivo, linea.ToString(), columna.ToString());
                TitusNotifiaciones.setDatosErrores(error);
            }
            return cadena;
        }

        private String GenerarArbolGraphics(ref int pos, Simbolo simbolo)
        {
            String cadena = "";
            int actual = pos;
            switch (simbolo.Rol)
            {
                case Constante.DECLARACION:
                    int actualobjeto = pos;
                    cadena = "\t\tnodo" + actualobjeto.ToString() + "[label=\"" + "Declaracion: " + simbolo.Nombre + "\"]\n";
                    cadena = cadena + GenerarArbolDeclaracion(ref pos, simbolo);
                    break;

                case Constante.ASIGNACION:
                    int actualobjetoasi = pos;
                    cadena = "\t\tnodo" + actualobjetoasi.ToString() + "[label=\"" + "Asignacion: " + simbolo.Nombre + "\"]\n";
                    cadena = cadena + GenerarArbolAsignacion(ref pos, simbolo);
                    break;

                case Constante.TDetener:
                    cadena = "\t\tnodo" + actual.ToString() + "[label=\"" + simbolo.Nombre + "\"]\n";
                    break;

                case Constante.TRetorno:
                    cadena = "\t\tnodo" + actual.ToString() + "[label=\"" + simbolo.Nombre + "\"]\n";
                    cadena = cadena + GenerarArbolRetorno(ref pos, simbolo);
                    break;

                case Constante.TContinuar:
                    cadena = "\t\tnodo" + actual.ToString() + "[label=\"" + simbolo.Nombre + "\"]\n";
                    break;

                case Constante.TSelecciona:
                    cadena = "\t\tnodo" + actual.ToString() + "[label=\"" + simbolo.Nombre + "\"]\n";
                    cadena = cadena + GenerarArbolSelecciona(ref pos, simbolo);
                    break;

                case Constante.TMientras:
                    cadena = "\t\tnodo" + actual.ToString() + "[label=\"" + simbolo.Nombre + "\"]\n";
                    cadena = cadena + GenerarArbolMientras(ref pos, simbolo);
                    break;

                case Constante.THasta:
                    cadena = "\t\tnodo" + actual.ToString() + "[label=\"" + simbolo.Nombre + "\"]\n";
                    cadena = cadena + GenerarArbolHasta(ref pos, simbolo);
                    break;

                case Constante.TMostrar:
                    cadena = "\t\tnodo" + actual.ToString() + "[label=\"" + simbolo.Nombre + "\"]\n";
                    break;

                case Constante.TDibujarEXP:
                    cadena = "\t\tnodo" + actual.ToString() + "[label=\"" + simbolo.Nombre + "\"]\n";
                    break;

                case Constante.TDibujarAST:
                    cadena = "\t\tnodo" + actual.ToString() + "[label=\"" + simbolo.Nombre + "\"]\n";
                    break;

                case Constante.TPara:
                    cadena = "\t\tnodo" + actual.ToString() + "[label=\"" + simbolo.Nombre + "\"]\n";
                    cadena = cadena + GenerarArbolPara(ref pos, simbolo);
                    break;

                case Constante.LLAMADA_FUNCION:
                    cadena = "\t\tnodo" + actual.ToString() + "[label=\"Llamada Funcion: " + simbolo.Nombre + "\"]\n";
                    break;

                case Constante.TSi:
                    cadena = "\t\tnodo" + actual.ToString() + "[label=\"" + simbolo.Nombre + "\"]\n";
                    cadena = cadena + GenerarArbolSi(ref pos, simbolo);
                    break;
            }

            return cadena;
        }

        private String GenerarArbolPara(ref int pos, Simbolo simbolo)
        {
            int actual = pos;//padre
            pos++;//hijo
            String cadena = "";
            //primero dibujamos al expresion

            if (simbolo.Objeto != null)
            {
                //dibujamos la expresion
                FPara funcion = (FPara)simbolo.Objeto;
                cadena = "\t\tnodo" + pos.ToString() + "[label=\"Declaracion: " + funcion.Declaracion.Nombre + "\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";

                pos++;
                cadena = cadena + "\t\tnodo" + pos.ToString() + "[label=\"Expresion\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";

                pos++;
                cadena = cadena + "\t\tnodo" + pos.ToString() + "[label=\"OP:" + funcion.Operacion + "\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";

                pos++;
                cadena = cadena + "\t\tnodo" + pos.ToString() + "[label=\"Cuerpo\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";
                

                if (funcion.ambito != null)
                {
                    int actualvalores = pos;
                    foreach (Simbolo sim in funcion.ambito.TablaSimbolo)
                    {
                        pos++;
                        int padre = pos;
                        cadena = cadena + GenerarArbolGraphics(ref pos, sim);
                        cadena = cadena + "\t\tnodo" + actualvalores.ToString() + "->nodo" + padre.ToString() + "\n";
                    }
                }
            }

            return cadena;
        }

        private String GenerarArbolSi(ref int pos, Simbolo simbolo)
        {
            int actual = pos;//padre
            pos++;//hijo
            String cadena = "";
            //primero dibujamos al expresion

            if (simbolo.Objeto != null)
            {
                //dibujamos la expresion
                FSi funcion = (FSi)simbolo.Objeto;
                cadena = "\t\tnodo" + pos.ToString() + "[label=\"Expresion\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";
                
                pos++;
                cadena = cadena + "\t\tnodo" + pos.ToString() + "[label=\"Cuerpo\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";

                if (simbolo.Ambito != null)
                {
                    int actualvalores = pos;
                    foreach (Simbolo sim in simbolo.Ambito.TablaSimbolo)
                    {
                        pos++;
                        int padre = pos;
                        cadena = cadena + GenerarArbolGraphics(ref pos, sim);
                        cadena = cadena + "\t\tnodo" + actualvalores.ToString() + "->nodo" + padre.ToString() + "\n";
                    }
                }

                pos++;
                if (funcion.Sino != null)
                {
                    cadena = cadena + "\t\tnodo" + pos.ToString() + "[label=\""+ Constante.SINO +"\"]\n";
                    cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";
                    int actualvalores = pos;
                    foreach (Simbolo sim in funcion.Sino.TablaSimbolo)
                    {
                        pos++;
                        int padre = pos;
                        cadena = cadena + GenerarArbolGraphics(ref pos, sim);
                        cadena = cadena + "\t\tnodo" + actualvalores.ToString() + "->nodo" + padre.ToString() + "\n";
                    }
                }
            }

            return cadena;
        }

        private String GenerarArbolDeclaracion(ref int pos, Simbolo simbolo)
        {
            int actual = pos;
            pos++;
            String cadena = "";
            if (simbolo.Objeto!= null)
            {
                cadena = "\t\tnodo" + pos.ToString() + "[label=\"Expresion\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() +"\n";
            }
            
            return cadena;
        }

        private String GenerarArbolAsignacion(ref int pos, Simbolo simbolo)
        {
            int actual = pos;
            pos++;
            String cadena = "";
            if (simbolo.Objeto != null)
            {
                cadena = "\t\tnodo" + pos.ToString() + "[label=\"Expresion\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";
            }

            return cadena;
        }

        private String GenerarArbolRetorno(ref int pos, Simbolo simbolo)
        {
            int actual = pos;
            pos++;
            String cadena = "";
            if (simbolo.Objeto != null)
            {
                cadena = "\t\tnodo" + pos.ToString() + "[label=\"Expresion\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";
            }

            return cadena;
        }

        private String GenerarArbolSelecciona(ref int pos, Simbolo simbolo)
        {
            int actual = pos;//padre
            pos++;//hijo
            String cadena = "";
            //primero dibujamos al expresion

            if (simbolo.Objeto != null)
            {
                //dibujamos la expresion
                FSelecciona funcion = (FSelecciona)simbolo.Objeto;
                cadena = "\t\tnodo" + pos.ToString() + "[label=\"Expresion\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";

                int actualcuerpo = pos;
                pos++;
                cadena = cadena + "\t\tnodo" + pos.ToString() + "[label=\"Cuerpo\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";

                int actualvalores = pos;
                foreach (FCaso caso in funcion.Casos)
                {
                    pos++;
                    int padre = pos;
                    cadena = cadena + GenerarArbolCaso(ref pos, caso);
                    cadena = cadena + "\t\tnodo" + actualvalores.ToString() + "->nodo" + padre.ToString() + "\n";
                }

                if (funcion.Defecto != null)
                {
                    pos++;
                    int padre = pos;
                    cadena = cadena + GenerarArbolCaso(ref pos, funcion.Defecto);
                    cadena = cadena + "\t\tnodo" + actualvalores.ToString() + "->nodo" + padre.ToString() + "\n";
                }
                //dibujamos el cuerpo y le asignamos los casos
            }

            return cadena;
        }

        private String GenerarArbolHasta(ref int pos, Simbolo simbolo)
        {
            int actual = pos;
            pos++;
            String cadena = "";
            //primero dibujamos al expresion

            if (simbolo.Objeto != null)
            {
                //dibujamos la expresion
                FHasta funcion = (FHasta)simbolo.Objeto;
                cadena = "\t\tnodo" + pos.ToString() + "[label=\"Expresion\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";

                int actualcuerpo = pos;
                pos++;
                cadena = cadena + "\t\tnodo" + pos.ToString() + "[label=\"Cuerpo\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";

                int actualvalores = pos;
                if (funcion.ambito != null)
                {
                    foreach (Simbolo sim in funcion.ambito.TablaSimbolo)
                    {
                        pos++;
                        int padre = pos;
                        cadena = cadena + GenerarArbolGraphics(ref pos, sim);
                        cadena = cadena + "\t\tnodo" + actualvalores.ToString() + "->nodo" + padre.ToString() + "\n";
                    }
                }
            }

            return cadena;
        }
        private String GenerarArbolMientras(ref int pos, Simbolo simbolo)
        {
            int actual = pos;
            pos++;
            String cadena = "";
            //primero dibujamos al expresion
            
            if (simbolo.Objeto != null)
            {
                //dibujamos la expresion
                FMientras funcion = (FMientras)simbolo.Objeto;
                cadena = "\t\tnodo" + pos.ToString() + "[label=\"Expresion\"]\n";                
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";

                int actualcuerpo = pos;
                pos++;
                cadena = cadena + "\t\tnodo" + pos.ToString() + "[label=\"Cuerpo\"]\n";
                cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + pos.ToString() + "\n";

                int actualvalores = pos;
                if (funcion.ambito != null)
                {
                    foreach (Simbolo sim in funcion.ambito.TablaSimbolo)
                    {
                        pos++;
                        int padre = pos;
                        cadena = cadena + GenerarArbolGraphics(ref pos, sim);
                        cadena = cadena + "\t\tnodo" + actualvalores.ToString() + "->nodo" + padre.ToString() + "\n";
                    }
                }
            }

            return cadena;
        }

        private String GenerarArbolCaso(ref int pos, FCaso caso)
        {
            int actual = pos;//numero del padre
            pos++;//aumenta para el nuevo hijo
            String cadena = "";
            if (caso.Tipo == Constante.TNumber)
            {
                cadena = "\t\tnodo" + actual.ToString() + "[label=\"Valor:"+ caso.ValNumero.ToString() + "\"]\n";

                if (caso.ambito != null)
                {
                 
                    foreach (Simbolo simbolo in caso.ambito.TablaSimbolo)
                    {
                        int padre = pos;
                        cadena = cadena + GenerarArbolGraphics(ref pos, simbolo);
                        cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + padre.ToString() + "\n";
                    }
                }
            }
            else if(caso.Tipo == Constante.TString)
            {
                cadena = "\t\tnodo" + actual.ToString() + "[label=\"Valor:" + caso.ValCadena + "\"]\n";

                if (caso.ambito != null)
                {

                    foreach (Simbolo simbolo in caso.ambito.TablaSimbolo)
                    {
                        int padre = pos;
                        cadena = cadena + GenerarArbolGraphics(ref pos, simbolo);
                        cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + padre.ToString() + "\n";
                    }
                }
            }
            else
            {
                cadena = "\t\tnodo" + actual.ToString() + "[label=\"Defecto\"]\n";

                if (caso.ambito != null)
                {

                    foreach (Simbolo simbolo in caso.ambito.TablaSimbolo)
                    {
                        int padre = pos;
                        cadena = cadena + GenerarArbolGraphics(ref pos, simbolo);
                        cadena = cadena + "\t\tnodo" + actual.ToString() + "->nodo" + padre.ToString() + "\n";
                    }
                }
            }

            return cadena;
        }

        
    }
}
