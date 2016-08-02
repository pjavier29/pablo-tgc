using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Comandos
{
    public class Menu : Comando
    {
        #region Constantes

        public const string Mochila = "Mochila";

        #endregion Constantes

        #region Atributos

        private readonly string tipo;

        #endregion Atributos

        #region Constructores

        public Menu(string tipo)
        {
            this.tipo = tipo;
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (tipo.Equals(Mochila))
            {
                contexto.mostrarMenuMochila = true;
            }
        }

        #endregion Comportamientos
    }
}