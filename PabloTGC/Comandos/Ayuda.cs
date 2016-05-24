using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.MiGrupo;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class Ayuda : Comando
    {
        #region Atributos
        private String mensajeAyuda;
        #endregion

        #region Constructores
        public Ayuda(String ayuda)
        {
            this.mensajeAyuda = ayuda;
        }
        #endregion

        #region Comportamientos
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            contexto.ayudaReglon1.Text = this.mensajeAyuda;
            contexto.mostrarAyuda = true;
        }
        #endregion
    }
}
