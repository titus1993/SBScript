using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Ast;

namespace _Compi2_Practica_201213587
{
    [Language("SBScript", "1.0", "SBScript Grammar")]
    class SBScriptGrammar : Grammar
    {
       
        private readonly TerminalSet mSkipTokensInPreview = new TerminalSet(); //used in token preview for conflict resolution

        public SBScriptGrammar() : base(caseSensitive: true)
        {
            //Comentarios
            CommentTerminal DelimitedComment = new CommentTerminal("DelimitedComment", "#*", "*#");
            CommentTerminal SingleLineComment = new CommentTerminal("SingleLineComment", "#", "\r", "\n", "\u2085", "\u2028", "\u2029");
            

            NonGrammarTerminals.Add(DelimitedComment);
            NonGrammarTerminals.Add(SingleLineComment);
            

            //Terminales                        
            //Tipo de datos
            var TNumber = ToTerm(Constante.TNumber);
            var TString = ToTerm(Constante.TString);
            var TBool = ToTerm(Constante.TBool);
            var TVoid = ToTerm(Constante.TVoid);

            //Operadores Aritmeticos
            var TMas = ToTerm(Constante.TMas);
            var TMenos = ToTerm(Constante.TMenos);
            var TPor = ToTerm(Constante.TPor);
            var TDivision = ToTerm(Constante.TDivision);
            var TModulo = ToTerm(Constante.TModulo);
            var TPotencia = ToTerm(Constante.TPotencia);

            //Operadores Relacionales
            var TIgualacion = ToTerm(Constante.TIgualacion);
            var TDiferente = ToTerm(Constante.TDiferente);
            var TMenor = ToTerm(Constante.TMenor);
            var TMayor = ToTerm(Constante.TMayor);
            var TMenorIgual = ToTerm(Constante.TMenorIgual);
            var TMayorIgual = ToTerm(Constante.TMayorIgual);

            //Operador Semejante
            var TSemejante = ToTerm(Constante.TSemejante);

            //Operadores Logicos
            var TAnd = ToTerm(Constante.TAnd);
            var TOr = ToTerm(Constante.TOr);
            var TXor = ToTerm(Constante.TXor);
            var TNot = ToTerm(Constante.TNot);

            //signos
            var TComa = ToTerm(",");
            var TPuntoyComa = ToTerm(";");
            var TParentesis_Abre = ToTerm("(");
            var TParentesis_Cierra = ToTerm(")");
            var TIgual = ToTerm("=");
            var TLlave_Abre = ToTerm("{");
            var TLlave_Cierra = ToTerm("}");
            var TCorchete_Abre = ToTerm("[");
            var TCorchete_Cierra = ToTerm("]");
            var TDosPuntos = ToTerm(":");
            var TAumento = ToTerm(Constante.TAumento);
            var TDecremento = ToTerm(Constante.TDecremento);


            //Palabras Reservadas
            var TRetorno = ToTerm(Constante.TRetorno);
            var TPrincipal = ToTerm(Constante.TPrincipal);
            var TIncluye = ToTerm(Constante.TIncluye);
            var TDefine = ToTerm(Constante.TDefine);
            var TSi = ToTerm(Constante.TSi);
            var TSino = ToTerm(Constante.TSino);
            var TContinuar = ToTerm(Constante.TContinuar);
            var TSelecciona = ToTerm(Constante.TSelecciona);
            var TDefecto = ToTerm(Constante.TDefecto);
            var TPara = ToTerm(Constante.TPara);
            var THasta = ToTerm(Constante.THasta);
            var TMientras = ToTerm(Constante.TMientras);
            var TDetener = ToTerm(Constante.TDetener);
            var TMostarar = ToTerm(Constante.TMostrar);
            var TDibujarAST = ToTerm(Constante.TDibujarAST);
            var TDibujarEXP = ToTerm(Constante.TDibujarEXP);
            var TSbs = ToTerm(Constante.TSbs);
            var TVerdadero = ToTerm(Constante.TVerdadero);
            var TFalso = ToTerm(Constante.TFalso);


            //Expresiones Regulares y Datos
            var Cadena = new StringLiteral(Constante.Cadena, "\"");
            var Numero = new NumberLiteral(Constante.Numero);
            var Id = new IdentifierTerminal(Constante.Id);

            //No terminales
            var INICIO = new NonTerminal(Constante.INICIO);
            var DECLARACION = new NonTerminal(Constante.DECLARACION);
            var TIPO = new NonTerminal(Constante.TIPO);
            var EXP = new NonTerminal(Constante.EXP);
            var LISTA_ID = new NonTerminal(Constante.LISTA_ID);
            var ASIGNACION = new NonTerminal(Constante.ASIGNACION);
            var LISTA_SENTENCIA = new NonTerminal(Constante.LISTA_SENTENCIA);
            var SENTENCIA = new NonTerminal(Constante.SENTENCIA);
            var FUNCION = new NonTerminal(Constante.FUNCION);
            var LISTA_PARAMETROS = new NonTerminal(Constante.LISTA_PARAMETROS);
            var LISTA_PARAMETRO = new NonTerminal(Constante.LISTA_PARAMETRO);
            var PARAMETRO = new NonTerminal(Constante.PARAMETRO);
            var LISTA_INSTRUCCIONES = new NonTerminal(Constante.LISTA_INSTRUCCIONES);
            var LISTA_INSTRUCCION = new NonTerminal(Constante.LISTA_INSTRUCCION);
            var INSTRUCCION = new NonTerminal(Constante.INSTRUCCION);
            var LLAMADA_FUNCION = new NonTerminal(Constante.LLAMADA_FUNCION);
            var LISTA_EXPRESIONES = new NonTerminal(Constante.LISTA_EXPRESIONES);
            var LISTA_EXPRESION = new NonTerminal(Constante.LISTA_EXPRESION);
            var RETORNO = new NonTerminal(Constante.RETORNO);
            var PRINCIPAL = new NonTerminal(Constante.PRINCIPAL);
            var SI = new NonTerminal(Constante.SI);
            var SINO = new NonTerminal(Constante.SINO);
            var SELECCIONA = new NonTerminal(Constante.SELECCIONA);
            var LISTA_CASOS = new NonTerminal(Constante.LISTA_CASOS);
            var CASO = new NonTerminal(Constante.CASO);
            var DEFECTO = new NonTerminal(Constante.DEFECTO);
            var PARA = new NonTerminal(Constante.PARA);
            var HASTA = new NonTerminal(Constante.HASTA);
            var MIENTRAS = new NonTerminal(Constante.MIENTRAS);
            var SIMPLIFICADA = new NonTerminal(Constante.SIMPLIFICADA);
            var FMOSTRAR = new NonTerminal(Constante.FMOSTRAR);
            var FDIBUJARAST = new NonTerminal(Constante.FDIBUJARAST);
            var FDIBUJAREXP = new NonTerminal(Constante.FDIBUJAREXP);
            var LISTA_ENCABEZADOS = new NonTerminal(Constante.LISTA_ENCABEZADOS);
            var LISTA_ENCABEZADO = new NonTerminal(Constante.LISTA_ENCABEZADO);
            var ENCABEZADO = new NonTerminal(Constante.ENCABEZADO);
            var INCLUYE = new NonTerminal(Constante.INCLUYE);
            var DEFINE = new NonTerminal(Constante.DEFINE);
            var VALOR = new NonTerminal(Constante.VALOR);
            var DEFECTOS = new NonTerminal(Constante.DEFECTOS);
            
            //Reglas
            INICIO.Rule = LISTA_ENCABEZADOS + LISTA_SENTENCIA;

            //Encabezado
            LISTA_ENCABEZADOS.Rule = LISTA_ENCABEZADO
                | Empty;

            LISTA_ENCABEZADO.Rule = this.MakePlusRule(LISTA_ENCABEZADO, ENCABEZADO);

            ENCABEZADO.Rule = INCLUYE
                | DEFINE;

            ENCABEZADO.ErrorRule = SyntaxError + ToTerm("\n");

            INCLUYE.Rule = TIncluye + Id + TSbs;

            DEFINE.Rule = TDefine + Numero
                |TDefine + Cadena;

            LISTA_SENTENCIA.Rule = this.MakePlusRule(LISTA_SENTENCIA, SENTENCIA);


            //Sentencias Globales
            SENTENCIA.Rule = DECLARACION + TPuntoyComa
                | FUNCION
                | PRINCIPAL;

            SENTENCIA.ErrorRule = SyntaxError + TPuntoyComa
                | TLlave_Abre + SyntaxError + TLlave_Cierra;
            
            //Intrucciones
            LISTA_INSTRUCCIONES.Rule = LISTA_INSTRUCCION
                | Empty;

            LISTA_INSTRUCCION.Rule = this.MakePlusRule(LISTA_INSTRUCCION, INSTRUCCION);

            INSTRUCCION.Rule = DECLARACION + TPuntoyComa
                | ASIGNACION + TPuntoyComa
                | LLAMADA_FUNCION + TPuntoyComa
                | RETORNO + TPuntoyComa
                | SI
                | SELECCIONA
                | PARA
                | HASTA
                | MIENTRAS
                | TDetener + TPuntoyComa
                | TContinuar + TPuntoyComa
                | FMOSTRAR + TPuntoyComa
                | FDIBUJARAST + TPuntoyComa
                | FDIBUJAREXP + TPuntoyComa;

            INSTRUCCION.ErrorRule = SyntaxError + TPuntoyComa
                | TLlave_Abre + SyntaxError + TLlave_Cierra;



            //Sentecias: Declaraciones, ciclos, asignaciones, etc
            DECLARACION.Rule = TIPO + LISTA_ID + TIgual + EXP
                |TIPO + LISTA_ID;
            
            LISTA_ID.Rule = this.MakeListRule(LISTA_ID, TComa, Id);

            
            //Funcion
            FUNCION.Rule = TIPO + Id + TParentesis_Abre + LISTA_PARAMETROS + TParentesis_Cierra + TLlave_Abre + LISTA_INSTRUCCIONES + TLlave_Cierra;

            LLAMADA_FUNCION.Rule = Id + TParentesis_Abre + LISTA_EXPRESIONES + TParentesis_Cierra;

            
            //Lista Expresion
            LISTA_EXPRESIONES.Rule = LISTA_EXPRESION
                |Empty;
            
            LISTA_EXPRESION.Rule = this.MakeListRule(LISTA_EXPRESION, TComa, EXP);
            
            
            //Parametros
            LISTA_PARAMETROS.Rule = LISTA_PARAMETRO
                |Empty;

            LISTA_PARAMETRO.Rule = this.MakeListRule(LISTA_PARAMETRO, TComa, PARAMETRO);

            PARAMETRO.Rule = TIPO + Id;

            
            //Asignacion
            ASIGNACION.Rule = Id + TIgual + EXP;


            //Retorno
            RETORNO.Rule = TRetorno + EXP
                |TRetorno;
            

            //PRINCIPAL
            PRINCIPAL.Rule = TPrincipal + TParentesis_Abre + TParentesis_Cierra + TLlave_Abre + LISTA_INSTRUCCIONES + TLlave_Cierra;


            //Sentencia SI
            SI.Rule = TSi + TParentesis_Abre + EXP + TParentesis_Cierra + TLlave_Abre + LISTA_INSTRUCCIONES + TLlave_Cierra + SINO;

            SINO.Rule = TSino + TLlave_Abre + LISTA_INSTRUCCIONES + TLlave_Cierra
                | Empty;


            //Selecciona
            SELECCIONA.Rule = TSelecciona + TParentesis_Abre + EXP + TParentesis_Cierra + LISTA_CASOS + DEFECTOS;

            LISTA_CASOS.Rule = this.MakePlusRule(LISTA_CASOS, CASO);            

            CASO.Rule = VALOR + TDosPuntos + TLlave_Abre + LISTA_INSTRUCCIONES + TLlave_Cierra;

            DEFECTOS.Rule = DEFECTO
                | Empty;
                
            DEFECTO.Rule = TDefecto + TDosPuntos + TLlave_Abre + LISTA_INSTRUCCIONES + TLlave_Cierra;

            VALOR.Rule = Numero
                | Cadena;


            //Para
            PARA.Rule = TPara + TParentesis_Abre + TIPO + Id + TIgual + EXP + TPuntoyComa + EXP + TPuntoyComa + SIMPLIFICADA + TParentesis_Cierra + TLlave_Abre + LISTA_INSTRUCCIONES + TLlave_Cierra;

            SIMPLIFICADA.Rule = TAumento
                | TDecremento;


            //Hasta
            HASTA.Rule = THasta + TParentesis_Abre + EXP + TParentesis_Cierra + TLlave_Abre + LISTA_INSTRUCCIONES + TLlave_Cierra;


            //Mientras
            MIENTRAS.Rule = TMientras + TParentesis_Abre + EXP + TParentesis_Cierra + TLlave_Abre + LISTA_INSTRUCCIONES + TLlave_Cierra;


            //FUNCIONES
            FMOSTRAR.Rule = TMostarar + TParentesis_Abre + Cadena + TParentesis_Cierra
                | TMostarar + TParentesis_Abre + Cadena + TComa + LISTA_EXPRESION + TParentesis_Cierra;

            FDIBUJARAST.Rule = TDibujarAST + TParentesis_Abre + Id + TParentesis_Cierra;

            FDIBUJAREXP.Rule = TDibujarEXP + TParentesis_Abre + EXP + TParentesis_Cierra;

            //Expresiones y tipo
            TIPO.Rule = TNumber
                | TString
                | TBool
                | TVoid;

            EXP.Rule = EXP + TAnd + EXP
                | EXP + TOr + EXP
                | EXP + TXor + EXP
                | TNot + EXP
                | EXP + TIgualacion + EXP
                | EXP + TDiferente + EXP
                | EXP + TMenor + EXP
                | EXP + TMayor + EXP
                | EXP + TMenorIgual + EXP
                | EXP + TMayorIgual + EXP
                | EXP + TSemejante + EXP
                | EXP + TMas + EXP
                | EXP + TMenos + EXP
                | EXP + TPor + EXP
                | EXP + TDivision + EXP
                | EXP + TModulo + EXP
                | EXP + TPotencia + EXP                
                | TParentesis_Abre + EXP + TParentesis_Cierra
                | TMenos + EXP
                | Numero
                | Id
                | Cadena
                | TVerdadero
                | TFalso
                | LLAMADA_FUNCION;

            RegisterOperators(1, Associativity.Left, TOr.ToString());
            RegisterOperators(2, Associativity.Left, TXor.ToString());
            RegisterOperators(3, Associativity.Left, TAnd.ToString());
            RegisterOperators(4, Associativity.Left, TNot.ToString());
            RegisterOperators(5, TIgualacion.ToString(), TDiferente.ToString(), TMenor.ToString(), TMayor.ToString(), TMenorIgual.ToString(), TMayorIgual.ToString(), TSemejante.ToString());
            RegisterOperators(6, Associativity.Left , TMas.ToString(), TMenos.ToString());
            RegisterOperators(7, Associativity.Left, TPor.ToString(), TDivision.ToString(), TModulo.ToString());
            RegisterOperators(8, Associativity.Right, TPotencia.ToString());


            MarkPunctuation(TDosPuntos, TPuntoyComa, TComa, TParentesis_Abre, TParentesis_Cierra, TLlave_Abre, TLlave_Cierra);
            MarkTransient(TIPO, SIMPLIFICADA);
            //No terminal de inicio
            this.Root = INICIO;

            //Para generar el AST
            //LanguageFlags = LanguageFlags.CreateAst;
        }
    }
}
