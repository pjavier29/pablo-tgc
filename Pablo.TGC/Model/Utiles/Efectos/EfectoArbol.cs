using Microsoft.DirectX.Direct3D;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.ElementosJuego;

namespace TGC.Group.Model.Utiles.Efectos
{
    public class EfectoArbol : Efecto
    {
        #region Constructores

        public EfectoArbol(Effect efectoShader, string tecnica) : base(efectoShader, tecnica)
        {
        }

        #endregion Constructores

        #region Comportamientos

        /// <summary>
        ///     //TODO. Refactorizar los parametros que recibe!!!!
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="elemento"></param>
        public override void ActualizarRenderizar(SuvirvalCraft contexto, Elemento elemento)
        {
            if (HayQueIluminarConElementos(contexto))
            {
                var iluminador = AlguienIluminaAElemento(elemento);
                if (iluminador != null)
                {
                    //Setea primero aquellos parámetros que son propios del efecto en cuestión.
                    GetEfectoShader().SetValue("time", contexto.tiempo);
                    GetEfectoShader().SetValue("distanciaAnimacion", elemento.Flexibilidad);
                    GetEfectoShader().SetValue("vibracionPorGolpe", elemento.ObtenerVibracion(contexto.tiempo));
                    iluminador.Iluminar(this, contexto.personaje.mesh.Position, elemento.ColorEmisor(),
                        elemento.ColorAmbiente(),
                        elemento.ColorDifuso(), elemento.ColorEspecular(), elemento.EspecularEx());
                    elemento.Mesh.render();
                }
                else
                {
                    GetEfectoShader().SetValue("time", contexto.tiempo);
                    GetEfectoShader().SetValue("distanciaAnimacion", elemento.Flexibilidad);
                    GetEfectoShader().SetValue("vibracionPorGolpe", elemento.ObtenerVibracion(contexto.tiempo));
                    contexto.dia.GetSol()
                        .Iluminar(contexto.personaje.mesh.Position, this, elemento.ColorEmisor(),
                            elemento.ColorAmbiente(),
                            elemento.ColorDifuso(), elemento.ColorEspecular(), elemento.EspecularEx());
                    elemento.Mesh.render();
                }
            }
            else
            {
                GetEfectoShader().SetValue("time", contexto.tiempo);
                GetEfectoShader().SetValue("distanciaAnimacion", elemento.Flexibilidad);
                GetEfectoShader().SetValue("vibracionPorGolpe", elemento.ObtenerVibracion(contexto.tiempo));
                contexto.dia.GetSol()
                    .Iluminar(contexto.personaje.mesh.Position, this, elemento.ColorEmisor(), elemento.ColorAmbiente(),
                        elemento.ColorDifuso(), elemento.ColorEspecular(), elemento.EspecularEx());
                elemento.Mesh.render();
            }
        }

        #endregion Comportamientos
    }
}