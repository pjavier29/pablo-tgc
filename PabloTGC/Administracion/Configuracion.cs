﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlumnoEjemplos.PabloTGC.Administracion
{
    public partial class Configuracion : Form
    {
        public ConfiguracionModel modelo;

        public Configuracion(ConfiguracionModel modelo)
        {
            InitializeComponent();
            this.modelo = modelo;
        }

        private void Configuracion_Load(object sender, EventArgs e)
        {
            textoPresipitaciones.Text = "" + trackBarPresipitaciones.Value;
            textoVelocidadTiempo.Text = "" + trackBarVelocidadTiempo.Value;
            textoFuerza.Text = "" + trackBarFuerza.Value;
            comboBoxDificultad.Items.Add("Fácil");
            comboBoxDificultad.Items.Add("Medio");
            comboBoxDificultad.Items.Add("Difícil");
            comboBoxDificultad.Items.Add("Muy Difícil");
            comboBoxDificultad.SelectedIndex = 0;
        }

        private void botonAceptar_Click(object sender, EventArgs e)
        {
            this.modelo.IniciarCreacion(this.ObtenerValorDificultad());
            progressBarCreacion.Increment(5);
            this.modelo.AdministracionDeEfectos();
            progressBarCreacion.Increment(5);
            this.modelo.CrearHeimap();
            progressBarCreacion.Increment(5);
            this.modelo.CrearIluminacion(this.ObtenerVelocidadTiempo(), this.ObtenerMomentoDeInicio(), 
                this.ObtenerLapsoPrecipitaciones());
            progressBarCreacion.Increment(5);
            this.modelo.CrearSkyBox();
            progressBarCreacion.Increment(5);
            this.modelo.CrearPalmerasComunes();
            progressBarCreacion.Increment(5);
            this.modelo.CrearArbolesBanana();
            progressBarCreacion.Increment(5);
            this.modelo.CrearArbolesDeLenia();
            progressBarCreacion.Increment(5);
            this.modelo.CrearPiedraParaTirar();
            progressBarCreacion.Increment(5);
            this.modelo.CrearOvejaYGallo();
            progressBarCreacion.Increment(5);
            this.modelo.CreamosLosCajones();
            progressBarCreacion.Increment(5);
            this.modelo.CrearArbolesGenerales();
            progressBarCreacion.Increment(5);
            this.modelo.CrearFuenteAgua();
            progressBarCreacion.Increment(5);
            this.modelo.CrearAlgas();
            progressBarCreacion.Increment(5);
            this.modelo.CrearPiedrasSobreAgua();
            progressBarCreacion.Increment(5);
            this.modelo.CrearCanoasSobreAgua();
            progressBarCreacion.Increment(5);
            this.modelo.CrearArbolFrutilla();
            progressBarCreacion.Increment(5);
            this.modelo.CrearPiso();
            progressBarCreacion.Increment(5);
            this.modelo.CrearPersonaje(this.ObtenerVelocidadCaminar(), this.ObtenerVelocidadRotar(), this.ObtenerFuerza());
            progressBarCreacion.Increment(5);
            this.modelo.CrearHud();
            progressBarCreacion.Increment(5);
            this.modelo.CrearPostProcesado();
            progressBarCreacion.Increment(5);

            this.Close();
        }

        private void trackBarPresipitaciones_Scroll(object sender, EventArgs e)
        {
            textoPresipitaciones.Text = "" + trackBarPresipitaciones.Value;
        }

        private void trackBarVelocidadTiempo_Scroll(object sender, EventArgs e)
        {
            textoVelocidadTiempo.Text = "" + trackBarVelocidadTiempo.Value;
        }

        private void trackBarFuerza_Scroll(object sender, EventArgs e)
        {
            textoFuerza.Text = "" + trackBarFuerza.Value;
        }

        private void comboBoxDificultad_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private int ObtenerValorDificultad()
        {
            String selectedItem = (String)comboBoxDificultad.SelectedItem;
            if (selectedItem.Equals("Fácil"))
            {
                return 300;
            }
            if (selectedItem.Equals("Medio"))
            {
                return 600;
            }
            if (selectedItem.Equals("Difícil"))
            {
                return 1200;
            }
            if (selectedItem.Equals("Muy Difícil"))
            {
                return 2400;
            }

            return 0;
        }

        private float ObtenerVelocidadTiempo()
        {
            return trackBarVelocidadTiempo.Value * 200;
        }

        private float ObtenerLapsoPrecipitaciones()
        {
            return trackBarPresipitaciones.Value;
        }

        private float ObtenerMomentoDeInicio()
        {
            return 3600 * (float)numericUpDownHoraInicio.Value;
        }

        private float ObtenerFuerza()
        {
            return trackBarFuerza.Value;
        }

        private float ObtenerVelocidadRotar()
        {
            return (float) numericUpDownVelocidadRotacion.Value;
        }

        private float ObtenerVelocidadCaminar()
        {
            return (float) numericUpDownVelocidadCaminar.Value;
        }
    }
}