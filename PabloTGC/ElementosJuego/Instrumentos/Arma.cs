using AlumnoEjemplos.PabloTGC.Administracion;
using AlumnoEjemplos.PabloTGC.Utiles.Efectos;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.ElementosJuego.Instrumentos
{
    public class Arma
    {
        #region Atributos
        public float potenciaGolpe { get; set; }
        public float alcance { get; set; }
        public Matrix translacion { get; set; }//Para poder ubicarla en la mano del personaje
        public TgcMesh mesh { get; set; }//Para poder representar el arma
        private Efecto efecto;
        private Color colorBase;

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

        public Arma(float potenciaGolpe, float alcance, Matrix translacion, TgcMesh mesh, Efecto efecto, Color colorBase)
        {
            this.potenciaGolpe = potenciaGolpe;
            this.alcance = alcance;
            this.translacion = translacion;
            this.mesh = mesh;
            this.SetEfecto(efecto);
            this.colorBase = colorBase;
        }
        #endregion

        #region Comportamientos

        public void renderizar(SuvirvalCraft contexto)
        {
            if (this.Efecto() != null)
            {
                //Delego en el efecto la responsabilidad del renderizado.
                this.Efecto().ActualizarRenderizar(contexto, this);
            }
            else
            {
                this.mesh.render();
            }
        }

        public void SetEfecto(Efecto efecto)
        {
            this.efecto = efecto;
            efecto.Aplicar(this.mesh);
        }

        public Efecto Efecto()
        {
            return this.efecto;
        }

        #region Para configurar la luz
        public virtual ColorValue ColorEmisor()
        {
            return ColorValue.FromColor(Color.Black);
        }

        public virtual ColorValue ColorAmbiente()
        {
            return ColorValue.FromColor(this.colorBase);
        }

        public virtual ColorValue ColorDifuso()
        {
            return ColorValue.FromColor(this.colorBase);
        }

        public virtual ColorValue ColorEspecular()
        {
            return ColorValue.FromColor(this.colorBase);
        }

        public virtual float EspecularEx()
        {
            return 20;
        }

        #endregion

        #endregion
    }
}
