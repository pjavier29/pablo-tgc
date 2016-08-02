using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Core.SceneLoader;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosJuego.Instrumentos
{
    public class Arma
    {
        #region Atributos

        public float potenciaGolpe { get; set; }
        public float alcance { get; set; }
        public Matrix translacion { get; set; } //Para poder ubicarla en la mano del personaje
        public TgcMesh mesh { get; set; } //Para poder representar el arma
        private Efecto efecto;
        private readonly Color colorBase;

        #endregion Atributos

        #region Contructores

        public Arma()
        {
        }

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
            SetEfecto(efecto);
            this.colorBase = colorBase;
        }

        #endregion Contructores

        #region Comportamientos

        public void renderizar(SuvirvalCraft contexto)
        {
            if (Efecto() != null)
            {
                //Delego en el efecto la responsabilidad del renderizado.
                Efecto().ActualizarRenderizar(contexto, this);
            }
            else
            {
                mesh.render();
            }
        }

        public void SetEfecto(Efecto efecto)
        {
            this.efecto = efecto;
            efecto.Aplicar(mesh);
        }

        public Efecto Efecto()
        {
            return efecto;
        }

        #region Para configurar la luz

        public virtual ColorValue ColorEmisor()
        {
            return ColorValue.FromColor(Color.Black);
        }

        public virtual ColorValue ColorAmbiente()
        {
            return ColorValue.FromColor(colorBase);
        }

        public virtual ColorValue ColorDifuso()
        {
            return ColorValue.FromColor(colorBase);
        }

        public virtual ColorValue ColorEspecular()
        {
            return ColorValue.FromColor(colorBase);
        }

        public virtual float EspecularEx()
        {
            return 20;
        }

        #endregion Para configurar la luz

        #endregion Comportamientos
    }
}