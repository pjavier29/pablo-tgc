using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Comandos
{
    public class MoverCamara : Comando
    {
        #region Atributos

        private readonly string accionCamara;

        #endregion Atributos

        #region Constructores

        public MoverCamara(string accion)
        {
            accionCamara = accion;
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (accionCamara.Equals(SubirCamara))
            {
                contexto.camara.SubirCamara(contexto.personaje);
            }
            if (accionCamara.Equals(BajarCamara))
            {
                contexto.camara.BajarCamara(contexto.personaje);
            }
            if (accionCamara.Equals(AlejarCamara))
            {
                contexto.camara.AlejarCamara(contexto.personaje);
            }
            if (accionCamara.Equals(AcercarCamara))
            {
                contexto.camara.AcercarCamara(contexto.personaje);
            }

            #endregion Comportamientos
        }

        #region Constantes

        public const string SubirCamara = "SubirCamara";
        public const string BajarCamara = "BajarCamara";
        public const string AlejarCamara = "AlejarCamara";
        public const string AcercarCamara = "AcercarCamara";

        #endregion Constantes
    }
}