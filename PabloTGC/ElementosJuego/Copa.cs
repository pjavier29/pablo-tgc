﻿using AlumnoEjemplos.PabloTGC.Administracion;
using AlumnoEjemplos.PabloTGC.Utiles.Efectos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.ElementosJuego
{
    public class Copa : Elemento
    {
        #region Atributos
        private String mensajeInformativo;
        #endregion

        #region Constructores

        public Copa(float peso, float resistencia, TgcMesh mesh, Efecto efecto) :base(peso, resistencia, mesh, efecto)
        {
            mensajeInformativo = "";
        }

        public Copa(float peso, float resistencia, TgcMesh mesh, Elemento elemento) : base(peso, resistencia, mesh, elemento)
        {
            mensajeInformativo = "";
        }

        #endregion

        #region Comportamientos

        public override void procesarInteraccion(String accion, SuvirvalCraft contexto, float elapsedTime)
        {
            mensajeInformativo = "Juntar (J)";
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
        }

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            return mensajeInformativo;
        }

        public override String GetTipo()
        {
            return Copa;
        }

        #endregion
    }
}
