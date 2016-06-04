using AlumnoEjemplos.PabloTGC.Instrumentos;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.PabloTGC
{
    public class Personaje
    {
        #region Atributos
        private TgcSphere boundingEsfera;
        private TgcSphere alcanceInteraccionEsfera;
        private Elemento[] mochila;
        private float direccionVision;
        private float tiempoCorriendo;
        private Arma instrumentoManoDerecha;//TODO. queda pendiente que las armas extiendan de algun objeto en comun.
        private List<Arma> instrumentos;//TODO. queda pendiente que las armas extiendan de algun objeto en comun.
        #endregion

        #region Propiedades
        public float Fuerza { get; set; }
        public TgcSkeletalMesh mesh { get; set; }
        public float VelocidadCaminar { get; set; }
        public float VelocidadRotacion { get; set; }
        public float ResistenciaFisica { get; set; }
        public float Hidratacion { get; set; }
        public float Alimentacion { get; set; }
        public float TemperaturaCorporal { get; set; }
        #endregion

        #region Constructores

        public Personaje()
        {
            this.mochila = new Elemento[9] {null, null, null, null, null, null, null, null, null};//La mochila solo tiene 9 elementos
            this.instrumentos = new List<Arma>();
            this.tiempoCorriendo = 0;
            this.direccionVision = 0;
            this.TemperaturaCorporal = 37;
        }
        #endregion

        #region Comportamientos

        public void IniciarBoundingEsfera()
        {
            //Esta esfera seria el "bounding box" del personaje
            boundingEsfera = new TgcSphere();
            boundingEsfera.setColor(Color.SkyBlue);
            boundingEsfera.Radius = 60;
            boundingEsfera.Position = this.mesh.Position;
            boundingEsfera.updateValues();
        }

        public void IniciarAlcanceInteraccionEsfera()
        {
            alcanceInteraccionEsfera = new TgcSphere();
            alcanceInteraccionEsfera.setColor(Color.White);
            alcanceInteraccionEsfera.Radius = 70;
            alcanceInteraccionEsfera.Position = this.mesh.Position;
            alcanceInteraccionEsfera.updateValues();
        }

        public void ActualizarEsferas()
        {
            //Actualizamos la esfera del bounding
            boundingEsfera.Position = this.mesh.Position + new Vector3(0, (this.mesh.BoundingBox.PMax.Y - this.mesh.BoundingBox.PMin.Y) / 2, 0);
            boundingEsfera.updateValues();

            //Actualizamos la esfera del alcance
            //TODO. Misma logica de siempre para saber la direccion del persnaje. REVISAR.
            Vector3 direccionEsferaGolpe = this.mesh.Position + new Vector3(-(float)Math.Sin((float)this.mesh.Rotation.Y) * 50,
                (this.mesh.BoundingBox.PMax.Y - this.mesh.BoundingBox.PMin.Y) / 2, -(float)Math.Cos((float)this.mesh.Rotation.Y) * 50);
            alcanceInteraccionEsfera.Position = direccionEsferaGolpe;
            alcanceInteraccionEsfera.updateValues();
        }

        public TgcSphere GetBoundingEsfera()
        {
            return this.boundingEsfera;
        }

        public TgcSphere GetAlcanceInteraccionEsfera()
        {
            return this.alcanceInteraccionEsfera;
        }

        public void agregarInstrumento(Arma instrumento)
        {
            this.instrumentos.Add(instrumento);
        }

        public void juntar(Elemento elemento)
        {
            for (int i = 0; i < 9; i++)
            {
                if (this.mochila[i] == null)
                {
                    this.mochila[i] = elemento;
                    return;
                }
            }
            throw new Exception("No tiene más lugar en la mochila");
        }

        public void Dejar(Elemento elemento)
        {
            for (int i = 0; i < 9; i++)
            {
                if (this.mochila[i] != null && this.mochila[i].Equals(elemento))
                {
                    this.mochila[i] = null;
                    return;
                }
            }
            throw new Exception("No tiene ese elementos en su mochila");
        }

        public void DejarElementos(List<Elemento> elementos)
        {
            foreach (Elemento elem in elementos)
            {
                this.Dejar(elem);
            }
        }

        public float correr(float tiempo)
        {
            if (this.tiempoCorriendo >= this.ResistenciaFisica)
            {
                this.tiempoCorriendo = this.ResistenciaFisica;
                return this.VelocidadCaminar;
            }
            else
            {
                this.tiempoCorriendo += tiempo;
                if (this.tiempoCorriendo >= this.ResistenciaFisica * 0.5)
                {
                    return this.VelocidadCaminar * 1.5f;
                }
                return this.VelocidadCaminar * 3.5f;
            }
        }

        public float rotarRapido()
        {
            return this.VelocidadRotacion * 2.5f;
        }

        /// <summary>
        /// Afecta la salud del personaje dado un tiempo pasado por parámetro. El tiempo es en segundos. La salud del personaje se compone por su hidratacion
        /// y por su alimentacion (mitad de cada una)
        /// </summary>
        /// <param name="tiempoEnSegundos"></param>
        public void AfectarSaludPorTiempo(float tiempoEnSegundos)
        {
            //Agregamos estos valores que serian como las defensas del personaje. Si en algun momento tiene traje podrian ser mas bajos (proteger más)
            this.AfectarHidratacion(tiempoEnSegundos * 0.1f);
            this.AfectarAlimentacion(tiempoEnSegundos * 0.3f);
        }

        private void AfectarHidratacion(float tiempoEnSegundos)
        {
            this.Hidratacion -= tiempoEnSegundos;
        }

        private void AfectarAlimentacion(float tiempoEnSegundos)
        {
            this.Alimentacion -= tiempoEnSegundos;
        }

        public void incrementoResistenciaFisica(float tiempoEnSegundos)
        {
            if ((this.tiempoCorriendo - tiempoEnSegundos) > 0)
            {
                this.tiempoCorriendo -= tiempoEnSegundos;
            }
            else
            {
                this.tiempoCorriendo = 0;
            }
        }

        public bool estaMuerto()
        {
            return (this.Hidratacion + this.Alimentacion <= 0) || (this.TemperaturaCorporal < 34);
        }

        public void ConsumirAlimento(float nutricion)
        {
            this.Alimentacion += nutricion;
            if (this.Alimentacion > 100)
            {
                this.Alimentacion = 100;
            }
        }

        public void ConsumirAguar(float cantidad)
        {
            this.Hidratacion += cantidad;
            if (this.Hidratacion > 100)
            {
                this.Hidratacion = 100;
            }
        }

        public void IncrementarTemperaturaCorporalPorTiempo(float temperatura, float tiempoEnSegundos)
        {
            this.TemperaturaCorporal += temperatura * tiempoEnSegundos * 0.05f;
            if (this.TemperaturaCorporal > 37)
            {
                this.TemperaturaCorporal = 37;
            }
        }

        public void AfectarSaludPorTemperatura(float temperatura, float tiempo)
        {
            if (temperatura < 24)
            {
                //Ese 0.001 seria la resistencia, se puede decrementar con trajes.
                this.TemperaturaCorporal -= ((24 - temperatura) * tiempo * 0.001f);
            }
            if (this.TemperaturaCorporal < 31)
            {
                this.TemperaturaCorporal = 31;
            }
        }

        public void morir()
        {
            this.Hidratacion = 0;
            this.Alimentacion = 0;
            this.TemperaturaCorporal = 0;
        }

        public List<Elemento> elementosEnMochila()
        {
            List<Elemento> elementos = new List<Elemento>();
            for (int i = 0; i < 9; i++)
            {
                if (this.mochila[i] != null)
                {
                    elementos.Add(this.mochila[i]);
                }
            }
            return elementos;
        }

        public bool ContieneElementoEnMochila(Elemento elemento)
        {
            for (int i = 0; i < 9; i++)
            {
                if (this.mochila[i] != null && this.mochila[i].Equals(elemento))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContieneElementoEnMochilaDeTipo(String tipo)
        {
            for (int i = 0; i < 9; i++)
            {
                if (this.mochila[i] != null && this.mochila[i].EsDeTipo(tipo))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContieneElementoEnPosicionDeMochila(int numero)
        {
            return (this.mochila[numero] != null) ;
        }

        public Elemento DarElementoEnPosicionDeMochila(int numero)
        {
            return this.mochila[numero];
        }

        /// <summary>
        /// El orden en el cual fueron cargados los instrumentos es el orden que se utilizará para seleccionar el instrumento
        /// </summary>
        /// <param name="numeroInstrumento"></param>
        public void seleccionarInstrumentoManoDerecha(int numeroInstrumento)
        {
            //Por el momento sabemos que el Attachment 0 es el que esta en la mano derecha
            this.instrumentoManoDerecha = this.instrumentos[numeroInstrumento];
            this.mesh.Attachments[0].Mesh = this.instrumentoManoDerecha.mesh;
            this.mesh.Attachments[0].Offset = this.instrumentoManoDerecha.translacion;
            this.mesh.Attachments[0].updateValues();
        }

        public float alcancePatada()
        {
            return 60;
        }

        public float fuerzaPatada()
        {
            return 66;
        }

        public float alcanceGolpe()
        {
            return this.instrumentoManoDerecha.alcance;
        }

        public float fuerzaGolpe()
        {
            return this.instrumentoManoDerecha.potenciaGolpe * this.Fuerza;
        }

        public float PorcentajeDeSalud()
        {
            float actual = (this.Hidratacion + this.Alimentacion) / 200;
            if (actual < 0)
            {
                return 0;
            }
            else if(actual > 1)
            {
                return 1;
            }
            return actual;
        }

        public float PorcentajeDeHidratacion()
        {
            float actual = this.Hidratacion / 100;
            if (actual < 0)
            {
                return 0;
            }
            else if (actual > 1)
            {
                return 1;
            }
            return actual;
        }

        public float PorcentajeDeAlimentacion()
        {
            float actual = this.Alimentacion / 100;
            if (actual < 0)
            {
                return 0;
            }
            else if (actual > 1)
            {
                return 1;
            }
            return actual;
        }

        public float PorcentajeDeCansancio()
        {
            return 1 - (this.tiempoCorriendo / this.ResistenciaFisica);
        }

        public String TemperaturaCorporalTexto()
        {
            return ((int)this.TemperaturaCorporal).ToString() + "°";
        }

        public Vector3 Direccion(float distancia)
        {
            //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
            float z = -(float)Math.Cos((float)this.mesh.Rotation.Y) * distancia;
            float x = -(float)Math.Sin((float)this.mesh.Rotation.Y) * distancia;
            //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
            //el vector salga del personaje.
            return this.mesh.Position + new Vector3(x, 0, z);
        }

        public Vector3 DireccionAlturaCabeza(float distancia)
        {
            //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
            float z = -(float)Math.Cos((float)this.mesh.Rotation.Y) * distancia;
            float x = -(float)Math.Sin((float)this.mesh.Rotation.Y) * distancia;
            //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
            //el vector salga del personaje.
            return this.PosicionAlturaCabeza() + new Vector3(x, this.direccionVision, z);
        }

        public Vector3 PosicionAlturaCabeza()
        {
            return this.mesh.Position + new Vector3(0, (this.mesh.BoundingBox.PMax.Y - this.mesh.BoundingBox.PMin.Y), 0);
        }

        public void SubirVision(float valor)
        {
            if (this.direccionVision < 200)
            {
                this.direccionVision += valor;
            }
        }

        public void BajarVision(float valor)
        {
            if (this.direccionVision > -(this.mesh.BoundingBox.PMax.Y - this.mesh.BoundingBox.PMin.Y))
            {
                this.direccionVision -= valor;
            }
        }

        public void Renderizar()
        {
            this.mesh.animateAndRender();
        }

        #endregion
    }
}
