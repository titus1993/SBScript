using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi2_Practica_201213587.Funciones
{
    class FMostrar
    {
        public String RutaArchivo { get; set; }
        public List<FExpresion> Parametros { get; set; }
        public String Cadena { get; set; }
        public String RutaImagen { get; set; }
        public Double Incerteza { get; set; }
        public FMostrar()
        {
            Parametros = new List<FExpresion>();
        }


        public void Mostrar(int Fila, int Columna, String ruta)
        {
            String cadena_nueva = "";
            String numero="";
            bool posicion = false;
            for (int i=0; i<Cadena.Length && TitusNotifiaciones.ContarErrores() == 0; i++)
            {
                char letra = Cadena[i];
                if (posicion)
                {
                    if (letra == '}')
                    {
                        posicion = false;
                        try
                        {
                            int pos = Int32.Parse(numero);
                            if (pos <= Parametros.Count - 1)
                            {
                                NodoExpresion val = Parametros[pos].ResolverExpresion(Incerteza, ruta);
                                if (TitusNotifiaciones.ContarErrores() == 0)
                                {
                                    if (val.Tipo == Constante.TNumber)
                                    {
                                        cadena_nueva = cadena_nueva + val.Numero.ToString();
                                    }
                                    else if (val.Tipo == Constante.TString)
                                    {
                                        cadena_nueva = cadena_nueva + val.Cadena;
                                    }
                                    else
                                    {
                                        cadena_nueva = cadena_nueva + val.Cadena;
                                    }
                                }
                            }
                            else
                            {
                                TabError Error = new TabError();
                                Error.InsertarFila(Constante.ErroEjecucion, "Acceso fuera de rango", "pendient", Fila.ToString(), Columna.ToString());
                                TitusNotifiaciones.setDatosErrores(Error);
                            }
                        }
                        catch
                        {
                            TabError Error = new TabError();
                            Error.InsertarFila(Constante.ErroEjecucion, "El valor debe ser numerico mayor o igual a 0", "pendient", Fila.ToString(), Columna.ToString());
                            TitusNotifiaciones.setDatosErrores(Error);
                        }
                    }
                    else
                    {
                        numero = numero + letra;
                    }
                }
                else
                {
                    if (letra == '{')
                    {
                        posicion = true;
                        numero = "";
                    }
                    else
                    {
                        cadena_nueva = cadena_nueva + letra;
                    }
                }
            }
            if (TitusNotifiaciones.ContarErrores() == 0)
            {

                TitusNotifiaciones.ImprimirConsola(cadena_nueva);
            }
        }
    }
}
