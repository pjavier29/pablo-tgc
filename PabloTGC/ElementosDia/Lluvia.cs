using AlumnoEjemplos.PabloTGC.Administracion;
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
        public bool estaLloviendo;
        private float momentoUltimoRayo;
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
            this.momentoUltimoRayo = 0;
        }
        #endregion

        #region Comportamientos
        /// <summary>
        /// Método que debería ser invocado una sola vez por dia.
        /// </summary>
        public void Actualizar(SuvirvalCraft contexto)
        {
            this.estaLloviendo = false;
            this.momentoUltimoRayo = 0;
            contexto.sonidoLluvia.stop();
            if (FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(0, LapsoPrecipitaciones) < this.probabilidadLluvia)
            {
                this.estaLloviendo = true;
                this.probabilidadLluvia = 0;
                contexto.sonidoLluvia.play(true);
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

        public float GetIntensidadRayo(float tiempo)
        {
            if (this.EstaLloviendo())
            {
                if (this.momentoUltimoRayo == 0)
                {
                    if (FuncionesMatematicas.Instance.NumeroAleatorioIntEntre(0, 1000) < 2)
                    {
                        this.momentoUltimoRayo = tiempo;
                    }
                }
                else
                {
                    float momento = tiempo - this.momentoUltimoRayo;
                    if (momento < 0.15f)
                    {
                        return 0.3f;
                    }
                    if (momento > 0.85f && momento < 1f)
                    {
                        return 0.3f;
                    }
                    if (momento > 1f)
                    {
                        this.momentoUltimoRayo = 0; ;
                    }
                }
            }
            return 0;
        }
        #endregion
    }
}
