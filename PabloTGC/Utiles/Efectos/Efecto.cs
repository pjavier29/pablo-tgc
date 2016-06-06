using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.Utiles.Efectos
{
    public class Efecto
    {
        #region Atributos
        private Effect efectoShader;
        private String tecnica;
        #endregion

        #region Constructores
        public Efecto(Effect efectoShader, String tecnica)
        {
            this.efectoShader = efectoShader;
            this.tecnica = tecnica;
        }

        public Efecto(Effect efectoShader)
        {
            this.efectoShader = efectoShader;
        }
        #endregion

        #region Comportamientos
        public virtual Effect GetEfectoShader()
        {
            return this.efectoShader;
        }

        public virtual String Tecnica()
        {
            return this.tecnica;
        }

        public virtual void Tecnica(String tecnica)
        {
            this.tecnica = tecnica;
        }

        /// <summary>
        /// Para el caso que el mesh haya cambiado de efecto, debemos aplicar todo de nuevo
        /// </summary>
        /// <param name="mesh"></param>
        public void Aplicar(TgcMesh mesh, SuvirvalCraft contexto)
        {
            mesh.Effect = this.efectoShader;
            mesh.Technique = this.tecnica;
            this.Actualizar(contexto);
        }

        public void Aplicar(TgcMesh mesh)
        {
            mesh.Effect = this.efectoShader;
            mesh.Technique = this.tecnica;
        }

        public virtual void Actualizar(SuvirvalCraft contexto)
        {
        }

        public virtual void Actualizar(SuvirvalCraft contexto, Elemento elemento)
        {
        }
        #endregion
    }
}
