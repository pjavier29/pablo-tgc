using AlumnoEjemplos.PabloTGC.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.PabloTGC.ElementosDia
{
    public class Lluvia
    {
        #region Atributos
        private float probabilidadLluvia;
        private bool estaLloviendo;
        #endregion

        #region Propiedades
        public float IncrementadorProbabilidad { get; set; }
        public float LapsoPrecipitaciones { get; set; }
        #endregion

        #region Constructores
        public Lluvia(float incrementador, float lapsoPrecipitaciones)
        {
            this.IncrementadorProbabilidad = incrementador;
            this.LapsoPrecipitaciones = lapsoPrecipitaciones;
            this.probabilidadLluvia = 0;//Al principio nunca llueve!!!
            this.estaLloviendo = false;
        }
        #endregion

        #region Comportamientos
        /// <summary>
        /// Método que debería ser invocado una sola vez por dia.
        /// </summary>
        public void Actualizar()
        {
            this.estaLloviendo = false;
            if (FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(0, LapsoPrecipitaciones) < this.probabilidadLluvia)
            {
                this.estaLloviendo = true;
                this.probabilidadLluvia = 0;
            }
            else
            {
                //Si no esta lloviendo incremento la probabilidad para tener más chances después
                this.probabilidadLluvia += this.IncrementadorProbabilidad;
            }
        }

        public float AnchoLluvia()
        {
            return FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(150, 300);
        }

        public float AltoLluvia()
        {
            return FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(20, 60);
        }

        public bool EstaLloviendo()
        {
            return this.estaLloviendo;
        }
        #endregion
    }
}
