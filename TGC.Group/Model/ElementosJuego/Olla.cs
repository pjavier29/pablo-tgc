using Microsoft.DirectX;
using System;
using TGC.Core.SceneLoader;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosJuego
{
    public class Olla : Elemento
    {
        #region Contructores

        public Olla(float peso, float resistencia, TgcMesh mesh, Efecto efecto) : base(peso, resistencia, mesh, efecto)
        {
            progresoCoccion = null;
            elementoCoccion = null;
            tiempoCoccion = 0;
            mensajeInformativo = "";
        }

        #endregion Contructores

        #region Atributos

        private BarraEstado progresoCoccion;
        private float tiempoCoccion;
        private Elemento elementoCoccion;
        private string mensajeInformativo;

        #endregion Atributos

        #region Comportamientos

        public override void procesarInteraccion(string accion, SuvirvalCraft contexto, float elapsedTime)
        {
            mensajeInformativo = "Juntar (J)";
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

        public override string getAcciones()
        {
            //TODO. Mejorar esta lógica
            //TODO. Al momento de juntar la hamburguesa no sera eliminado de la coleccion y seguira todo igual (de todas formas por ahora nos sirve)
            if (!EstaCocinando())
            {
                return mensajeInformativo;
            }
            return "";
        }

        public override void ProcesarColisionConElemento(Elemento elemento)
        {
            if (elemento.GetTipo().Equals(Alimento))
            {
                if (!elementosQueContiene().Contains(elemento))
                {
                    //Le coloca la misma posicion que tiene la olla pero sobre su altura
                    elemento.posicion(posicion() + new Vector3(0, 25, 0));
                    agregarElemento(elemento);
                    elementoCoccion = elemento;
                    tiempoCoccion = 0;
                    progresoCoccion =
                        new BarraEstado(new Vector3(BoundingBox().PMin.X, BoundingBox().PMax.Y, BoundingBox().PMax.Z),
                            BoundingBox().PMax, TiempoCoccionElementos());
                }
            }
        }

        public override void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (EstaCocinando())
            {
                tiempoCoccion += elapsedTime;
                if (tiempoCoccion > TiempoCoccionElementos())
                {
                    if (elementoCoccion != null)
                    {
                        EliminarElemento(elementoCoccion);
                        tiempoCoccion = 0;
                        progresoCoccion.Liberar();
                        progresoCoccion = null;
                        var contenido = elementoCoccion.DestruirSolo();
                        foreach (var cont in contenido)
                        {
                            cont.posicion(elementoCoccion.posicion());
                        }
                        //this.AgregarElementos(contenido);
                        contexto.elementos.Remove(elementoCoccion);
                        elementoCoccion = null;
                        contexto.elementos.AddRange(contenido);
                        contexto.optimizador.ForzarActualizacionElementosColision();
                    }
                }
                else
                {
                    progresoCoccion.ActualizarEstado(tiempoCoccion);
                }
            }
        }

        /// <summary>
        ///     Renderiza el objeto
        /// </summary>
        public override void renderizar(SuvirvalCraft contexto)
        {
            base.renderizar(contexto);
            if (EstaCocinando())
            {
                progresoCoccion.Render();
            }
        }

        public override bool AdmiteMultipleColision()
        {
            return EstaCocinando();
        }

        public override string GetTipo()
        {
            return Olla;
        }

        private float TiempoCoccionElementos()
        {
            return 30;
        }

        private bool EstaCocinando()
        {
            return elementoCoccion != null;
        }

        #endregion Comportamientos
    }
}