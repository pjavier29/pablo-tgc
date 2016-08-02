using Microsoft.DirectX;
using System;
using TGC.Core.SceneLoader;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosJuego
{
    public class ElementoDoble : Elemento
    {
        #region constructores

        public ElementoDoble(float peso, float resistencia, TgcMesh mesh1, TgcMesh mesh2, Efecto efecto)
            : base(peso, resistencia, mesh1, efecto)
        {
            //Por defecto comienza mostrando el primer mesh
            Mesh1 = mesh1;
            Mesh2 = mesh2;
            estaCreando = false;
            tieneQueCrear = false;
            progresoCreacion = null;
            tiempoCreacion = 0;
            CompletarSetEfecto(efecto);
            mensajeInformativo = "";
        }

        #endregion constructores

        #region Atributos

        private bool estaCreando;
        private bool tieneQueCrear;
        private BarraEstado progresoCreacion;
        private float tiempoCreacion;
        private string mensajeInformativo;

        #endregion Atributos

        #region Propiedades

        public TgcMesh Mesh1 { get; set; }
        public TgcMesh Mesh2 { get; set; }

        #endregion Propiedades

        #region Comportamientos

        public override void procesarInteraccion(string accion, SuvirvalCraft contexto, float elapsedTime)
        {
            mensajeInformativo = "Juntar (J), Consumir (C)";
            base.procesarInteraccion(accion, contexto, elapsedTime);

            if (accion.Equals("Juntar"))
            {
                if ((elementosQueContiene().Count > 0) && !estaCreando)
                {
                    try
                    {
                        //Si tiene elementos para dar
                        var elem = elementosQueContiene()[0];
                        contexto.personaje.juntar(elem);
                        EliminarElemento(elem);
                        Mesh = Mesh2;
                        tieneQueCrear = true;
                    }
                    catch (Exception ex)
                    {
                        mensajeInformativo = ex.Message;
                    }
                }
            }
            if (accion.Equals("Consumir"))
            {
                if ((elementosQueContiene().Count > 0) && !estaCreando)
                {
                    //Si tiene elementos para dar
                    var elem = elementosQueContiene()[0];
                    if (elem.EsDeTipo(Alimento))
                    {
                        var ali = (Alimento)elem;
                        contexto.personaje.ConsumirAlimento(ali.GetNutricion());
                        EliminarElemento(elem);
                        Mesh = Mesh2;
                        tieneQueCrear = true;
                    }
                }
            }
        }

        public override void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
            base.Actualizar(contexto, elapsedTime);
            if (estaCreando)
            {
                tiempoCreacion += elapsedTime;
                if (tiempoCreacion > TiempoTotalCreacion())
                {
                    progresoCreacion.Liberar();
                    progresoCreacion = null;
                    tiempoCreacion = 0;
                    estaCreando = false;
                    tieneQueCrear = false;
                    Mesh = Mesh1;
                }
                else
                {
                    progresoCreacion.ActualizarEstado(tiempoCreacion);
                }
            }
            else
            {
                if (tieneQueCrear && elementosQueContiene().Count > 0)
                {
                    progresoCreacion =
                        new BarraEstado(new Vector3(BoundingBox().PMin.X, BoundingBox().PMax.Y, BoundingBox().PMax.Z),
                            BoundingBox().PMax, TiempoTotalCreacion());
                    estaCreando = true;
                    tieneQueCrear = false;
                }
            }
        }

        /// <summary>
        ///     Renderiza el objeto
        /// </summary>
        public override void renderizar(SuvirvalCraft contexto)
        {
            base.renderizar(contexto);
            if (estaCreando)
            {
                progresoCreacion.Render();
            }
        }

        public override string getAcciones()
        {
            //TODO. Mejorar esta lógica
            if (estaCreando)
            {
                return "Creación de nuevos elementos";
            }
            if (elementosQueContiene().Count > 0)
            {
                //Si aun tiene elementos para entregar sigue procesando
                return mensajeInformativo;
            }
            return "Sin elementos";
        }

        public override string GetTipo()
        {
            return ElementoDoble;
        }

        private float TiempoTotalCreacion()
        {
            return 60;
        }

        public void CompletarSetEfecto(Efecto efecto)
        {
            efecto.Aplicar(Mesh1);
            efecto.Aplicar(Mesh2);
        }

        #endregion Comportamientos
    }
}