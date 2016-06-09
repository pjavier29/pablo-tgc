using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.Utiles.Efectos
{
    public class EfectoBote : Efecto
    {
        #region Atributos
        #endregion

        #region Constructores
        public EfectoBote(Effect efectoShader, String tecnica): base(efectoShader, tecnica)
        {
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
                    //Setea primero aquellos parámetros que son propios del efecto en cuestión.
                    this.GetEfectoShader().SetValue("time", contexto.tiempo);
                    iluminador.Iluminar(this, contexto.personaje.mesh.Position, elemento.ColorEmisor(), elemento.ColorAmbiente(),
                    elemento.ColorDifuso(), elemento.ColorEspecular(), elemento.EspecularEx());
                    elemento.Mesh.render();
                }
                else
                {
                    this.GetEfectoShader().SetValue("time", contexto.tiempo);
                    contexto.dia.GetSol().Iluminar(contexto.personaje.mesh.Position, this, elemento.ColorEmisor(), elemento.ColorAmbiente(),
                        elemento.ColorDifuso(), elemento.ColorEspecular(), elemento.EspecularEx());
                    elemento.Mesh.render();
                }
            }
            else
            {
                this.GetEfectoShader().SetValue("time", contexto.tiempo);
                contexto.dia.GetSol().Iluminar(contexto.personaje.mesh.Position, this, elemento.ColorEmisor(), elemento.ColorAmbiente(),
                    elemento.ColorDifuso(), elemento.ColorEspecular(), elemento.EspecularEx());
                elemento.Mesh.render();
            }
        }
        #endregion
    }
}
