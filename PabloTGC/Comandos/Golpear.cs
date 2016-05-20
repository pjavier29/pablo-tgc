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
        private float timer;
        #endregion

        #region Propiedades
        public String GolpeActual { get; set; }
        public bool PuedeGolpear { get; set; }
        #endregion

        #region Constantes
        public const String Patear = "Patear";
        public const String Pegar = "Pegar";
        #endregion

        #region Constructores
        public Golpear(String tipoDeGolpe)
        {
            this.timer = 0;
            this.PuedeGolpear = true;
            this.GolpeActual = tipoDeGolpe;
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

            this.ActualizarTimer(elapsedTime);

            //Si golpeo un obstáculo deberé esperar 2 segundos para poder golpearlo nuevamente
            if (this.PuedeGolpear)
            {
                //Buscamos si esta al alcance alguno de los obstáculos
                foreach (Elemento elem in contexto.elementos)
                {
                    if (ControladorColisiones.EsferaColisionaCuadrado(contexto.personaje.GetAlcanceInteraccionEsfera(), elem.BoundingBox()))
                    {
                        elem.recibirDanio(fuerzaGolpe);
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
                        this.PuedeGolpear = false;
                        break;
                    }
                }
            }
        }

        private void ActualizarTimer(float elapsedTime)
        {
            //TODO. el timer deberia ser manejado por otro objeto general, porque sino el tiempo solo se actualiza cuando se esta pegando,
            //entonces si doy un golpe y me voy a golpear otro objeto debo espear el tiempo restante hasta completar el segundo y medio para poder
            //golpearlo, pero yo en el medio pude caminar por mucho tiempo que deberia ser contado para la espera.
            if (this.timer >= 1.5f)
            {
                this.timer = 0;
                this.PuedeGolpear = true;
            }
            this.timer += elapsedTime;
        }
        #endregion
    }
}
