using AlumnoEjemplos.PabloTGC.Administracion;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.PabloTGC.Utiles.Camaras
{
    public class CamaraTerceraPersona : Camara
    {
        #region Atributos
        private TgcThirdPersonCamera camaraTgc;
        private TgcFrustum frustum;
        private Microsoft.DirectX.Direct3D.Device d3dDevice;
        #endregion

        #region Constructores
        public CamaraTerceraPersona(TgcThirdPersonCamera camaraTgc, Vector3 posicion, TgcFrustum frustum, Microsoft.DirectX.Direct3D.Device d3dDevice)
        {
            this.camaraTgc = camaraTgc;
            this.camaraTgc.Enable = true;
            this.camaraTgc.setCamera(posicion, 0, -300);
            this.frustum = frustum;
            this.d3dDevice = d3dDevice;
        }

        public void Render(Personaje personaje)
        {
            //Hacer que la camara siga al personaje en su nueva posicion. Sumamos 100 en el posición de Y porque queremos que la cámara este un poco más alta.
            this.camaraTgc.Target = personaje.mesh.Position + new Vector3(0,100,0);
            //Actualizamos la rotacion si es que hubo
            this.camaraTgc.RotationY = personaje.mesh.Rotation.Y;
            //Porque el personaje esta rotado 180 grados respecto de la camara.
            this.camaraTgc.rotateY(Geometry.DegreeToRadian(180f));

            //Actualizar volumen del Frustum con nuevos valores de camara
            this.frustum.updateVolume(this.d3dDevice.Transform.View, this.d3dDevice.Transform.Projection);

            personaje.Renderizar();
        }

        public void SubirCamara(Personaje personaje)
        {
            float offsetHeight;
            offsetHeight = this.camaraTgc.OffsetHeight;
            if (offsetHeight > -20 && offsetHeight <= 600)
            {
                this.camaraTgc.OffsetHeight = offsetHeight - 1;
            }
        }

        public void AcercarCamara(Personaje personaje)
        {
            float offsetForward;
            offsetForward = this.camaraTgc.OffsetForward;
            if (offsetForward < -200 && offsetForward >= -600)
            {
                this.camaraTgc.OffsetForward = offsetForward + 1;
            }  
        }

        public void AlejarCamara(Personaje personaje)
        {
            float offsetForward;
            offsetForward = this.camaraTgc.OffsetForward;
            if (offsetForward <= -200 && offsetForward > -600)
            {
                this.camaraTgc.OffsetForward = offsetForward - 1;
            }    
        }

        public void BajarCamara(Personaje personaje)
        {
            float offsetHeight;
            offsetHeight = this.camaraTgc.OffsetHeight;     
            if (offsetHeight >= -20 && offsetHeight < 600)
            {
                this.camaraTgc.OffsetHeight = offsetHeight + 1;
            }
        }
        #endregion
    }
}
