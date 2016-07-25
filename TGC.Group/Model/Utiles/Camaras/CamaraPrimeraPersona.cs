using Microsoft.DirectX;
using TGC.Core.Geometry;
using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Utiles.Camaras
{
    /// <summary>
    /// TODO. Colocar el arma delante de la pantalla con cierto movimiento al caminar o golpear.
    /// </summary>
    public class CamaraPrimeraPersona : Camara
    {
        #region Atributos

        private Microsoft.DirectX.Direct3D.Device d3dDevice;
        private TgcFrustum frustum;

        #endregion Atributos

        #region Constructores

        public CamaraPrimeraPersona(TgcFrustum frustum, Microsoft.DirectX.Direct3D.Device d3dDevice)
        {
            this.d3dDevice = d3dDevice;
            this.frustum = frustum;
        }

        #endregion Constructores

        #region Comportamientos

        public void Render(Personaje personaje, SuvirvalCraft contexto)
        {
            this.d3dDevice.Transform.View = Matrix.LookAtLH(personaje.PosicionAlturaCabeza(), personaje.DireccionAlturaCabeza(150), new Vector3(0, 1, 0));

            //Actualizar volumen del Frustum con nuevos valores de camara
            this.frustum.updateVolume(d3dDevice.Transform.View, d3dDevice.Transform.Projection);

            //El personaje no debe animarse cuando se esta en primera persona
            personaje.RenderizarPrimeraPersona(contexto);
        }

        public void SubirCamara(Personaje personaje)
        {
            personaje.SubirVision(1);
        }

        public void AcercarCamara(Personaje personaje)
        {
        }

        public void AlejarCamara(Personaje personaje)
        {
        }

        public void BajarCamara(Personaje personaje)
        {
            personaje.BajarVision(1);
        }

        #endregion Comportamientos
    }
}