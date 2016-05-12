using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC
{
    /// <summary>
    ///
    /// </summary>
    public class Elemento
    {
        #region Atributos
        public float peso { get; set; }
        public float resistencia { get; set; }
        private List<Elemento> elementosComposicion { get; set; }//Al romperse un obstaculo puede generar otros
        public TgcMesh mesh { get; set; }

        public TgcArrow linea = new TgcArrow();
        #endregion

        #region Contructores
        public Elemento()
        {

        }

        public Elemento(float peso, float resistencia, TgcBox caja)
        {
            this.peso = peso;
            this.resistencia = resistencia;
            this.mesh = caja.toMesh("CAJA");//TODO. Pasar el nombre por parámetro
            this.elementosComposicion = new List<Elemento>();
        }

        public Elemento(float peso, float resistencia, TgcMesh mesh)
        {
            this.peso = peso;
            this.resistencia = resistencia;
            this.mesh = mesh;
            this.elementosComposicion = new List<Elemento>();
        }

        public Elemento(float peso, float resistencia, TgcMesh mesh, Elemento elemento)
        {
            this.peso = peso;
            this.resistencia = resistencia;
            this.mesh = mesh;
            this.elementosComposicion = new List<Elemento>();
            this.agregarElemento(elemento);
        }

        #endregion

        #region Comportamientos

        /// <summary>
        /// TODO. Ver si no aplica poner una interfaz colisionable
        /// Procesa una colisión cuando la misma es en contra del personaje
        /// </summary>
        public virtual void procesarColision(Personaje personaje, float elapsedTime, String accion, List<Elemento> elementos, float moveForward, Vector3 movementVector)
        {
            if (moveForward < 0)
            {//Si esta caminando para adelante entonces empujamos la caja, sino no hacemos nada.
                if (this.seMueveConUnaFuerza(personaje.fuerza))
                {
                    Vector3 direccionMovimiento = movementVector;
                    direccionMovimiento.Normalize();
                    direccionMovimiento.Multiply(moveForward * elapsedTime * -0.1f);
                    this.mover(direccionMovimiento);
                }
                personaje.mesh.playAnimation("Empujar", true);
            }
        }

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
            foreach (Elemento elemento in this.elementosComposicion)
            {
                elemento.destruir();
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

            linea.PStart = this.mesh.BoundingBox.PMin;
            linea.PEnd = new Vector3(this.mesh.BoundingBox.PMin.X, this.mesh.BoundingBox.PMax.Y, this.mesh.BoundingBox.PMin.Z);
            linea.BodyColor = Color.Green;
            linea.HeadColor = Color.Green;
            linea.Thickness = 1;
            linea.HeadSize = new Vector2(1, 1);
            linea.updateValues();
            linea.render();
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

        public void agregarElemento(Elemento elemento)
        {
            this.elementosComposicion.Add(elemento);
        }

        public List<Elemento> elementosQueContiene()
        {
            return this.elementosComposicion;
        }

        /// <summary>
        /// Determina si un obstáculo fue destruído completamente o puede arrojar sus partes
        /// </summary>
        /// <returns></returns>
        public bool destruccionTotal()
        {
            if (this.resistencia < 0)
            {
                if (this.resistencia < -1000)
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
            //TODO. Refactorizar esto
            return this.mesh.Name;
        }

        public float distanciaA(Elemento unElemento)
        {
            Vector3 aux = new Vector3(unElemento.posicion().X - this.posicion().X,
                unElemento.posicion().Y - this.posicion().Y, unElemento.posicion().Z - this.posicion().Z);
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
