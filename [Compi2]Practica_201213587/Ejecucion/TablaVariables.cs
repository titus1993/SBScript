using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Compi2_Practica_201213587.Funciones;

namespace _Compi2_Practica_201213587.Ejecucion
{
    static class TablaVariables
    {
        public static List<Variables> Tabla { get; set; }
        public static List<String> Archivos { get; set; }

        public static void IncializarTabla()
        {
            Tabla = new List<Variables>();
            Archivos = new List<string>();
        }
        public static void SacarVariable()
        {
            Tabla.RemoveAt(Tabla.Count - 1);
        }

        public static Boolean ExisteArchivo(String ruta)
        {
            Boolean resultado = false;

            foreach (String r in Archivos)
            {
                if (r.Equals(ruta))
                {
                    return true;
                }
            }
            return resultado;
        }

        public static Boolean BuscarNombre(String nombre)
        {
            int cont = TablaVariables.Tabla.Count - 1;
            bool encontrado = false;
            while (cont >= 0 && TitusNotifiaciones.ContarErrores() == 0 && !encontrado)
            {
                if (TablaVariables.Tabla[cont].Nombre == nombre)
                {
                    encontrado = !encontrado;
                }
                cont--;
            }
            return encontrado;
        }

        public static Variables BuscarVariable(String nombre)
        {
            int cont = TablaVariables.Tabla.Count - 1;
            bool encontrado = false;
            Variables variable = null;
            while (cont >= 0 && TitusNotifiaciones.ContarErrores() == 0 && !encontrado)
            {
                if (TablaVariables.Tabla[cont].Nombre == nombre && Tabla[cont].Rol == Constante.TVariable)
                {
                    encontrado = !encontrado;
                    variable = TablaVariables.Tabla[cont];
                }
                cont--;
            }
            return variable;
        }

        public static Variables BuscarMetodo(String nombre, FLlamada lista)
        {
            int cont = TablaVariables.Tabla.Count - 1;
            bool encontrado = false;
            Variables variable = null;
            while (cont >= 0 && TitusNotifiaciones.ContarErrores() == 0 && !encontrado)
            {
                if (TablaVariables.Tabla[cont].Nombre == nombre && TablaVariables.Tabla[cont].Rol == Constante.TMetodo)
                {
                    FFuncion funcion = (FFuncion)Tabla[cont].Valor;

                    if (funcion.Parametros.Count == lista.Parametros.Count)
                    {
                        int i = 0;
                        Boolean estado = true;
                        while (i < funcion.Parametros.Count && estado)
                        {
                            NodoExpresion exp = (NodoExpresion)lista.Parametros[i].ResolverExpresion(Constante.DefaultDefineNumber,funcion.RutaArchivo);
                            if (!(exp.Tipo == funcion.Parametros[i].Tipo))
                            {
                                estado = false;
                            }
                            i++;
                        }
                        if (estado == true)
                        {
                            encontrado = !encontrado;
                            variable = TablaVariables.Tabla[cont];
                        }
                    }                    
                }
                cont--;
            }
            return variable;
        }

        public static List<Variables> BuscarMetodo(String nombre)
        {
            int cont = TablaVariables.Tabla.Count - 1;
            List<Variables> variable = new List<Variables>();
            while (cont >= 0 && TitusNotifiaciones.ContarErrores() == 0)
            {
                if (TablaVariables.Tabla[cont].Nombre == nombre && TablaVariables.Tabla[cont].Rol == Constante.TMetodo)
                {
                    variable.Add(TablaVariables.Tabla[cont]);
                }
                cont--;
            }
            return variable;
        }

        public static Boolean BuscarMetodo(String nombre, FFuncion lista)
        {
            int cont = TablaVariables.Tabla.Count - 1;
            bool encontrado = false;
            while (cont >= 0 && TitusNotifiaciones.ContarErrores() == 0 && !encontrado)
            {
                if (TablaVariables.Tabla[cont].Nombre == nombre && TablaVariables.Tabla[cont].Rol == Constante.TMetodo)
                {
                    FFuncion funcion = (FFuncion)Tabla[cont].Valor;

                    if (funcion.Parametros.Count == lista.Parametros.Count)
                    {
                        int i = 0;
                        Boolean estado = true;
                        while (i < funcion.Parametros.Count && estado)
                        {
                            
                            if (!(lista.Parametros[i].Tipo == funcion.Parametros[i].Tipo))
                            {
                                estado = false;
                            }
                            i++;
                        }
                        if (estado == true)
                        {
                            encontrado = !encontrado;
                            
                        }
                    }
                }
                cont--;
            }
            return encontrado;
        }

        public static void SacarVariable(String nombre)
        {
            int cont = TablaVariables.Tabla.Count - 1;
            bool encontrado = false;
            while (cont >= 0 && TitusNotifiaciones.ContarErrores() == 0 && !encontrado)
            {
                if (TablaVariables.Tabla[cont].Nombre == nombre)
                {
                    encontrado = !encontrado;
                    TablaVariables.Tabla.RemoveAt(cont);
                }
                cont--;
            }
        }

        public static bool IsDetener()
        {
            return Tabla[Tabla.Count - 1].Rol == Constante.TDetener;
        }

        public static bool IsContinuar()
        {
            return Tabla[Tabla.Count - 1].Rol == Constante.TContinuar;
        }

        public static bool ExisteVariableTope(String nombre)
        {
            return Tabla[Tabla.Count - 1].Nombre == nombre;
        }

        public static Variables ObtenerTope()
        {
            return Tabla[Tabla.Count - 1];
        }

        public static bool IsRetorno()
        {
            return Tabla[Tabla.Count - 1].Rol == Constante.TRetorno;
        }
    }
}
