using TGC.Core.Direct3D;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles.Camaras;

namespace TGC.Group.Model.Comandos
{
    public class CambiarCamara : Comando
    {
        #region Atributos

        private readonly string camaraElegida;

        #endregion Atributos

        #region Constructores

        public CambiarCamara(string camera)
        {
            camaraElegida = camera;
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (camaraElegida.Equals(PrimeraPersona))
            {
                contexto.camara = new CamaraPrimeraPersona(contexto.Frustum, D3DDevice.Instance.Device);
            }
            if (camaraElegida.Equals(TerceraPersona))
            {
                //contexto.camara = new CamaraTerceraPersona(GuiController.Instance.ThirdPersonCamera, contexto.personaje.mesh.Position, contexto.Frustum, D3DDevice.Instance.Device);
            }
        }

        #endregion Comportamientos

        #region Constantes

        public const string PrimeraPersona = "PrimeraPersona";
        public const string TerceraPersona = "TerceraPersona";

        #endregion Constantes
    }
}