using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TGC.Core.SceneLoader;
using TGC.Core.Utils;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosJuego
{
    public class Animal : Elemento
    {
        #region Contructores

        public Animal(float peso, float resistencia, TgcMesh mesh, Efecto efecto)
            : base(peso, resistencia, mesh, efecto)
        {
            tiempoEnActividad = 7;
            tiempoInactivo = 3;
            tiempo = 0;
            velocidadCaminar = 30f;
            velocidadRotar = 10F;
            movimientoActual = "Caminar";
        }

        #endregion Contructores

        #region Atributos

        private readonly float tiempoEnActividad;
        private readonly float tiempoInactivo;
        private float tiempo;
        private readonly float velocidadCaminar;
        private readonly float velocidadRotar;
        private string movimientoActual;

        #endregion Atributos

        #region Comportamientos

        public override void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
            base.Actualizar(contexto, elapsedTime);
            tiempo += elapsedTime;
            if (tiempo < tiempoEnActividad)
            {
                simularMovimiento(elapsedTime, contexto.terreno);
                //TODO. Colocar animación de caminar
            }
            else
            {
                //TODO. Colocar animacion de comer pasto
                if (tiempo > tiempoEnActividad + tiempoInactivo)
                {
                    tiempo = 0;
                    var aleatorioActual = FuncionesMatematicas.Instance.NumeroAleatorioDouble();
                    if (aleatorioActual < 0.2F)
                    {
                        movimientoActual = "Caminar";
                    }
                    else
                    {
                        if (aleatorioActual < 0.6F)
                        {
                            movimientoActual = "CaminarDerecha";
                        }
                        else
                        {
                            movimientoActual = "CaminarIzquierda";
                        }
                    }
                }
            }
            //Tenemos que actualizar los puntos de la barra ya que el animal se mueve por el terreno
            ActualizarBarraEstadoCompleta(Mesh.BoundingBox.PMin,
                new Vector3(Mesh.BoundingBox.PMin.X, Mesh.BoundingBox.PMax.Y, Mesh.BoundingBox.PMin.Z));
        }

        private void simularMovimiento(float elapsedTime, Terreno terreno)
        {
            if (movimientoActual.Equals("Caminar"))
            {
                moverse(elapsedTime, terreno);
            }

            if (movimientoActual.Equals("CaminarDerecha"))
            {
                moverseRotando(elapsedTime, terreno, 1);
            }

            if (movimientoActual.Equals("CaminarIzquierda"))
            {
                moverseRotando(elapsedTime, terreno, -1);
            }
        }

        private void moverse(float elapsedTime, Terreno terreno)
        {
            //Aplicamos el movimiento
            //TODO Ver si es correcta la forma que aplico para representar que se esta a la altura del terreno.
            var xm = FastMath.Sin(Mesh.Rotation.Y) * velocidadCaminar;
            var zm = FastMath.Cos(Mesh.Rotation.Y) * velocidadCaminar;
            var movementVector = new Vector3(xm, 0, zm);
            Mesh.move(movementVector * elapsedTime);
            Mesh.Position = new Vector3(Mesh.Position.X, terreno.CalcularAltura(Mesh.Position.X, Mesh.Position.Z),
                Mesh.Position.Z);
        }

        private void moverseRotando(float elapsedTime, Terreno terreno, int direccion)
        {
            var rotAngle = Geometry.DegreeToRadian(direccion * velocidadRotar * elapsedTime);
            Mesh.rotateY(rotAngle);
            moverse(elapsedTime, terreno);
        }

        public override string GetTipo()
        {
            return Animal;
        }

        #endregion Comportamientos
    }
}