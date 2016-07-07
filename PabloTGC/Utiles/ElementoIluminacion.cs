using AlumnoEjemplos.PabloTGC.Administracion;
using AlumnoEjemplos.PabloTGC.Utiles.Efectos;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.PabloTGC.Utiles
{
    public class ElementoIluminacion
    {
        #region Propiedades
        public float Distancia { get; set; }
        public Elemento Elemento { get; set; }
        #endregion

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
        #endregion
    }
}
