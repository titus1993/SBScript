using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Compi2_Practica_201213587.Funciones
{
    class FDibujarExpresion
    {
        public FExpresion Expresion { get; set; }
        public String RutaImagen { get; set; }
        public Double Incerteza { get; set; }
        public String RutaArchivo { get; set; }

        public void DibujarExpresion()
        {
           String a = Expresion.GenerarArbolGraphics();
            try
            {
                String ruta = RutaImagen+"\\";
                String rutatxt = ruta + "EXP_" + (TitusNotifiaciones.ListaImagenes.Images.Count + 1).ToString() + ".txt";
                String rutaimg = ruta + "EXP_" + (TitusNotifiaciones.ListaImagenes.Images.Count + 1).ToString() + ".png";
                String nombreimg = "EXP_" + (TitusNotifiaciones.ListaImagenes.Images.Count + 1).ToString() + ".png";
                /*if (File.Exists(rutaimg))
                {
                    File.Delete(rutaimg);
                }*/
                File.WriteAllText(rutatxt, a);

                
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
}
