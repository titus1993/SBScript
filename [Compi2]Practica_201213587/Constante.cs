using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Practica_201213587
{
    static class Constante
    {
        //Filtro para abrir y guardar
        public static String DialogFilter ="SBS File|*.sbs|All Files|*.*";

        //
        public static Double DefaultDefineNumber = 0.5;
        public static String DefaultDefineRuta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        //valores de datos
        public const String Cadena = "string";
        public const String Numero = "numero";
        public const String Id = "id";


        //tipo de datos
        public const String TNumber = "Number";
        public const String TString = "String";
        public const String TBool = "Bool";
        public const String TVoid = "Void";

        //Operadores Aritmeticos
        public const String TMas = "+";
        public const String TMenos = "-";
        public const String TPor = "*";
        public const String TDivision = "/";
        public const String TModulo = "%";
        public const String TPotencia = "^";

        //Operadores Relacionales
        public const String TIgualacion = "==";
        public const String TDiferente = "!=";
        public const String TMenor = "<";
        public const String TMayor = ">";
        public const String TMenorIgual = "<=";
        public const String TMayorIgual = ">=";

        //Operador Semejante
        public const String TSemejante = "~";

        //Operadores Logicos
        public const String TAnd = "&&";
        public const String TOr = "||";
        public const String TXor = "|&";
        public const String TNot = "!";

        public const String TAumento = "++";
        public const String TDecremento =  "--";

        //Palabras reservadas
        public const String TRetorno = "Retorno";
        public const String TPrincipal = "Principal";
        public const String TIncluye = "Incluye";
        public const String TDefine = "Define";
        public const String TSi = "Si";
        public const String TSino = "Sino";
        public const String TContinuar = "Continuar";
        public const String TSelecciona = "Selecciona";
        public const String TDefecto = "Defecto";
        public const String TPara = "Para";
        public const String THasta = "Hasta";
        public const String TMientras = "Mientras";
        public const String TDetener = "Detener";
        public const String TMostrar = "Mostrar";
        public const String TDibujarAST = "DibujarAST";
        public const String TDibujarEXP = "DibujarEXP";
        public const String TSbs = ".sbs";
        public const String TVerdadero = "true";
        public const String TFalso = "false";

        //No Terminaless
        public const String INICIO = "INICIO";
        public const String DECLARACION = "DECLARACION";
        public const String TIPO = "TIPO";
        public const String EXP = "EXP";
        public const String LISTA_ID = "LISTA_ID";
        public const String ASIGNACION = "ASIGNACION";
        public const String LISTA_SENTENCIA = "LISTA_SENTENCIA";
        public const String SENTENCIA = "SENTENCIA";
        public const String FUNCION = "FUNCION";
        public const String LISTA_PARAMETROS = "LISTA_PARAMETROS";
        public const String LISTA_PARAMETRO = "LISTA_PARAMETRO";
        public const String PARAMETRO = "PARAMETRO";
        public const String LISTA_INSTRUCCIONES = "LISTA_INSTRUCCIONES";
        public const String LISTA_INSTRUCCION = "LISTA_INSTRUCCION";
        public const String INSTRUCCION = "INSTRUCCION";
        public const String LLAMADA_FUNCION = "LLAMADA_FUNCION";
        public const String LISTA_EXPRESIONES = "LISTA_EXPRESIONES";
        public const String LISTA_EXPRESION = "LISTA_EXPRESION";
        public const String RETORNO = "RETORNO";
        public const String PRINCIPAL = "PRINCIPAL";
        public const String SI = "SI";
        public const String SINO = "SINO";
        public const String SELECCIONA = "SELECCIONA";
        public const String LISTA_CASOS = "LISTA_CASOS";
        public const String CASO = "CASO";
        public const String DEFECTO = "DEFECTO";
        public const String PARA = "PARA";
        public const String HASTA = "HASTA";
        public const String MIENTRAS = "MIENTRAS";
        public const String SIMPLIFICADA = "SIMPLIFICADA";
        public const String FMOSTRAR = "FMOSTRAR";
        public const String FDIBUJARAST = "FDIBUJARAST";
        public const String FDIBUJAREXP = "FDIBUJAREXP";
        public const String LISTA_ENCABEZADOS = "LISTA_ENCABEZADOS";
        public const String LISTA_ENCABEZADO = "LISTA_ENCABEZADO";
        public const String ENCABEZADO = "ENCABEZADO";
        public const String INCLUYE = "INCLUYE";
        public const String DEFINE = "DEFINE";
        public const String VALOR = "VALOR";
        public const String DEFECTOS = "DEFECTOS";


        public const String TCaso = "Caso";
        public const String TError = "Error";


        public const String TVariable = "Variable";
        public const String TMetodo = "Metodo";
        public const String TIncerteza = "Incerteza";
        public const String TFuncion = "Funcion";
        public const String ErroEjecucion = "Ejecucion";
        public const String ErrorSintactico = "Sintactico";
    }
}
