using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TGC.Core.SceneLoader;
using TGC.Core.Sound;
using TGC.Core.Utils;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosJuego
{
    public class Fuego : Elemento
    {
        #region Atributos

        private Tgc3dSound sonido;

        #endregion Atributos

        #region Contructores

        public Fuego(float peso, float resistencia, TgcMesh mesh) : base(peso, resistencia, mesh)
        {
        }

        public Fuego(float peso, float resistencia, TgcMesh mesh, Efecto efecto, Tgc3dSound sonidoFuego) : base(peso, resistencia, mesh, efecto)
        {
            sonido = sonidoFuego;
        }

        #endregion Contructores

        #region Comportamientos

        /// <summary>
        /// Procesa una colisión cuando el personaje colisiona contra el fuego
        /// </summary>
        public override void procesarColision(Personaje personaje, float elapsedTime, List<Elemento> elementos, float moveForward, Vector3 movementVector, Vector3 lastPos)
        {
            if (this.distanciaA(personaje.mesh.Position) > 20)
            {
                //Cerca del fuego se genera un anmbiente de 24 grados.
                personaje.IncrementarTemperaturaCorporalPorTiempo(24, elapsedTime);
            }
            else
            {
                personaje.morir();
            }
        }

        public override void procesarInteraccion(String accion, SuvirvalCraft contexto, float elapsedTime)
        {
            //En el fuego no queremos que se muestre barra de estado.
            if (accion.Equals("Parado"))
            {
                //Cerca del fuego se genera un anmbiente de 24 grados.
                contexto.personaje.IncrementarTemperaturaCorporalPorTiempo(24, elapsedTime);
            }
        }

        public override void ProcesarColisionConElemento(Elemento elemento)
        {
            if (elemento.GetTipo().Equals(Olla))
            {
                //Le coloca la misma posicion que tiene el fuego pero sobre su altura
                elemento.posicion(this.posicion() + new Vector3(0, this.BoundingBox().PMax.Y - this.BoundingBox().PMin.Y, 0));
                //Agrega el elemento a su lista
                this.agregarElemento(elemento);
            }
            if (elemento.GetTipo().Equals(Alimento))
            {
                if (this.elementosQueContiene().Count > 0)
                {//Por el momento asumimos que esta cocinando si tiene un elementos
                    foreach (Elemento elem in this.elementosQueContiene())
                    {
                        if (elem.GetTipo().Equals(Olla))
                        {
                            elem.ProcesarColisionConElemento(elemento);
                        }
                    }
                }
            }
        }

        public override bool AdmiteMultipleColision()
        {
            return true;
        }

        public override String GetTipo()
        {
            return Fuego;
        }

        public override void Iluminar(Efecto efecto, Vector3 posicionVision, ColorValue colorEmisor, ColorValue colorAmbiente,
            ColorValue colorDifuso, ColorValue colorEspecular, float especularEx)
        {
            efecto.GetEfectoShader().SetValue("lightColor", ColorValue.FromColor(Color.LightYellow));
            efecto.GetEfectoShader().SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(this.Mesh.Position));
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
            return (this.Mesh.BoundingBox.PMax.Y - this.Mesh.BoundingBox.PMin.Y) * 0.2f;
        }

        public override void destruir()
        {
            base.destruir();
            sonido.dispose();
        }

        public override void Activar()
        {
            sonido.Position = this.Mesh.Position;
            sonido.play(true);
        }

        #endregion Comportamientos
    }
}