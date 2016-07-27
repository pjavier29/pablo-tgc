using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Comandos
{
    public class Seleccionar : Comando
    {
        #region Atributos

        private readonly int numeroSeleccionado;

        #endregion Atributos

        #region Constructores

        public Seleccionar(int numero)
        {
            numeroSeleccionado = numero;
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            contexto.personaje.seleccionarInstrumentoManoDerecha(numeroSeleccionado);
        }

        #endregion Comportamientos

        #region Constantes

        public const int NumeroUno = 0;
        public const int NumeroDos = 1;

        #endregion Constantes
    }
}