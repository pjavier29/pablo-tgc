﻿using AlumnoEjemplos.PabloTGC.Administracion;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;

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
            this.GetEfectoShader().SetValue("altura", 0);
        }

        public override void Actualizar(SuvirvalCraft contexto, Elemento elemento)
        {
            this.GetEfectoShader().SetValue("time", contexto.tiempo);
            this.GetEfectoShader().SetValue("altura", elemento.GetAlturaAnimacion());
        }
        public override void ActualizarRenderizar(SuvirvalCraft contexto, Elemento elemento)
        {
            this.GetEfectoShader().SetValue("time", contexto.tiempo);
            this.GetEfectoShader().SetValue("altura", elemento.GetAlturaAnimacion());
            elemento.Mesh.render();
        }
        #endregion
    }
}
