using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.Instrumentos
{
    public class Arma
    {
        #region Atributos
        public float potenciaGolpe { get; set; }
        public float alcance { get; set; }
        public Matrix translacion { get; set; }//Para poder ubicarla en la mano del personaje
        public TgcMesh mesh { get; set; }//Para poder representar el arma

        #endregion

        #region Contructores
        public Arma() { }

        public Arma(float potenciaGolpe, float alcance, Matrix translacion, TgcMesh mesh)
        {
            this.potenciaGolpe = potenciaGolpe;
            this.alcance = alcance;
            this.translacion = translacion;
            this.mesh = mesh;
        }
        #endregion

        #region Comportamientos
        #endregion
    }
}
