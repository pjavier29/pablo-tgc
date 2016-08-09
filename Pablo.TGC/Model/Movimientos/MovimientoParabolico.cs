using Microsoft.DirectX;
using TGC.Core.Utils;
using TGC.Group.Model.Utiles;

namespace TGC.Group.Model.Movimientos
{
    public class MovimientoParabolico
    {
        private const float Gravedad = 9.8f;
        private MallaEnvoltura mesh;

        private float proporcionalX;
        private float proporcionalZ;
        private float velocidadInicialXZ;

        private float velocidadInicialY;

        public MovimientoParabolico()
        {
            mesh = null;
        }

        public MovimientoParabolico(Vector3 posicionInicial, Vector3 direccion, float velocidad, MallaEnvoltura mesh)
        {
            tiempo = 0;
            this.posicionInicial = posicionInicial;
            this.direccion = direccion;
            this.velocidad = velocidad;
            this.mesh = mesh;
            Finalizo = false;
            inicializar();
        }

        private Vector3 posicionInicial { get; }
        private Vector3 direccion { get; }
        private float velocidad { get; }
        private float tiempo { get; set; }
        public bool Finalizo { get; set; }

        private void inicializar()
        {
            var componenteXZ =
                FastMath.Sqrt(FastMath.Pow2(direccion.X - posicionInicial.X) +
                              FastMath.Pow2(direccion.Z - posicionInicial.Z));
            var componenteY = direccion.Y - posicionInicial.Y;
            var componenteX = direccion.X - posicionInicial.X;
            var componenteZ = direccion.Z - posicionInicial.Z;
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

            var largoVector =
                FastMath.Sqrt(FastMath.Pow2(direccion.X - posicionInicial.X) +
                              FastMath.Pow2(direccion.Y - posicionInicial.Y) +
                              FastMath.Pow2(direccion.Z - posicionInicial.Z));
            angulo = FastMath.Asin((direccion.Y - posicionInicial.Y) / largoVector);

            velocidadInicialXZ = FastMath.Cos(angulo) * velocidad;
            velocidadInicialY = FastMath.Sin(angulo) * velocidad;
        }

        public void update(float elapsedTime, Terreno terreno)
        {
            if (mesh != null)
            {
                tiempo += elapsedTime;

                var posicionUltima = mesh.Posicion();

                var distanciaRecorridaXZ = velocidadInicialXZ * tiempo /** elapsedTime*/;
                var distanciaRecorridaY = FastMath.Pow2(tiempo) * -0.5f * Gravedad + velocidadInicialY * tiempo
                    /** elapsedTime*/;

                var x = mesh.Posicion().X + proporcionalX * distanciaRecorridaXZ;
                var z = mesh.Posicion().Z + proporcionalZ * distanciaRecorridaXZ;

                //TODO. Por el momentos nos manejamos con Y siempre positivas
                mesh.Posicion(new Vector3(x, mesh.Posicion().Y + distanciaRecorridaY, z));

                //TODO necesitamos el tamaño del elemento para poder saber cuando choca contra en terreno
                if (mesh.MinimoPunto().Y - mesh.FactorCorreccion() <
                    terreno.CalcularAltura(mesh.MinimoPunto().X, mesh.MinimoPunto().Z))
                {
                    //Esto debe ser cuando colosiona con el terreno.
                    mesh.Posicion(posicionUltima);
                    mesh = null;
                    tiempo = 0;
                    Finalizo = true;
                }
            }
        }

        public void render()
        {
            if (mesh != null)
            {
                mesh.Render();
            }
        }
    }
}