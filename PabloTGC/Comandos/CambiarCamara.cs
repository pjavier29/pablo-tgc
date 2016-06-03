using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.MiGrupo;
using AlumnoEjemplos.PabloTGC.Utiles.Camaras;
using TgcViewer;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class CambiarCamara : Comando
    {
        #region Constantes
        public const String PrimeraPersona = "PrimeraPersona";
        public const String TerceraPersona = "TerceraPersona";
        #endregion

        #region Atributos
        private String camaraElegida;
        #endregion

        #region Constructores
        public CambiarCamara(String camera)
        {
            this.camaraElegida = camera;
        }
        #endregion

        #region Comportamientos
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (this.camaraElegida.Equals(PrimeraPersona))
            {
                contexto.camara = new CamaraPrimeraPersona(GuiController.Instance.Frustum, GuiController.Instance.D3dDevice);
            }
            if (this.camaraElegida.Equals(TerceraPersona))
            {
                contexto.camara = new CamaraTerceraPersona(GuiController.Instance.ThirdPersonCamera, contexto.personaje.mesh.Position, GuiController.Instance.Frustum, GuiController.Instance.D3dDevice);
            }
        }
        #endregion
    }
}
