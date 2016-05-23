using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.PabloTGC.Utiles
{
    public class BarraEstado
    {
        #region Atributos
        private TgcArrow linea;
        private bool progreso;
        #endregion

        #region Propiedades
        public Vector3 PuntoMinimo { get; set; }
        public Vector3 PuntoMaximo { get; set; }
        public float ValorMaximo { get; set; }
        #endregion

        #region Contructores
        public BarraEstado(Vector3 puntoMinimo, Vector3 puntoMaximo, float valorMaximo)
        {
            this.progreso = false;//Por defecto es una barra de regreso (va del 100% al 0%)
            this.Iniciar(puntoMinimo, puntoMaximo, valorMaximo);
        }

        public BarraEstado(Vector3 puntoMinimo, Vector3 puntoMaximo, float valorMaximo, bool progreso)
        {
            this.progreso = progreso;
            this.Iniciar(puntoMinimo, puntoMaximo, valorMaximo);
        }

        private void Iniciar(Vector3 puntoMinimo, Vector3 puntoMaximo, float valorMaximo)
        {
            this.PuntoMinimo = puntoMinimo;
            this.PuntoMaximo = puntoMaximo;
            this.ValorMaximo = valorMaximo;
            this.linea = new TgcArrow();
            this.linea.PStart = puntoMinimo;
            this.linea.PEnd = puntoMaximo;
            this.linea.BodyColor = Color.Green;
            this.linea.HeadColor = Color.Green;
            this.linea.Thickness = 3;
            this.linea.HeadSize = new Vector2(1, 1);
        }
        #endregion

        #region Comportamientos

        public void ActualizarPuntosBase(Vector3 puntoMinimo, Vector3 puntoMaximo)
        {
            this.PuntoMinimo = puntoMinimo;
            this.PuntoMaximo = puntoMaximo;
            this.linea.PStart = puntoMinimo;
            this.linea.PEnd = puntoMaximo;
        }

        public void Render()
        {
            this.linea.updateValues();
            this.linea.render();
        }

        public void ActualizarEstado(float valorActual)
        {
            float porcentajerelativo = valorActual / this.ValorMaximo;
            this.ActualizarAltura(porcentajerelativo);
            this.ActualizarColor(porcentajerelativo);
        }

        private void ActualizarAltura(float porcentajerelativo)
        {
            float nuevoLargo = this.LargoBarra() * porcentajerelativo;
            Vector3 nuevoDestino = new Vector3(this.PuntoMaximo.X - this.PuntoMinimo.X, this.PuntoMaximo.Y - this.PuntoMinimo.Y, this.PuntoMaximo.Z - this.PuntoMinimo.Z);
            nuevoDestino.Normalize();
            nuevoDestino.Multiply(nuevoLargo);
            this.linea.PEnd = this.linea.PStart + nuevoDestino;
        }

        private void ActualizarColor(float porcentajerelativo)
        {
            Color nuevoColor;
            if (porcentajerelativo > 0.7f)
            {
                nuevoColor = Color.Green;
            }
            else
            {
                if (porcentajerelativo > 0.5f)
                {
                    nuevoColor = Color.GreenYellow;
                }
                else
                {
                    if (porcentajerelativo > 0.2f)
                    {
                        nuevoColor = Color.Yellow;
                    }
                    else
                    {
                        nuevoColor = Color.Red;
                    }
                }
            }
            this.linea.BodyColor = nuevoColor;
            this.linea.HeadColor = nuevoColor;
        }

        public float LargoBarra()
        {
            return FuncionesMatematicas.Instance.DistanciaEntrePuntos(this.PuntoMinimo, this.PuntoMaximo);
        }

        public void Liberar()
        {
            this.linea.dispose();
        }

        #endregion
    }
}
