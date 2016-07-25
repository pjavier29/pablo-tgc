using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using TGC.Core.SceneLoader;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosJuego
{
    public class ElementoSinInteraccion : Elemento
    {
        #region Constructores

        public ElementoSinInteraccion(float peso, float resistencia, TgcMesh mesh) : base(peso, resistencia, mesh)
        {
        }

        public ElementoSinInteraccion(float peso, float resistencia, TgcMesh mesh, Efecto efecto) : base(peso, resistencia, mesh, efecto)
        {
        }

        #endregion Constructores

        #region Comportamientos

        public override void procesarColision(Personaje personaje, float elapsedTime, List<Elemento> elementos, float moveForward, Vector3 movementVector, Vector3 lastPos)
        {
        }

        public override void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
        }

        public override void procesarInteraccion(String accion, SuvirvalCraft contexto, float elapsedTime)
        {
        }

        public override void ProcesarColisionConElemento(Elemento elemento)
        {
        }

        #endregion Comportamientos
    }
}