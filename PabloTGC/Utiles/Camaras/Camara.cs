using AlumnoEjemplos.PabloTGC.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.PabloTGC.Utiles.Camaras
{
    public interface Camara
    {
        #region Firmas
        void Render(Personaje personaje);
        void SubirCamara(Personaje personaje);
        void BajarCamara(Personaje personaje);
        void AcercarCamara(Personaje personaje);
        void AlejarCamara(Personaje personaje);
        #endregion
    }
}
