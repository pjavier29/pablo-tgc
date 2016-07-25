using System;
using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Comandos
{
    public class Menu : Comando
    {
        #region Constantes

        public const String Mochila = "Mochila";

        #endregion Constantes

        #region Atributos

        private String tipo;

        #endregion Atributos

        #region Constructores

        public Menu(String tipo)
        {
            this.tipo = tipo;
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (this.tipo.Equals(Mochila))
            {
                contexto.mostrarMenuMochila = true;
            }
        }

        #endregion Comportamientos
    }
}