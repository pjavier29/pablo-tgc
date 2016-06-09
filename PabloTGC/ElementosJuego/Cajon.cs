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
    public class Cajon : Elemento
    {
        #region Atributos
        private bool estaAbierto;
        private BarraEstado progresoApertura;
        private float tiempoApertura;
        #endregion

        #region Contructores
        public Cajon(float peso, float resistencia, TgcMesh mesh, Efecto efecto) : base(peso, resistencia, mesh, efecto)
        {
            this.estaAbierto = false;
            this.progresoApertura = null;
            this.tiempoApertura = 0;
        }
        #endregion

        #region Comportamientos

        public override void procesarInteraccion(String accion, SuvirvalCraft contexto, float elapsedTime)
        {
            base.procesarInteraccion(accion, contexto, elapsedTime);
            //TODO. Podria agregarse para que se cambie el mesh del cajon y se muestre cerrado o abierto.
            if (!this.estaAbierto)
            {
                if (accion.Equals("Abrir"))
                {
                    if (! this.SeEstaAbriendo())
                    {
                        this.progresoApertura = new BarraEstado(new Vector3(this.BoundingBox().PMin.X, this.BoundingBox().PMax.Y, this.BoundingBox().PMax.Z),
                                this.BoundingBox().PMax, this.TiempoTotalApertura());
                    }
                }
            }
            else
            {
                //TODO. Por el momento podemos mantener todo en un renglon ya que no imprimimos ninguna imagen de los elementos en cuestion
                contexto.cajonReglon1.Text = "";
                contexto.mostrarMenuCajon = true;
                contexto.mostrarMenuMochila = true;//Porque la mochila y el cajon se muestran juntos
                //TODO. Agregar despues validacion para que el cajon no admita mas de 9 posiciones
                int i = 0;
                foreach(Elemento cont in this.elementosQueContiene())
                {
                    contexto.cajonReglon1.Text = contexto.cajonReglon1.Text + (i + 1).ToString() + "    " + cont.GetDescripcion() + System.Environment.NewLine;
                    i++;
                }

                if (accion.Equals("Juntar Todo"))
                {
                    List<Elemento> auxiliar = new List<Elemento>();
                    foreach (Elemento elem in this.elementosQueContiene())
                    {
                        if (!contexto.personaje.ContieneElementoEnMochila(elem))
                        {
                            contexto.personaje.juntar(elem);
                         auxiliar.Add(elem);
                        }
                    }
                    this.EliminarElementos(auxiliar);
                }
                if (accion.Equals("Dejar Todo"))
                {
                    List<Elemento> auxiliar = new List<Elemento>();
                    foreach (Elemento elem in contexto.personaje.elementosEnMochila())
                    {
                        if (!this.elementosQueContiene().Contains(elem))
                        {
                            this.agregarElemento(elem);
                            auxiliar.Add(elem);
                        }
                    }
                    contexto.personaje.DejarElementos(auxiliar);
                }
            }
        }

        public override void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
            base.Actualizar(contexto, elapsedTime);
            if (this.SeEstaAbriendo())
            {
                this.tiempoApertura += elapsedTime;
                if (this.tiempoApertura > this.TiempoTotalApertura())
                {
                    this.progresoApertura.Liberar();
                    this.progresoApertura = null;
                    this.tiempoApertura = 0;
                    this.estaAbierto = true;
                }
                else
                {
                    this.progresoApertura.ActualizarEstado(this.tiempoApertura);
                }
            }
        }

        /// <summary>
        /// Renderiza el objeto
        /// </summary>
        public override void renderizar(SuvirvalCraft contexto)
        {
            base.renderizar(contexto);
            if (this.SeEstaAbriendo())
            {
                this.progresoApertura.Render();
            }
        }

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            if (estaAbierto)
            {
                return " Juntar Todo (Y), Dejar Todo (U)";
            }
            else if(! this.SeEstaAbriendo())
            {
                return "Abrir (B)";
            }
            return "";
        }

        public override String GetTipo()
        {
            return Cajon;
        }

        private float TiempoTotalApertura()
        {
            return 2;
        }

        private bool SeEstaAbriendo()
        {
            return this.progresoApertura != null;
        }

        #endregion
    }
}
