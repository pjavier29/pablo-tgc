using AlumnoEjemplos.PabloTGC.Administracion;
using AlumnoEjemplos.PabloTGC.Utiles.Efectos;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.ElementosJuego
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
        #endregion

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
        #endregion
    }
}
