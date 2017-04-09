using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using _Compi2_Practica_201213587.Encabezado;
using _Compi2_Practica_201213587.Funciones;


namespace _Compi2_Practica_201213587
{
    class GenerarTablaSimbolo
    {
        public EjecutarSBS Ejecutar { get; set; }
        public GenerarTablaSimbolo(ParseTree Arbol, String Ruta)
        {
            Ejecutar = (EjecutarSBS)RecorrerArbol(Arbol.Root);
            Ejecutar.Ruta = Ruta;          
        }


        public Object RecorrerArbol(ParseTreeNode Nodo)
        {
            switch (Nodo.Term.Name)
            {
                case Constante.INICIO:
                    EjecutarSBS ejecucion = new EjecutarSBS();

                    foreach (ParseTreeNodeList hijo in (List<ParseTreeNodeList>)RecorrerArbol(Nodo.ChildNodes[0])) //enviamos a analizar el encabezado
                    {
                        if (hijo[0].Term.Name == Constante.TDefine)
                        {
                            if (hijo[1].Term.Name == "numero")
                            {
                                ejecucion.SetDefineNumber(Double.Parse(hijo[1].Token.ValueString));
                            } else if (hijo[1].Term.Name == Constante.Cadena)
                            {
                                ejecucion.SetDefineRuta((String)hijo[1].Token.Value);
                            }
                        } else if (hijo[0].Term.Name == Constante.TIncluye)
                        {
                            EIncluye incluye = new Encabezado.EIncluye(hijo[1].Token.Text + ".sbs", "");
                            
                            ejecucion.AgregarIncluye(new Simbolo(incluye.Archivo, Constante.TIncluye, Constante.Cadena, hijo[1].Token.Location.Line + 1, hijo[1].Token.Location.Column + 1, null, incluye));
                        }
                    }

                    Ambito global = new Ambito("Global");//creamos el ambito global para la ejecucion

                    global.Incerteza = ejecucion.DefineNumber;
                    global.RutaImagenes = ejecucion.DefineRuta;
                    global.TablaSimbolo = (List<Simbolo>)RecorrerArbol(Nodo.ChildNodes[1]);//enviamos a analizar el cuerpo que retornara una lista de simbolos

                    //asignamos el ambito padre a las instrucciones
                    foreach (Simbolo simbolo in global.TablaSimbolo)
                    {
                        if (simbolo.Ambito != null)
                        {
                            simbolo.Ambito.Padre = global;
                        }
                    }

                    ejecucion.Global = global;
                    return ejecucion;

                case Constante.LISTA_ENCABEZADOS:
                    if (Nodo.ChildNodes.Count > 0)
                    {
                        return RecorrerArbol(Nodo.ChildNodes[0]);
                    }
                    else
                    {
                        List<ParseTreeNodeList> vacio = new List<ParseTreeNodeList>();
                        return vacio;
                    }

                case Constante.LISTA_ENCABEZADO:
                    List<ParseTreeNodeList> listae = new List<ParseTreeNodeList>();
                    foreach (ParseTreeNode hijo in Nodo.ChildNodes)
                    {
                        listae.Add((ParseTreeNodeList)RecorrerArbol(hijo));
                    }
                    return listae;

                case Constante.ENCABEZADO:
                    return RecorrerArbol(Nodo.ChildNodes[0]);

                case Constante.DEFINE:
                    return Nodo.ChildNodes;

                case Constante.INCLUYE:
                    return Nodo.ChildNodes;

                case Constante.LISTA_SENTENCIA:
                    List<Simbolo> listas = new List<Simbolo>();
                    foreach (ParseTreeNode hijo in Nodo.ChildNodes)
                    {
                        foreach (Simbolo s in (List<Simbolo>)RecorrerArbol(hijo))
                        {
                            listas.Add(s);
                        }
                    }
                    return listas;

                case Constante.SENTENCIA:
                    return RecorrerArbol(Nodo.ChildNodes[0]);


                case Constante.PRINCIPAL:

                    Ambito ambito = new Ambito(Constante.TPrincipal);
                    if (Nodo.ChildNodes.Count > 0)
                    {
                        //asignamos el listado de instrucciones al ambito principal
                        if (Nodo.ChildNodes[1].ChildNodes.Count > 0)
                        {
                            ambito.TablaSimbolo = (List<Simbolo>)RecorrerArbol(Nodo.ChildNodes[1]);
                            //asignamos el ambito padre a la lista de instrucciones
                            foreach (Simbolo simbolo in ambito.TablaSimbolo)
                            {
                                if (simbolo.Ambito != null)
                                {
                                    simbolo.Ambito.Padre = ambito;
                                }
                            }
                        }

                    }
                    List<Simbolo> aux = new List<Simbolo>();
                    aux.Add(new Simbolo(Constante.TPrincipal, Constante.TPrincipal, Constante.TPrincipal, Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column + 1, ambito, null));
                    return aux;

                case Constante.LISTA_INSTRUCCIONES:
                    if (Nodo.ChildNodes.Count > 0)
                    {

                        return RecorrerArbol(Nodo.ChildNodes[0]);
                    } else
                    {
                        List<Simbolo> vacio = new List<Simbolo>();
                        return vacio;
                    }

                case Constante.LISTA_INSTRUCCION:
                    List<Simbolo> listai = new List<Simbolo>();
                    foreach (ParseTreeNode hijo in Nodo.ChildNodes)
                    {
                        List<Simbolo> listhijo = (List<Simbolo>)RecorrerArbol(hijo);
                        foreach (Simbolo simbolo in listhijo)
                        {
                            listai.Add(simbolo);
                        }
                    }
                    return listai;


                case Constante.INSTRUCCION:
                    return RecorrerArbol(Nodo.ChildNodes[0]);

                case Constante.DECLARACION:
                    List<Simbolo> listadecla = new List<Simbolo>();

                    foreach (ParseTreeNode id in Nodo.ChildNodes[1].ChildNodes)
                    {
                        if (Nodo.ChildNodes.Count == 4)
                        {
                            listadecla.Add(new Simbolo(id.Token.ValueString, Constante.DECLARACION, Nodo.ChildNodes[0].Token.ValueString, id.Token.Location.Line + 1, id.Token.Location.Column + 1, new Ambito(id.Token.ValueString), new FExpresion(Nodo.ChildNodes[3])));
                        } else
                        {
                            listadecla.Add(new Simbolo(id.Token.ValueString, Constante.DECLARACION, Nodo.ChildNodes[0].Token.ValueString, id.Token.Location.Line + 1, id.Token.Location.Column + 1, new Ambito(id.Token.ValueString), null));
                        }
                    }

                    return listadecla;


                case Constante.ASIGNACION:
                    List<Simbolo> listasigna = new List<Simbolo>();

                    listasigna.Add(new Simbolo(Nodo.ChildNodes[0].Token.ValueString, Constante.ASIGNACION, "", Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column + 1, new Ambito(Constante.ASIGNACION), new FExpresion(Nodo.ChildNodes[2])));

                    FExpresion a = (FExpresion)(listasigna[0].Objeto);
                    
                    return listasigna;

                case Constante.FUNCION:
                    Ambito ambitof = new Ambito(Nodo.ChildNodes[1].Token.ValueString);
                    FFuncion ffuncion = new FFuncion();

                    //enviamos a recorrer si tiene parametros
                    if (Nodo.ChildNodes[2].ChildNodes.Count > 0)
                    {
                        foreach (ParseTreeNode hijo in Nodo.ChildNodes[2].ChildNodes[0].ChildNodes)
                        {
                            ffuncion.Parametros.Add(new Simbolo(hijo.ChildNodes[1].Token.ValueString, Constante.DECLARACION, hijo.ChildNodes[0].Token.ValueString, hijo.ChildNodes[1].Token.Location.Line + 1, hijo.ChildNodes[1].Token.Location.Column + 1, new Ambito(hijo.ChildNodes[1].Token.ValueString), null));
                        }
                    }


                    //asignamos el listado de instrucciones al ambito
                    if (Nodo.ChildNodes[3].ChildNodes.Count > 0)
                    {
                        ambitof.TablaSimbolo = (List<Simbolo>)RecorrerArbol(Nodo.ChildNodes[3]);
                    }
                    //asignamos el ambito padre a la lista de instrucciones
                    foreach (Simbolo simbolo in ambitof.TablaSimbolo)
                    {
                        if (simbolo.Ambito != null)
                        {
                            simbolo.Ambito.Padre = ambitof;
                        }
                    }
                    List<Simbolo> auxf = new List<Simbolo>();
                    ffuncion.Ambito = ambitof;
                    auxf.Add(new Simbolo(Nodo.ChildNodes[1].Token.ValueString, Constante.FUNCION, Nodo.ChildNodes[0].Token.ValueString, Nodo.ChildNodes[1].Token.Location.Line + 1, Nodo.ChildNodes[1].Token.Location.Column + 1, ambitof, ffuncion));
                    return auxf;


                case Constante.LLAMADA_FUNCION:
                    FLlamada fllamadafuncion = new FLlamada();

                    //enviamos a recorrer si tiene parametros
                    if (Nodo.ChildNodes[1].ChildNodes.Count > 0)
                    {
                        foreach (ParseTreeNode hijo in Nodo.ChildNodes[1].ChildNodes[0].ChildNodes)
                        {
                            fllamadafuncion.Parametros.Add(new FExpresion(hijo));
                        }
                    }
                    List<Simbolo> listllada = new List<Simbolo>();
                    fllamadafuncion.Ambito = new Ambito(Nodo.ChildNodes[0].Token.ValueString);
                    listllada.Add(new Simbolo(Nodo.ChildNodes[0].Token.ValueString, Constante.LLAMADA_FUNCION, "", Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column + 1, fllamadafuncion.Ambito, fllamadafuncion));
                    return listllada;


                case Constante.RETORNO:
                    List<Simbolo> listaretorno = new List<Simbolo>();
                    if (Nodo.ChildNodes.Count == 2)
                    {
                        listaretorno.Add(new Simbolo(Constante.TRetorno, Constante.TRetorno, Constante.TRetorno, Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column + 1, new Ambito(Constante.TRetorno), new FExpresion(Nodo.ChildNodes[1])));
                    }
                    else
                    {
                        listaretorno.Add(new Simbolo(Constante.TRetorno, Constante.TRetorno, Constante.TRetorno, Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column + 1, new Ambito(Constante.TRetorno), null));
                    }
                    return listaretorno;

                case Constante.SI:
                    List<Simbolo> listasi = new List<Simbolo>();

                    FSi fsi = new FSi();
                    Ambito ambitosi = new Ambito(Constante.TSi);

                    fsi.Condicion = new FExpresion(Nodo.ChildNodes[1]);//asignamos la condicion
                    if (Nodo.ChildNodes[2].ChildNodes.Count > 0)
                    {
                        List<Simbolo> listasis = (List<Simbolo>)RecorrerArbol(Nodo.ChildNodes[2]);
                        foreach (Simbolo hijo in listasis)
                        {
                            //agregamos el ambito padre que seria el si
                            hijo.Ambito.Padre = ambitosi;
                            //agregamos los simbolos al ambito de si
                            ambitosi.TablaSimbolo.Add(hijo);
                        }
                    }

                    if (Nodo.ChildNodes[3].ChildNodes.Count > 0)
                    {
                        fsi.Sino = new Ambito(Constante.TSino);
                        fsi.Sino.Padre = ambitosi;
                        List<Simbolo> listasino = (List<Simbolo>)RecorrerArbol(Nodo.ChildNodes[3]);
                        foreach (Simbolo hijo in listasino)
                        {
                            //agregamos el ambito padre que seria el sino
                            hijo.Ambito.Padre = fsi.Sino;
                            //agregamos los simbolos al ambito de sino
                            fsi.Sino.TablaSimbolo.Add(hijo);
                        }
                    }
                    listasi.Add(new Simbolo(Constante.TSi, Constante.TSi, Constante.TSi, Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column + 1, ambitosi, fsi));

                    return listasi;

                case Constante.SINO:
                    List<Simbolo> listsino = (List<Simbolo>)RecorrerArbol(Nodo.ChildNodes[1]);
                    return listsino;

                case Constante.SELECCIONA:
                    List<Simbolo> listselecciona = new List<Simbolo>();
                    FSelecciona fselecciona = new FSelecciona();
                    //enviamos la expresion
                    fselecciona.Expresion = new FExpresion(Nodo.ChildNodes[1]);
                    //asignamos la lista de casos
                    fselecciona.Casos = (List<FCaso>)RecorrerArbol(Nodo.ChildNodes[2]);
                    //comprobamos si hay defecto
                    if (Nodo.ChildNodes[3].ChildNodes.Count > 0)
                    {
                        List<Simbolo> simbolodefecto = (List<Simbolo>)RecorrerArbol(Nodo.ChildNodes[3].ChildNodes[0].ChildNodes[1]);
                        Ambito ambitodefecto = new Ambito(Constante.TDefecto);
                        foreach (Simbolo hijo in simbolodefecto)
                        {
                            hijo.Ambito.Padre = ambitodefecto;
                            ambitodefecto.TablaSimbolo.Add(hijo);
                        }
                        fselecciona.Defecto = new FCaso(ambitodefecto, Constante.TDefecto, "");
                    }
                    Ambito ambitosele = new Ambito(Constante.TSelecciona);
                    fselecciona.Ambito = ambitosele;
                    listselecciona.Add(new Simbolo(Constante.TSelecciona, Constante.TSelecciona, Constante.TSelecciona, Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column + 1, fselecciona.Ambito, fselecciona));
                    //retornamos la lsita 
                    return listselecciona;

                case Constante.LISTA_CASOS:
                    List<FCaso> listacaso = new List<FCaso>();
                    foreach (ParseTreeNode hijo in Nodo.ChildNodes)
                    {
                        listacaso.Add((FCaso)RecorrerArbol(hijo));
                    }
                    return listacaso;

                case Constante.CASO:

                    Ambito ambitocaso = new Ambito(Constante.TCaso);
                    FCaso caso;
                    if (Nodo.ChildNodes[0].ChildNodes[0].Token.Terminal.ErrorAlias == Constante.Numero)
                    {
                        caso = new FCaso(ambitocaso, Constante.TNumber, Nodo.ChildNodes[0].ChildNodes[0].Token.ValueString);
                    }else
                    {
                        caso = new FCaso(ambitocaso, Constante.TString, Nodo.ChildNodes[0].ChildNodes[0].Token.ValueString);
                    }
                   

                    foreach (Simbolo hijo in (List<Simbolo>)RecorrerArbol(Nodo.ChildNodes[1]))
                    {
                        caso.ambito.TablaSimbolo.Add(hijo);
                    }
                    return caso;


                case Constante.PARA:
                    List<Simbolo> listapara = new List<Simbolo>();
                    FPara fpara = new FPara();

                    fpara.ambito = new Ambito(Constante.TPara);
                    fpara.Condicion = new FExpresion(Nodo.ChildNodes[5]);
                    fpara.Declaracion = new Simbolo(Nodo.ChildNodes[2].Token.ValueString, Constante.DECLARACION, Constante.TNumber, Nodo.ChildNodes[2].Token.Location.Line + 1, Nodo.ChildNodes[2].Token.Location.Column + 1, new Ambito(Constante.DECLARACION), new FExpresion(Nodo.ChildNodes[4]));
                    fpara.Operacion = Nodo.ChildNodes[6].Token.ValueString;

                    foreach (Simbolo hijo in (List<Simbolo>)RecorrerArbol(Nodo.ChildNodes[7]))
                    {
                        hijo.Ambito.Padre = fpara.ambito;
                        fpara.ambito.TablaSimbolo.Add(hijo);
                    }
                    listapara.Add(new Simbolo(Constante.TPara, Constante.TPara, Constante.TPara, Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column + 1, fpara.ambito, fpara));
                    return listapara;


                case Constante.HASTA:
                    List<Simbolo> listahasta = new List<Simbolo>();

                    FHasta fhasta = new FHasta();

                    fhasta.ambito = new Ambito(Constante.THasta);
                    fhasta.Condicion = new FExpresion(Nodo.ChildNodes[1]);

                    foreach (Simbolo hijo in (List<Simbolo>)RecorrerArbol(Nodo.ChildNodes[2]))
                    {
                        hijo.Ambito.Padre = fhasta.ambito;
                        fhasta.ambito.TablaSimbolo.Add(hijo);
                    }
                    listahasta.Add(new Simbolo(Constante.THasta, Constante.THasta, Constante.THasta, Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column + 1, fhasta.ambito, fhasta));
                    return listahasta;

                case Constante.MIENTRAS:
                    List<Simbolo> listamientras = new List<Simbolo>();

                    FMientras fmientras = new FMientras();

                    fmientras.ambito = new Ambito(Constante.THasta);
                    fmientras.Condicion = new FExpresion(Nodo.ChildNodes[1]);

                    foreach (Simbolo hijo in (List<Simbolo>)RecorrerArbol(Nodo.ChildNodes[2]))
                    {
                        hijo.Ambito.Padre = fmientras.ambito;
                        fmientras.ambito.TablaSimbolo.Add(hijo);
                    }
                    listamientras.Add(new Simbolo(Constante.TMientras, Constante.TMientras, Constante.TMientras, Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column + 1, fmientras.ambito, fmientras));
                    return listamientras;

                case Constante.TDetener:
                    List<Simbolo> listadetener = new List<Simbolo>();
                    listadetener.Add(new Simbolo(Constante.TDetener, Constante.TDetener, Constante.TDetener, Nodo.Token.Location.Line + 1, Nodo.Token.Location.Line + 1, new Ambito(Constante.TDetener), null));
                    return listadetener;

                case Constante.TContinuar:
                    List<Simbolo> listacontinuar = new List<Simbolo>();
                    listacontinuar.Add(new Simbolo(Constante.TContinuar, Constante.TContinuar, Constante.TContinuar, Nodo.Token.Location.Line + 1, Nodo.Token.Location.Line + 1, new Ambito(Constante.TContinuar), null));
                    return listacontinuar;

                case Constante.FMOSTRAR:
                    FMostrar fmostrar = new FMostrar();
                    if (Nodo.ChildNodes.Count > 2)
                    {                        
                        foreach (ParseTreeNode hijo in Nodo.ChildNodes[2].ChildNodes)
                        {
                            fmostrar.Parametros.Add(new FExpresion(hijo));
                        }                        
                    }

                    fmostrar.Cadena = Nodo.ChildNodes[1].Token.ValueString;
                    List<Simbolo> listafmostrar = new List<Simbolo>();
                    listafmostrar.Add(new Simbolo(Constante.TMostrar, Constante.TMostrar, Constante.TMostrar, Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column +1, new Ambito(Constante.TMostrar), fmostrar));
                    return listafmostrar;


                case Constante.FDIBUJARAST:
                    FDibujarAST fdibujarast = new FDibujarAST();
                    
                    fdibujarast.Id = Nodo.ChildNodes[1].Token.ValueString;
                    List<Simbolo> listafdibujarast = new List<Simbolo>();
                    listafdibujarast.Add(new Simbolo(Constante.TDibujarAST, Constante.TDibujarAST, Constante.TDibujarAST, Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column + 1, new Ambito(Constante.TDibujarAST), fdibujarast));

                    return listafdibujarast;

                case Constante.FDIBUJAREXP:
                    FDibujarExpresion fdibujarexp = new FDibujarExpresion();

                    fdibujarexp.Expresion = new FExpresion(Nodo.ChildNodes[1]);
                    List<Simbolo> listafdibujarexp = new List<Simbolo>();
                    listafdibujarexp.Add(new Simbolo(Constante.TDibujarEXP, Constante.TDibujarEXP, Constante.TDibujarEXP, Nodo.ChildNodes[0].Token.Location.Line + 1, Nodo.ChildNodes[0].Token.Location.Column + 1, new Ambito(Constante.TDibujarEXP), fdibujarexp));
                    return listafdibujarexp;

                default:
                    return null;
                                        
            }
        }

        public EjecutarSBS getEjecutar()
        {
            return Ejecutar;
        }
    }
}
