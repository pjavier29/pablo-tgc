using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Input;

namespace AlumnoEjemplos.PabloTGC.Utiles.Camaras
{
    public class CamaraTerceraPersona : Camara
    {
        #region Atributos
        private TgcThirdPersonCamera camaraTgc;
        #endregion

        #region Constructores
        public CamaraTerceraPersona(TgcThirdPersonCamera camaraTgc, Personaje personaje)
        {
            this.camaraTgc = camaraTgc;
            this.camaraTgc.Enable = true;
            this.camaraTgc.setCamera(personaje.mesh.Position, 200, -300);
        }

        public void Render(Personaje personaje)
        {
            //Hacer que la camara siga al personaje en su nueva posicion. Sumamos 100 en el posición de Y porque queremos que la cámara este un poco más alta.
            this.camaraTgc.Target = personaje.mesh.Position + new Vector3(0,100,0);
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
