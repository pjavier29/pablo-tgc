using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.PabloTGC
{
    public class Personaje
    {
        #region Propiedades
        public float fuerza { get; set; }
        public float golpe { get; set; }
        public TgcSkeletalMesh mesh { get; set; }
        public float velocidadCaminar { get; set; }
        public float velocidadRotacion { get; set; }
        public float salud { get; set; }
        public float tiempoCorriendo { get; set; }
        public float resistenciaFisica { get; set; }
        private List<Obstaculo> mochila { get; set; }
        #endregion

        #region Constructores

        public Personaje()
        {
            this.mochila = new List<Obstaculo>();
            this.tiempoCorriendo = 0;
        }
        #endregion

        #region Comportamientos

        public void juntar(Obstaculo obstaculo)
        {
            this.mochila.Add(obstaculo);
        }

        public float correr(float tiempo)
        {
            if (this.tiempoCorriendo >= this.resistenciaFisica)
            {
                this.tiempoCorriendo = this.resistenciaFisica;
                return this.velocidadCaminar;
            }
            else
            {
                this.tiempoCorriendo += tiempo;
                if (this.tiempoCorriendo >= this.resistenciaFisica * 0.5)
                {
                    return this.velocidadCaminar * 1.5f;
                }
                return this.velocidadCaminar * 3.5f;
            }
        }

        public float rotarRapido()
        {
            return this.velocidadRotacion * 2.5f;
        }

        /// <summary>
        /// Afecta la salud del personaje dado un tiempo pasado por parámetro. El tiempo es en segundos.
        /// </summary>
        /// <param name="tiempoEnSegundos"></param>
        public void afectarSaludPorTiempo(float tiempoEnSegundos)
        {
            //Afectamos la salud de esta manera tan agresiva para que el juego sea más interactivo
            this.salud -= tiempoEnSegundos;
        }

        public void incrementoResistenciaFisica(float tiempoEnSegundos)
        {
            if ((this.tiempoCorriendo - tiempoEnSegundos) > 0)
            {
                this.tiempoCorriendo -= tiempoEnSegundos;
            }
            else
            {
                this.tiempoCorriendo = 0;
            }
        }

        public bool estaMuerto()
        {
            return this.salud <= 0;
        }

        public void consumirAlimento()
        {
            //TODO. Este método debe ser definido en forma mas precisa
            this.salud = 100;
        }

        public void incrementarSaludPorTiempo(float tiempoEnSegundos)
        {
            //TODO. Este método debe ser definido en forma mas precisa
            this.salud += tiempoEnSegundos * 1.2f;
            if (this.salud > 100)
            {
                this.salud = 100;
            }
        }

        public void morir()
        {
            this.salud = 0;
        }

        public List<Obstaculo> elementosEnMochila()
        {
            return this.mochila;
        }

        #endregion
    }
}
