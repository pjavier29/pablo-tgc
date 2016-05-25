using AlumnoEjemplos.MiGrupo;
using AlumnoEjemplos.PabloTGC.Utiles;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class Mover : Comando
    {
        #region Atributos
        private float sentido;
        #endregion

        #region Propiedades
        public bool MovimientoRapido { get; set; }
        #endregion

        #region Constructores
        public Mover(float sentido)
        {
            this.sentido = sentido;
            this.MovimientoRapido = false;
        }

        public Mover(float sentido, bool rapido)
        {
            this.sentido = sentido;
            this.MovimientoRapido = rapido;
        }
        #endregion

        #region Comportamientos
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            float movimiento;
            //Aplicar movimiento hacia adelante o atras segun la orientacion actual del Mesh
            Vector3 lastPos = contexto.personaje.mesh.Position;

            if (MovimientoRapido)
            {
                movimiento = this.sentido * contexto.personaje.correr(elapsedTime);
            }
            else
            {
                movimiento = this.sentido* contexto.personaje.velocidadCaminar;
            }

            //Aplicamos el movimiento
            float xm = FastMath.Sin(contexto.personaje.mesh.Rotation.Y) * movimiento;
            float zm = FastMath.Cos(contexto.personaje.mesh.Rotation.Y) * movimiento;
            Vector3 movementVector = new Vector3(xm, 0, zm);
            contexto.personaje.mesh.move(movementVector * elapsedTime);
            contexto.personaje.mesh.Position = new Vector3(contexto.personaje.mesh.Position.X,
                contexto.terreno.CalcularAltura(contexto.personaje.mesh.Position.X, contexto.personaje.mesh.Position.Z), contexto.personaje.mesh.Position.Z);

            contexto.personaje.ActualizarEsferas();

            //Detectar colisiones
            bool collide = false;
            foreach (Elemento elem in contexto.elementos)
            {
                if (ControladorColisiones.EsferaColisionaCuadrado(contexto.personaje.GetBoundingEsfera(), elem.BoundingBox()))
                {
                    collide = true;
                    elem.procesarColision(contexto.personaje, elapsedTime, contexto.elementos, movimiento, movementVector, lastPos);
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
                //Si no hubo colisiones y el personaje se movio finalmente
                contexto.skyBox.Center += new Vector3((contexto.personaje.mesh.Position.X - lastPos.X), 0, (contexto.personaje.mesh.Position.Z - lastPos.Z));
                contexto.skyBox.updateValues();
                //TODO. Ver si es la mejor forma de hacer que el piso sea infinito
                contexto.piso.Position += new Vector3((contexto.personaje.mesh.Position.X - lastPos.X), 0, (contexto.personaje.mesh.Position.Z - lastPos.Z));
                contexto.piso.updateValues();
            }
        }
        #endregion
    }
}
