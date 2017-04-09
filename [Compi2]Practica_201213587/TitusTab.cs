using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;
using FastColoredTextBoxNS;
using _Compi2_Practica_201213587.Ejecucion;
namespace _Compi2_Practica_201213587
{
    class TitusTab : LidorSystems.IntegralUI.Containers.TabPage
    {
        IronyFCTB TBContenido;
        Label panel;
        bool modificado;
        string ruta;

        //Variables para usar Irony
        SBScriptGrammar Gramatica;
        LanguageData language;
        Parser parser;

        public TitusTab(String nombre, String texto, String ruta)
        {
            this.Text = nombre;
            //inicializamos la variable que tendra la ruta del archivo
            this.ruta = ruta;
            
            initComponent(texto);
        }


        public void Analizar()
        {
            if (guardarArchivo())
            {
                TablaVariables.IncializarTabla();
                TitusNotifiaciones.Limpiar();
                GenerarArbol a = new GenerarArbol();
                EjecutarSBS primerpasada = a.GenerarSimbolo(TBContenido.Text, ruta);//primera pasada solo para generar los simbolos
                //segunda pasada para buscar el principal y algunos errores
                Ejecucion.Ejecutar Iniciar = new Ejecucion.Ejecutar(primerpasada);
                
            }
                
        }

        private void initComponent(String texto)
        {
            //iniciamos la bandera para saber si ha sido guardado los datos
            this.modificado = false;

            //Inicializamos la gramatica y su lenguage para tener el parse
            Gramatica = new SBScriptGrammar();
            language = new LanguageData(Gramatica);
            parser = new Parser(language);

            //creamos el textbox
            TBContenido = new IronyFCTB();
            TBContenido.Grammar = Gramatica;
            
            
            TBContenido.Multiline = true;
            TBContenido.Text = texto;
            //TBContenido.ScrollBars = RichTextBoxScrollBars.Both;
 
            TBContenido.WordWrap = false;
            TBContenido.Dock = DockStyle.Fill;

            //configuramos el label
            panel = new Label();
            panel.Dock = DockStyle.Bottom;
            panel.Text = "Linea: 1, Columna: 1";
            panel.TextAlign = ContentAlignment.MiddleRight;
            

            //agregamos los eventos
            TBContenido.TextChanged += TBContenido_TextChanged;
            TBContenido.SelectionChanged += TBContenido_SelectionChanged;

            //agregamos los elementos
            this.Controls.Add(TBContenido);
            this.Controls.Add(panel);

            //creamos el boton cerrar
            LidorSystems.IntegralUI.Controls.CommandButton closeButton = new LidorSystems.IntegralUI.Controls.CommandButton();
            closeButton.ImageIndex = 0;
            closeButton.Key = "TAB_CLOSE";

            //agreagamos el boton al tab
            this.Buttons.Add(closeButton);
            this.UseParentButtons = false;
        }

        private void TBContenido_SelectionChanged(object sender, EventArgs e)
        {
            panel.Text = "Linea: " + (TBContenido.Selection.Start.iLine + 1).ToString() + ", Columna: " + (TBContenido.Selection.Start.iChar + 1).ToString();
        }

        private void TBContenido_TextChanged(object sender, EventArgs e)
        {
            modificado = true;
        }

        public bool esModificado()
        {
            return modificado;
        }

        public Boolean guardarArchivo()
        {
            Boolean estado = false;
            if (String.IsNullOrWhiteSpace(ruta))
            {
                SaveFileDialog guardar = new SaveFileDialog();
                
                guardar.Filter = Constante.DialogFilter;
                guardar.Title = "Guardar Archivo";
                guardar.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                if (guardar.ShowDialog() == DialogResult.OK)
                {
                    System.IO.FileStream fs = (System.IO.FileStream)guardar.OpenFile();
                    fs.Close();
                    System.IO.File.WriteAllText(guardar.FileName, this.TBContenido.Text);
                    ruta = guardar.FileName;
                    modificado = false;
                    this.Text = System.IO.Path.GetFileName(ruta);
                    estado = true;
                }
            }else
            {
                System.IO.File.WriteAllText(this.ruta, this.TBContenido.Text);
                modificado = false;
                this.Text = System.IO.Path.GetFileName(ruta);
                estado = true;
            }
            return estado;
        }

        public void guardarComoArchivo()
        {
            SaveFileDialog guardar = new SaveFileDialog();

            guardar.Filter = Constante.DialogFilter;
            guardar.Title = "Guardar Archivo";
            guardar.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            

            if (guardar.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream fs = (System.IO.FileStream)guardar.OpenFile();
                fs.Close();
                System.IO.File.WriteAllText(guardar.FileName, this.TBContenido.Text);
                ruta = guardar.FileName;

                modificado = false;
                this.Text = System.IO.Path.GetFileName(ruta);
            }            
        }
    }    
}
