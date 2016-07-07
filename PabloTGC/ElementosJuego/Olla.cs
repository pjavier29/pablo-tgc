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
    public class Olla : Elemento
    {
        #region Atributos
        private BarraEstado progresoCoccion;
        private float tiempoCoccion;
        private Elemento elementoCoccion;
        private String mensajeInformativo;
        #endregion

        #region Contructores
        public Olla(float peso, float resistencia, TgcMesh mesh, Efecto efecto) :base(peso, resistencia, mesh, efecto)
        {
            this.progresoCoccion = null;
            this.elementoCoccion = null;
            this.tiempoCoccion = 0;
            mensajeInformativo = "";
        }
        #endregion

        #region Comportamientos

        public override void procesarInteraccion(String accion, SuvirvalCraft contexto, float elapsedTime)
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

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            //TODO. Al momento de juntar la hamburguesa no sera eliminado de la coleccion y seguira todo igual (de todas formas por ahora nos sirve)
            if (! this.EstaCocinando())
            {
                return mensajeInformativo;
            }
            return "";
        }

        public override void ProcesarColisionConElemento(Elemento elemento)
        {
            if (elemento.GetTipo().Equals(Alimento))
            {
                if (! this.elementosQueContiene().Contains(elemento))
                {
                    //Le coloca la misma posicion que tiene la olla pero sobre su altura
                    elemento.posicion(this.posicion() + new Vector3(0, 25, 0));
                    this.agregarElemento(elemento);
                    this.elementoCoccion = elemento;
                    this.tiempoCoccion = 0;
                    this.progresoCoccion = new BarraEstado(new Vector3(this.BoundingBox().PMin.X, this.BoundingBox().PMax.Y, this.BoundingBox().PMax.Z),
                        this.BoundingBox().PMax, this.TiempoCoccionElementos());
                }
            }
        }

        public override void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (this.EstaCocinando())
            {
                this.tiempoCoccion += elapsedTime;
                if (this.tiempoCoccion > this.TiempoCoccionElementos())
                {
                    if (this.elementoCoccion != null)
                    {
                        this.EliminarElemento(this.elementoCoccion);
                        this.tiempoCoccion = 0;
                        this.progresoCoccion.Liberar();
                        this.progresoCoccion = null;
                        List<Elemento> contenido = this.elementoCoccion.DestruirSolo();
                        foreach (Elemento cont in contenido)
                        {
                            cont.posicion(this.elementoCoccion.posicion());
                        }
                        //this.AgregarElementos(contenido);
                        contexto.elementos.Remove(this.elementoCoccion);
                        this.elementoCoccion = null;
                        contexto.elementos.AddRange(contenido);
                        contexto.optimizador.ForzarActualizacionElementosColision();
                    }
                }
                else
                {
                    this.progresoCoccion.ActualizarEstado(this.tiempoCoccion);
                }
            }
        }

        /// <summary>
        /// Renderiza el objeto
        /// </summary>
        public override void renderizar(SuvirvalCraft contexto)
        {
            base.renderizar(contexto);
            if (this.EstaCocinando())
            {
                this.progresoCoccion.Render();
            }
        }

        public override bool AdmiteMultipleColision()
        {
            return this.EstaCocinando();
        }

        public override String GetTipo()
        {
            return Olla;
        }

        private float TiempoCoccionElementos()
        {
            return 30;
        }

        private bool EstaCocinando()
        {
            return this.elementoCoccion != null;
        }

        #endregion
    }
}
