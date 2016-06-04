using AlumnoEjemplos.MiGrupo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.PabloTGC.Dia
{
    public class Dia
    {
        #region Atributos
        private Tiempo tiempo;
        private Sol sol;
        private float velocidadTiempo;
        private double relojInterno;//Son segundos virtuales
        #endregion

        #region Propiedades
        #endregion

        #region Constructores
        public Dia(float velocidadTiempo, Sol sol)
        {
            this.velocidadTiempo = velocidadTiempo;
            this.tiempo = new Tiempo();
            this.tiempo.CalcularTemperaturaDeDia();
            this.relojInterno = 0;
            this.sol = sol;
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
            //El reloj interno lo interpretamos en segundos
            this.relojInterno += elapsedTime * this.velocidadTiempo;
            //Si pasaron mas de 86400 segundos quiere decir que el dia termino.
            if (this.relojInterno > 86400)
            {
                this.relojInterno = 0;
                this.tiempo.CalcularTemperaturaDeDia();
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
        #endregion
    }
}
