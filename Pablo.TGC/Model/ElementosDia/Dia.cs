using System;
using TGC.Core.Utils;
using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.ElementosDia
{
    public class Dia
    {
        #region Atributos

        private readonly Tiempo tiempo;
        private readonly Sol sol;
        private readonly Lluvia lluvia;
        private readonly float velocidadTiempo;
        private double relojInterno; //Son segundos virtuales

        #endregion Atributos

        #region Constructores

        public Dia(float velocidadTiempo, Sol sol, Lluvia lluvia)
        {
            this.velocidadTiempo = velocidadTiempo;
            tiempo = new Tiempo();
            tiempo.CalcularTemperaturaDeDia();
            relojInterno = 0;
            this.sol = sol;
            this.lluvia = lluvia;
        }

        public Dia(float velocidadTiempo, Sol sol, float relojInterno, Lluvia lluvia)
        {
            this.velocidadTiempo = velocidadTiempo;
            tiempo = new Tiempo();
            tiempo.CalcularTemperaturaDeDia();
            this.relojInterno = relojInterno;
            this.sol = sol;
            this.lluvia = lluvia;
        }

        #endregion Constructores

        #region Comportamientos

        public void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
            ActualizarRelojInterno(elapsedTime, contexto);
            sol.Actualizar(GetAnguloSegunSegundos());
        }

        private float GetAnguloSegunSegundos()
        {
            return (float)(relojInterno * FastMath.PI * 2 / 86400 - FastMath.PI / 2);
        }

        public int HoraActual()
        {
            return (int)Math.Floor(relojInterno / 3600);
        }

        public int Minutoactual()
        {
            return (int)(relojInterno % 3600 / 60);
        }

        public string HoraActualTexto()
        {
            return HoraActual().ToString("D2") + ":" + Minutoactual().ToString("D2");
        }

        public float TemperaturaActual()
        {
            return tiempo.TemperaturaActualPorHora(HoraActual());
        }

        public string TemperaturaActualTexto()
        {
            return TemperaturaActual() + "°";
        }

        private void ActualizarRelojInterno(float elapsedTime, SuvirvalCraft contexto)
        {
            //Si el elapsedTime es mayor de 0.5 segundos, solo tenemos en cuenta 0,5 segundos sino el reloj se torna inestable
            //Esto viene muy bien en la etapa de configuración, que viene un elapsedTime muy grande.
            if (elapsedTime > 0.5f)
            {
                //El reloj interno lo interpretamos en segundos
                relojInterno += 0.5f * velocidadTiempo;
            }
            else
            {
                //El reloj interno lo interpretamos en segundos
                relojInterno += elapsedTime * velocidadTiempo;
            }

            //Si pasaron mas de 86400 segundos quiere decir que el dia termino.
            if (relojInterno > 86400)
            {
                relojInterno = 0;
                tiempo.CalcularTemperaturaDeDia();
                lluvia.Actualizar(contexto);
                if (lluvia.EstaLloviendo())
                {
                    sol.ActualizarIntensidadMaximaLuz(500f);
                }
                else
                {
                    sol.ActualizarIntensidadMaximaLuz(1000f);
                }
            }
        }

        public Sol GetSol()
        {
            return sol;
        }

        public bool EsDeDia()
        {
            return sol.EsDeDia();
        }

        public Lluvia GetLluvia()
        {
            return lluvia;
        }

        #endregion Comportamientos
    }
}