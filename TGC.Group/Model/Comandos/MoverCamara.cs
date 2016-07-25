using System;
using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Comandos
{
    public class MoverCamara : Comando
    {
        #region Constantes

        public const String SubirCamara = "SubirCamara";
        public const String BajarCamara = "BajarCamara";
        public const String AlejarCamara = "AlejarCamara";
        public const String AcercarCamara = "AcercarCamara";

        #endregion Constantes

        #region Atributos

        private String accionCamara;

        #endregion Atributos

        #region Constructores

        public MoverCamara(String accion)
        {
            this.accionCamara = accion;
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (this.accionCamara.Equals(SubirCamara))
            {
                contexto.camara.SubirCamara(contexto.personaje);
            }
            if (this.accionCamara.Equals(BajarCamara))
            {
                contexto.camara.BajarCamara(contexto.personaje);
            }
            if (this.accionCamara.Equals(AlejarCamara))
            {
                contexto.camara.AlejarCamara(contexto.personaje);
            }
            if (this.accionCamara.Equals(AcercarCamara))
            {
                contexto.camara.AcercarCamara(contexto.personaje);
            }

            #endregion Comportamientos
        }
    }
}