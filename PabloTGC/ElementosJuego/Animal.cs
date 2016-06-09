using AlumnoEjemplos.MiGrupo;
using AlumnoEjemplos.PabloTGC.Utiles;
using AlumnoEjemplos.PabloTGC.Utiles.Efectos;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC
{
    public class Animal : Elemento
    {
        #region Atributos
        private float tiempoEnActividad;
        private float tiempoInactivo;
        private float tiempo;
        private float velocidadCaminar;
        private float velocidadRotar;
        private String movimientoActual;
        #endregion

        #region Contructores
        public Animal(float peso, float resistencia, TgcMesh mesh, Efecto efecto) :base(peso, resistencia, mesh, efecto)
        {
            this.tiempoEnActividad = 7;
            this.tiempoInactivo = 3;
            this.tiempo = 0;
            this.velocidadCaminar = 30f;
            this.velocidadRotar = 10F;
            this.movimientoActual = "Caminar";
        }
        #endregion

        #region Comportamientos
        public override void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
            base.Actualizar(contexto, elapsedTime);
            tiempo += elapsedTime;
            if (tiempo < tiempoEnActividad)
            {
                this.simularMovimiento(elapsedTime, contexto.terreno);
                //TODO. Colocar animación de caminar
            }
            else
            {
                //TODO. Colocar animacion de comer pasto
                if (tiempo > tiempoEnActividad + tiempoInactivo)
                {
                    tiempo = 0;
                    double aleatorioActual = FuncionesMatematicas.Instance.NumeroAleatorioDouble();
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
            this.ActualizarBarraEstadoCompleta(this.Mesh.BoundingBox.PMin, new Vector3(this.Mesh.BoundingBox.PMin.X, this.Mesh.BoundingBox.PMax.Y, this.Mesh.BoundingBox.PMin.Z));
        }

        private void simularMovimiento(float elapsedTime, Terreno terreno)
        {
            if (movimientoActual.Equals("Caminar"))
            {
                this.moverse(elapsedTime, terreno);
            }

            if (movimientoActual.Equals("CaminarDerecha"))
            {
                this.moverseRotando(elapsedTime, terreno, 1);
            }

            if (movimientoActual.Equals("CaminarIzquierda"))
            {
                this.moverseRotando(elapsedTime, terreno, -1);
            }

        }

        private void moverse(float elapsedTime, Terreno terreno)
        {
            //Aplicamos el movimiento
            //TODO Ver si es correcta la forma que aplico para representar que se esta a la altura del terreno.
            float xm = FastMath.Sin(this.Mesh.Rotation.Y) * velocidadCaminar;
            float zm = FastMath.Cos(this.Mesh.Rotation.Y) * velocidadCaminar;
            Vector3 movementVector = new Vector3(xm, 0, zm);
            this.Mesh.move(movementVector * elapsedTime);
            this.Mesh.Position = new Vector3(this.Mesh.Position.X, terreno.CalcularAltura(this.Mesh.Position.X, this.Mesh.Position.Z), this.Mesh.Position.Z);
        }

        private void moverseRotando(float elapsedTime, Terreno terreno, int direccion)
        {
            float rotAngle = Geometry.DegreeToRadian(direccion * velocidadRotar * elapsedTime);
            this.Mesh.rotateY(rotAngle);
            this.moverse(elapsedTime, terreno);
        }

        public override String GetTipo()
        {
            return Animal;
        }

        #endregion
    }
}
