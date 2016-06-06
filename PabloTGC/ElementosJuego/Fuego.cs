﻿using AlumnoEjemplos.MiGrupo;
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
    public class Fuego : Elemento
    {
        #region Atributos
        #endregion

        #region Contructores
        public Fuego(float peso, float resistencia, TgcMesh mesh) :base(peso, resistencia, mesh)
        {

        }

        public Fuego(float peso, float resistencia, TgcMesh mesh, Efecto efecto) : base(peso, resistencia, mesh, efecto)
        {

        }

        #endregion

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
                if(this.elementosQueContiene().Count > 0)
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

        #endregion
    }
}
