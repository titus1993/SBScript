using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Diagnostics;

namespace _Compi2_Practica_201213587
{
    public partial class Form1 : Form
    {
        private TitusTabControl TTControl;

        public Form1()
        {
            InitializeComponent();
            TTControl = new TitusTabControl();
            splitContainer1.Panel1.Controls.Add(TTControl);
            TTControl.Dock = DockStyle.Fill;
            //this.Controls.Add(TTControl);
            this.StartPosition = FormStartPosition.CenterScreen;

            //agreagmos la consola al tab de consola
            TitusNotifiaciones.IniciarConsola();
            splitContainer1.Panel2.Controls.Add(TabControlNotificaciones);
            TabConsola.Controls.Add(TitusNotifiaciones.Consola);

            //agreagmos el visor de errores
            TitusNotifiaciones.IniciarErrores();
            TabErrores.Controls.Add(TitusNotifiaciones.getErrores());
            TTControl.agregarNewTab();

            //agreagmos el listview 
            TitusNotifiaciones.IniciarImagenes(listView1);
            TabImagenes.Controls.Add(TitusNotifiaciones.Imagenes);

            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TTControl.agregarNewTab();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TTControl.abrirTab();
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TTControl.guardarTab();
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TTControl.guardarComoTab();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ejecutarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TitusTab aux = (TitusTab)TTControl.SelectedPage;
            if (aux != null)
            {
                aux.Analizar();
            }
        }

        private void reporteDeErroresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabControlNotificaciones.SelectedIndex = 1;
        }

        private void abrirAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabControlNotificaciones.SelectedIndex = 3;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {            
            try
            {
                int i = listView1.FocusedItem.Index;
                ProcessStartInfo startInfo = new ProcessStartInfo(TitusNotifiaciones.RutaImagenes[i]);
                Process.Start(startInfo);
            }
            catch
            {

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
