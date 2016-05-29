using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Terrain;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class Girar : Comando
    {
        #region Atributos
        private float sentido;
        #endregion

        #region Propiedades
        public bool MovimientoRapido { get; set; }
        #endregion

        #region Constructores
        public Girar(float sentido)
        {
            this.sentido = sentido;
            this.MovimientoRapido = false;
        }

        public Girar(float sentido, bool rapido)
        {
            this.sentido = sentido;
            this.MovimientoRapido = rapido;
        }
        #endregion

        #region Comportamientos
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            float rotate;

            if (MovimientoRapido)
            {
                rotate = this.sentido * contexto.personaje.rotarRapido();
            }
            else
            {
                rotate = this.sentido * contexto.personaje.VelocidadRotacion;
            }

            //Rotar personaje y la camara, hay que multiplicarlo por el tiempo transcurrido para no atarse a la velocidad el hardware
            float rotAngle = Geometry.DegreeToRadian(rotate * elapsedTime);
            contexto.personaje.mesh.rotateY(rotAngle);
            GuiController.Instance.ThirdPersonCamera.rotateY(rotAngle);

            contexto.personaje.ActualizarEsferas();
        }
        #endregion
    }

}
