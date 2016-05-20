using AlumnoEjemplos.MiGrupo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class Seleccionar : Comando
    {
        #region Constantes
        public const int NumeroUno = 0;
        public const int NumeroDos = 1;
        #endregion

        #region Atributos
        private int numeroSeleccionado;
        #endregion

        #region Propiedades
        #endregion

        #region Constructores
        public Seleccionar(int numero)
        {
            this.numeroSeleccionado = numero;
        }

        #endregion

        #region Comportamientos
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            contexto.personaje.seleccionarInstrumentoManoDerecha(this.numeroSeleccionado);
        }

        #endregion
    }
}
