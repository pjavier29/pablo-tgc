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

        public override void procesarInteraccion(String accion, Personaje personaje, List<Elemento> elementos, float elapsedTime)
        {
            if (accion.Equals("Consumir"))
            {
                if (personaje.ContieneElementoEnMochilaDeTipo(Copa))
                {
                    personaje.ConsumirAguar(50f);
                }
                else
                {
                    personaje.ConsumirAguar(0.4f);
                }
            }
        }

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            return "Consumir (U)";
        }

        public override String GetTipo()
        {
            return FuenteAgua;
        }

        #endregion
    }
}
