using System;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles.Camaras;

namespace TGC.Group.Model.Comandos
{
    public class CambiarCamara : Comando
    {
        #region Constantes

        public const String PrimeraPersona = "PrimeraPersona";
        public const String TerceraPersona = "TerceraPersona";

        #endregion Constantes

        #region Atributos

        private String camaraElegida;

        #endregion Atributos

        #region Constructores

        public CambiarCamara(String camera)
        {
            this.camaraElegida = camera;
        }

        #endregion Constructores

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

        #endregion Comportamientos
    }
}