using AlumnoEjemplos.PabloTGC.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.PabloTGC.ElementosDia
{
    public class Dia
    {
        #region Atributos
        private Tiempo tiempo;
        private Sol sol;
        private Lluvia lluvia;
        private float velocidadTiempo;
        private double relojInterno;//Son segundos virtuales
        #endregion

        #region Propiedades
        #endregion

        #region Constructores
        public Dia(float velocidadTiempo, Sol sol, Lluvia lluvia)
        {
            this.velocidadTiempo = velocidadTiempo;
            this.tiempo = new Tiempo();
            this.tiempo.CalcularTemperaturaDeDia();
            this.relojInterno = 0;
            this.sol = sol;
            this.lluvia = lluvia;
        }
        public Dia(float velocidadTiempo, Sol sol, float relojInterno, Lluvia lluvia)
        {
            this.velocidadTiempo = velocidadTiempo;
            this.tiempo = new Tiempo();
            this.tiempo.CalcularTemperaturaDeDia();
            this.relojInterno = relojInterno;
            this.sol = sol;
            this.lluvia = lluvia;
        }
        #endregion

        #region Comportamientos
        public void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
            this.ActualizarRelojInterno(elapsedTime);
            this.sol.Actualizar(this.GetAnguloSegunSegundos());
        }

        private float GetAnguloSegunSegundos()
        {
            return (float)(((this.relojInterno * FastMath.PI * 2) / 86400) - FastMath.PI / 2);
        }

        public int HoraActual()
        {
            return (int)Math.Floor(this.relojInterno / 3600);
        }

        public int Minutoactual()
        {
            return (int)((this.relojInterno % 3600) / 60);
        }

        public String HoraActualTexto()
        {
            return this.HoraActual().ToString("D2") + ":" + this.Minutoactual().ToString("D2");
        }

        public float TemperaturaActual()
        {
            return this.tiempo.TemperaturaActualPorHora(this.HoraActual());
        }

        public String TemperaturaActualTexto()
        {
            return this.TemperaturaActual().ToString() + "°";
        }

        private void ActualizarRelojInterno(float elapsedTime)
        {
            //Si el elapsedTime es mayor de 2 segundos, solo tenemos en cuenta 2 segundos sino el reloj se torna inestable
            //Esto viene muy bien en la etapa de configuración, que viene un elapsedTime muy grande.
            if (elapsedTime > 1f)
            {
                //El reloj interno lo interpretamos en segundos
                this.relojInterno += 1f * this.velocidadTiempo;
            }
            else
            {
                //El reloj interno lo interpretamos en segundos
                this.relojInterno += elapsedTime * this.velocidadTiempo;
            }

            //Si pasaron mas de 86400 segundos quiere decir que el dia termino.
            if (this.relojInterno > 86400)
            {
                this.relojInterno = 0;
                this.tiempo.CalcularTemperaturaDeDia();
                this.lluvia.Actualizar();
                if (this.lluvia.EstaLloviendo())
                {
                    this.sol.ActualizarIntensidadMaximaLuz(8000f);
                }
                else
                {
                    this.sol.ActualizarIntensidadMaximaLuz(15000f);
                }
            }
        }

        public Sol GetSol()
        {
            return this.sol;
        }

        public bool EsDeDia()
        {
            return this.sol.EsDeDia();
        }

        public Lluvia GetLluvia()
        {
            return this.lluvia;
        }
        #endregion
    }
}
