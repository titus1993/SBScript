using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Compi2_Practica_201213587.Ejecucion;

namespace _Compi2_Practica_201213587
{
    class EjecutarSBS
    {
        public Double DefineNumber { get; set; }
        public String DefineRuta { get; set; }
        public List<Simbolo> Incluye { get; set; }
        public List<EjecutarSBS> Archivos { get; set; }
        public String Ruta { get; set; }
        public Ambito Global { get; set; }

        public TabError Errores { get; set; }

        public EjecutarSBS()
        {
            Archivos = new List<EjecutarSBS>();
            DefineNumber = Constante.DefaultDefineNumber;
            DefineRuta = Constante.DefaultDefineRuta;
            Incluye = new List<Simbolo>();
            Global = null;
            Errores = new TabError();
        }

        public void SetDefineNumber(Double numero)
        {
            DefineNumber = numero;
        }

        public void SetDefineRuta(String ruta)
        {
            DefineRuta = ruta;
        }

        public void AgregarIncluye(Simbolo archivo)
        {
            Incluye.Add(archivo);
        }


        public void BuscarIncluyes()
        {
            String path =  Path.GetFullPath(Ruta).Replace(Path.GetFileName(Ruta),"");
            foreach (Simbolo archivo in Incluye)
            {
                String rutanueva = path + archivo.Nombre;
                if (File.Exists(rutanueva))
                {
                    if (!TablaVariables.ExisteArchivo(rutanueva))//si todavia no a sido metido el archivo
                    {
                        TablaVariables.Archivos.Add(rutanueva);
                        GenerarArbol arbol = new GenerarArbol();
                        EjecutarSBS aux = arbol.GenerarSimbolo(File.ReadAllText(rutanueva), rutanueva);
                        if (aux != null)
                        {
                            Archivos.Add(aux);
                        }
                    }
                }
                else
                {
                    TabError error = new TabError();
                    error.InsertarFila(Constante.ErroEjecucion, "No existe el archivo: " + archivo.Nombre, Path.GetFileName(Ruta), archivo.Fila.ToString(), archivo.Columna.ToString());
                    TitusNotifiaciones.setDatosErrores(error);
                }
            }
        }

        public void Iniciar()
        {
            BuscarIncluyes();
        }
        
    }
}
