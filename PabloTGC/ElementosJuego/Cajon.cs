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
        #endregion

        #region Contructores
        public Cajon(float peso, float resistencia, TgcMesh mesh) : base(peso, resistencia, mesh)
        {
            this.estaAbierto = false;
        }
        #endregion

        #region Comportamientos

        public override void procesarInteraccion(String accion, Personaje personaje, List<Elemento> elementos, float elapsedTime)
        {
            //TODO. Podria agregarse para que se cambie el mesh del cajon y se muestre cerrado o abierto.
            if (accion.Equals("Abrir"))
            {
                this.estaAbierto = true;
            }
            if (accion.Equals("Juntar Todo"))
            {
                List<Elemento> auxiliar = new List<Elemento>();
                foreach (Elemento elem in this.elementosQueContiene())
                {
                    if (!personaje.ContieneElementoEnMochila(elem))
                    {
                        personaje.juntar(elem);
                        auxiliar.Add(elem);
                    }
                }
                this.EliminarElementos(auxiliar);
            }
            if (accion.Equals("Dejar Elemento"))
            {
                List<Elemento> auxiliar = new List<Elemento>();
                foreach (Elemento elem in personaje.elementosEnMochila())
                {
                    if (!this.elementosQueContiene().Contains(elem))
                    {
                        this.agregarElemento(elem);
                        auxiliar.Add(elem);
                    }
                }
                personaje.DejarElementos(auxiliar);
            }
        }

        public override String getAcciones()
        {
            //TODO. Mejorar esta lógica
            if (estaAbierto)
            {
                return this.GetElementos() + " Juntar Todo (J), Dejar Elemento (H)";
            }
            else
            {
                return "Abrir (B)";
            }
        }

        public override String GetTipo()
        {
            return Cajon;
        }
        #endregion
    }
}
