using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.PabloTGC.Utiles
{
    public class Optimizador
    {
        #region Atributos
        private int referenciaActualizacion;
        private float distanciaColision;
        private int cicloActual;
        #endregion

        #region Propiedades
        public List<Elemento> Elementos { get; set; }
        public List<Elemento> ElementosColision { get; set; }
        #endregion

        #region Constructores
        public Optimizador(List<Elemento> elementos, int referenciaActualizacion, float distanciaColision)
        {
            this.ElementosColision = new List<Elemento>();
            this.Elementos = elementos;
            this.referenciaActualizacion = referenciaActualizacion;
            this.distanciaColision = distanciaColision;
            this.cicloActual = referenciaActualizacion;// Lo inicializamos en la referencia para que se ejecute la primera vez que se invoca al Actualizar
        }
        #endregion

        #region Comportamientos
        public void Renderizar()
        {
            foreach (Elemento elem in this.Elementos)
            {
                if (ControladorColisiones.FrustumColisionaCuadrado(GuiController.Instance.Frustum, elem.BoundingBox()))
                {
                    elem.renderizar();
                }
            }
        }

        public void Actualizar(Vector3 posicionActual)
        {
            this.cicloActual++;
            if (this.cicloActual > this.referenciaActualizacion)
            {
                this.cicloActual = 0;
                this.ActualizarElementosColision(posicionActual);
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
        public void ForzarActualizacion()
        {
            this.cicloActual = this.referenciaActualizacion;
        }
        #endregion
    }
}
