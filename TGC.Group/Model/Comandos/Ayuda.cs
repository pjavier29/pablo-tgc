using System;
using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Comandos
{
    public class Ayuda : Comando
    {
        #region Atributos

        private String mensajeAyuda;

        #endregion Atributos

        #region Constructores

        public Ayuda(String ayuda)
        {
            this.mensajeAyuda = ayuda;
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            contexto.ayudaReglon1.Text = this.mensajeAyuda;
            contexto.mostrarAyuda = true;
        }

        #endregion Comportamientos
    }
}