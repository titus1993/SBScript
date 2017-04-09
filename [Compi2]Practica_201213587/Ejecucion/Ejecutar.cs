using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Compi2_Practica_201213587.Funciones;
using System.Windows.Forms;
using System.IO;

namespace _Compi2_Practica_201213587.Ejecucion
{
    class Ejecutar
    {
        EjecutarSBS Inicio;
        public Ejecutar()
        {

        }
        public Ejecutar(EjecutarSBS inicio)
        {
            Inicio = inicio;
            
            if (TitusNotifiaciones.ContarErrores() == 0)
            {
                BuscarErroresGlobal(Inicio);
                if (TitusNotifiaciones.ContarErrores() == 0)
                {
                    TablaVariables.Tabla.Add(new Variables());//asignamos el espacio para el principal
                    LlenarGlobal();
                }

                if (TitusNotifiaciones.ContarErrores() == 0)
                {
                    LlenarIncluyes(Inicio);
                }

                if (TitusNotifiaciones.ContarErrores() == 0)
                {
                    EjecutarPrincipal();
                }

                if (TitusNotifiaciones.ContarErrores() == 0)
                {
                    TitusNotifiaciones.MostrarImagen();
                }             
                             
                
            }
            if (TitusNotifiaciones.ContarErrores() > 0)
            {
                MessageBox.Show("Se encontraron error, favor revisar las notifiaciones", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else
            {
                MessageBox.Show("Ejecucion realizada con exito.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void BuscarErroresGlobal(EjecutarSBS start)
        {
            if (!Directory.Exists(start.Global.RutaImagenes))
            {
                MessageBox.Show("La carpeta de imagenes " + start.Global.RutaImagenes + " no existe, se utilizara por defecto " + Constante.DefaultDefineRuta, "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
            foreach (Simbolo simbolo in start.Global.TablaSimbolo)
            {
                simbolo.RutaArchivo = start.Ruta;
                simbolo.Ambito.Incerteza = start.Global.Incerteza;
                if (Directory.Exists(start.Global.RutaImagenes))
                {
                    simbolo.Ambito.RutaImagenes = start.Global.RutaImagenes;
                }else
                {
                    //MessageBox.Show("La carpeta de imagenes " + Inicio.Global.RutaImagenes + " no existe, se utilizara por defecto " + Constante.DefaultDefineRuta,"Alerta",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    simbolo.Ambito.RutaImagenes = Constante.DefaultDefineRuta;
                }
                
                simbolo.Ambito.RutaArchivo = start.Ruta;
                if(simbolo.Ambito.TablaSimbolo.Count > 0)
                {
                    foreach (Simbolo simbolo2 in simbolo.Ambito.TablaSimbolo)
                    {
                        BuscarErroresAmbitos(simbolo2, start.Ruta, start.Global.Incerteza, simbolo.Ambito.RutaImagenes);
                    }
                    
                }
            }
        }

        public void BuscarErroresAmbitos(Simbolo simbolo, String archivo, Double Incerteza, String rutaimagen)
        {

            simbolo.RutaArchivo = archivo;
            if (simbolo.Ambito != null)
            {
                simbolo.Ambito.Incerteza = Inicio.Global.Incerteza;
                simbolo.Ambito.RutaImagenes = Inicio.Global.RutaImagenes;
                simbolo.Ambito.RutaArchivo = Inicio.Ruta;

                foreach (Simbolo s in simbolo.Ambito.TablaSimbolo)
                {

                    BuscarErroresAmbitos(s, archivo, Incerteza, rutaimagen);
                }
            }


            switch (simbolo.Rol)
            {
                case Constante.DECLARACION:

                    if (simbolo.Objeto != null)
                    {
                        FExpresion expdecla = (FExpresion)simbolo.Objeto;
                        expdecla.RutaArchivo = archivo;
                        expdecla.RutaImagen = rutaimagen;
                        expdecla.Incerteza = Incerteza;


                    }

                    break;

                case Constante.ASIGNACION:
                    if (simbolo.Objeto != null)
                    {
                        FExpresion expdecla = (FExpresion)simbolo.Objeto;
                        expdecla.RutaArchivo = archivo;
                        expdecla.RutaImagen = rutaimagen;
                        expdecla.Incerteza = Incerteza;

                    }
                    break;

                case Constante.LLAMADA_FUNCION:

                    if (simbolo.Objeto != null)
                    {
                        FLlamada fllamada = (FLlamada)simbolo.Objeto;
                        fllamada.RutaArchivo = archivo;
                        fllamada.RutaImagen = rutaimagen;
                        fllamada.Incerteza = Incerteza;


                        foreach (FExpresion parametro in fllamada.Parametros)
                        {
                            parametro.RutaArchivo = archivo;
                            parametro.RutaImagen = rutaimagen;
                            parametro.Incerteza = Incerteza;
                        }

                        if (fllamada.Ambito != null)
                        {
                            foreach (Simbolo s in fllamada.Ambito.TablaSimbolo)
                            {
                                BuscarErroresAmbitos(s, archivo, Incerteza, rutaimagen);
                            }
                        }
                    }


                    break;

                case Constante.TRetorno:
                    if (simbolo.Objeto != null)
                    {
                        FExpresion fretorno = (FExpresion)simbolo.Objeto;
                        fretorno.Incerteza = Incerteza;
                        fretorno.RutaArchivo = archivo;
                        fretorno.RutaImagen = rutaimagen;
                    }
                    break;

                case Constante.TSi:
                    if (simbolo.Objeto != null)
                    {
                        FSi fsi = (FSi)simbolo.Objeto;
                        fsi.RutaArchivo = archivo;
                        fsi.RutaImagen = rutaimagen;
                        fsi.Incerteza = Incerteza;

                        fsi.Condicion.RutaArchivo = archivo;
                        fsi.Condicion.RutaImagen = rutaimagen;
                        fsi.Condicion.Incerteza = Incerteza;


                        if (fsi.Sino != null)
                        {
                            foreach (Simbolo s in fsi.Sino.TablaSimbolo)
                            {
                                BuscarErroresAmbitos(s, archivo, Incerteza, rutaimagen);
                            }
                        }
                    }
                    break;

                case Constante.TSelecciona:
                    if (simbolo.Objeto != null)
                    {
                        FSelecciona fselecciona = (FSelecciona)simbolo.Objeto;
                        fselecciona.RutaArchivo = archivo;
                        fselecciona.RutaImagen = rutaimagen;
                        fselecciona.Incerteza = Incerteza;

                        if (fselecciona.Ambito != null)
                        {
                            foreach (Simbolo s in fselecciona.Ambito.TablaSimbolo)
                            {
                                BuscarErroresAmbitos(s, archivo, Incerteza, rutaimagen);
                            }
                        }

                        if (fselecciona.Expresion != null)
                        {
                            fselecciona.Expresion.RutaArchivo = archivo;
                            fselecciona.Expresion.RutaImagen = rutaimagen;
                            fselecciona.Expresion.Incerteza = Incerteza;
                        }

                        if (fselecciona.Casos != null)
                        {
                            foreach (FCaso caso in fselecciona.Casos)
                            {
                                caso.RutaArchivo = archivo;
                                caso.RutaImagen = rutaimagen;
                                caso.Incerteza = Incerteza;
                                if (caso.ambito != null)
                                {
                                    foreach (Simbolo s in caso.ambito.TablaSimbolo)
                                    {
                                        BuscarErroresAmbitos(s, archivo, Incerteza, rutaimagen);
                                    }
                                }
                            }
                        }

                        if (fselecciona.Defecto != null)
                        {
                            fselecciona.Defecto.RutaArchivo = archivo;
                            fselecciona.Defecto.RutaImagen = rutaimagen;
                            fselecciona.Defecto.Incerteza = Incerteza;
                            if (fselecciona.Defecto.ambito != null)
                            {
                                foreach (Simbolo s in fselecciona.Defecto.ambito.TablaSimbolo)
                                {
                                    BuscarErroresAmbitos(s, archivo, Incerteza, rutaimagen);
                                }
                            }
                        }
                    }
                    break;

                case Constante.TPara:
                    if (simbolo.Objeto != null)
                    {
                        FPara fpara = (FPara)simbolo.Objeto;
                        fpara.Incerteza = Incerteza;
                        fpara.RutaArchivo = archivo;
                        fpara.RutaImagen = rutaimagen;

                        if (fpara.Condicion != null)
                        {
                            fpara.Condicion.Incerteza = Incerteza;
                            fpara.Condicion.RutaArchivo = archivo;
                            fpara.Condicion.RutaImagen = rutaimagen;
                        }

                        if (fpara.Declaracion != null)
                        {
                            foreach (Simbolo s in fpara.ambito.TablaSimbolo)
                            {
                                BuscarErroresAmbitos(s, archivo, Incerteza, rutaimagen);
                            }
                        }
                    }
                    break;

                case Constante.THasta:
                    if (simbolo.Objeto != null)
                    {
                        FHasta fhasta = (FHasta)simbolo.Objeto;

                        if (fhasta.Condicion != null)
                        {
                            fhasta.Condicion.Incerteza = Incerteza;
                            fhasta.Condicion.RutaArchivo = archivo;
                            fhasta.Condicion.RutaImagen = rutaimagen;
                        }

                        if (fhasta.ambito != null)
                        {
                            foreach (Simbolo s in fhasta.ambito.TablaSimbolo)
                            {
                                BuscarErroresAmbitos(s, archivo, Incerteza, rutaimagen);
                            }
                        }
                    }
                    break;

                case Constante.TMientras:
                    if (simbolo.Objeto != null)
                    {
                        FMientras fmientras = (FMientras)simbolo.Objeto;

                        if (fmientras.Condicion != null)
                        {
                            fmientras.Condicion.Incerteza = Incerteza;
                            fmientras.Condicion.RutaArchivo = archivo;
                            fmientras.Condicion.RutaImagen = rutaimagen;
                        }

                        if (fmientras.ambito != null)
                        {
                            foreach (Simbolo s in fmientras.ambito.TablaSimbolo)
                            {
                                BuscarErroresAmbitos(s, archivo, Incerteza, rutaimagen);
                            }
                        }
                    }
                    break;

                case Constante.TDetener:
                    if (simbolo.Objeto != null)
                    {

                    }
                    break;

                case Constante.TContinuar:
                    if (simbolo.Objeto != null)
                    {

                    }
                    break;

                case Constante.TMostrar:
                    if (simbolo.Objeto != null)
                    {
                        FMostrar funcion = (FMostrar)simbolo.Objeto;
                        funcion.Incerteza = Incerteza;
                        funcion.RutaArchivo = archivo;
                        funcion.RutaImagen = rutaimagen;

                        foreach (FExpresion expmostrar in funcion.Parametros)
                        {
                            expmostrar.Incerteza = Incerteza;
                            expmostrar.RutaImagen = rutaimagen;
                            expmostrar.RutaArchivo = archivo;
                        }
                    }
                    break;

                case Constante.TDibujarAST:
                    if (simbolo.Objeto != null)
                    {
                        FDibujarAST funcion = (FDibujarAST)simbolo.Objeto;
                        funcion.Incerteza = Incerteza;
                        funcion.RutaArchivo = archivo;
                        funcion.RutaImagen = rutaimagen;

                    }
                    break;

                case Constante.TDibujarEXP:
                    if (simbolo.Objeto != null)
                    {
                        FDibujarExpresion funcion = (FDibujarExpresion)simbolo.Objeto;
                        funcion.Incerteza = Incerteza;
                        funcion.RutaArchivo = archivo;
                        funcion.RutaImagen = rutaimagen;

                        if (funcion.Expresion != null)
                        {
                            funcion.Expresion.Incerteza = Incerteza;
                            funcion.Expresion.RutaArchivo = archivo;
                            funcion.Expresion.RutaImagen = rutaimagen;
                        }
                    }
                    break;


            }

            if (simbolo.Ambito.TablaSimbolo.Count > 0)
            {
                foreach (Simbolo simbolo2 in simbolo.Ambito.TablaSimbolo)
                {
                    BuscarErroresAmbitos(simbolo2, archivo, Incerteza, rutaimagen);
                }

            }
        }

        
        public void LlenarIncluyes(EjecutarSBS arc)
        {
            foreach (EjecutarSBS archiv in arc.Archivos)
            {
                BuscarErroresGlobal(archiv);
                LlenarIncluyes(archiv);
                //metemos las variables globales a la tabla y los metodos del primer archivo
                foreach (Simbolo global in archiv.Global.TablaSimbolo)
                {
                    switch (global.Rol)
                    {
                        case Constante.DECLARACION:
                            if (!TablaVariables.BuscarNombre(global.Nombre))
                            {
                                EjecutarDeclaracion(global);
                            }
                            else
                            {
                                TabError error = new TabError();
                                error.InsertarFila(Constante.ErroEjecucion, "Ya hay declarado un metodo o variable con el nombre " + global.Nombre, global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                                TitusNotifiaciones.setDatosErrores(error);
                            }
                            break;

                        

                        case Constante.FUNCION:
                            if (TablaVariables.BuscarVariable(global.Nombre) == null)
                            {
                                if (TablaVariables.BuscarMetodo(global.Nombre, (FFuncion)global.Objeto) == false)
                                {
                                    TablaVariables.Tabla.Add(new Variables(global.Tipo, global.Nombre, Constante.TMetodo, global.Objeto, global.Ambito, global.Fila, global.Columna));
                                }
                                else
                                {
                                    TabError error = new TabError();
                                    error.InsertarFila(Constante.ErroEjecucion, "Ya hay declarado un metodo con el nombre " + global.Nombre + " y los mismos parametros", global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                                    TitusNotifiaciones.setDatosErrores(error);
                                }
                            }
                            else
                            {
                                TabError error = new TabError();
                                error.InsertarFila(Constante.ErroEjecucion, "Ya hay declarada una variable con el metodo " + global.Nombre, global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                                TitusNotifiaciones.setDatosErrores(error);
                            }
                            break;
                    }
                }
            }
        }


        public void LlenarGlobal()
        {
            //metemos las variables globales a la tabla y los metodos del primer archivo
            foreach (Simbolo global in Inicio.Global.TablaSimbolo)
            {
                switch (global.Rol)
                {
                    case Constante.DECLARACION:
                        if (!TablaVariables.BuscarNombre(global.Nombre))
                        {
                            EjecutarDeclaracion(global);
                        }else
                        {
                            TabError error = new TabError();
                            error.InsertarFila(Constante.ErroEjecucion, "Ya hay declarado un metodo o variable con el nombre " + global.Nombre, global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                            TitusNotifiaciones.setDatosErrores(error);
                        }
                        break;

                    case Constante.TPrincipal:
                        if (TablaVariables.Tabla[0].Nombre != Constante.TPrincipal)
                        {
                            TablaVariables.Tabla[0] = new Variables(Constante.TPrincipal, Constante.TPrincipal, Constante.TPrincipal, global.Objeto, global.Ambito, global.Fila, global.Columna);
                        }else
                        {
                            TabError error = new TabError();
                            error.InsertarFila(Constante.ErroEjecucion, "Ya hay declarado un metodo principal", global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                            TitusNotifiaciones.setDatosErrores(error);
                        }
                        
                        break;

                    case Constante.FUNCION:
                        if (TablaVariables.BuscarVariable(global.Nombre) == null)
                        {
                            if (TablaVariables.BuscarMetodo(global.Nombre, (FFuncion)global.Objeto) == false)
                            {
                                TablaVariables.Tabla.Add(new Variables(global.Tipo, global.Nombre, Constante.TMetodo, global.Objeto, global.Ambito, global.Fila, global.Columna));
                            }
                            else
                            {
                                TabError error = new TabError();
                                error.InsertarFila(Constante.ErroEjecucion, "Ya hay declarado un metodo con el nombre " + global.Nombre + " y los mismos parametros", global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                                TitusNotifiaciones.setDatosErrores(error);
                            }
                        }else
                        {
                            TabError error = new TabError();
                            error.InsertarFila(Constante.ErroEjecucion, "Ya hay declarada una variable con el metodo "+ global.Nombre, global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                            TitusNotifiaciones.setDatosErrores(error);
                        }
                        break;
                }
            }
        }

        public void EjecutarPrincipal()
        {
            if (TitusNotifiaciones.ContarErrores() == 0)
            {
                if (TablaVariables.Tabla[0].Nombre == Constante.TPrincipal)
                {
                    EjecutarInstrucciones(TablaVariables.Tabla[0].Ambito.TablaSimbolo);
                }else
                {
                    TabError error = new TabError();
                    error.InsertarFila(Constante.ErroEjecucion, "No existe metodo principal", Inicio.Ruta, "1", "1");
                    TitusNotifiaciones.setDatosErrores(error);
                }
                
            }
        }

        public void SacarAmbito(List<Simbolo> variables)
        {
            int cont = variables.Count -1;
            while (cont >=0 )
            {
                if (variables[cont].Rol == Constante.DECLARACION)
                {
                    if (TablaVariables.ExisteVariableTope(variables[cont].Nombre))
                    {
                        TablaVariables.SacarVariable(variables[cont].Nombre);
                    }
                }
                cont--;
            }
        }

        public void EjecutarInstrucciones(List<Simbolo> instrucciones)
        {
            foreach ( Simbolo instruccion in instrucciones)
            {
                if (TitusNotifiaciones.ContarErrores() == 0 && !TablaVariables.IsContinuar() && !TablaVariables.IsDetener() && !TablaVariables.IsRetorno())
                {
                    switch (instruccion.Rol)
                    {
                        case Constante.DECLARACION:
                            EjecutarDeclaracion(instruccion);
                            break;

                        case Constante.ASIGNACION:
                            EjecutarAsignacion(instruccion);
                            break;

                        case Constante.TDetener:
                            EjecutarDetener(instruccion);
                            break;

                        case Constante.TRetorno:
                            EjecutarRetorno(instruccion);
                            break;

                        case Constante.TContinuar:
                            EjecutarContinuar(instruccion);
                            break; 

                        case Constante.TSelecciona:
                            EjecutarSelecciona(instruccion);
                            break;

                        case Constante.TMientras:
                            EjecutarMientras(instruccion);
                            break;

                        case Constante.THasta:
                            EjecutarHasta(instruccion);
                            break;

                        case Constante.TMostrar:
                            FMostrar mostrar = (FMostrar)instruccion.Objeto;
                            mostrar.Mostrar(instruccion.Fila, instruccion.Columna, mostrar.RutaArchivo);
                            break;

                        case Constante.TDibujarEXP:
                            FDibujarExpresion dexp = (FDibujarExpresion)instruccion.Objeto;
                            dexp.DibujarExpresion();
                            break;

                        case Constante.TDibujarAST:
                            FDibujarAST dast = (FDibujarAST)instruccion.Objeto;
                            dast.DibujarAst(instruccion.Fila, instruccion.Columna);
                            break;

                        case Constante.TPara:
                            EjecutarPara(instruccion);
                            break;

                        case Constante.LLAMADA_FUNCION:
                            EjecutarFuncion(instruccion);
                            break;

                        case Constante.TSi:
                            EjecutarSi(instruccion);
                            break;
                    }
                }
            }
        }


        public void EjecutarPara(Simbolo para)
        {
            FPara fpara = (FPara)para.Objeto;
            //ejecutamos la declaracion
            EjecutarDeclaracion(fpara.Declaracion);
            NodoExpresion condicion = fpara.Condicion.ResolverExpresion(fpara.Incerteza, fpara.RutaArchivo);
            
            if (TitusNotifiaciones.ContarErrores() == 0)
            {
                if (condicion.Tipo == Constante.TBool)
                {
                    while (TitusNotifiaciones.ContarErrores() == 0 && condicion.Booleano && !TablaVariables.IsRetorno() && !TablaVariables.IsDetener() && !TablaVariables.IsRetorno())
                    {
                        condicion = fpara.Condicion.ResolverExpresion(fpara.Incerteza, fpara.RutaArchivo);
                        if (condicion.Booleano)
                        {
                            EjecutarInstrucciones(fpara.ambito.TablaSimbolo);
                        }

                        
                        if (TablaVariables.IsContinuar())
                        {
                            TablaVariables.SacarVariable();
                        }
                        condicion = fpara.Condicion.ResolverExpresion(fpara.Incerteza, fpara.RutaArchivo);
                        //sacamos el ambito
                        SacarAmbito(fpara.ambito.TablaSimbolo);
                        //realizamos la operacion ya sea ++ o --
                        if (fpara.Operacion == Constante.TAumento)
                        {
                            Variables var = TablaVariables.ObtenerTope();
                            if (var.Nombre == fpara.Declaracion.Nombre)
                            {
                                NodoExpresion val = (NodoExpresion)(var.Valor);
                                val.Numero = val.Numero + 1;
                                val.Cadena = val.Numero.ToString();
                            }
                        }else
                        {
                            Variables var = TablaVariables.ObtenerTope();
                            if (var.Nombre == fpara.Declaracion.Nombre)
                            {
                                NodoExpresion val = (NodoExpresion)(var.Valor);
                                val.Numero = val.Numero - 1;
                                val.Cadena = val.Numero.ToString();
                            }
                        }
                        
                    }
                    
                    if (TablaVariables.IsDetener())
                    {
                        TablaVariables.SacarVariable();
                    }
                    //sacamos la variable de operacion
                    if (TablaVariables.ExisteVariableTope(fpara.Declaracion.Nombre))
                    {
                        TablaVariables.SacarVariable();
                    }
                }
                else
                {
                    TabError error = new TabError();
                    error.InsertarFila(Constante.ErroEjecucion, "Se esperaba un valor Bool no uno de tipo: " + condicion.Tipo, fpara.RutaArchivo, para.Fila.ToString(), para.Columna.ToString());
                    TitusNotifiaciones.setDatosErrores(error);
                }
            }
        }

        public void EjecutarFuncion(Simbolo simbolo)
        {
            FLlamada listaparametros = (FLlamada)simbolo.Objeto;
            Variables metodo = TablaVariables.BuscarMetodo(simbolo.Nombre, listaparametros);
            if (metodo != null)
            {
                FFuncion funcion = (FFuncion)metodo.Valor;
                FLlamada llamada = (FLlamada)simbolo.Objeto;
                

                if (funcion.Parametros.Count == llamada.Parametros.Count)
                {
                    //metemos el return 
                    //Variables retorno = new Variables(Constante.TRetorno, Constante.TRetorno, Constante.RETORNO, null, null, 0, 0);
                    //TablaVariables.Tabla.Add(retorno);
                    int cont = 0;
                    //meter variables de los parametros
                    while (cont < funcion.Parametros.Count && TitusNotifiaciones.ContarErrores() == 0)
                    {
                        FExpresion f = (FExpresion)llamada.Parametros[cont];
                        NodoExpresion resultadoparametro = f.ResolverExpresion(funcion.Incerteza, funcion.RutaArchivo);
                        if (TitusNotifiaciones.ContarErrores() == 0)
                        {
                            if (funcion.Parametros[cont].Tipo == resultadoparametro.Tipo)
                            {
                                Variables parametro = new Variables(funcion.Parametros[cont].Tipo, funcion.Parametros[cont].Nombre, Constante.TVariable, resultadoparametro, null, simbolo.Fila, simbolo.Columna);
                                TablaVariables.Tabla.Add(parametro);
                            }
                            else
                            {
                                //error de asignacion del tipo de parametro
                                TabError error = new TabError();
                                error.InsertarFila(Constante.ErroEjecucion, "Se esperaba un tipo: " + funcion.Parametros[cont].Tipo + ", no un tipo: " + resultadoparametro.Tipo, funcion.RutaArchivo, resultadoparametro.Linea.ToString(), resultadoparametro.Columna.ToString());
                                TitusNotifiaciones.setDatosErrores(error);
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
                            TablaVariables.SacarVariable();
                        }
                        execute.SacarAmbito(metodo.Ambito.TablaSimbolo);
                        execute.SacarAmbito(funcion.Parametros);
                    }
                }
                else
                {
                    //error de cantidad de parametros
                    TabError error = new TabError();
                    error.InsertarFila(Constante.ErroEjecucion, "La funcion esperaba " + funcion.Parametros.Count + " parametros", funcion.RutaArchivo, simbolo.Fila.ToString(), simbolo.Columna.ToString());
                    TitusNotifiaciones.setDatosErrores(error);
                }


            }
            else
            {
                TabError error = new TabError();
                error.InsertarFila(Constante.ErroEjecucion, "No existe la funcion " + simbolo.Nombre + "()", simbolo.RutaArchivo, simbolo.Fila.ToString(), simbolo.Columna.ToString());
                TitusNotifiaciones.setDatosErrores(error);
                
            }
        }

        public void EjecutarRetorno(Simbolo retorno)
        {
            if (retorno.Objeto != null)
            {
                FExpresion aux = (FExpresion)retorno.Objeto;
                NodoExpresion val = aux.ResolverExpresion(retorno.Ambito.Incerteza, retorno.Ambito.RutaArchivo);
                if (TitusNotifiaciones.ContarErrores() == 0)
                {
                    TablaVariables.Tabla.Add(new Variables(Constante.TRetorno, Constante.TRetorno, Constante.TRetorno, val, null, retorno.Fila, retorno.Columna));
                }
            }
            else
            {
                TablaVariables.Tabla.Add(new Variables(Constante.TRetorno, Constante.TRetorno, Constante.TRetorno, null, null, retorno.Fila, retorno.Columna));
            }
               
        }

        public void EjecutarHasta(Simbolo hasta)
        {
            FHasta fhasta = (FHasta)hasta.Objeto;
            NodoExpresion condicion = fhasta.Condicion.ResolverExpresion(fhasta.Incerteza, fhasta.RutaArchivo);
            if (TitusNotifiaciones.ContarErrores() == 0)
            {
                if (condicion.Tipo == Constante.TBool)
                {
                    while (TitusNotifiaciones.ContarErrores() == 0 && !condicion.Booleano && !TablaVariables.IsRetorno() && !TablaVariables.IsDetener() && !TablaVariables.IsRetorno())
                    {
                        EjecutarInstrucciones(fhasta.ambito.TablaSimbolo);
                        if (TablaVariables.IsContinuar())
                        {
                            TablaVariables.SacarVariable();
                        }
                        condicion = fhasta.Condicion.ResolverExpresion(fhasta.Incerteza, fhasta.RutaArchivo);

                        SacarAmbito(fhasta.ambito.TablaSimbolo);
                    }
                    if (TablaVariables.IsDetener())
                    {
                        TablaVariables.SacarVariable();
                    }
                }
                else
                {
                    TabError error = new TabError();
                    error.InsertarFila(Constante.ErroEjecucion, "Se esperaba un valor Bool no uno de tipo: " + condicion.Tipo, fhasta.RutaArchivo, hasta.Fila.ToString(), hasta.Columna.ToString());
                    TitusNotifiaciones.setDatosErrores(error);
                }
            }
        }
        public void EjecutarMientras(Simbolo mientras)
        {
            FMientras fmientras = (FMientras)mientras.Objeto;
            NodoExpresion condicion = fmientras.Condicion.ResolverExpresion(fmientras.Incerteza, fmientras.RutaArchivo);
            if (TitusNotifiaciones.ContarErrores() == 0)
            {
                if (condicion.Tipo == Constante.TBool)
                {
                    while (TitusNotifiaciones.ContarErrores() == 0 && condicion.Booleano && !TablaVariables.IsRetorno() && !TablaVariables.IsDetener() && !TablaVariables.IsRetorno())
                    {
                        EjecutarInstrucciones(fmientras.ambito.TablaSimbolo);
                        if (TablaVariables.IsContinuar())
                        {
                            TablaVariables.SacarVariable();
                        }
                        condicion = fmientras.Condicion.ResolverExpresion(fmientras.Incerteza, fmientras.RutaArchivo);

                        SacarAmbito(fmientras.ambito.TablaSimbolo);
                    }
                    if (TablaVariables.IsDetener())
                    {
                        TablaVariables.SacarVariable();
                    }
                }
                else
                {
                    TabError error = new TabError();
                    error.InsertarFila(Constante.ErroEjecucion, "Se esperaba un valor Bool no uno de tipo: " + condicion.Tipo, fmientras.RutaArchivo, mientras.Fila.ToString(), mientras.Columna.ToString());
                    TitusNotifiaciones.setDatosErrores(error);
                }
            }
        }
        public void EjecutarCaso(FCaso caso)
        {
            EjecutarInstrucciones(caso.ambito.TablaSimbolo);
        }
        public void EjecutarSelecciona(Simbolo selecciona)
        {
            FSelecciona fselecciona = (FSelecciona)selecciona.Objeto;
            NodoExpresion condicion = fselecciona.Expresion.ResolverExpresion(fselecciona.Incerteza, fselecciona.RutaArchivo);
            if (TitusNotifiaciones.ContarErrores() == 0)
            {
                int cont = 0;
                int posencontrado = -1;
                Boolean encontrado = false;
                while (cont < fselecciona.Casos.Count && !TablaVariables.IsDetener() && !TablaVariables.IsRetorno() && TitusNotifiaciones.ContarErrores() == 0 && !TablaVariables.IsRetorno())
                {
                    if (fselecciona.Casos[cont].Tipo == condicion.Tipo)
                    {
                        if (!encontrado)
                        {
                            if (fselecciona.Casos[cont].Tipo == Constante.TNumber)
                            {
                                if (fselecciona.Casos[cont].ValNumero == condicion.Numero)
                                {
                                    EjecutarCaso(fselecciona.Casos[cont]);
                                    encontrado = true;
                                    posencontrado = cont;
                                    SacarAmbito(fselecciona.Casos[cont].ambito.TablaSimbolo);
                                }
                            }
                            else
                            {
                                if (fselecciona.Casos[cont].ValCadena == condicion.Cadena)
                                {
                                    EjecutarCaso(fselecciona.Casos[cont]);

                                    SacarAmbito(fselecciona.Casos[cont].ambito.TablaSimbolo);
                                }
                            }
                        }
                        
                        //seguimos ejcutando si no hay detener en el algun caso
                        if (encontrado && cont > posencontrado)
                        {
                            EjecutarCaso(fselecciona.Casos[cont]);

                            SacarAmbito(fselecciona.Casos[cont].ambito.TablaSimbolo);
                        }
                    }                   

                    cont++;
                }
                if (!encontrado && !TablaVariables.IsDetener() && !TablaVariables.IsRetorno() && TitusNotifiaciones.ContarErrores() == 0 && !TablaVariables.IsRetorno())
                {
                    if (fselecciona.Defecto != null)
                    {
                        EjecutarCaso(fselecciona.Defecto);
                    }
                }
                //si encontro un detener lo sacamos para que siga metiendo mas ambitos
                if (TablaVariables.IsDetener())
                {
                    TablaVariables.SacarVariable();
                }
            }
        }
        public void EjecutarSi(Simbolo si)
        {
            FSi fsi = (FSi)si.Objeto;
            NodoExpresion condicion = fsi.Condicion.ResolverExpresion(fsi.Incerteza, fsi.RutaArchivo);
            if (TitusNotifiaciones.ContarErrores() == 0 && condicion.Tipo == Constante.TBool)
            {
                //agregar error
                if (TitusNotifiaciones.ContarErrores() == 0 && condicion.Booleano)
                {
                    EjecutarInstrucciones(si.Ambito.TablaSimbolo);
                    SacarAmbito(si.Ambito.TablaSimbolo);
                }else
                {
                    if (fsi.Sino != null)
                    {
                        EjecutarInstrucciones(fsi.Sino.TablaSimbolo);
                        SacarAmbito(fsi.Sino.TablaSimbolo);
                    }
                }
            }
            else
            {
                if (TitusNotifiaciones.ContarErrores() == 0)
                {
                    TabError error = new TabError();
                    error.InsertarFila(Constante.ErroEjecucion, "Se esperaba un valor Bool no uno de tipo: " + condicion.Tipo, fsi.RutaArchivo, si.Fila.ToString(), si.Columna.ToString());
                    TitusNotifiaciones.setDatosErrores(error);
                }
            }
        }        
        public void EjecutarAsignacion(Simbolo global)
        {
            FExpresion expresion = (FExpresion)global.Objeto;
            if (expresion != null)
            {
                NodoExpresion valor = expresion.ResolverExpresion(expresion.Incerteza, expresion.RutaArchivo);
                Variables aux = TablaVariables.BuscarVariable(global.Nombre);
                if (TitusNotifiaciones.ContarErrores() == 0)
                {
                    if (aux != null)
                    {
                        if (aux.Tipo == Constante.TString)
                        {
                            if (valor.Tipo == Constante.TString || valor.Tipo == Constante.TNumber || valor.Tipo == Constante.TBool)
                            {
                                valor.Tipo = valor.Tipo;
                                aux.Valor = new NodoExpresion(valor);                                
                            }else
                            {
                                TabError error = new TabError();
                                error.InsertarFila(Constante.ErroEjecucion, "No se puede asignar una variable de tipo " + global.Tipo + " un valor " + valor.Tipo,global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                                TitusNotifiaciones.setDatosErrores(error);
                            }
                        }else if (aux.Tipo == Constante.TNumber)
                        {
                            if (valor.Tipo == Constante.TNumber || valor.Tipo == Constante.TBool)
                            {
                                valor.Tipo = valor.Tipo;
                                aux.Valor = new NodoExpresion(valor);
                            }else
                            {
                                TabError error = new TabError();
                                error.InsertarFila(Constante.ErroEjecucion, "No se puede asignar una variable de tipo " + global.Tipo + " un valor " + valor.Tipo, global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                                TitusNotifiaciones.setDatosErrores(error);
                            }
                        }else if (aux.Tipo == Constante.TBool)
                        {
                            if (valor.Tipo == Constante.TBool)
                            {
                                valor.Tipo = valor.Tipo;
                                aux.Valor = new NodoExpresion(valor);
                            }
                            else
                            {
                                TabError error = new TabError();
                                error.InsertarFila(Constante.ErroEjecucion, "No se puede asignar una variable de tipo " + global.Tipo + " un valor " + valor.Tipo, global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                                TitusNotifiaciones.setDatosErrores(error);
                            }
                        }
                    }
                    else
                    {
                        TabError error = new TabError();
                        error.InsertarFila(Constante.ErroEjecucion, "Variable no declarada: " + global.Nombre, global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                        TitusNotifiaciones.setDatosErrores(error);
                    }                    
                }
            }
        }
        public void EjecutarDeclaracion(Simbolo global)
        {
            FExpresion expresion = (FExpresion)global.Objeto;
            if (expresion != null)
            {
                NodoExpresion valor = expresion.ResolverExpresion(expresion.Incerteza, expresion.RutaArchivo);
                if (TitusNotifiaciones.ContarErrores() == 0)
                {
                    if (global.Tipo == Constante.TString)
                    {
                        if (valor.Tipo == Constante.TString || valor.Tipo == Constante.TNumber || valor.Tipo == Constante.TBool)
                        {
                            valor.Tipo = valor.Tipo;
                            TablaVariables.Tabla.Add(new Variables(global.Tipo, global.Nombre, Constante.TVariable, valor, null, global.Fila, global.Columna));
                        }
                        else
                        {
                            TabError error = new TabError();
                            error.InsertarFila(Constante.ErroEjecucion, "No se puede asignar una variable de tipo " + global.Tipo + " un valor " + valor.Tipo, global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                            TitusNotifiaciones.setDatosErrores(error);
                        }
                    }
                    else if (global.Tipo == Constante.TNumber)
                    {
                        if (valor.Tipo == Constante.TNumber || valor.Tipo == Constante.TBool)
                        {
                            valor.Tipo = valor.Tipo;
                            TablaVariables.Tabla.Add(new Variables(global.Tipo, global.Nombre, Constante.TVariable, valor, null, global.Fila, global.Columna));
                        }
                        else
                        {
                            TabError error = new TabError();
                            error.InsertarFila(Constante.ErroEjecucion, "No se puede asignar una variable de tipo " + global.Tipo + " un valor " + valor.Tipo, global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                            TitusNotifiaciones.setDatosErrores(error);
                        }
                    }
                    else if (global.Tipo == Constante.TBool)
                    {
                        if (valor.Tipo == Constante.TBool)
                        {
                            valor.Tipo = valor.Tipo;
                            TablaVariables.Tabla.Add(new Variables(global.Tipo, global.Nombre, Constante.TVariable, valor, null, global.Fila, global.Columna));
                        }
                        else
                        {
                            TabError error = new TabError();
                            error.InsertarFila(Constante.ErroEjecucion, "No se puede asignar una variable de tipo " + global.Tipo + " un valor " + valor.Tipo, global.RutaArchivo, global.Fila.ToString(), global.Columna.ToString());
                            TitusNotifiaciones.setDatosErrores(error);
                        }
                    }
                }
            }
            else
            {
                TablaVariables.Tabla.Add(new Variables(global.Tipo, global.Nombre, Constante.TVariable, null, null, global.Fila, global.Columna));
            }
        }        
        public void EjecutarDetener(Simbolo detener)
        {
            TablaVariables.Tabla.Add(new Variables(detener.Tipo, detener.Nombre, detener.Rol, detener.Objeto, detener.Ambito, detener.Fila, detener.Columna));
        }           
        public void EjecutarContinuar(Simbolo continuar)
        {
            TablaVariables.Tabla.Add(new Variables(continuar.Tipo, continuar.Nombre, continuar.Rol, continuar.Objeto, continuar.Ambito, continuar.Fila, continuar.Columna));
        }     
    }
}
