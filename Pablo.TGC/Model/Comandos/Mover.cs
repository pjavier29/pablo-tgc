using Microsoft.DirectX;
using TGC.Core.Utils;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles;

namespace TGC.Group.Model.Comandos
{
    public class Mover : Comando
    {
        #region Atributos

        private readonly float sentido;

        #endregion Atributos

        #region Propiedades

        public bool MovimientoRapido { get; set; }

        #endregion Propiedades

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            float movimiento;
            //Aplicar movimiento hacia adelante o atras segun la orientacion actual del Mesh
            var lastPos = contexto.personaje.mesh.Position;

            if (MovimientoRapido)
            {
                movimiento = sentido * contexto.personaje.correr(elapsedTime);
            }
            else
            {
                movimiento = sentido * contexto.personaje.VelocidadCaminar;
            }

            //Aplicamos el movimiento
            var xm = FastMath.Sin(contexto.personaje.mesh.Rotation.Y) * movimiento;
            var zm = FastMath.Cos(contexto.personaje.mesh.Rotation.Y) * movimiento;
            var movementVector = new Vector3(xm, 0, zm);
            contexto.personaje.mesh.move(movementVector * elapsedTime);
            contexto.personaje.mesh.Position = new Vector3(contexto.personaje.mesh.Position.X,
                contexto.terreno.CalcularAltura(contexto.personaje.mesh.Position.X, contexto.personaje.mesh.Position.Z),
                contexto.personaje.mesh.Position.Z);

            //Para saber si sale o no del mapa
            //TODO. Queda pendiente aplicarlo a los animales y cuando salta para adelante.
            if (!FuncionesMatematicas.Instance.EstaDentroDelCuadrado(contexto.personaje.mesh.Position, contexto.esquina))
            {
                contexto.personaje.mesh.Position = lastPos;
                return;
            }

            contexto.personaje.ActualizarEsferas();

            //Detectar colisiones
            var collide = false;
            foreach (var elem in contexto.optimizador.ElementosColision)
            {
                if (ControladorColisiones.EsferaColisionaCuadrado(contexto.personaje.GetBoundingEsfera(),
                    elem.BoundingBox()))
                {
                    collide = true;
                    elem.procesarColision(contexto.personaje, elapsedTime, contexto.elementos, movimiento,
                        movementVector, lastPos);
                    break;
                }
            }

            if (!collide)
            {
                if (MovimientoRapido)
                {
                    //Activar animacion de corriendo
                    contexto.personaje.mesh.playAnimation("Correr", true);
                }
                else
                {
                    //Activar animacion de caminando
                    contexto.personaje.mesh.playAnimation("Caminando", true);
                }
            }

            //Si hubo mivimiento actualizamos el centro del SkyBox para simular que es infinito, tambien el del piso
            if (!contexto.personaje.mesh.Position.Equals(lastPos))
            {
                contexto.ActualizarPosicionSkyBox(new Vector3(contexto.personaje.mesh.Position.X - lastPos.X, 0,
                    contexto.personaje.mesh.Position.Z - lastPos.Z));
                contexto.ActualizarPosicionSuelo(new Vector3(contexto.personaje.mesh.Position.X - lastPos.X, 0,
                    contexto.personaje.mesh.Position.Z - lastPos.Z));
            }
        }

        #endregion Comportamientos

        #region Constructores

        public Mover(float sentido)
        {
            this.sentido = sentido;
            MovimientoRapido = false;
        }

        public Mover(float sentido, bool rapido)
        {
            this.sentido = sentido;
            MovimientoRapido = rapido;
        }

        #endregion Constructores
    }
}