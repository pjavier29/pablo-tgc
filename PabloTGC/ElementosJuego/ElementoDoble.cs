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
    public class ElementoDoble : Elemento
    {
        #region Atributos
        private bool estaCreando;
        private bool tieneQueCrear;
        private BarraEstado progresoCreacion;
        private float tiempoCreacion;
        #endregion

        #region Propiedades
        public TgcMesh Mesh1 { get; set; }
        public TgcMesh Mesh2 { get; set; }
        #endregion

        #region constructores
        public ElementoDoble(float peso, float resistencia, TgcMesh mesh1, TgcMesh mesh2, Efecto efecto) : base(peso, resistencia, mesh1, efecto)
        {
            //Por defecto comienza mostrando el primer mesh
            this.Mesh1 = mesh1;
            this.Mesh2 = mesh2;
            this.estaCreando = false;
            this.tieneQueCrear = false;
            this.progresoCreacion = null;
            this.tiempoCreacion = 0;
        }
        #endregion

        #region Comportamientos
        public override void procesarInteraccion(String accion, SuvirvalCraft contexto, float elapsedTime)
        {
            base.procesarInteraccion(accion, contexto, elapsedTime);

            if (accion.Equals("Juntar"))
            {
                if ((this.elementosQueContiene().Count > 0) && !(this.estaCreando))
                {
                    //Si tiene elementos para dar
                    Elemento elem = this.elementosQueContiene()[0];
                    contexto.personaje.juntar(elem);
                    this.EliminarElemento(elem);
                    this.Mesh = this.Mesh2;
                    this.tieneQueCrear = true;
                }
            }
            if (accion.Equals("Consumir"))
            {
                if ((this.elementosQueContiene().Count > 0) && !(this.estaCreando))
                {
                    //Si tiene elementos para dar
                    Elemento elem = this.elementosQueContiene()[0];
                    if (elem.EsDeTipo(Alimento))
                    {
                        Alimento ali = (Alimento)elem;
                        contexto.personaje.ConsumirAlimento(ali.GetNutricion());
                        this.EliminarElemento(elem);
                        this.Mesh = this.Mesh2;
                        this.tieneQueCrear = true;
                    }
                }
            }
        }

        public override void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
            base.Actualizar(contexto, elapsedTime);
            if (this.estaCreando)
            {
                this.tiempoCreacion += elapsedTime;
                if (this.tiempoCreacion > this.TiempoTotalCreacion())
                {
                    this.progresoCreacion.Liberar();
                    this.progresoCreacion = null;
                    this.tiempoCreacion = 0;
                    this.estaCreando = false;
                    this.tieneQueCrear = false;
                    this.Mesh = this.Mesh1;
                }
                else
                {
                    this.progresoCreacion.ActualizarEstado(this.tiempoCreacion);
                }
            }
            else
            {
                if (this.tieneQueCrear && this.elementosQueContiene().Count > 0)
                {
                    this.progresoCreacion = new BarraEstado(new Vector3(this.BoundingBox().PMin.X, this.BoundingBox().PMax.Y, this.BoundingBox().PMax.Z),
                                this.BoundingBox().PMax, this.TiempoTotalCreacion());
                    this.estaCreando = true;
                    this.tieneQueCrear = false;
                }
            }
        }

        /// <summary>
        /// Renderiza el objeto
        /// </summary>
        public override void renderizar(SuvirvalCraft contexto)
        {
            base.renderizar(contexto);
            if (this.estaCreando)
            {
                this.progresoCreacion.Render();
            }
        }

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            if (estaCreando)
            {
                return "Creación de nuevos elementos";
            }
            else
            {
                if (this.elementosQueContiene().Count > 0)
                {
                    //Si aun tiene elementos para entregar sigue procesando 
                    return "Juntar (J), Consumir (C)";
                }
                else
                {
                    return "Sin elementos";
                }
            }
        }

        public override String GetTipo()
        {
            return ElementoDoble;
        }

        private float TiempoTotalCreacion()
        {
            return 60;
        }
        #endregion
    }
}
