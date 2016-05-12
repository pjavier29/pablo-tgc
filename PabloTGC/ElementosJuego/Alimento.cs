using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.ElementosJuego
{
    public class Alimento : Elemento
    {
        #region Atributos
        #endregion

        #region Contructores
        public Alimento(float peso, float resistencia, TgcMesh mesh) :base(peso, resistencia, mesh)
        {

        }

        public Alimento(float peso, float resistencia, TgcMesh mesh, Elemento elemento) : base(peso, resistencia, mesh, elemento)
        {

        }

        #endregion

        #region Comportamientos

        /// <summary>
        /// Procesa una colisión cuando el personaje colisiona contra un pedazo de madera
        /// </summary>
        public override void procesarColision(Personaje personaje, float elapsedTime, String accion, List<Elemento> elementos, float moveForward, Vector3 movementVector)
        {
            personaje.consumirAlimento();
            this.liberar();
            elementos.Remove(this);
        }

        #endregion
    }

}
