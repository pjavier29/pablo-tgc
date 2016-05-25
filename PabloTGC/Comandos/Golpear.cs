using AlumnoEjemplos.MiGrupo;
using AlumnoEjemplos.PabloTGC.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Terrain;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class Golpear : Comando
    {
        #region Atributo;
        private float momentoUltimoGolpe;
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
        }
        #endregion

        #region Comportamientos
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            float alcance = 0;
            float fuerzaGolpe = 0;

            if (this.GolpeActual.Equals(Patear))
            {
                contexto.personaje.mesh.playAnimation("Patear", true);
                alcance = contexto.personaje.alcancePatada();
                fuerzaGolpe = contexto.personaje.fuerzaPatada();
            }

            if (this.GolpeActual.Equals(Pegar))
            {
                contexto.personaje.mesh.playAnimation("Pegar", true);
                alcance = contexto.personaje.alcanceGolpe();
                fuerzaGolpe = contexto.personaje.fuerzaGolpe();
            }

            //Si golpeo un obstáculo deberé esperar 2 segundos para poder golpearlo nuevamente
            if (this.PuedeGolpear(contexto.tiempo))
            {
                //Buscamos si esta al alcance alguno de los obstáculos
                foreach (Elemento elem in contexto.elementos)
                {
                    if (ControladorColisiones.EsferaColisionaCuadrado(contexto.personaje.GetAlcanceInteraccionEsfera(), elem.BoundingBox()))
                    {
                        //Si golpeo actualizamos el tiempo local
                        this.momentoUltimoGolpe = contexto.tiempo;
                        elem.recibirDanio(fuerzaGolpe, contexto.tiempo);
                        if (elem.estaDestruido())
                        {
                            if (!elem.destruccionTotal())
                            {
                                foreach (Elemento obs in elem.elementosQueContiene())
                                {
                                    //TODO. Aplicar algun algoritmo de dispersion copado
                                    obs.posicion(elem.posicion());
                                    contexto.elementos.Add(obs);
                                }
                            }
                            elem.liberar();
                            contexto.elementos.Remove(elem);
                        }

                        //En principio solo se puede golpear un obstaculo a la vez.
                        break;
                    }
                }
            }
        }

        private bool PuedeGolpear(float tiempo)
        {
            if (this.momentoUltimoGolpe == 0) { return true; }
            return (tiempo - this.momentoUltimoGolpe) > 1.5f;
        }
        #endregion
    }
}
