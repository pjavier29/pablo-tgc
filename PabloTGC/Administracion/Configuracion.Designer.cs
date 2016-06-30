namespace AlumnoEjemplos.PabloTGC.Administracion
{
    partial class Configuracion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.botonAceptar = new System.Windows.Forms.Button();
            this.process1 = new System.Diagnostics.Process();
            this.numericUpDownHoraInicio = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarVelocidadTiempo = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarPresipitaciones = new System.Windows.Forms.TrackBar();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textoVelocidadTiempo = new System.Windows.Forms.Label();
            this.textoPresipitaciones = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textoFuerza = new System.Windows.Forms.Label();
            this.numericUpDownVelocidadRotacion = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownVelocidadCaminar = new System.Windows.Forms.NumericUpDown();
            this.trackBarFuerza = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelVelocidadCaminar = new System.Windows.Forms.Label();
            this.progressBarCreacion = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxDificultad = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHoraInicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVelocidadTiempo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPresipitaciones)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVelocidadRotacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVelocidadCaminar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFuerza)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Velocidad del Tiempo:";
            // 
            // botonAceptar
            // 
            this.botonAceptar.Location = new System.Drawing.Point(173, 290);
            this.botonAceptar.Name = "botonAceptar";
            this.botonAceptar.Size = new System.Drawing.Size(75, 23);
            this.botonAceptar.TabIndex = 1;
            this.botonAceptar.Text = "Aceptar";
            this.botonAceptar.UseVisualStyleBackColor = true;
            this.botonAceptar.Click += new System.EventHandler(this.botonAceptar_Click);
            // 
            // process1
            // 
            this.process1.StartInfo.Domain = "";
            this.process1.StartInfo.LoadUserProfile = false;
            this.process1.StartInfo.Password = null;
            this.process1.StartInfo.StandardErrorEncoding = null;
            this.process1.StartInfo.StandardOutputEncoding = null;
            this.process1.StartInfo.UserName = "";
            this.process1.SynchronizingObject = this;
            // 
            // numericUpDownHoraInicio
            // 
            this.numericUpDownHoraInicio.Location = new System.Drawing.Point(188, 118);
            this.numericUpDownHoraInicio.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericUpDownHoraInicio.Name = "numericUpDownHoraInicio";
            this.numericUpDownHoraInicio.Size = new System.Drawing.Size(149, 20);
            this.numericUpDownHoraInicio.TabIndex = 2;
            this.numericUpDownHoraInicio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownHoraInicio.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Hora de Inicio:";
            // 
            // trackBarVelocidadTiempo
            // 
            this.trackBarVelocidadTiempo.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarVelocidadTiempo.Location = new System.Drawing.Point(188, 6);
            this.trackBarVelocidadTiempo.Maximum = 15;
            this.trackBarVelocidadTiempo.Minimum = 1;
            this.trackBarVelocidadTiempo.Name = "trackBarVelocidadTiempo";
            this.trackBarVelocidadTiempo.Size = new System.Drawing.Size(149, 45);
            this.trackBarVelocidadTiempo.TabIndex = 4;
            this.trackBarVelocidadTiempo.Value = 1;
            this.trackBarVelocidadTiempo.Scroll += new System.EventHandler(this.trackBarVelocidadTiempo_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Lapso entre Precipitaciones:";
            // 
            // trackBarPresipitaciones
            // 
            this.trackBarPresipitaciones.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarPresipitaciones.Location = new System.Drawing.Point(188, 57);
            this.trackBarPresipitaciones.Minimum = 1;
            this.trackBarPresipitaciones.Name = "trackBarPresipitaciones";
            this.trackBarPresipitaciones.Size = new System.Drawing.Size(149, 45);
            this.trackBarPresipitaciones.TabIndex = 6;
            this.trackBarPresipitaciones.Value = 1;
            this.trackBarPresipitaciones.Scroll += new System.EventHandler(this.trackBarPresipitaciones_Scroll);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(400, 214);
            this.tabControl.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.comboBoxDificultad);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.textoVelocidadTiempo);
            this.tabPage1.Controls.Add(this.textoPresipitaciones);
            this.tabPage1.Controls.Add(this.trackBarVelocidadTiempo);
            this.tabPage1.Controls.Add(this.trackBarPresipitaciones);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.numericUpDownHoraInicio);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(392, 188);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textoVelocidadTiempo
            // 
            this.textoVelocidadTiempo.AutoSize = true;
            this.textoVelocidadTiempo.Location = new System.Drawing.Point(343, 18);
            this.textoVelocidadTiempo.Name = "textoVelocidadTiempo";
            this.textoVelocidadTiempo.Size = new System.Drawing.Size(35, 13);
            this.textoVelocidadTiempo.TabIndex = 8;
            this.textoVelocidadTiempo.Text = "label6";
            // 
            // textoPresipitaciones
            // 
            this.textoPresipitaciones.AutoSize = true;
            this.textoPresipitaciones.Location = new System.Drawing.Point(343, 65);
            this.textoPresipitaciones.Name = "textoPresipitaciones";
            this.textoPresipitaciones.Size = new System.Drawing.Size(35, 13);
            this.textoPresipitaciones.TabIndex = 7;
            this.textoPresipitaciones.Text = "label6";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textoFuerza);
            this.tabPage2.Controls.Add(this.numericUpDownVelocidadRotacion);
            this.tabPage2.Controls.Add(this.numericUpDownVelocidadCaminar);
            this.tabPage2.Controls.Add(this.trackBarFuerza);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.labelVelocidadCaminar);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(392, 188);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Personaje";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textoFuerza
            // 
            this.textoFuerza.AutoSize = true;
            this.textoFuerza.Location = new System.Drawing.Point(343, 18);
            this.textoFuerza.Name = "textoFuerza";
            this.textoFuerza.Size = new System.Drawing.Size(35, 13);
            this.textoFuerza.TabIndex = 6;
            this.textoFuerza.Text = "label6";
            // 
            // numericUpDownVelocidadRotacion
            // 
            this.numericUpDownVelocidadRotacion.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownVelocidadRotacion.Location = new System.Drawing.Point(188, 118);
            this.numericUpDownVelocidadRotacion.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownVelocidadRotacion.Name = "numericUpDownVelocidadRotacion";
            this.numericUpDownVelocidadRotacion.Size = new System.Drawing.Size(149, 20);
            this.numericUpDownVelocidadRotacion.TabIndex = 5;
            this.numericUpDownVelocidadRotacion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownVelocidadRotacion.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numericUpDownVelocidadRotacion.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // numericUpDownVelocidadCaminar
            // 
            this.numericUpDownVelocidadCaminar.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownVelocidadCaminar.Location = new System.Drawing.Point(188, 63);
            this.numericUpDownVelocidadCaminar.Maximum = new decimal(new int[] {
            450,
            0,
            0,
            0});
            this.numericUpDownVelocidadCaminar.Minimum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numericUpDownVelocidadCaminar.Name = "numericUpDownVelocidadCaminar";
            this.numericUpDownVelocidadCaminar.Size = new System.Drawing.Size(149, 20);
            this.numericUpDownVelocidadCaminar.TabIndex = 4;
            this.numericUpDownVelocidadCaminar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownVelocidadCaminar.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numericUpDownVelocidadCaminar.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // trackBarFuerza
            // 
            this.trackBarFuerza.Location = new System.Drawing.Point(188, 6);
            this.trackBarFuerza.Maximum = 5;
            this.trackBarFuerza.Minimum = 1;
            this.trackBarFuerza.Name = "trackBarFuerza";
            this.trackBarFuerza.Size = new System.Drawing.Size(149, 45);
            this.trackBarFuerza.TabIndex = 3;
            this.trackBarFuerza.Value = 1;
            this.trackBarFuerza.Scroll += new System.EventHandler(this.trackBarFuerza_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Fuerza:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Velocidad al Rotar:";
            // 
            // labelVelocidadCaminar
            // 
            this.labelVelocidadCaminar.AutoSize = true;
            this.labelVelocidadCaminar.Location = new System.Drawing.Point(6, 65);
            this.labelVelocidadCaminar.Name = "labelVelocidadCaminar";
            this.labelVelocidadCaminar.Size = new System.Drawing.Size(109, 13);
            this.labelVelocidadCaminar.TabIndex = 0;
            this.labelVelocidadCaminar.Text = "Velocidad al Caminar:";
            // 
            // progressBarCreacion
            // 
            this.progressBarCreacion.Location = new System.Drawing.Point(16, 249);
            this.progressBarCreacion.Name = "progressBarCreacion";
            this.progressBarCreacion.Size = new System.Drawing.Size(392, 23);
            this.progressBarCreacion.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Nivel de Dificultad:";
            // 
            // comboBoxDificultad
            // 
            this.comboBoxDificultad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDificultad.FormattingEnabled = true;
            this.comboBoxDificultad.Location = new System.Drawing.Point(188, 160);
            this.comboBoxDificultad.Name = "comboBoxDificultad";
            this.comboBoxDificultad.Size = new System.Drawing.Size(149, 21);
            this.comboBoxDificultad.TabIndex = 10;
            this.comboBoxDificultad.SelectedIndexChanged += new System.EventHandler(this.comboBoxDificultad_SelectedIndexChanged);
            // 
            // Configuracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 325);
            this.Controls.Add(this.progressBarCreacion);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.botonAceptar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(440, 364);
            this.MinimumSize = new System.Drawing.Size(440, 364);
            this.Name = "Configuracion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuracion Suvirval Craft";
            this.Load += new System.EventHandler(this.Configuracion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHoraInicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVelocidadTiempo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPresipitaciones)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVelocidadRotacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVelocidadCaminar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFuerza)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button botonAceptar;
        private System.Diagnostics.Process process1;
        private System.Windows.Forms.NumericUpDown numericUpDownHoraInicio;
        private System.Windows.Forms.TrackBar trackBarVelocidadTiempo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBarPresipitaciones;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label labelVelocidadCaminar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownVelocidadCaminar;
        private System.Windows.Forms.TrackBar trackBarFuerza;
        private System.Windows.Forms.NumericUpDown numericUpDownVelocidadRotacion;
        private System.Windows.Forms.Label textoPresipitaciones;
        private System.Windows.Forms.Label textoVelocidadTiempo;
        private System.Windows.Forms.Label textoFuerza;
        private System.Windows.Forms.ProgressBar progressBarCreacion;
        private System.Windows.Forms.ComboBox comboBoxDificultad;
        private System.Windows.Forms.Label label6;
    }
}