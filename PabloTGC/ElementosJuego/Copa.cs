using AlumnoEjemplos.MiGrupo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.ElementosJuego
{
    public class Copa : Elemento
    {
        #region Constructores

        public Copa(float peso, float resistencia, TgcMesh mesh) :base(peso, resistencia, mesh)
        {

        }

        public Copa(float peso, float resistencia, TgcMesh mesh, Elemento elemento) : base(peso, resistencia, mesh, elemento)
        {

        }

        #endregion

        #region Comportamientos

        public override void procesarInteraccion(String accion, SuvirvalCraft contexto, float elapsedTime)
        {
            base.procesarInteraccion(accion, contexto, elapsedTime);
            if (accion.Equals("Juntar"))
            {
                //TODO. Esta validacion es porque se ejecuta muchas veces al presionar la tecla. Se deberia solucioanr cuando implementemos los comandos
                if (!contexto.personaje.ContieneElementoEnMochila(this))
                {
                    contexto.personaje.juntar(this);
                    contexto.elementos.Remove(this);
                }
            }
        }

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            return "Juntar (J)";
        }

        public override String GetTipo()
        {
            return Copa;
        }

        #endregion
    }
}
