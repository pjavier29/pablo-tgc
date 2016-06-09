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
    public class EfectoTerreno : Efecto
    {
        #region Atributos
        #endregion

        #region Constructores
        public EfectoTerreno(Effect efectoShader, String tecnica): base(efectoShader, tecnica)
        {
        }
        #endregion

        #region Comportamientos
        public override void ActualizarRenderizar(SuvirvalCraft contexto, Terreno terreno)
        {
            if (this.HayQueIluminarConElementos(contexto))
            {
                ElementoIluminacion elem = this.IluminadorMasCercanoA(contexto.personaje.mesh.Position, contexto);
                if(elem != null)
                { 
                    //Setea primero aquellos parámetros que son propios del efecto en cuestión.
                    this.GetEfectoShader().SetValue("time", contexto.tiempo);
                    this.GetEfectoShader().SetValue("lightIntensityRelitive", 0.5f);
                    elem.Iluminar(this, contexto.personaje.mesh.Position, ColorValue.FromColor(Color.White), ColorValue.FromColor(Color.White),
                        ColorValue.FromColor(Color.White), ColorValue.FromColor(Color.White), 20);
                    terreno.executeRender(this.GetEfectoShader());
                }
            }
            else
            { 
                this.GetEfectoShader().SetValue("time", contexto.tiempo);
                this.GetEfectoShader().SetValue("lightIntensityRelitive", contexto.dia.GetSol().IntensidadRelativa());
                contexto.dia.GetSol().Iluminar(contexto.personaje.mesh.Position, this, ColorValue.FromColor(Color.White), ColorValue.FromColor(Color.White),
                    ColorValue.FromColor(Color.White), ColorValue.FromColor(Color.White), 20);
                terreno.executeRender(this.GetEfectoShader());
            }
        }
        #endregion
    }
}
