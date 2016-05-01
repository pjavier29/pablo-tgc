using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

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
        public TgcMesh mesh { get; set; }
        #endregion

        #region Contructores
        public Obstaculo()
        {

        }

        public Obstaculo(float peso, float resistencia, TgcBox caja)
        {
            this.peso = peso;
            this.resistencia = resistencia;
            this.mesh = caja.toMesh("CAJA");//Pasar el nombre por parámetro
            this.obstaculosComposicion = new List<Obstaculo>();
        }

        public Obstaculo(float peso, float resistencia, TgcMesh mesh)
        {
            this.peso = peso;
            this.resistencia = resistencia;
            this.mesh = mesh;
            this.obstaculosComposicion = new List<Obstaculo>();
        }

        public Obstaculo(float peso, float resistencia, TgcMesh mesh, Obstaculo obstaculo)
        {
            this.peso = peso;
            this.resistencia = resistencia;
            this.mesh = mesh;
            this.obstaculosComposicion = new List<Obstaculo>();
            this.agregarObstaculo(obstaculo);
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
            foreach (Obstaculo obstaculo in this.obstaculosComposicion)
            {
                obstaculo.destruir();
            }
            this.mesh.dispose();
        }

        /// <summary>
        /// Renderiza el objeto
        /// </summary>
        public void renderizar()
        {
            this.mesh.render();
            this.mesh.BoundingBox.render();
        }

        /// <summary>
        /// Caja que encierra al objeto
        /// </summary>
        /// <returns></returns>
        public TgcBoundingBox BoundingBox()
        {
            return this.mesh.BoundingBox;
        }

        public void mover(Vector3 movimiento)
        {
            this.mesh.move(movimiento.X, movimiento.Y, movimiento.Z);
        }

        public Vector3 posicion()
        {
            return this.mesh.Position;
        }

        public void posicion(Vector3 posicion)
        {
            this.mesh.Position = posicion;
        }

        public void liberar()
        {
            this.mesh.dispose();
        }


        /// <summary>
        /// TODO. Aplicar de ser posible ecuaciones fisicas de rosamiento y demás de modo tal que sea más real el movimiento.
        /// </summary>
        /// <param name="fuerza"></param>
        public bool seMueveConUnaFuerza(float fuerza)
        {
            return this.peso < fuerza;
        }

        public void agregarObstaculo(Obstaculo obstaculo)
        {
            this.obstaculosComposicion.Add(obstaculo);
        }

        public List<Obstaculo> obstaculosQueContiene()
        {
            return this.obstaculosComposicion;
        }

        /// <summary>
        /// Determina si un obstáculo fue destruído completamente o puede arrojar sus partes
        /// </summary>
        /// <returns></returns>
        public bool destruccionTotal()
        {
            if (this.resistencia < 0)
            {
                if (this.resistencia < -100)
                {
                    return true;
                }
                return false;
            }
            //TODO. Manejar excepciones propias.
            throw new Exception("El obstáculo no esta destruido.");
        }

        public string nombre()
        {
            return this.mesh.Name;
        }

        public float distanciaA(Obstaculo unObstaculo)
        {
            Vector3 aux = new Vector3(unObstaculo.posicion().X - this.posicion().X, 
                unObstaculo.posicion().Y - this.posicion().Y, unObstaculo.posicion().Z - this.posicion().Z);
            return aux.Length();
        }

        public float distanciaA(Vector3 posicion)
        {
            Vector3 aux = new Vector3(posicion.X - this.posicion().X,
                posicion.Y - this.posicion().Y, posicion.Z - this.posicion().Z);
            return aux.Length();
        }

        #endregion

    }
}
