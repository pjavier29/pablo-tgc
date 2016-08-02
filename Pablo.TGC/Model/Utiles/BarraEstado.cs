using Microsoft.DirectX;
using System.Drawing;
using TGC.Core.Geometry;

namespace TGC.Group.Model.Utiles
{
    public class BarraEstado
    {
        #region Atributos

        private TgcArrow linea;

        #endregion Atributos

        #region Propiedades

        public Vector3 PuntoMinimo { get; set; }
        public Vector3 PuntoMaximo { get; set; }
        public float ValorMaximo { get; set; }

        #endregion Propiedades

        #region Contructores

        public BarraEstado(Vector3 puntoMinimo, Vector3 puntoMaximo, float valorMaximo)
        {
            Iniciar(puntoMinimo, puntoMaximo, valorMaximo);
        }

        private void Iniciar(Vector3 puntoMinimo, Vector3 puntoMaximo, float valorMaximo)
        {
            PuntoMinimo = puntoMinimo;
            PuntoMaximo = puntoMaximo;
            ValorMaximo = valorMaximo;
            linea = new TgcArrow();
            linea.PStart = puntoMinimo;
            linea.PEnd = puntoMaximo;
            linea.BodyColor = Color.Green;
            linea.HeadColor = Color.Green;
            linea.Thickness = 3;
            linea.HeadSize = new Vector2(1, 1);
        }

        #endregion Contructores

        #region Comportamientos

        public void ActualizarPuntosBase(Vector3 puntoMinimo, Vector3 puntoMaximo)
        {
            PuntoMinimo = puntoMinimo;
            PuntoMaximo = puntoMaximo;
            linea.PStart = puntoMinimo;
            linea.PEnd = puntoMaximo;
        }

        public void Render()
        {
            linea.updateValues();
            linea.render();
        }

        public void ActualizarEstado(float valorActual)
        {
            var porcentajerelativo = valorActual / ValorMaximo;
            ActualizarAltura(porcentajerelativo);
            ActualizarColor(porcentajerelativo);
        }

        private void ActualizarAltura(float porcentajerelativo)
        {
            var nuevoLargo = LargoBarra() * porcentajerelativo;
            var nuevoDestino = new Vector3(PuntoMaximo.X - PuntoMinimo.X, PuntoMaximo.Y - PuntoMinimo.Y,
                PuntoMaximo.Z - PuntoMinimo.Z);
            nuevoDestino.Normalize();
            nuevoDestino.Multiply(nuevoLargo);
            linea.PEnd = linea.PStart + nuevoDestino;
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
            linea.BodyColor = nuevoColor;
            linea.HeadColor = nuevoColor;
        }

        public float LargoBarra()
        {
            return FuncionesMatematicas.Instance.DistanciaEntrePuntos(PuntoMinimo, PuntoMaximo);
        }

        public void Liberar()
        {
            linea.dispose();
        }

        #endregion Comportamientos
    }
}