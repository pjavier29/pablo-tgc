using AlumnoEjemplos.MiGrupo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class MoverCamara : Comando
    {
        #region Constantes
        public const String SubirCamara = "SubirCamara";
        public const String BajarCamara = "BajarCamara";
        public const String AlejarCamara = "AlejarCamara";
        public const String AcercarCamara = "AcercarCamara";
        #endregion

        #region Atributos
        private String accionCamara;
        #endregion

        #region Constructores
        public MoverCamara(String accion)
        {
            this.accionCamara = accion;
        }
        #endregion

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
            #endregion
        }
    }
}
