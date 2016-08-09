using Microsoft.DirectX;
using System.Collections.Generic;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.ElementosJuego;

namespace TGC.Group.Model.Utiles
{
    public class Optimizador
    {
        #region Constructores

        public Optimizador(List<Elemento> elementos, int referenciaActualizacion, float distanciaColision)
        {
            ElementosColision = new List<Elemento>();
            ElementosRenderizacion = new List<Elemento>();
            Elementos = elementos;
            this.referenciaActualizacion = referenciaActualizacion;
            this.distanciaColision = distanciaColision;
            cicloActual = referenciaActualizacion;
            // Lo inicializamos en la referencia para que se ejecute la primera vez que se invoca al Actualizar
        }

        #endregion Constructores

        #region Atributos

        private readonly int referenciaActualizacion;
        private readonly float distanciaColision;
        private int cicloActual;

        #endregion Atributos

        #region Propiedades

        public List<Elemento> Elementos { get; set; }
        public List<Elemento> ElementosColision { get; set; }
        public List<Elemento> ElementosRenderizacion { get; set; }

        #endregion Propiedades

        #region Comportamientos

        public void Actualizar(Vector3 posicionActual, SuvirvalCraft contexto)
        {
            cicloActual++;
            if (cicloActual > referenciaActualizacion)
            {
                cicloActual = 0;
                ActualizarElementosColision(posicionActual);
            }
            ActualizarElementosRenderizacion(contexto);
        }

        private void ActualizarElementosRenderizacion(SuvirvalCraft contexto)
        {
            ElementosRenderizacion.Clear();
            foreach (var elem in Elementos)
            {
                if (ControladorColisiones.FrustumColisionaCuadrado(contexto.Frustum, elem.BoundingBox()))
                {
                    ElementosRenderizacion.Add(elem);
                }
            }
        }

        private void ActualizarElementosColision(Vector3 posicionActual)
        {
            ElementosColision.Clear();
            foreach (var elem in Elementos)
            {
                if (elem.distanciaA(posicionActual) < distanciaColision)
                {
                    //Quiere decir que tenemos que tener en cuenta este elemento para posibles colisiones
                    ElementosColision.Add(elem);
                }
            }
        }

        /// <summary>
        ///     Fuerza la actualizacion del optimizador en el próximo ciclo de ejecución
        /// </summary>
        public void ForzarActualizacionElementosColision()
        {
            cicloActual = referenciaActualizacion;
        }

        #endregion Comportamientos
    }
}