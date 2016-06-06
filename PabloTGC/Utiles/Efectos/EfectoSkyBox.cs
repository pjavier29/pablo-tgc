using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.PabloTGC.Utiles.Efectos
{
    public class EfectoSkyBox : Efecto
    {
        #region Atributos
        #endregion

        #region Constructores
        public EfectoSkyBox(Effect efectoShader, String tecnica): base(efectoShader, tecnica)
        {
        }
        #endregion

        #region Comportamientos
        public override void Actualizar(SuvirvalCraft contexto)
        {
            this.GetEfectoShader().SetValue("time", contexto.tiempo);
            this.GetEfectoShader().SetValue("lightIntensityRelitive", contexto.dia.GetSol().IntensidadRelativa());
            //Necesitamos rojo y verde para formar el rojo en si mismo y tambien el amarillo
            ColorValue color = contexto.dia.GetSol().GetColorAmanecerAnochecer();
            this.GetEfectoShader().SetValue("colorCieloRojo", color.Red);
            this.GetEfectoShader().SetValue("colorCieloVerde", color.Green);
        }
        #endregion
    }
}
