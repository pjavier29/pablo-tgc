using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Comandos
{
    public class Seleccionar : Comando
    {
        #region Constantes

        public const int NumeroUno = 0;
        public const int NumeroDos = 1;

        #endregion Constantes

        #region Atributos

        private int numeroSeleccionado;

        #endregion Atributos



        #region Constructores

        public Seleccionar(int numero)
        {
            this.numeroSeleccionado = numero;
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            contexto.personaje.seleccionarInstrumentoManoDerecha(this.numeroSeleccionado);
        }

        #endregion Comportamientos
    }
}