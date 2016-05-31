using AlumnoEjemplos.MiGrupo;
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
        public ElementoSinInteraccion(float peso, float resistencia, TgcMesh mesh, Effect efecto) : base(peso, resistencia, mesh, efecto)
        {

        }
        #endregion

        #region Comportamientos
        public override void procesarColision(Personaje personaje, float elapsedTime, List<Elemento> elementos, float moveForward, Vector3 movementVector, Vector3 lastPos)
        {
        }

        public override void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (this.tieneEfecto)
            {
                //TODO. Esto debe de manejarse de otra manera para que se pueda setear el valor que corresponda de manera mas generica.
                this.Efecto().SetValue("time", contexto.tiempo);
            }
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
