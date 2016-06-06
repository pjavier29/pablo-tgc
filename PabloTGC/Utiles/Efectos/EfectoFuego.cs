using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.PabloTGC.Utiles.Efectos
{
    public class EfectoFuego : Efecto
    {
        #region Atributos
        #endregion

        #region Constructores
        public EfectoFuego(Effect efectoShader, String tecnica): base(efectoShader, tecnica)
        {
        }
        #endregion

        #region Comportamientos
        public override void Actualizar(SuvirvalCraft contexto)
        {
            this.GetEfectoShader().SetValue("time", contexto.tiempo);
        }

        public override void Actualizar(SuvirvalCraft contexto, Elemento elemento)
        {
            this.GetEfectoShader().SetValue("time", contexto.tiempo);
        }
        #endregion
    }
}
