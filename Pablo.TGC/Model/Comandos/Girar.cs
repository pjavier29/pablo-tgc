using Microsoft.DirectX.Direct3D;
using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Comandos
{
    public class Girar : Comando
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
            float rotate;

            if (MovimientoRapido)
            {
                rotate = sentido * contexto.personaje.rotarRapido();
            }
            else
            {
                rotate = sentido * contexto.personaje.VelocidadRotacion;
            }

            //Rotar personaje, hay que multiplicarlo por el tiempo transcurrido para no atarse a la velocidad el hardware
            var rotAngle = Geometry.DegreeToRadian(rotate * elapsedTime);
            contexto.personaje.mesh.rotateY(rotAngle);
            contexto.personaje.ActualizarEsferas();
        }

        #endregion Comportamientos

        #region Constructores

        public Girar(float sentido)
        {
            this.sentido = sentido;
            MovimientoRapido = false;
        }

        public Girar(float sentido, bool rapido)
        {
            this.sentido = sentido;
            MovimientoRapido = rapido;
        }

        #endregion Constructores
    }
}