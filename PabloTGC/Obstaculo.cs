using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.PabloTGC
{
    /// <summary>
    /// TODO. Ver si esta clase debe transformarse en una un poco mas genérica
    /// </summary>
    public class Obstaculo
    {
        #region Atributos
        public float peso { get; set; }
        public float resistencia { get; set; }
        private List<Obstaculo> obstaculosComposicion { get; set; }//Al romperse un obstaculo puede generar otros
        public TgcBox caja { get; set; }
        #endregion

        #region Contructores
        public Obstaculo()
        {

        }

        public Obstaculo(float peso, float resistencia, TgcBox caja)
        {
            this.peso = peso;
            this.resistencia = resistencia;
            this.caja = caja;
            this.obstaculosComposicion = new List<Obstaculo>();
        }
        #endregion

        #region Comportamientos

        /// <summary>
        /// Aplica el daño que se recibe por parametro y retorna verdadero si el objeto sigue en pie o false si se destruyo
        /// </summary>
        /// <returns></returns>
        public bool recibirDanio(float danio)
        {
            this.resistencia -= danio;
            return this.resistencia > 0;
        }

        /// <summary>
        /// Destruye el obstáculo
        /// </summary>
        public void destruir()
        {
            this.caja.dispose();
        }

        /// <summary>
        /// TODO. Aplicar de ser posible ecuaciones fisicas de rosamiento y demás de modo tal que sea más real el movimiento.
        /// </summary>
        /// <param name="fuerza"></param>
        public bool seMueveConUnaFuerza(int fuerza)
        {
            return this.peso < fuerza;
        }
        #endregion

    }
}
