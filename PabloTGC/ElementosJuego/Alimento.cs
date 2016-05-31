using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.ElementosJuego
{
    public class Alimento : Elemento
    {
        #region Atributos
        private float nutricion;
        #endregion

        #region Contructores
        public Alimento(float peso, float resistencia, TgcMesh mesh, float nutricion) :base(peso, resistencia, mesh)
        {
            this.nutricion = nutricion;
        }

        public Alimento(float peso, float resistencia, TgcMesh mesh, Elemento elemento, float nutricion) : base(peso, resistencia, mesh, elemento)
        {
            this.nutricion = nutricion;
        }

        #endregion

        #region Comportamientos

        /// <summary>
        /// Procesa una colisión cuando el personaje colisiona contra un pedazo de madera
        /// </summary>
        public override void procesarColision(Personaje personaje, float elapsedTime, List<Elemento> elementos, float moveForward, Vector3 movementVector, Vector3 lastPos)
        {
        }

        public override void procesarInteraccion(String accion, SuvirvalCraft contexto, float elapsedTime)
        {
            base.procesarInteraccion(accion, contexto, elapsedTime);
            if (accion.Equals("Juntar"))
            {
                //TODO. Esta validacion es porque se ejecuta muchas veces al presionar la tecla. Se deberia solucioanr cuando implementemos los comandos
                if (!contexto.personaje.ContieneElementoEnMochila(this))
                {
                    contexto.personaje.juntar(this);
                    contexto.elementos.Remove(this);
                    contexto.optimizador.ForzarActualizacion();
                }
            }
            if (accion.Equals("Consumir"))
            {
                contexto.personaje.ConsumirAlimento(this.nutricion);
                this.liberar();
                contexto.elementos.Remove(this);
                contexto.optimizador.ForzarActualizacion();
            }
        }

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            return "Juntar (J), Consumir (C)";
        }

        public override String GetTipo()
        {
            return Alimento;
        }

        public override String GetDescripcion()
        {
            return this.nombre() + " - " + this.nutricion;
        }

        #endregion
    }

}
