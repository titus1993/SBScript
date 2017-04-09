using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Compi2_Practica_201213587
{
    class TitusTabControl : LidorSystems.IntegralUI.Containers.TabControl
    {
        int contador = 1;
        public TitusTabControl()
        {
            initComponents();
        }


        private void initComponents()
        {              
            ImageList imgButtons = new System.Windows.Forms.ImageList();
            
            // imgButtons
            imgButtons = new ImageList();
            imgButtons.TransparentColor = System.Drawing.Color.Transparent;
            imgButtons.Images.Add("Cerrar", Image.FromFile(@"cerrar.ico"));

            //configuraciones inciales del tab
            this.Name = "TabControl";
            this.Size = new System.Drawing.Size(960, 480);
            this.Location = new System.Drawing.Point(12, 30);
            this.ButtonImageList = imgButtons;
            this.TabButtonClicked += TitusTabControl_TabButtonClicked;
        }

        //evento para cerrar las pestañas
        private void TitusTabControl_TabButtonClicked(object sender, LidorSystems.IntegralUI.ObjectClickEventArgs e)
        {
            if (e.Object is LidorSystems.IntegralUI.Controls.CommandButton)
            {
                LidorSystems.IntegralUI.Controls.CommandButton btn = (LidorSystems.IntegralUI.Controls.CommandButton)e.Object;

                // Check whether the type of a button is a close button
                if (btn.Key == "TAB_CLOSE")
                {
                    // Locate the tab in which the command button was clicked
                    LidorSystems.IntegralUI.Containers.TabPage page = this.GetPageAt(e.Position);

                    if (page != null)
                    {
                        // Depending on the action, you can determine whether you want to hide or dispose the tab
                        switch (this.CloseAction)
                        {
                            case LidorSystems.IntegralUI.CloseAction.Hide:
                                page.Hide();
                                break;

                            default:
                                TitusTab TTaux = (TitusTab)this.SelectedPage;
                                //preguntamos si ha sido modificado el contenido para preguntar si desea guardar
                                if (TTaux.esModificado())
                                {
                                    switch (MessageBox.Show("Desea guardar el archivo", "Guardar archivo", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk))
                                    {
                                        case DialogResult.Yes:
                                            guardarTab();
                                            page.Remove();
                                            break;

                                        case DialogResult.No:
                                            page.Remove();
                                            break;

                                        default:
                                            break;
                                    }
                                }else
                                {
                                    page.Remove();
                                }
                                break;
                        }
                    }
                }
            }
        }


        public void agregarNewTab()
        {
            String nombre = "Nuevo Documento " + contador.ToString();
            TitusTab aux = new TitusTab(nombre,"", "");
            this.Pages.Add(aux);
            contador++;
        }

        public void abrirTab()
        {
            OpenFileDialog abrir = new OpenFileDialog();
            abrir.Filter = Constante.DialogFilter;
            abrir.Title = "Abrir";
            abrir.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            abrir.ShowDialog();

            if (abrir.FileName != "")
            {
                TitusTab aux = new TitusTab(abrir.SafeFileName, File.ReadAllText(abrir.FileName), abrir.FileName);
                this.Pages.Add(aux);
            }                      
        }
        
        public void guardarTab()
        {
            TitusTab TTaux = (TitusTab)this.SelectedPage;
            if (TTaux != null)
            {
                TTaux.guardarArchivo();
                this.Refresh();
                this.UpdateLayout();
            }            
        }

        public void guardarComoTab()
        {
            TitusTab TTaux = (TitusTab)this.SelectedPage;
            if (TTaux != null)
            {
                TTaux.guardarComoArchivo();
                this.Refresh();
                this.UpdateLayout();
            }
        }
    }
}
