using Microsoft.DirectX.Direct3D;
using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Utiles.Efectos
{
    public class EfectoSkyBox : Efecto
    {
        #region Constructores

        public EfectoSkyBox(Effect efectoShader, string tecnica) : base(efectoShader, tecnica)
        {
        }

        #endregion Constructores

        #region Comportamientos

        public override void Actualizar(SuvirvalCraft contexto)
        {
            GetEfectoShader().SetValue("time", contexto.tiempo);
            GetEfectoShader().SetValue("lightIntensityRelitive", contexto.dia.GetSol().IntensidadRelativa());
            //Necesitamos rojo y verde para formar el rojo en si mismo y tambien el amarillo
            GetEfectoShader().SetValue("colorCielo", contexto.dia.GetSol().GetColorAmanecerAnochecer());
            GetEfectoShader().SetValue("intendidadRayo", contexto.dia.GetLluvia().GetIntensidadRayo(contexto.tiempo));
        }

        #endregion Comportamientos
    }
}