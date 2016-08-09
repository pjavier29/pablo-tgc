using TGC.Group.Model.Utiles;

namespace TGC.Group.Model.ElementosDia
{
    public class Tiempo
    {
        #region Constructores

        public Tiempo()
        {
            TemperaturaMinima = 0;
            TemperaturaMaxima = 0;
            TemperaturaActual = 0;
            horaUltimoCalculo = -1;
            temperaturaMinimaPronostico = 14; //Estos datos deberian venir del pronostico del tiempo
            temperaturaMaximaPronostico = 30; //Estos datos deberian venir del pronostico del tiempo
        }

        #endregion Constructores

        #region Atributos

        private readonly int temperaturaMinimaPronostico;
        private readonly int temperaturaMaximaPronostico;
        private int horaUltimoCalculo;

        #endregion Atributos

        #region Propiedades

        public int TemperaturaMinima { get; set; }
        public int TemperaturaMaxima { get; set; }
        public int TemperaturaActual { get; set; }

        #endregion Propiedades

        #region Comportamientos

        public void CalcularTemperaturaDeDia()
        {
            TemperaturaMinima = temperaturaMinimaPronostico +
                                FuncionesMatematicas.Instance.NumeroAleatorioIntEntre(-5, 5);
            TemperaturaMaxima = temperaturaMaximaPronostico +
                                FuncionesMatematicas.Instance.NumeroAleatorioIntEntre(-5, 5);
            horaUltimoCalculo = -1;
        }

        private void CalcularTemperaturaDeHora(int hora)
        {
            horaUltimoCalculo = hora;
            if (hora <= 12)
            {
                TemperaturaActual = (TemperaturaMaxima - TemperaturaMinima) / 12 * hora + TemperaturaMinima;
            }
            else
            {
                TemperaturaActual = (TemperaturaMaxima - TemperaturaMinima) / 12 * (24 - hora) + TemperaturaMinima;
            }
        }

        public int TemperaturaActualPorHora(int hora)
        {
            if (horaUltimoCalculo != hora)
            {
                CalcularTemperaturaDeHora(hora);
            }
            return TemperaturaActual;
        }

        #endregion Comportamientos
    }
}