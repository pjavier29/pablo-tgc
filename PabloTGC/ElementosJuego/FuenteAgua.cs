using AlumnoEjemplos.MiGrupo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.ElementosJuego
{
    public class FuenteAgua : Elemento
    {
        #region Contructores
        public FuenteAgua(float peso, float resistencia, TgcMesh mesh) :base(peso, resistencia, mesh)
        {

        }

        #endregion

        #region Comportamientos

        public override void procesarInteraccion(String accion, SuvirvalCraft contexto, float elapsedTime)
        {
            //En la fuente de agua no queremos que se miestre barra de estado
            base.procesarInteraccion(accion, contexto, elapsedTime);
            if (accion.Equals("Consumir"))
            {
                if (contexto.personaje.ContieneElementoEnMochilaDeTipo(Copa))
                {
                    contexto.personaje.ConsumirAguar(50f);
                }
                else
                {
                    contexto.personaje.ConsumirAguar(0.04f);
                }
            }
        }

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            return "Consumir (C)";
        }

        public override String GetTipo()
        {
            return FuenteAgua;
        }

        #endregion
    }
}
