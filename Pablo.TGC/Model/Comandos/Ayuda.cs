using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Comandos
{
    public class Ayuda : Comando
    {
        #region Atributos

        private readonly string mensajeAyuda;

        #endregion Atributos

        #region Constructores

        public Ayuda(string ayuda)
        {
            mensajeAyuda = ayuda;
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            contexto.ayudaReglon1.Text = mensajeAyuda;
            contexto.mostrarAyuda = true;
        }

        #endregion Comportamientos
    }
}