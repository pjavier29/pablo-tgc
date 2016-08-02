using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using TGC.Core.SceneLoader;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosJuego
{
    public class Madera : Elemento
    {
        #region Atributos

        private string mensajeInformativo;

        #endregion Atributos

        #region Contructores

        public Madera(float peso, float resistencia, TgcMesh mesh) : base(peso, resistencia, mesh)
        {
            mensajeInformativo = "";
        }

        public Madera(float peso, float resistencia, TgcMesh mesh, Elemento elemento, Efecto efecto)
            : base(peso, resistencia, mesh, elemento, efecto)
        {
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
            base.procesarInteraccion(accion, contexto, elapsedTime);
            mensajeInformativo = "Juntar(J), Encender(E)";
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
            if (accion.Equals("Encender"))
            {
                if (contexto.personaje.TieneAntorchaSeleccionada())
                {
                    foreach (var elem in elementosQueContiene())
                    {
                        elem.posicion(posicion());
                        elem.Activar();
                        elem.Mesh.BoundingBox.scaleTranslate(posicion(), new Vector3(2f, 0.25f, 2f));
                        contexto.elementos.Add(elem);
                        //TODO. ver si es la mejor forma de manejar los elementos de iluminacion
                        contexto.efectoTerreno.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoLuz.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoAlgas.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoAlgas2.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoBotes.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoArbol.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoArbol2.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        contexto.efectoLuz2.AgregarElementoDeIluminacion(new ElementoIluminacion(elem, 1000));
                        //TODO+++++++++++++++++++++++++++++++++
                    }
                    liberar();
                    contexto.elementos.Remove(this);
                    contexto.optimizador.ForzarActualizacionElementosColision();
                }
                else
                {
                    mensajeInformativo = "Debe Utilizar Fuego";
                }
            }
        }

        public override string getAcciones()
        {
            //TODO. Mejorar esta lógica
            //return "Juntar (J), Encender (E)";
            return mensajeInformativo;
        }

        public override string GetTipo()
        {
            return Madera;
        }

        #endregion Comportamientos
    }
}