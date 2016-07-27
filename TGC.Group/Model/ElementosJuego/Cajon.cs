using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using TGC.Core.SceneLoader;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosJuego
{
    public class Cajon : Elemento
    {
        #region Contructores

        public Cajon(float peso, float resistencia, TgcMesh mesh, Efecto efecto) : base(peso, resistencia, mesh, efecto)
        {
            estaAbierto = false;
            progresoApertura = null;
            tiempoApertura = 0;
            mensajeInformativo = "";
        }

        #endregion Contructores

        #region Atributos

        private bool estaAbierto;
        private BarraEstado progresoApertura;
        private float tiempoApertura;
        private string mensajeInformativo;

        #endregion Atributos

        #region Comportamientos

        public override void procesarInteraccion(string accion, SuvirvalCraft contexto, float elapsedTime)
        {
            mensajeInformativo = " Juntar Todo (Y), Dejar Todo (U)";
            base.procesarInteraccion(accion, contexto, elapsedTime);
            //TODO. Podria agregarse para que se cambie el mesh del cajon y se muestre cerrado o abierto.
            if (!estaAbierto)
            {
                if (accion.Equals("Abrir"))
                {
                    if (!SeEstaAbriendo())
                    {
                        progresoApertura =
                            new BarraEstado(
                                new Vector3(BoundingBox().PMin.X, BoundingBox().PMax.Y, BoundingBox().PMax.Z),
                                BoundingBox().PMax, TiempoTotalApertura());
                    }
                }
            }
            else
            {
                //TODO. Por el momento podemos mantener todo en un renglon ya que no imprimimos ninguna imagen de los elementos en cuestion
                contexto.cajonReglon1.Text = "";
                contexto.mostrarMenuCajon = true;
                contexto.mostrarMenuMochila = true; //Porque la mochila y el cajon se muestran juntos
                //TODO. Agregar despues validacion para que el cajon no admita mas de 9 posiciones
                var i = 0;
                foreach (var cont in elementosQueContiene())
                {
                    contexto.cajonReglon1.Text = contexto.cajonReglon1.Text + (i + 1) + "    " + cont.GetDescripcion() +
                                                 Environment.NewLine;
                    i++;
                }

                if (accion.Equals("Juntar Todo"))
                {
                    var auxiliar = new List<Elemento>();
                    foreach (var elem in elementosQueContiene())
                    {
                        if (!contexto.personaje.ContieneElementoEnMochila(elem))
                        {
                            try
                            {
                                contexto.personaje.juntar(elem);
                                auxiliar.Add(elem);
                            }
                            catch (Exception ex)
                            {
                                mensajeInformativo = ex.Message;
                            }
                        }
                    }
                    EliminarElementos(auxiliar);
                }
                if (accion.Equals("Dejar Todo"))
                {
                    var auxiliar = new List<Elemento>();
                    foreach (var elem in contexto.personaje.elementosEnMochila())
                    {
                        if (!elementosQueContiene().Contains(elem))
                        {
                            agregarElemento(elem);
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
            if (SeEstaAbriendo())
            {
                tiempoApertura += elapsedTime;
                if (tiempoApertura > TiempoTotalApertura())
                {
                    progresoApertura.Liberar();
                    progresoApertura = null;
                    tiempoApertura = 0;
                    estaAbierto = true;
                }
                else
                {
                    progresoApertura.ActualizarEstado(tiempoApertura);
                }
            }
        }

        /// <summary>
        ///     Renderiza el objeto
        /// </summary>
        public override void renderizar(SuvirvalCraft contexto)
        {
            base.renderizar(contexto);
            if (SeEstaAbriendo())
            {
                progresoApertura.Render();
            }
        }

        public override string getAcciones()
        {
            //TODO. Mejorar esta lógica
            if (estaAbierto)
            {
                return mensajeInformativo;
            }
            if (!SeEstaAbriendo())
            {
                return "Abrir (B)";
            }
            return "";
        }

        public override string GetTipo()
        {
            return Cajon;
        }

        private float TiempoTotalApertura()
        {
            return 2;
        }

        private bool SeEstaAbriendo()
        {
            return progresoApertura != null;
        }

        #endregion Comportamientos
    }
}