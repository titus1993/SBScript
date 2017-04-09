namespace _Compi2_Practica_201213587
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarComoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ejecutarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ejecutarArchivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteDeErroresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TabControlNotificaciones = new System.Windows.Forms.TabControl();
            this.TabConsola = new System.Windows.Forms.TabPage();
            this.TabErrores = new System.Windows.Forms.TabPage();
            this.TabImagenes = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.menuStrip1.SuspendLayout();
            this.TabControlNotificaciones.SuspendLayout();
            this.TabImagenes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.ejecutarToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(985, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoToolStripMenuItem,
            this.abrirToolStripMenuItem,
            this.guardarToolStripMenuItem,
            this.guardarComoToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // nuevoToolStripMenuItem
            // 
            this.nuevoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("nuevoToolStripMenuItem.Image")));
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.nuevoToolStripMenuItem.Text = "Nuevo";
            this.nuevoToolStripMenuItem.Click += new System.EventHandler(this.nuevoToolStripMenuItem_Click);
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("abrirToolStripMenuItem.Image")));
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.abrirToolStripMenuItem.Text = "Abrir";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // guardarToolStripMenuItem
            // 
            this.guardarToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("guardarToolStripMenuItem.Image")));
            this.guardarToolStripMenuItem.Name = "guardarToolStripMenuItem";
            this.guardarToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.guardarToolStripMenuItem.Text = "Guardar";
            this.guardarToolStripMenuItem.Click += new System.EventHandler(this.guardarToolStripMenuItem_Click);
            // 
            // guardarComoToolStripMenuItem
            // 
            this.guardarComoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("guardarComoToolStripMenuItem.Image")));
            this.guardarComoToolStripMenuItem.Name = "guardarComoToolStripMenuItem";
            this.guardarComoToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.guardarComoToolStripMenuItem.Text = "Guardar como";
            this.guardarComoToolStripMenuItem.Click += new System.EventHandler(this.guardarComoToolStripMenuItem_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("salirToolStripMenuItem.Image")));
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // ejecutarToolStripMenuItem
            // 
            this.ejecutarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ejecutarArchivoToolStripMenuItem,
            this.reporteDeErroresToolStripMenuItem,
            this.abrirAlbumToolStripMenuItem});
            this.ejecutarToolStripMenuItem.Name = "ejecutarToolStripMenuItem";
            this.ejecutarToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.ejecutarToolStripMenuItem.Text = "Ejecutar";
            // 
            // ejecutarArchivoToolStripMenuItem
            // 
            this.ejecutarArchivoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ejecutarArchivoToolStripMenuItem.Image")));
            this.ejecutarArchivoToolStripMenuItem.Name = "ejecutarArchivoToolStripMenuItem";
            this.ejecutarArchivoToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.ejecutarArchivoToolStripMenuItem.Text = "Ejecutar Archivo";
            this.ejecutarArchivoToolStripMenuItem.Click += new System.EventHandler(this.ejecutarArchivoToolStripMenuItem_Click);
            // 
            // reporteDeErroresToolStripMenuItem
            // 
            this.reporteDeErroresToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("reporteDeErroresToolStripMenuItem.Image")));
            this.reporteDeErroresToolStripMenuItem.Name = "reporteDeErroresToolStripMenuItem";
            this.reporteDeErroresToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.reporteDeErroresToolStripMenuItem.Text = "Reporte de Errores";
            this.reporteDeErroresToolStripMenuItem.Click += new System.EventHandler(this.reporteDeErroresToolStripMenuItem_Click);
            // 
            // abrirAlbumToolStripMenuItem
            // 
            this.abrirAlbumToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("abrirAlbumToolStripMenuItem.Image")));
            this.abrirAlbumToolStripMenuItem.Name = "abrirAlbumToolStripMenuItem";
            this.abrirAlbumToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.abrirAlbumToolStripMenuItem.Text = "Abrir Album";
            this.abrirAlbumToolStripMenuItem.Click += new System.EventHandler(this.abrirAlbumToolStripMenuItem_Click);
            // 
            // TabControlNotificaciones
            // 
            this.TabControlNotificaciones.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.TabControlNotificaciones.Controls.Add(this.TabConsola);
            this.TabControlNotificaciones.Controls.Add(this.TabErrores);
            this.TabControlNotificaciones.Controls.Add(this.TabImagenes);
            this.TabControlNotificaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControlNotificaciones.ImageList = this.imageList1;
            this.TabControlNotificaciones.Location = new System.Drawing.Point(0, 0);
            this.TabControlNotificaciones.Multiline = true;
            this.TabControlNotificaciones.Name = "TabControlNotificaciones";
            this.TabControlNotificaciones.SelectedIndex = 0;
            this.TabControlNotificaciones.ShowToolTips = true;
            this.TabControlNotificaciones.Size = new System.Drawing.Size(985, 197);
            this.TabControlNotificaciones.TabIndex = 1;
            // 
            // TabConsola
            // 
            this.TabConsola.ImageIndex = 3;
            this.TabConsola.Location = new System.Drawing.Point(4, 4);
            this.TabConsola.Name = "TabConsola";
            this.TabConsola.Size = new System.Drawing.Size(977, 170);
            this.TabConsola.TabIndex = 1;
            this.TabConsola.Text = "Consola";
            this.TabConsola.UseVisualStyleBackColor = true;
            // 
            // TabErrores
            // 
            this.TabErrores.ImageIndex = 5;
            this.TabErrores.Location = new System.Drawing.Point(4, 4);
            this.TabErrores.Name = "TabErrores";
            this.TabErrores.Padding = new System.Windows.Forms.Padding(3);
            this.TabErrores.Size = new System.Drawing.Size(977, 170);
            this.TabErrores.TabIndex = 0;
            this.TabErrores.Text = "Errores";
            this.TabErrores.UseVisualStyleBackColor = true;
            // 
            // TabImagenes
            // 
            this.TabImagenes.Controls.Add(this.listView1);
            this.TabImagenes.ImageIndex = 1;
            this.TabImagenes.Location = new System.Drawing.Point(4, 4);
            this.TabImagenes.Name = "TabImagenes";
            this.TabImagenes.Size = new System.Drawing.Size(977, 170);
            this.TabImagenes.TabIndex = 2;
            this.TabImagenes.Text = "Imagenes";
            this.TabImagenes.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(977, 170);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Abrir.png");
            this.imageList1.Images.SetKeyName(1, "Album.png");
            this.imageList1.Images.SetKeyName(2, "cerrar.ico");
            this.imageList1.Images.SetKeyName(3, "Console.png");
            this.imageList1.Images.SetKeyName(4, "Ejecutar.png");
            this.imageList1.Images.SetKeyName(5, "Error.png");
            this.imageList1.Images.SetKeyName(6, "Guardar.png");
            this.imageList1.Images.SetKeyName(7, "guardarcomo.png");
            this.imageList1.Images.SetKeyName(8, "Logo2.ico");
            this.imageList1.Images.SetKeyName(9, "Nuevo.png");
            this.imageList1.Images.SetKeyName(10, "Salir.png");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.TabControlNotificaciones);
            this.splitContainer1.Size = new System.Drawing.Size(985, 737);
            this.splitContainer1.SplitterDistance = 536;
            this.splitContainer1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 761);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "SBScript";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.TabControlNotificaciones.ResumeLayout(false);
            this.TabImagenes.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarComoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.TabControl TabControlNotificaciones;
        private System.Windows.Forms.TabPage TabErrores;
        private System.Windows.Forms.ToolStripMenuItem ejecutarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ejecutarArchivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirAlbumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporteDeErroresToolStripMenuItem;
        private System.Windows.Forms.TabPage TabConsola;
        private System.Windows.Forms.TabPage TabImagenes;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView1;
    }
}

