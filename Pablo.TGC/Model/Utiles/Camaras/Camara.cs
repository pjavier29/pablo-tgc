using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Utiles.Camaras
{
    public interface Camara
    {
        #region Firmas

        void Render(Personaje personaje, SuvirvalCraft contexto);

        void SubirCamara(Personaje personaje);

        void BajarCamara(Personaje personaje);

        void AcercarCamara(Personaje personaje);

        void AlejarCamara(Personaje personaje);

        #endregion Firmas
    }
}