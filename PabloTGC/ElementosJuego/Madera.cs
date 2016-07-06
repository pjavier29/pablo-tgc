using AlumnoEjemplos.PabloTGC.Administracion;
using AlumnoEjemplos.PabloTGC.Utiles;
using AlumnoEjemplos.PabloTGC.Utiles.Efectos;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.ElementosJuego
{
    public class Madera : Elemento
    {
        #region Atributos
        private String mensajeInformativo;
        #endregion

        #region Contructores
        public Madera(float peso, float resistencia, TgcMesh mesh) :base(peso, resistencia, mesh)
        {
            mensajeInformativo = "";
        }

        public Madera(float peso, float resistencia, TgcMesh mesh, Elemento elemento, Efecto efecto) : base(peso, resistencia, mesh, elemento, efecto)
        {
            mensajeInformativo = "";
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
            mensajeInformativo = "Juntar(J), Encender(E)";
            if (accion.Equals("Juntar"))
            {
                //TODO. Esta validacion es porque se ejecuta muchas veces al presionar la tecla. Se deberia solucioanr cuando implementemos los comandos
                if (! contexto.personaje.ContieneElementoEnMochila(this))
                {
                    contexto.personaje.juntar(this);
                    contexto.elementos.Remove(this);
                    contexto.optimizador.ForzarActualizacionElementosColision();
                }
            }
            if (accion.Equals("Encender"))
            {
                if (contexto.personaje.TieneAntorchaSeleccionada())
                {
                    foreach (Elemento elem in this.elementosQueContiene())
                    {
                        elem.posicion(this.posicion());
                        elem.Mesh.BoundingBox.scaleTranslate(this.posicion(), new Vector3(2f, 0.25f, 2f));
                        contexto.elementos.Add(elem);
                        //TODO. ver si es la mejor forma de manejar los elementos de iluminacion
                        contexto.efectoTerreno.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoLuz.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoAlgas.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoAlgas2.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoBotes.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoArbol.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoArbol2.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        //TODO+++++++++++++++++++++++++++++++++
                    }
                    this.liberar();
                    contexto.elementos.Remove(this);
                    contexto.optimizador.ForzarActualizacionElementosColision();
                }
                else
                {
                    mensajeInformativo = "Debe Utilizar Fuego";
                }
            }
        }

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            //return "Juntar (J), Encender (E)";
            return mensajeInformativo;
        }

        public override String GetTipo()
        {
            return Madera;
        }

        #endregion
    }
}
