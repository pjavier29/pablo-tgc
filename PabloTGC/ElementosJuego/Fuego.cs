using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.ElementosJuego
{
    public class Fuego : Elemento
    {
        #region Atributos
        #endregion

        #region Contructores
        public Fuego(float peso, float resistencia, TgcMesh mesh) :base(peso, resistencia, mesh)
        {

        }
        #endregion

        #region Comportamientos

        /// <summary>
        /// Procesa una colisión cuando el personaje colisiona contra el fuego
        /// </summary>
        public override void procesarColision(Personaje personaje, float elapsedTime, List<Elemento> elementos, float moveForward, Vector3 movementVector)
        {
            if (this.distanciaA(personaje.mesh.Position) > 20)
            {
                personaje.incrementarSaludPorTiempo(elapsedTime);
            }
            else
            {
                personaje.morir();
            }
        }

        #endregion
    }
}
