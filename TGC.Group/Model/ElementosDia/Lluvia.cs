using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles;

namespace TGC.Group.Model.ElementosDia
{
    public class Lluvia
    {
        #region Constructores

        public Lluvia(float incrementador, float lapsoPrecipitaciones)
        {
            IncrementadorProbabilidad = incrementador;
            LapsoPrecipitaciones = lapsoPrecipitaciones;
            probabilidadLluvia = 0; //Al principio nunca llueve!!!
            estaLloviendo = false;
            momentoUltimoRayo = 0;
        }

        #endregion Constructores

        #region Atributos

        private float probabilidadLluvia;
        public bool estaLloviendo;
        private float momentoUltimoRayo;

        #endregion Atributos

        #region Propiedades

        public float IncrementadorProbabilidad { get; set; }
        public float LapsoPrecipitaciones { get; set; }

        #endregion Propiedades

        #region Comportamientos

        /// <summary>
        ///     Método que debería ser invocado una sola vez por dia.
        /// </summary>
        public void Actualizar(SuvirvalCraft contexto)
        {
            estaLloviendo = false;
            momentoUltimoRayo = 0;
            contexto.sonidoLluvia.stop();
            if (FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(0, LapsoPrecipitaciones) < probabilidadLluvia)
            {
                estaLloviendo = true;
                probabilidadLluvia = 0;
                contexto.sonidoLluvia.play(true);
            }
            else
            {
                //Si no esta lloviendo incremento la probabilidad para tener más chances después
                probabilidadLluvia += IncrementadorProbabilidad;
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
            return estaLloviendo;
        }

        public float GetIntensidadRayo(float tiempo)
        {
            if (EstaLloviendo())
            {
                if (momentoUltimoRayo == 0)
                {
                    if (FuncionesMatematicas.Instance.NumeroAleatorioIntEntre(0, 1000) < 2)
                    {
                        momentoUltimoRayo = tiempo;
                    }
                }
                else
                {
                    var momento = tiempo - momentoUltimoRayo;
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
                        momentoUltimoRayo = 0;
                        ;
                    }
                }
            }
            return 0;
        }

        #endregion Comportamientos
    }
}