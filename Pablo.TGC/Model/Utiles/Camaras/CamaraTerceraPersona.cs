﻿using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TGC.Core.Camara;
using TGC.Core.Geometry;
using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Utiles.Camaras
{
    public class CamaraTerceraPersona : Camara
    {
        #region Atributos

        private TgcThirdPersonCamera camaraTgc;
        private TgcFrustum frustum;
        private Device d3dDevice;

        #endregion Atributos

        #region Constructores

        public CamaraTerceraPersona(TgcThirdPersonCamera camaraTgc, Vector3 posicion, TgcFrustum frustum,
            Device d3dDevice)
        {
            //this.camaraTgc = camaraTgc;
            //this.camaraTgc.Enable = true;
            //this.camaraTgc.setCamera(posicion, 0, -300);
            //this.frustum = frustum;
            //this.d3dDevice = d3dDevice;
        }

        public void Render(Personaje personaje, SuvirvalCraft contexto)
        {
            //Hacer que la camara siga al personaje en su nueva posicion. Sumamos 100 en el posición de Y porque queremos que la cámara este un poco más alta.
            camaraTgc.Target = personaje.mesh.Position + new Vector3(0, 100, 0);
            //Actualizamos la rotacion si es que hubo
            camaraTgc.RotationY = personaje.mesh.Rotation.Y;
            //Porque el personaje esta rotado 180 grados respecto de la camara.
            camaraTgc.rotateY(Geometry.DegreeToRadian(180f));

            //Actualizar volumen del Frustum con nuevos valores de camara
            frustum.updateVolume(d3dDevice.Transform.View, d3dDevice.Transform.Projection);

            personaje.RenderizarTerceraPersona(contexto);
        }

        public void SubirCamara(Personaje personaje)
        {
            float offsetHeight;
            offsetHeight = camaraTgc.OffsetHeight;
            if (offsetHeight > -20 && offsetHeight <= 600)
            {
                camaraTgc.OffsetHeight = offsetHeight - 1;
            }
        }

        public void AcercarCamara(Personaje personaje)
        {
            float offsetForward;
            offsetForward = camaraTgc.OffsetForward;
            if (offsetForward < -200 && offsetForward >= -600)
            {
                camaraTgc.OffsetForward = offsetForward + 1;
            }
        }

        public void AlejarCamara(Personaje personaje)
        {
            float offsetForward;
            offsetForward = camaraTgc.OffsetForward;
            if (offsetForward <= -200 && offsetForward > -600)
            {
                camaraTgc.OffsetForward = offsetForward - 1;
            }
        }

        public void BajarCamara(Personaje personaje)
        {
            float offsetHeight;
            offsetHeight = camaraTgc.OffsetHeight;
            if (offsetHeight >= -20 && offsetHeight < 600)
            {
                camaraTgc.OffsetHeight = offsetHeight + 1;
            }
        }

        #endregion Constructores
    }
}