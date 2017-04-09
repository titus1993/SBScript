using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Compi2_Practica_201213587
{
    static class TitusNotifiaciones
    {
        public static RichTextBox Consola = new RichTextBox();
        public static DataGridView Errores = new DataGridView();
        public static ListView Imagenes = new ListView();
        public static ImageList ListaImagenes = new ImageList();
        public static List<String> RutaImagenes = new List<String>();
        
        public static void IniciarConsola()
        {
            Consola.Dock = DockStyle.Fill;
            //Consola = new RichTextBox();
            Consola.Multiline = true;
            Consola.ScrollBars = RichTextBoxScrollBars.Both;
            Consola.WordWrap = false;
            Consola.Dock = DockStyle.Fill;
            Consola.ReadOnly = true;
            Consola.BackColor = System.Drawing.Color.LightGray;
            Consola.ForeColor = System.Drawing.Color.Black;
        }
        
        public static void ImprimirConsola(String mensaje)
        {
            Consola.Text = Consola.Text + "> " + mensaje + "\n";
            Consola.Select(Consola.Text.Length, 0);
            Consola.ScrollToCaret();
        }

        public static void LimpiarConsola()
        {
            Consola.Text = "";
        }

        public static void IniciarImagenes(ListView ima)
        {
            
            Imagenes = ima;
            Imagenes.View = View.LargeIcon;
            Imagenes.LargeImageList = ListaImagenes;
            Imagenes.Dock = DockStyle.Fill;
            ListaImagenes = new ImageList();
            ListaImagenes.ImageSize = new Size(128, 128);
            RutaImagenes = new List<string>();
        }

        public static void MeterImagen(String nombre, String ruta) {
            if (File.Exists(ruta))
            {
                ListaImagenes.Images.Add(nombre, Image.FromFile(ruta));
                RutaImagenes.Add(ruta);
            }
        }

        public static void MostrarImagen()
        {
            Imagenes.Clear();
            Imagenes.LargeImageList = ListaImagenes;

            for (int j = 0; j < ListaImagenes.Images.Count; j++)
            {
                ListViewItem item = new ListViewItem();
                item.ImageIndex = j;
                item.Text = ListaImagenes.Images.Keys[j];
                Imagenes.Items.Add(item);
            }
        }

        public static void IniciarErrores()
        {
            Errores.Columns.Add("No.", "No.");
            Errores.Columns.Add("Tipo", "Tipo");
            Errores.Columns.Add("Descripcion", "Descripcion");
            Errores.Columns.Add("Archivo", "Archivo");
            Errores.Columns.Add("Linea", "Linea");
            Errores.Columns.Add("Columna", "Columna");
            Errores.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Errores.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Errores.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Errores.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Errores.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Errores.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Errores.Dock = DockStyle.Fill;
            Errores.ReadOnly = true;
            Errores.ScrollBars = ScrollBars.Both;
            Errores.AutoGenerateColumns = true;
            Errores.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            foreach (DataGridViewColumn col in Errores.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            }        
             
        }

        public static DataGridView getErrores()
        {
            return Errores;
        }

        public static void setDatosErrores(DataTable tabla)
        {
            foreach (DataRow row in tabla.Rows)
            {
                Errores.Rows.Insert(Errores.Rows.Count-1, Errores.Rows.Count, row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4], row.ItemArray[5]);   
            }
        }

        public static void LimpiarDatosErrores()
        {
            Errores.Rows.Clear();
        }

        public static void LimpiarImagenes()
        {
            Imagenes.View = View.LargeIcon;
            Imagenes.LargeImageList = ListaImagenes;
            Imagenes.Dock = DockStyle.Fill;
            ListaImagenes = new ImageList();
            ListaImagenes.ImageSize = new Size(128, 128);
            RutaImagenes = new List<string>();
            ListaImagenes = new ImageList();
            ListaImagenes.ImageSize = new Size(128, 128);
            Imagenes.Clear();
        }

        public static void Limpiar()
        {
            LimpiarConsola();
            LimpiarDatosErrores();
            LimpiarImagenes();
        }

        public static int ContarErrores()
        {
            return Errores.Rows.Count - 1;
        }
        
    }
}
