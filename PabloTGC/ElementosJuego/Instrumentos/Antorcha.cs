using AlumnoEjemplos.PabloTGC.Administracion;
using AlumnoEjemplos.PabloTGC.Utiles.Efectos;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using TgcViewer.Utils.Sound;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.ElementosJuego.Instrumentos
{
    public class Antorcha : Elemento
    {
        #region Atributos
        private Vector3 posicionActual;
        public TgcStaticSound sonidoAntorcha;
        private bool estaActivada;
        #endregion

        #region Constructores
        public Antorcha()
        {
            estaActivada = false;
        }

        public Antorcha(float peso, float resistencia, TgcMesh mesh) : base(peso, resistencia, mesh)
        {
            estaActivada = false;
        }
        public Antorcha(float peso, float resistencia, TgcMesh mesh, Efecto efecto) : base(peso, resistencia, mesh, efecto)
        {
            estaActivada = false;
        }
        #endregion

        #region Comportamientos
        public override void procesarColision(Personaje personaje, float elapsedTime, List<Elemento> elementos, float moveForward, Vector3 movementVector, Vector3 lastPos)
        {
        }

        public override void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
            this.SetPosicion(contexto.personaje.mesh.Position);
        }

        public override void procesarInteraccion(String accion, SuvirvalCraft contexto, float elapsedTime)
        {

        }

        public override void ProcesarColisionConElemento(Elemento elemento)
        {

        }

        public void SetPosicion(Vector3 posicion)
        {
            this.posicionActual = posicion;
        }

        public override Vector3 posicion()
        {
            return this.posicionActual;
        }

        public override String GetTipo()
        {
            return Antorcha;
        }

        public override void Iluminar(Efecto efecto, Vector3 posicionVision, ColorValue colorEmisor, ColorValue colorAmbiente,
            ColorValue colorDifuso, ColorValue colorEspecular, float especularEx)
        {
            efecto.GetEfectoShader().SetValue("lightColor", ColorValue.FromColor(Color.LightYellow));
            efecto.GetEfectoShader().SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(this.posicion()));
            efecto.GetEfectoShader().SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(posicionVision));
            efecto.GetEfectoShader().SetValue("lightIntensity", 100f);
            efecto.GetEfectoShader().SetValue("lightAttenuation", 0.5f);
            efecto.GetEfectoShader().SetValue("materialEmissiveColor", colorEmisor);
            efecto.GetEfectoShader().SetValue("materialAmbientColor", colorAmbiente);
            efecto.GetEfectoShader().SetValue("materialDiffuseColor", colorDifuso);
            efecto.GetEfectoShader().SetValue("materialSpecularColor", colorEspecular);
            efecto.GetEfectoShader().SetValue("materialSpecularExp", especularEx);
        }

        public override float GetAlturaAnimacion()
        {
            return (this.Mesh.BoundingBox.PMax.Y - this.Mesh.BoundingBox.PMin.Y) * 0.9f;
        }

        public override void Activar()
        {
            this.estaActivada = true;
            this.sonidoAntorcha.play(true);
        }

        public override void Desactivar()
        {
            this.estaActivada = false;
            this.sonidoAntorcha.stop();
        }
        #endregion
    }
}
