using AlumnoEjemplos.MiGrupo;
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
        public override void procesarColision(Personaje personaje, float elapsedTime, List<Elemento> elementos, float moveForward, Vector3 movementVector, Vector3 lastPos)
        {
        }

        public override void procesarInteraccion(String accion, SuvirvalCraft contexto, float elapsedTime)
        {
            base.procesarInteraccion(accion, contexto, elapsedTime);
            if (accion.Equals("Juntar"))
            {
                //TODO. Esta validacion es porque se ejecuta muchas veces al presionar la tecla. Se deberia solucioanr cuando implementemos los comandos
                if (! contexto.personaje.ContieneElementoEnMochila(this))
                {
                    contexto.personaje.juntar(this);
                    contexto.elementos.Remove(this);
                    contexto.optimizador.ForzarActualizacionElementosColision();
                }
            }
            if (accion.Equals("Encender"))
            {
                foreach (Elemento elem in this.elementosQueContiene())
                {
                    elem.posicion(this.posicion());
                    elem.Mesh.BoundingBox.scaleTranslate(this.posicion(), new Vector3(2f, 0.25f, 2f));
                    contexto.elementos.Add(elem);
                }
                this.liberar();
                contexto.elementos.Remove(this);
                contexto.optimizador.ForzarActualizacionElementosColision();
            }
        }

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            return "Juntar (J), Encender (E)";
        }

        public override String GetTipo()
        {
            return Madera;
        }

        #endregion
    }
}
