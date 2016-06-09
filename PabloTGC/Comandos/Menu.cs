using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.PabloTGC.Administracion;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class Menu : Comando
    {
        #region Constantes
        public const String Mochila = "Mochila";
        #endregion

        #region Atributos
        private String tipo;
        #endregion

        #region Constructores

        public Menu(String tipo)
        {
            this.tipo = tipo;
        }

        #endregion

        #region Comportamientos
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (this.tipo.Equals(Mochila))
            {
                contexto.mostrarMenuMochila = true;
            }
        }
        #endregion
    }
}
