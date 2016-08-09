using TGC.Core.SceneLoader;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosJuego
{
    public class FuenteAgua : Elemento
    {
        #region Contructores

        public FuenteAgua(float peso, float resistencia, TgcMesh mesh, Efecto efecto)
            : base(peso, resistencia, mesh, efecto)
        {
        }

        #endregion Contructores

        #region Comportamientos

        public override void procesarInteraccion(string accion, SuvirvalCraft contexto, float elapsedTime)
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

        public override string getAcciones()
        {
            //TODO. Mejorar esta lógica
            return "Consumir (C)";
        }

        public override string GetTipo()
        {
            return FuenteAgua;
        }

        #endregion Comportamientos
    }
}