using AlumnoEjemplos.PabloTGC.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Terrain;
using AlumnoEjemplos.PabloTGC.Administracion;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class Golpear : Comando
    {
        #region Atributo;
        private float momentoUltimoGolpe;
        private Elemento elementoEnColision;
        #endregion

        #region Propiedades
        public String GolpeActual { get; set; }
        #endregion

        #region Constantes
        public const String Patear = "Patear";
        public const String Pegar = "Pegar";
        #endregion

        #region Constructores
        public Golpear(String tipoDeGolpe)
        {
            this.GolpeActual = tipoDeGolpe;
            this.momentoUltimoGolpe = 0;
            this.elementoEnColision = null;
        }
        #endregion

        #region Comportamientos
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            float alcance = 0;
            float fuerzaGolpe = 0;

            this.BuscarColisionConGolpe(contexto);

            if (this.GolpeActual.Equals(Patear))
            {
                if (this.elementoEnColision != null)
                {
                    contexto.sonidoGolpePatada.play(false);
                    this.elementoEnColision.GenerarVivracion();
                }
                contexto.personaje.mesh.playAnimation("Patear", true);
                alcance = contexto.personaje.alcancePatada();
                fuerzaGolpe = contexto.personaje.fuerzaPatada();
            }

            if (this.GolpeActual.Equals(Pegar))
            {
                if (this.elementoEnColision != null)
                {
                    contexto.sonidoGolpe.play(false);
                    this.elementoEnColision.GenerarVivracion();
                }
                contexto.personaje.mesh.playAnimation("Pegar", true);
                alcance = contexto.personaje.alcanceGolpe();
                fuerzaGolpe = contexto.personaje.fuerzaGolpe();
            }

            //Si golpeo un obstáculo deberé esperar 2 segundos para poder golpearlo nuevamente
            if (this.PuedeGolpear(contexto.tiempo) && this.elementoEnColision != null)
            {
                //Buscamos si esta al alcance alguno de los obstáculos
                //foreach (Elemento elem in contexto.optimizador.ElementosColision)
                //{
                    //if (ControladorColisiones.EsferaColisionaCuadrado(contexto.personaje.GetAlcanceInteraccionEsfera(), elem.BoundingBox()))
                    //{
                        //Si golpeo actualizamos el tiempo local
                        this.momentoUltimoGolpe = contexto.tiempo;
                        this.elementoEnColision.recibirDanio(fuerzaGolpe, contexto.tiempo);
                        if (this.elementoEnColision.estaDestruido())
                        {
                            if (!this.elementoEnColision.destruccionTotal())
                            {
                                foreach (Elemento obs in this.elementoEnColision.elementosQueContiene())
                                {
                                    //TODO. Aplicar algun algoritmo de dispersion copado
                                    obs.posicion(this.elementoEnColision.posicion());
                                    contexto.elementos.Add(obs);
                                }
                            }
                            this.elementoEnColision.liberar();
                            contexto.elementos.Remove(this.elementoEnColision);
                            contexto.optimizador.ForzarActualizacionElementosColision();
                        }

                        //En principio solo se puede golpear un obstaculo a la vez.
                       // break;
                    //}
                //}
            }
        }

        private bool PuedeGolpear(float tiempo)
        {
            if (this.momentoUltimoGolpe == 0) { return true; }
            return (tiempo - this.momentoUltimoGolpe) > 1.5f;
        }

        private void BuscarColisionConGolpe(SuvirvalCraft contexto)
        {
            this.elementoEnColision = null;
            //Buscamos si esta al alcance alguno de los obstáculos
            foreach (Elemento elem in contexto.optimizador.ElementosColision)
            {
                if (ControladorColisiones.EsferaColisionaCuadrado(contexto.personaje.GetAlcanceInteraccionEsfera(), elem.BoundingBox()))
                {
                    this.elementoEnColision = elem;
                    //En principio solo se puede golpear un obstaculo a la vez.
                    break;
                }
            }
        }
        #endregion
    }
}
