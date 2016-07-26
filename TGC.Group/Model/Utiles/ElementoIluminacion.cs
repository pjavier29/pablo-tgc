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
            this.Elemento = elemento;
            this.Distancia = distancia;
        }

        public bool IluminoAElemento(Elemento elementoAIluminar)
        {
            return this.Elemento.distanciaA(elementoAIluminar) < this.Distancia;
        }

        public bool IluminoAElemento(Vector3 posicion)
        {
            return this.Elemento.distanciaA(posicion) < this.Distancia;
        }

        public virtual void Iluminar(Efecto efecto, Vector3 posicionVision, ColorValue colorEmisor, ColorValue colorAmbiente,
            ColorValue colorDifuso, ColorValue colorEspecular, float especularEx)
        {
            this.Elemento.Iluminar(efecto, posicionVision, colorEmisor, colorAmbiente, colorDifuso, colorEspecular, especularEx);
        }

        #endregion Constructores
    }
}