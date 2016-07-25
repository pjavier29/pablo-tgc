using System.Collections.Generic;
using Microsoft.DirectX;
using TGC.Group.Model.ElementosJuego;

namespace TGC.Group.Model.Utiles
{
    public class Optimizador
    {
        #region Atributos

        private int referenciaActualizacion;
        private float distanciaColision;
        private int cicloActual;

        #endregion Atributos

        #region Propiedades

        public List<Elemento> Elementos { get; set; }
        public List<Elemento> ElementosColision { get; set; }
        public List<Elemento> ElementosRenderizacion { get; set; }

        #endregion Propiedades

        #region Constructores

        public Optimizador(List<Elemento> elementos, int referenciaActualizacion, float distanciaColision)
        {
            this.ElementosColision = new List<Elemento>();
            this.ElementosRenderizacion = new List<Elemento>();
            this.Elementos = elementos;
            this.referenciaActualizacion = referenciaActualizacion;
            this.distanciaColision = distanciaColision;
            this.cicloActual = referenciaActualizacion;// Lo inicializamos en la referencia para que se ejecute la primera vez que se invoca al Actualizar
        }

        #endregion Constructores

        #region Comportamientos

        public void Actualizar(Vector3 posicionActual)
        {
            this.cicloActual++;
            if (this.cicloActual > this.referenciaActualizacion)
            {
                this.cicloActual = 0;
                this.ActualizarElementosColision(posicionActual);
            }
            this.ActualizarElementosRenderizacion();
        }

        private void ActualizarElementosRenderizacion()
        {
            this.ElementosRenderizacion.Clear();
            foreach (Elemento elem in this.Elementos)
            {
                if (ControladorColisiones.FrustumColisionaCuadrado(GuiController.Instance.Frustum, elem.BoundingBox()))
                {
                    this.ElementosRenderizacion.Add(elem);
                }
            }
        }

        private void ActualizarElementosColision(Vector3 posicionActual)
        {
            this.ElementosColision.Clear();
            foreach (Elemento elem in this.Elementos)
            {
                if (elem.distanciaA(posicionActual) < this.distanciaColision)
                {
                    //Quiere decir que tenemos que tener en cuenta este elemento para posibles colisiones
                    this.ElementosColision.Add(elem);
                }
            }
        }

        /// <summary>
        /// Fuerza la actualizacion del optimizador en el próximo ciclo de ejecución
        /// </summary>
        public void ForzarActualizacionElementosColision()
        {
            this.cicloActual = this.referenciaActualizacion;
        }

        #endregion Comportamientos
    }
}