using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TGC.Group.Model.ElementosJuego;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.Utiles
{
    public class ElementoIluminacion
    {
        #region Propiedades

        public float Distancia { get; set; }
        public Elemento Elemento { get; set; }

        #endregion Propiedades

        #region Constructores

        public ElementoIluminacion(Elemento elemento, float distancia)
        {
            Elemento = elemento;
            Distancia = distancia;
        }

        public bool IluminoAElemento(Elemento elementoAIluminar)
        {
            return Elemento.distanciaA(elementoAIluminar) < Distancia;
        }

        public bool IluminoAElemento(Vector3 posicion)
        {
            return Elemento.distanciaA(posicion) < Distancia;
        }

        public virtual void Iluminar(Efecto efecto, Vector3 posicionVision, ColorValue colorEmisor,
            ColorValue colorAmbiente,
            ColorValue colorDifuso, ColorValue colorEspecular, float especularEx)
        {
            Elemento.Iluminar(efecto, posicionVision, colorEmisor, colorAmbiente, colorDifuso, colorEspecular,
                especularEx);
        }

        #endregion Constructores
    }
}