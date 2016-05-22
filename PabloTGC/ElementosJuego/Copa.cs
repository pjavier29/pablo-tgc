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

        public override void procesarInteraccion(String accion, Personaje personaje, List<Elemento> elementos, float elapsedTime)
        {
            if (accion.Equals("Juntar"))
            {
                //TODO. Esta validacion es porque se ejecuta muchas veces al presionar la tecla. Se deberia solucioanr cuando implementemos los comandos
                if (!personaje.ContieneElementoEnMochila(this))
                {
                    personaje.juntar(this);
                    elementos.Remove(this);
                }
            }
        }

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            return "Juntar";
        }

        public override String GetTipo()
        {
            return Copa;
        }

        #endregion
    }
}
