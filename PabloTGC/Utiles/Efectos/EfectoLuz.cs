using AlumnoEjemplos.PabloTGC.Administracion;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.Utiles.Efectos
{
    public class EfectoLuz : Efecto
    {
        #region Atributos
        #endregion

        #region Constructores
        public EfectoLuz(Effect efectoShader, String tecnica): base(efectoShader, tecnica)
        {
        }
        public EfectoLuz(Effect efectoShader) : base(efectoShader)
        {
            this.Tecnica(null);
        }
        #endregion

        #region Comportamientos
        public override void ActualizarRenderizar(SuvirvalCraft contexto, Elemento elemento)
        {
            if (this.HayQueIluminarConElementos(contexto))
            {
                ElementoIluminacion iluminador = this.AlguienIluminaAElemento(elemento);
                if (iluminador != null)
                {
                    //this.Tecnica(GuiController.Instance.Shaders.getTgcMeshTechnique(elemento.Mesh.RenderType));
                    //this.Aplicar(elemento.Mesh);
                    //Setea primero aquellos parámetros que son propios del efecto en cuestión.
                    iluminador.Iluminar(this, contexto.personaje.mesh.Position, elemento.ColorEmisor(), elemento.ColorAmbiente(),
                    elemento.ColorDifuso(), elemento.ColorEspecular(), elemento.EspecularEx());
                    elemento.Mesh.render();
                }
                else
                {
                    contexto.dia.GetSol().Iluminar(contexto.personaje.mesh.Position, this, elemento.ColorEmisor(), elemento.ColorAmbiente(),
    elemento.ColorDifuso(), elemento.ColorEspecular(), elemento.EspecularEx());
                    elemento.Mesh.render();
                }
            }
            else
            {
                //this.Tecnica(GuiController.Instance.Shaders.getTgcMeshTechnique(elemento.Mesh.RenderType));
                //this.Aplicar(elemento.Mesh);
                contexto.dia.GetSol().Iluminar(contexto.personaje.mesh.Position, this, elemento.ColorEmisor(), elemento.ColorAmbiente(),
                    elemento.ColorDifuso(), elemento.ColorEspecular(), elemento.EspecularEx());
                elemento.Mesh.render();
            }
        }
        #endregion
    }
}
