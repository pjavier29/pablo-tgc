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
        public override void procesarColision(Personaje personaje, float elapsedTime, List<Elemento> elementos, float moveForward, Vector3 movementVector)
        {
        }

        public override void procesarInteraccion(String accion, Personaje personaje, List<Elemento> elementos)
        {
            if (accion.Equals("Juntar"))
            {
                //TODO. Esta validacion es porque se ejecuta muchas veces al presionar la tecla. Se deberia solucioanr cuando implementemos los comandos
                if (! personaje.elementosEnMochila().Contains(this))
                {
                    personaje.juntar(this);
                    elementos.Remove(this);
                }
            }
            if (accion.Equals("Encender"))
            {
                foreach (Elemento elem in this.elementosQueContiene())
                {
                    elem.posicion(this.posicion());
                    elem.Mesh.BoundingBox.scaleTranslate(this.posicion(), new Vector3(2f, 2f, 2f));
                    elementos.Add(elem);
                }
                this.liberar();
                elementos.Remove(this);
            }
        }

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            return "Juntar, Encender";
        }

        #endregion
    }
}
