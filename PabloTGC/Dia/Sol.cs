using AlumnoEjemplos.MiGrupo;
using AlumnoEjemplos.PabloTGC.Movimientos;
using AlumnoEjemplos.PabloTGC.Utiles;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.Dia
{
    public class Sol
    {
        #region Atributos
        private ColorValue colorDeLuz;
        private float intensidadDeLuz;
        private float atenuacionDeLuz;
        private MovimientoEliptico movimientoSol;
        #endregion

        #region Propiedades
        public TgcMesh Mesh { get; set; }
        #endregion

        #region Constructores
        public Sol()
        {
            this.colorDeLuz = ColorValue.FromColor(Color.White);
            this.intensidadDeLuz = 3;
            this.atenuacionDeLuz = 0.5f;
        }
        #endregion

        #region Comportamientos
        public void CrearMovimiento()
        {
            movimientoSol = new MovimientoEliptico(new Vector3(0f, 0f, 0f), new Vector3(12000f, 0f, 0f), new Vector3(0f, 5000f, 0f), this.Mesh);
        }

        public void Actualizar(float valor)
        {
            this.movimientoSol.Actualizar(valor);
        }

        public void Iluminar(Elemento elemento, Personaje personaje)
        {
            elemento.Mesh.Effect.SetValue("lightColor", this.colorDeLuz);
            elemento.Mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(this.Mesh.Position));
            elemento.Mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(personaje.mesh.Position));
            elemento.Mesh.Effect.SetValue("lightIntensity", this.IntensidadDeLuz());
            elemento.Mesh.Effect.SetValue("lightAttenuation", this.Atenuacion());

            elemento.Mesh.Effect.SetValue("materialEmissiveColor", elemento.ColorEmisor());
            elemento.Mesh.Effect.SetValue("materialAmbientColor", elemento.ColorAmbiente());
            elemento.Mesh.Effect.SetValue("materialDiffuseColor", elemento.ColorDifuso());
            elemento.Mesh.Effect.SetValue("materialSpecularColor", elemento.ColorEspecular());
            elemento.Mesh.Effect.SetValue("materialSpecularExp", elemento.EspecularEx());
        }

        private float Atenuacion()
        {
            if (this.Mesh.Position.Y < 350)
            {
                return 2f;
            }
            return this.atenuacionDeLuz;
        }

        private float IntensidadDeLuz()
        {
            if (this.Mesh.Position.Y < 350)
            {
                return 1000f;
            }
            return this.intensidadDeLuz * this.Mesh.Position.Y;
        }

        /// <summary>
        /// retorna la intendidad actual de la luz pero llevada a porcentaje entre 0 y 1
        /// </summary>
        /// <returns></returns>
        public float IntensidadRelativa()
        {
            return FuncionesMatematicas.Instance.PorcentajeRelativo(1000f, this.intensidadDeLuz * this.movimientoSol.AlturaMaxima(), this.IntensidadDeLuz());
        }

        public bool EsDeDia()
        {
            return this.Mesh.Position.Y > 350;
        }
        #endregion

    }
}
