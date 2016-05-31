using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.PabloTGC.Utiles.Camaras
{
    /// <summary>
    /// TODO. Colocar el arma delante de la pantalla con cierto movimiento al caminar o golpear.
    /// </summary>
    public class CamaraPrimeraPersona : Camara
    {
        #region Atributos
        private Microsoft.DirectX.Direct3D.Device d3dDevice;
        #endregion

        #region Constructores
        public CamaraPrimeraPersona(Microsoft.DirectX.Direct3D.Device d3dDevice)
        {
            this.d3dDevice = d3dDevice;
        }

        #endregion

        #region Comportamientos
        public void Render(Personaje personaje)
        {
            this.d3dDevice.Transform.View = Matrix.LookAtLH(personaje.PosicionAlturaCabeza(), personaje.DireccionAlturaCabeza(150), new Vector3(0, 1, 0));
            GuiController.Instance.Frustum.updateVolume(d3dDevice.Transform.View, d3dDevice.Transform.Projection);
            //El personaje no debe animarse cuando se esta en primera persona
            //personaje.Renderizar();
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
        #endregion
    }
}
