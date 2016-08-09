using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using TGC.Core.SceneLoader;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosJuego
{
    public class Alimento : Elemento
    {
        #region Atributos

        private readonly float nutricion;
        private string mensajeInformativo;

        #endregion Atributos

        #region Contructores

        public Alimento(float peso, float resistencia, TgcMesh mesh, float nutricion, Efecto efecto)
            : base(peso, resistencia, mesh, efecto)
        {
            this.nutricion = nutricion;
            mensajeInformativo = "";
        }

        public Alimento(float peso, float resistencia, TgcMesh mesh, Elemento elemento, float nutricion, Efecto efecto)
            : base(peso, resistencia, mesh, elemento, efecto)
        {
            this.nutricion = nutricion;
            mensajeInformativo = "";
        }

        #endregion Contructores

        #region Comportamientos

        /// <summary>
        ///     Procesa una colisión cuando el personaje colisiona contra un pedazo de madera
        /// </summary>
        public override void procesarColision(Personaje personaje, float elapsedTime, List<Elemento> elementos,
            float moveForward, Vector3 movementVector, Vector3 lastPos)
        {
        }

        public override void procesarInteraccion(string accion, SuvirvalCraft contexto, float elapsedTime)
        {
            mensajeInformativo = "Juntar (J), Consumir (C)";
            base.procesarInteraccion(accion, contexto, elapsedTime);
            if (accion.Equals("Juntar"))
            {
                //TODO. Esta validacion es porque se ejecuta muchas veces al presionar la tecla. Se deberia solucioanr cuando implementemos los comandos
                if (!contexto.personaje.ContieneElementoEnMochila(this))
                {
                    try
                    {
                        contexto.personaje.juntar(this);
                        contexto.elementos.Remove(this);
                        contexto.optimizador.ForzarActualizacionElementosColision();
                    }
                    catch (Exception ex)
                    {
                        mensajeInformativo = ex.Message;
                    }
                }
            }
            if (accion.Equals("Consumir"))
            {
                contexto.personaje.ConsumirAlimento(nutricion);
                liberar();
                contexto.elementos.Remove(this);
                contexto.optimizador.ForzarActualizacionElementosColision();
            }
        }

        public override string getAcciones()
        {
            return mensajeInformativo;
        }

        public override string GetTipo()
        {
            return Alimento;
        }

        public override string GetDescripcion()
        {
            return nombre() + " - " + nutricion;
        }

        public float GetNutricion()
        {
            return nutricion;
        }

        #endregion Comportamientos
    }
}