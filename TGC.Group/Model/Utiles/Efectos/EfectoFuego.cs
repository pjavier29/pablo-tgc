using Microsoft.DirectX.Direct3D;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.ElementosJuego;

namespace TGC.Group.Model.Utiles.Efectos
{
    public class EfectoFuego : Efecto
    {
        #region Constructores

        public EfectoFuego(Effect efectoShader, string tecnica) : base(efectoShader, tecnica)
        {
        }

        #endregion Constructores

        #region Comportamientos

        public override void Actualizar(SuvirvalCraft contexto)
        {
            GetEfectoShader().SetValue("time", contexto.tiempo);
            GetEfectoShader().SetValue("altura", 0);
        }

        public override void Actualizar(SuvirvalCraft contexto, Elemento elemento)
        {
            GetEfectoShader().SetValue("time", contexto.tiempo);
            GetEfectoShader().SetValue("altura", elemento.GetAlturaAnimacion());
        }

        public override void ActualizarRenderizar(SuvirvalCraft contexto, Elemento elemento)
        {
            GetEfectoShader().SetValue("time", contexto.tiempo);
            GetEfectoShader().SetValue("altura", elemento.GetAlturaAnimacion());
            elemento.Mesh.render();
        }

        #endregion Comportamientos
    }
}