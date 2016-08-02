using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Core.Utils;
using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Utiles.Efectos
{
    public class EfectoTerreno : Efecto
    {
        #region Constructores

        public EfectoTerreno(Effect efectoShader, string tecnica) : base(efectoShader, tecnica)
        {
        }

        #endregion Constructores

        #region Comportamientos

        /// <summary>
        ///     Este metodo esta horrible ya que todas las luces utilizan los parametros de la primera, solo aportan su posición.
        ///     Los parámetros deberían se de cada luz.
        ///     Por el momento no es tan grave ya que solo puedo crear luces con fuegos.
        ///     Soporta un máximo de 4 luces
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="terreno"></param>
        public override void ActualizarRenderizar(SuvirvalCraft contexto, Terreno terreno)
        {
            if (HayQueIluminarConElementos(contexto))
            {
                var elem = GetElementosIluminacion()[0];
                if (GetElementosIluminacion().Count > 1)
                {
                    var elem2 = GetElementosIluminacion()[1];
                    GetEfectoShader().SetValue("segundaLuz", 1);
                    GetEfectoShader()
                        .SetValue("lightPosition2", TgcParserUtils.vector3ToFloat4Array(elem2.Elemento.posicion()));
                }
                if (GetElementosIluminacion().Count > 2)
                {
                    var elem3 = GetElementosIluminacion()[2];
                    GetEfectoShader().SetValue("terceraLuz", 1);
                    GetEfectoShader()
                        .SetValue("lightPosition3", TgcParserUtils.vector3ToFloat4Array(elem3.Elemento.posicion()));
                }
                if (GetElementosIluminacion().Count > 3)
                {
                    var elem4 = GetElementosIluminacion()[3];
                    GetEfectoShader().SetValue("cuartaLuz", 1);
                    GetEfectoShader()
                        .SetValue("lightPosition4", TgcParserUtils.vector3ToFloat4Array(elem4.Elemento.posicion()));
                }

                //Setea primero aquellos parámetros que son propios del efecto en cuestión.
                GetEfectoShader().SetValue("time", contexto.tiempo);
                GetEfectoShader().SetValue("lightIntensityRelitive", 0.5f);
                elem.Iluminar(this, contexto.personaje.mesh.Position, ColorValue.FromColor(Color.White),
                    ColorValue.FromColor(Color.White),
                    ColorValue.FromColor(Color.White), ColorValue.FromColor(Color.White), 20);
                terreno.executeRender(GetEfectoShader());
            }
            else
            {
                GetEfectoShader().SetValue("time", contexto.tiempo);
                GetEfectoShader().SetValue("lightIntensityRelitive", contexto.dia.GetSol().IntensidadRelativa());
                contexto.dia.GetSol()
                    .Iluminar(contexto.personaje.mesh.Position, this, ColorValue.FromColor(Color.White),
                        ColorValue.FromColor(Color.White),
                        ColorValue.FromColor(Color.White), ColorValue.FromColor(Color.White), 20);
                terreno.executeRender(GetEfectoShader());
            }
        }

        #endregion Comportamientos
    }
}