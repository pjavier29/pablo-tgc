using System;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using TGC.Core.Utils;
using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Utiles.Efectos
{
    public class EfectoTerreno : Efecto
    {
        #region Constructores

        public EfectoTerreno(Effect efectoShader, String tecnica) : base(efectoShader, tecnica)
        {
        }

        #endregion Constructores

        #region Comportamientos

        /// <summary>
        /// Este metodo esta horrible ya que todas las luces utilizan los parametros de la primera, solo aportan su posición. Los parámetros deberían se de cada luz.
        /// Por el momento no es tan grave ya que solo puedo crear luces con fuegos.
        /// Soporta un máximo de 4 luces
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="terreno"></param>
        public override void ActualizarRenderizar(SuvirvalCraft contexto, Terreno terreno)
        {
            if (this.HayQueIluminarConElementos(contexto))
            {
                ElementoIluminacion elem = this.GetElementosIluminacion()[0];
                if (this.GetElementosIluminacion().Count > 1)
                {
                    ElementoIluminacion elem2 = this.GetElementosIluminacion()[1];
                    this.GetEfectoShader().SetValue("segundaLuz", 1);
                    this.GetEfectoShader().SetValue("lightPosition2", TgcParserUtils.vector3ToFloat4Array(elem2.Elemento.posicion()));
                }
                if (this.GetElementosIluminacion().Count > 2)
                {
                    ElementoIluminacion elem3 = this.GetElementosIluminacion()[2];
                    this.GetEfectoShader().SetValue("terceraLuz", 1);
                    this.GetEfectoShader().SetValue("lightPosition3", TgcParserUtils.vector3ToFloat4Array(elem3.Elemento.posicion()));
                }
                if (this.GetElementosIluminacion().Count > 3)
                {
                    ElementoIluminacion elem4 = this.GetElementosIluminacion()[3];
                    this.GetEfectoShader().SetValue("cuartaLuz", 1);
                    this.GetEfectoShader().SetValue("lightPosition4", TgcParserUtils.vector3ToFloat4Array(elem4.Elemento.posicion()));
                }

                //Setea primero aquellos parámetros que son propios del efecto en cuestión.
                this.GetEfectoShader().SetValue("time", contexto.tiempo);
                this.GetEfectoShader().SetValue("lightIntensityRelitive", 0.5f);
                elem.Iluminar(this, contexto.personaje.mesh.Position, ColorValue.FromColor(Color.White), ColorValue.FromColor(Color.White),
                        ColorValue.FromColor(Color.White), ColorValue.FromColor(Color.White), 20);
                terreno.executeRender(this.GetEfectoShader());
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

        #endregion Comportamientos
    }
}