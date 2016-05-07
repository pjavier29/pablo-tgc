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
    public class Animal : Obstaculo
    {
        private float tiempoEnActividad;
        private float tiempoInactivo;
        private float tiempo;
        private float velocidadCaminar;
        private float velocidadRotar;
        private String movimientoActual;
        private Random aleatorio;

        public Animal(float peso, float resistencia, TgcMesh mesh) :base(peso, resistencia, mesh)
        {
            this.tiempoEnActividad = 7;
            this.tiempoInactivo = 3;
            this.tiempo = 0;
            aleatorio = new Random();
            this.velocidadCaminar = 30f;
            this.velocidadRotar = 10F;
            this.movimientoActual = "Caminar";
        }

        public void update(float elapsedTime, Terreno terreno)
        {
            tiempo += elapsedTime;
            if (tiempo < tiempoEnActividad)
            {
                this.simularMovimiento(elapsedTime, terreno);
                //TODO. Colocar animación de caminar
            }
            else
            {
                //TODO. Colocar animacion de comer pasto
                if (tiempo > tiempoEnActividad + tiempoInactivo)
                {
                    tiempo = 0;
                    double aleatorioActual = aleatorio.NextDouble();
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
            float xm = FastMath.Sin(this.mesh.Rotation.Y) * velocidadCaminar;
            float zm = FastMath.Cos(this.mesh.Rotation.Y) * velocidadCaminar;
            Vector3 movementVector = new Vector3(xm, 0, zm);
            this.mesh.move(movementVector * elapsedTime);
            this.mesh.Position = new Vector3(this.mesh.Position.X, terreno.CalcularAltura(this.mesh.Position.X, this.mesh.Position.Z), this.mesh.Position.Z);
        }

        private void moverseRotando(float elapsedTime, Terreno terreno, int direccion)
        {
            float rotAngle = Geometry.DegreeToRadian(direccion * velocidadRotar * elapsedTime);
            this.mesh.rotateY(rotAngle);
            this.moverse(elapsedTime, terreno);
        }
    }
}
