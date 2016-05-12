using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.ElementosJuego
{
    public class Madera : Elemento
    {
        #region Atributos
        #endregion

        #region Contructores
        public Madera(float peso, float resistencia, TgcMesh mesh) :base(peso, resistencia, mesh)
        {

        }

        public Madera(float peso, float resistencia, TgcMesh mesh, Elemento elemento) : base(peso, resistencia, mesh, elemento)
        {

        }

        #endregion

        #region Comportamientos

        /// <summary>
        /// Procesa una colisión cuando el personaje colisiona contra un pedazo de madera
        /// </summary>
        public override void procesarColision(Personaje personaje, float elapsedTime, String accion, List<Elemento> elementos, float moveForward, Vector3 movementVector)
        {
            if (accion.Equals("Juntar"))
            {
                personaje.juntar(this);
                elementos.Remove(this);
            }
            if (accion.Equals("Encender"))
            {
                foreach (Elemento elem in this.elementosQueContiene())
                {
                    elem.posicion(this.posicion());
                    elem.mesh.BoundingBox.scaleTranslate(this.posicion(), new Vector3(2f, 2f, 2f));
                    elementos.Add(elem);
                }
                this.liberar();
                elementos.Remove(this);
            }
        }

        #endregion
    }
}
