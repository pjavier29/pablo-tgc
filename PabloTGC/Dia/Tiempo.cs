using AlumnoEjemplos.PabloTGC.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.PabloTGC.Dia
{
    public class Tiempo
    {
        #region Atributos
        private int temperaturaMinimaPronostico;
        private int temperaturaMaximaPronostico;
        private int horaUltimoCalculo;
        #endregion

        #region Propiedades
        public int TemperaturaMinima { get; set; }
        public int TemperaturaMaxima { get; set; }
        public int TemperaturaActual { get; set; }
        #endregion

        #region Constructores
        public Tiempo()
        {
            this.TemperaturaMinima = 0;
            this.TemperaturaMaxima = 0;
            this.TemperaturaActual = 0;
            this.horaUltimoCalculo = -1;
            this.temperaturaMinimaPronostico = 14;//Estos datos deberian venir del pronostico del tiempo
            this.temperaturaMaximaPronostico = 30;//Estos datos deberian venir del pronostico del tiempo
        }
        #endregion

        #region Comportamientos
        public void CalcularTemperaturaDeDia()
        {
            this.TemperaturaMinima = this.temperaturaMinimaPronostico + FuncionesMatematicas.Instance.NumeroAleatorioIntEntre(-5, 5);
            this.TemperaturaMaxima = this.temperaturaMaximaPronostico + FuncionesMatematicas.Instance.NumeroAleatorioIntEntre(-5, 5);
            this.horaUltimoCalculo = -1;
        }

        private void CalcularTemperaturaDeHora(int hora)
        {
            this.horaUltimoCalculo = hora;
            if (hora <= 12)
            {
                this.TemperaturaActual = (((this.TemperaturaMaxima - this.TemperaturaMinima) / 12) * hora) + this.TemperaturaMinima;
            }
            else
            {
                this.TemperaturaActual = (((this.TemperaturaMaxima - this.TemperaturaMinima) / 12) * (24 - hora)) + this.TemperaturaMinima;
            }
        }

        public int TemperaturaActualPorHora(int hora)
        {
            if (this.horaUltimoCalculo != hora)
            {
                this.CalcularTemperaturaDeHora(hora);
            }
            return this.TemperaturaActual;
        }
        #endregion
    }
}
