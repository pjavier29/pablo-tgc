using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.Geometry;
using TGC.Core.SkeletalAnimation;
using TGC.Group.Model.ElementosJuego;
using TGC.Group.Model.ElementosJuego.Instrumentos;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.Administracion
{
    public class Personaje
    {
        #region Constructores

        public Personaje()
        {
            mochila = new Elemento[9] { null, null, null, null, null, null, null, null, null };
            //La mochila solo tiene 9 elementos
            instrumentos = new List<Arma>();
            tiempoCorriendo = 0;
            direccionVision = 0;
            TemperaturaCorporal = 37;
            antorcha = null;
        }

        #endregion Constructores

        #region Atributos

        private TgcSphere boundingEsfera;
        private TgcSphere alcanceInteraccionEsfera;
        private readonly Elemento[] mochila;
        private float direccionVision;
        private float tiempoCorriendo;
        private Arma instrumentoManoDerecha; //TODO. queda pendiente que las armas extiendan de algun objeto en comun.

        private readonly List<Arma> instrumentos;
        //TODO. queda pendiente que las armas extiendan de algun objeto en comun.

        public Antorcha antorcha;
        private Efecto efecto;

        #endregion Atributos

        #region Propiedades

        public float Fuerza { get; set; }
        public TgcSkeletalMesh mesh { get; set; }
        public float VelocidadCaminar { get; set; }
        public float VelocidadRotacion { get; set; }
        public float ResistenciaFisica { get; set; }
        public float Hidratacion { get; set; }
        public float Alimentacion { get; set; }
        public float TemperaturaCorporal { get; set; }
        public Color Color { get; set; }

        #endregion Propiedades

        #region Comportamientos

        public void IniciarBoundingEsfera()
        {
            //Esta esfera seria el "bounding box" del personaje
            boundingEsfera = new TgcSphere();
            boundingEsfera.setColor(Color.SkyBlue);
            boundingEsfera.Radius = 60;
            boundingEsfera.Position = mesh.Position;
            boundingEsfera.updateValues();
        }

        public void IniciarAlcanceInteraccionEsfera()
        {
            alcanceInteraccionEsfera = new TgcSphere();
            alcanceInteraccionEsfera.setColor(Color.White);
            alcanceInteraccionEsfera.Radius = 70;
            alcanceInteraccionEsfera.Position = mesh.Position;
            alcanceInteraccionEsfera.updateValues();
        }

        public void ActualizarEsferas()
        {
            //Actualizamos la esfera del bounding
            boundingEsfera.Position = mesh.Position +
                                      new Vector3(0, (mesh.BoundingBox.PMax.Y - mesh.BoundingBox.PMin.Y) / 2, 0);
            boundingEsfera.updateValues();

            //Actualizamos la esfera del alcance
            //TODO. Misma logica de siempre para saber la direccion del persnaje. REVISAR.
            var direccionEsferaGolpe = mesh.Position + new Vector3(-(float)Math.Sin(mesh.Rotation.Y) * 50,
                (mesh.BoundingBox.PMax.Y - mesh.BoundingBox.PMin.Y) / 2, -(float)Math.Cos(mesh.Rotation.Y) * 50);
            alcanceInteraccionEsfera.Position = direccionEsferaGolpe;
            alcanceInteraccionEsfera.updateValues();

            //TODO. Esto esta muy mal
            if (antorcha != null)
            {
                antorcha.SetPosicion(Direccion(50) + new Vector3(0, 20, 0));
            }
        }

        public TgcSphere GetBoundingEsfera()
        {
            return boundingEsfera;
        }

        public TgcSphere GetAlcanceInteraccionEsfera()
        {
            return alcanceInteraccionEsfera;
        }

        public void agregarInstrumento(Arma instrumento)
        {
            instrumentos.Add(instrumento);
        }

        public void juntar(Elemento elemento)
        {
            for (var i = 0; i < 9; i++)
            {
                if (mochila[i] == null)
                {
                    mochila[i] = elemento;
                    return;
                }
            }
            throw new Exception("No tiene más lugar en la mochila");
        }

        public void Dejar(Elemento elemento)
        {
            for (var i = 0; i < 9; i++)
            {
                if (mochila[i] != null && mochila[i].Equals(elemento))
                {
                    mochila[i] = null;
                    return;
                }
            }
            throw new Exception("No tiene ese elementos en su mochila");
        }

        public void DejarElementos(List<Elemento> elementos)
        {
            foreach (var elem in elementos)
            {
                Dejar(elem);
            }
        }

        public float correr(float tiempo)
        {
            if (tiempoCorriendo >= ResistenciaFisica)
            {
                tiempoCorriendo = ResistenciaFisica;
                return VelocidadCaminar;
            }
            tiempoCorriendo += tiempo;
            if (tiempoCorriendo >= ResistenciaFisica * 0.5)
            {
                return VelocidadCaminar * 1.5f;
            }
            return VelocidadCaminar * 3.5f;
        }

        public float rotarRapido()
        {
            return VelocidadRotacion * 2.5f;
        }

        /// <summary>
        ///     Afecta la salud del personaje dado un tiempo pasado por parámetro. El tiempo es en segundos. La salud del personaje
        ///     se compone por su hidratacion
        ///     y por su alimentacion (mitad de cada una)
        /// </summary>
        /// <param name="tiempoEnSegundos"></param>
        public void AfectarSaludPorTiempo(float tiempoEnSegundos)
        {
            //Agregamos estos valores que serian como las defensas del personaje. Si en algun momento tiene traje podrian ser mas bajos (proteger más)
            AfectarHidratacion(tiempoEnSegundos * 0.1f);
            AfectarAlimentacion(tiempoEnSegundos * 0.3f);
        }

        private void AfectarHidratacion(float tiempoEnSegundos)
        {
            Hidratacion -= tiempoEnSegundos;
        }

        private void AfectarAlimentacion(float tiempoEnSegundos)
        {
            Alimentacion -= tiempoEnSegundos;
        }

        public void incrementoResistenciaFisica(float tiempoEnSegundos)
        {
            if (tiempoCorriendo - tiempoEnSegundos > 0)
            {
                tiempoCorriendo -= tiempoEnSegundos;
            }
            else
            {
                tiempoCorriendo = 0;
            }
        }

        public bool estaMuerto()
        {
            return (Hidratacion + Alimentacion <= 0) || (TemperaturaCorporal < 34);
        }

        public void ConsumirAlimento(float nutricion)
        {
            Alimentacion += nutricion;
            if (Alimentacion > 100)
            {
                Alimentacion = 100;
            }
        }

        public void ConsumirAguar(float cantidad)
        {
            Hidratacion += cantidad;
            if (Hidratacion > 100)
            {
                Hidratacion = 100;
            }
        }

        public void IncrementarTemperaturaCorporalPorTiempo(float temperatura, float tiempoEnSegundos)
        {
            TemperaturaCorporal += temperatura * tiempoEnSegundos * 0.05f;
            if (TemperaturaCorporal > 37)
            {
                TemperaturaCorporal = 37;
            }
        }

        public void AfectarSaludPorTemperatura(float temperatura, float tiempo)
        {
            if (temperatura < 24)
            {
                //Ese 0.001 seria la resistencia, se puede decrementar con trajes.
                TemperaturaCorporal -= (24 - temperatura) * tiempo * 0.001f;
            }
            if (TemperaturaCorporal < 31)
            {
                TemperaturaCorporal = 31;
            }
        }

        public void morir()
        {
            Hidratacion = 0;
            Alimentacion = 0;
            TemperaturaCorporal = 0;
        }

        public List<Elemento> elementosEnMochila()
        {
            var elementos = new List<Elemento>();
            for (var i = 0; i < 9; i++)
            {
                if (mochila[i] != null)
                {
                    elementos.Add(mochila[i]);
                }
            }
            return elementos;
        }

        public bool ContieneElementoEnMochila(Elemento elemento)
        {
            for (var i = 0; i < 9; i++)
            {
                if (mochila[i] != null && mochila[i].Equals(elemento))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContieneElementoEnMochilaDeTipo(string tipo)
        {
            for (var i = 0; i < 9; i++)
            {
                if (mochila[i] != null && mochila[i].EsDeTipo(tipo))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContieneElementoEnPosicionDeMochila(int numero)
        {
            return mochila[numero] != null;
        }

        public Elemento DarElementoEnPosicionDeMochila(int numero)
        {
            return mochila[numero];
        }

        /// <summary>
        ///     El orden en el cual fueron cargados los instrumentos es el orden que se utilizará para seleccionar el instrumento
        /// </summary>
        /// <param name="numeroInstrumento"></param>
        public void seleccionarInstrumentoManoDerecha(int numeroInstrumento)
        {
            //Por el momento sabemos que el Attachment 0 es el que esta en la mano derecha
            instrumentoManoDerecha = instrumentos[numeroInstrumento];
            mesh.Attachments[0].Mesh = instrumentoManoDerecha.mesh;
            mesh.Attachments[0].Offset = instrumentoManoDerecha.translacion;
            mesh.Attachments[0].updateValues();
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
            return instrumentoManoDerecha.alcance;
        }

        public float fuerzaGolpe()
        {
            return instrumentoManoDerecha.potenciaGolpe * Fuerza;
        }

        public float PorcentajeDeSalud()
        {
            var actual = (Hidratacion + Alimentacion) / 200;
            if (actual < 0)
            {
                return 0;
            }
            if (actual > 1)
            {
                return 1;
            }
            return actual;
        }

        public float PorcentajeDeHidratacion()
        {
            var actual = Hidratacion / 100;
            if (actual < 0)
            {
                return 0;
            }
            if (actual > 1)
            {
                return 1;
            }
            return actual;
        }

        public float PorcentajeDeAlimentacion()
        {
            var actual = Alimentacion / 100;
            if (actual < 0)
            {
                return 0;
            }
            if (actual > 1)
            {
                return 1;
            }
            return actual;
        }

        public float PorcentajeDeCansancio()
        {
            return 1 - tiempoCorriendo / ResistenciaFisica;
        }

        public string TemperaturaCorporalTexto()
        {
            return (int)TemperaturaCorporal + "°";
        }

        public Vector3 Direccion(float distancia)
        {
            //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
            var z = -(float)Math.Cos(mesh.Rotation.Y) * distancia;
            var x = -(float)Math.Sin(mesh.Rotation.Y) * distancia;
            //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
            //el vector salga del personaje.
            return mesh.Position + new Vector3(x, 0, z);
        }

        public Vector3 DireccionAlturaCabeza(float distancia)
        {
            //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
            var z = -(float)Math.Cos(mesh.Rotation.Y) * distancia;
            var x = -(float)Math.Sin(mesh.Rotation.Y) * distancia;
            //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
            //el vector salga del personaje.
            return PosicionAlturaCabeza() + new Vector3(x, direccionVision, z);
        }

        public Vector3 PosicionAlturaCabeza()
        {
            return mesh.Position + new Vector3(0, mesh.BoundingBox.PMax.Y - mesh.BoundingBox.PMin.Y, 0);
        }

        public void SubirVision(float valor)
        {
            if (direccionVision < 200)
            {
                direccionVision += valor;
            }
        }

        public void BajarVision(float valor)
        {
            if (direccionVision > -(mesh.BoundingBox.PMax.Y - mesh.BoundingBox.PMin.Y))
            {
                direccionVision -= valor;
            }
        }

        public void RenderizarTerceraPersona(SuvirvalCraft contexto)
        {
            if (efecto != null)
            {
                efecto.ActualizarRenderizar(contexto, contexto.ElapsedTime);
            }
            else
            {
                mesh.animateAndRender(contexto.ElapsedTime);
            }

            //TODO esto es muy feo
            if (TieneAntorchaSeleccionada())
            {
                //Si esta seleccionada la antorcha la renderizamos
                antorcha.renderizar(contexto);
            }
            else
            {
                instrumentoManoDerecha.renderizar(contexto);
            }
        }

        public void RenderizarPrimeraPersona(SuvirvalCraft contexto)
        {
            mesh.updateAnimation(contexto.ElapsedTime);
            mesh.Transform = Matrix.Scaling(mesh.Scale)
                             * Matrix.RotationYawPitchRoll(mesh.Rotation.Y, mesh.Rotation.X, mesh.Rotation.Z)
                             * Matrix.Translation(mesh.Position);

            //Renderizar attachments
            foreach (var attach in mesh.Attachments)
            {
                attach.Mesh.Transform = attach.Offset * attach.Bone.MatFinal * mesh.Transform;
                //attach.Mesh.render();
            }

            //TODO esto es muy feo
            if (TieneAntorchaSeleccionada())
            {
                //Si esta seleccionada la antorcha la renderizamos
                antorcha.renderizar(contexto);
            }
            else
            {
                instrumentoManoDerecha.renderizar(contexto);
            }
        }

        public bool TieneAntorchaSeleccionada()
        {
            //TODO. Esto esta muyy malll.
            return instrumentoManoDerecha.mesh == antorcha.Mesh;
        }

        public void Dispose()
        {
            /*foreach (Arma arma in this.instrumentos)
            {
                arma.mesh.dispose();
            }*/
            antorcha.sonidoAntorcha.dispose();
            mesh.dispose();
        }

        public void SetEfecto(Efecto efecto)
        {
            this.efecto = efecto;
            efecto.Aplicar(mesh);
        }

        #region Para configurar la luz

        public virtual ColorValue ColorEmisor()
        {
            return ColorValue.FromColor(Color.Black);
        }

        public virtual ColorValue ColorAmbiente()
        {
            return ColorValue.FromColor(Color);
        }

        public virtual ColorValue ColorDifuso()
        {
            return ColorValue.FromColor(Color);
        }

        public virtual ColorValue ColorEspecular()
        {
            return ColorValue.FromColor(Color);
        }

        public virtual float EspecularEx()
        {
            return 20;
        }

        #endregion Para configurar la luz

        #endregion Comportamientos
    }
}