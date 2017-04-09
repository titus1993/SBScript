using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using _Compi2_Practica_201213587.Ejecucion;
namespace _Compi2_Practica_201213587
{
    class GenerarArbol
    {
        public GenerarArbol()
        {
            
            
        }


        public EjecutarSBS GenerarSimbolo(String texto, String ruta)
        {
            SBScriptGrammar Gramatica;
            LanguageData language;
            Parser parser;
            Gramatica = new SBScriptGrammar();
            language = new LanguageData(Gramatica);
            parser = new Parser(language);

            ParseTree parseTree = parser.Parse(texto);
            EjecutarSBS ejecutar = null;
            if (parseTree.Root != null && parseTree.ParserMessages.Count == 0)
            {
                GenerarTablaSimbolo a = new GenerarTablaSimbolo(parseTree, ruta);
                ejecutar = a.Ejecutar;
                ejecutar.Iniciar();
                
            }
            else
            {
                TitusNotifiaciones.LimpiarDatosErrores();
                TabError tablaerror = new TabError();
                foreach (Irony.LogMessage error in parseTree.ParserMessages)
                {
                    if (error.Message.Contains("Syntax error,"))
                    {
                        tablaerror.InsertarFila("Sintactico", error.Message.Replace("Syntax error", " "), ruta, (error.Location.Line + 1).ToString(), (error.Location.Column + 1).ToString());
                    }
                    else if (error.Message.Contains("Invalid character"))
                    {
                        tablaerror.InsertarFila("Lexico", error.Message.Replace("Invalid character", "Caracter invalido"), ruta, (error.Location.Line + 1).ToString(), (error.Location.Column + 1).ToString());
                    }
                    else
                    {
                        tablaerror.InsertarFila("Sintactico", error.Message.Replace("Unclosed cooment block", "Comentario de bloque sin cerrar"), ruta, (error.Location.Line + 1).ToString(), (error.Location.Column + 1).ToString());
                    }

                }
                TitusNotifiaciones.setDatosErrores(tablaerror);
            }
            return ejecutar;
        }



    }
}
