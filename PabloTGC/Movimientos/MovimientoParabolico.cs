using AlumnoEjemplos.PabloTGC.Utiles;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC
{
    public class MovimientoParabolico
    {
        private const float Gravedad = 9.8f;

        private Vector3 posicionInicial { get; set; }
        private Vector3 direccion { get; set; }
        private float velocidad { get; set; }
        private float tiempo { get; set; }
        private MallaEnvoltura mesh;
        public bool Finalizo { get; set; }

        float proporcionalX;
        float proporcionalZ;

        private float velocidadInicialY;
        private float velocidadInicialXZ;

        public MovimientoParabolico()
        {
            this.mesh = null;
        }

        public MovimientoParabolico(Vector3 posicionInicial, Vector3 direccion, float velocidad, MallaEnvoltura mesh)
        {
            this.tiempo = 0;
            this.posicionInicial = posicionInicial;
            this.direccion = direccion;
            this.velocidad = velocidad;
            this.mesh = mesh;
            this.Finalizo = false;
            this.inicializar();
        }

        private void inicializar()
        {
            float componenteXZ = FastMath.Sqrt(FastMath.Pow2(direccion.X - posicionInicial.X) + FastMath.Pow2(direccion.Z - posicionInicial.Z));
            float componenteY = direccion.Y - posicionInicial.Y;
            float componenteX = direccion.X - posicionInicial.X;
            float componenteZ = direccion.Z - posicionInicial.Z;
            float angulo;
            float anguloxz;

            if (componenteXZ == 0)
            {
                anguloxz = 0;
            }
            else
            {
                anguloxz = FastMath.Asin((direccion.Z - posicionInicial.Z) / componenteXZ);
            }

            proporcionalX = FastMath.Cos(anguloxz);
            proporcionalZ = FastMath.Sin(anguloxz);

            //Si la componente de X es positiva, el proporcional debe ser positivo, y si es negativa, el proporcional debe ser negativo
            if (componenteX > 0)
            {
                if (proporcionalX < 0)
                {
                    proporcionalX *= -1;
                }
            }
            else
            {
                if (proporcionalX > 0)
                {
                    proporcionalX *= -1;
                }
            }

            //Si la componente de Z es positiva, el proporcional debe ser positivo, y si es negativa, el proporcional debe ser negativo
            if (componenteZ > 0)
            {
                if (proporcionalZ < 0)
                {
                    proporcionalX *= -1;
                }
            }
            else
            {
                if (proporcionalZ > 0)
                {
                    proporcionalZ *= -1;
                }
            }

            float largoVector = FastMath.Sqrt(FastMath.Pow2(direccion.X - posicionInicial.X) + FastMath.Pow2(direccion.Y - posicionInicial.Y)+ FastMath.Pow2(direccion.Z - posicionInicial.Z));
            angulo = FastMath.Asin((direccion.Y - posicionInicial.Y) / largoVector);

            this.velocidadInicialXZ = FastMath.Cos(angulo) * this.velocidad;
            this.velocidadInicialY = FastMath.Sin(angulo) * this.velocidad;
        }

        public void update(float elapsedTime, Terreno terreno)
        {
            if (this.mesh != null) { 
                tiempo += elapsedTime;

                Vector3 posicionUltima = this.mesh.Posicion();

                float distanciaRecorridaXZ = this.velocidadInicialXZ * tiempo /** elapsedTime*/;
                float distanciaRecorridaY = ((FastMath.Pow2(tiempo) * -0.5f * Gravedad) + this.velocidadInicialY * tiempo) /** elapsedTime*/;

                float x = this.mesh.Posicion().X + proporcionalX * distanciaRecorridaXZ;
                float z = this.mesh.Posicion().Z + proporcionalZ * distanciaRecorridaXZ;

                //TODO. Por el momentos nos manejamos con Y siempre positivas
                this.mesh.Posicion(new Vector3(x, this.mesh.Posicion().Y + distanciaRecorridaY, z));

                //TODO necesitamos el tamaño del elemento para poder saber cuando choca contra en terreno
                if ((this.mesh.MinimoPunto().Y - this.mesh.FactorCorreccion()) < terreno.CalcularAltura(this.mesh.MinimoPunto().X, this.mesh.MinimoPunto().Z))
                {
                    //Esto debe ser cuando colosiona con el terreno.
                    this.mesh.Posicion(posicionUltima);
                    this.mesh = null;
                    tiempo = 0;
                    this.Finalizo = true;
                }
            }
        }

        public void render()
        {
            if (this.mesh != null)
            {
                this.mesh.Render();
            }
        }
    }
}
