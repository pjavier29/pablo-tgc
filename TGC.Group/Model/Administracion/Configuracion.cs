using System;
using System.Windows.Forms;

namespace TGC.Group.Model.Administracion
{
    public partial class Configuracion : System.Windows.Forms.Form
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
            radioButtonPrimeraPersona.Checked = true;
            modelo.IniciarMusicaConfiguracion();
        }

        private void botonAceptar_Click(object sender, EventArgs e)
        {
            modelo.IniciarCreacion(ObtenerValorDificultad(), checkBoxPantallaCompleta.Checked);
            progressBarCreacion.Increment(5);
            modelo.AdministracionDeEfectos();
            progressBarCreacion.Increment(5);
            modelo.CrearHeimap();
            progressBarCreacion.Increment(5);
            modelo.CrearIluminacion(ObtenerVelocidadTiempo(), ObtenerMomentoDeInicio(),
                ObtenerLapsoPrecipitaciones());
            progressBarCreacion.Increment(5);
            modelo.CrearSkyBox();
            progressBarCreacion.Increment(5);
            modelo.CrearPalmerasComunes();
            progressBarCreacion.Increment(5);
            modelo.CrearArbolesBanana();
            progressBarCreacion.Increment(5);
            modelo.CrearArbolesDeLenia();
            progressBarCreacion.Increment(5);
            modelo.CrearPiedraParaTirar();
            progressBarCreacion.Increment(5);
            modelo.CrearOvejaYGallo();
            progressBarCreacion.Increment(5);
            modelo.CreamosLosCajones();
            progressBarCreacion.Increment(5);
            modelo.CrearArbolesGenerales();
            progressBarCreacion.Increment(5);
            modelo.CrearFuenteAgua();
            progressBarCreacion.Increment(5);
            modelo.CrearAlgas();
            progressBarCreacion.Increment(5);
            modelo.CrearPiedrasSobreAgua();
            progressBarCreacion.Increment(5);
            modelo.CrearCanoasSobreAgua();
            progressBarCreacion.Increment(5);
            modelo.CrearArbolFrutilla();
            progressBarCreacion.Increment(5);
            modelo.CrearPiso();
            progressBarCreacion.Increment(5);
            modelo.CrearPersonaje(ObtenerVelocidadCaminar(), ObtenerVelocidadRotar(), ObtenerFuerza(),
                pictureBoxColorPersonaje.BackColor);
            progressBarCreacion.Increment(5);
            modelo.CrearHud();
            progressBarCreacion.Increment(5);
            modelo.CrearPostProcesado();
            progressBarCreacion.Increment(5);
            if (radioButtonPrimeraPersona.Checked)
            {
                modelo.IniciarCamaraPrimeraPersona();
            }
            else
            {
                modelo.IniciarCamaraTerceraPersona();
            }
            progressBarCreacion.Increment(5);
            modelo.IniciarMusicasYSonidos();
            Close();
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
            var selectedItem = (string)comboBoxDificultad.SelectedItem;
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
            return (float)numericUpDownVelocidadRotacion.Value;
        }

        private float ObtenerVelocidadCaminar()
        {
            return (float)numericUpDownVelocidadCaminar.Value;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var color = new ColorDialog();
            color.AllowFullOpen = false;
            color.ShowHelp = true;
            color.Color = pictureBoxColorPersonaje.BackColor;
            if (color.ShowDialog() == DialogResult.OK)
                pictureBoxColorPersonaje.BackColor = color.Color;
        }
    }
}