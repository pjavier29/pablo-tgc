using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public override void Actualizar(SuvirvalCraft contexto)
        {
            this.GetEfectoShader().SetValue("time", contexto.tiempo);
            this.GetEfectoShader().SetValue("lightIntensityRelitive", contexto.dia.GetSol().IntensidadRelativa());
        }
        #endregion
    }
}
