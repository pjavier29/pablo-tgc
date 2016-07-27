using Microsoft.DirectX.Direct3D;
using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Utiles.Efectos
{
    public class EfectoAgua : Efecto
    {
        #region Constructores

        public EfectoAgua(Effect efectoShader, string tecnica) : base(efectoShader, tecnica)
        {
        }

        #endregion Constructores

        #region Comportamientos

        public override void Actualizar(SuvirvalCraft contexto)
        {
            GetEfectoShader().SetValue("time", contexto.tiempo);
            GetEfectoShader().SetValue("lightIntensityRelitive", contexto.dia.GetSol().IntensidadRelativa());
        }

        #endregion Comportamientos
    }
}