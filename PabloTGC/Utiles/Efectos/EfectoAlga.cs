using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.Utiles.Efectos
{
    public class EfectoAlga : Efecto
    {
        #region Atributos
        #endregion

        #region Constructores
        public EfectoAlga(Effect efectoShader, String tecnica): base(efectoShader, tecnica)
        {
        }
        #endregion

        #region Comportamientos
        public override void Actualizar(SuvirvalCraft contexto, Elemento elemento)
        {
            //TODO. Refactorizar los parametros que recibe!!!!
            this.GetEfectoShader().SetValue("time", contexto.tiempo);
            this.GetEfectoShader().SetValue("lightColor", ColorValue.FromColor(Color.White));
            this.GetEfectoShader().SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(contexto.dia.GetSol().Mesh.Position));
            this.GetEfectoShader().SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(contexto.personaje.mesh.Position));
            this.GetEfectoShader().SetValue("lightIntensity", contexto.dia.GetSol().IntensidadDeLuz());
            this.GetEfectoShader().SetValue("lightAttenuation", contexto.dia.GetSol().Atenuacion());
            this.GetEfectoShader().SetValue("materialEmissiveColor", elemento.ColorEmisor());
            this.GetEfectoShader().SetValue("materialAmbientColor", elemento.ColorEmisor());
            this.GetEfectoShader().SetValue("materialDiffuseColor", elemento.ColorAmbiente());
            this.GetEfectoShader().SetValue("materialSpecularColor", elemento.ColorEspecular());
            this.GetEfectoShader().SetValue("materialSpecularExp", elemento.EspecularEx());
        }
        #endregion
    }
}
