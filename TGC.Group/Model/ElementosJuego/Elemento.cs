using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosJuego
{
    /// <summary>
    /// </summary>
    public class Elemento
    {
        #region Constantes

        public const string Alimento = "Alimento";
        public const string Animal = "Animal";
        public const string Cajon = "Cajon";
        public const string Fuego = "Fuego";
        public const string FuenteAgua = "FuenteAgua";
        public const string Madera = "Madera";
        public const string Olla = "Olla";
        public const string General = "Elemento";
        public const string Copa = "Copa";
        public const string ElementoSinInteraccion = "ElementoSinInteraccion";
        public const string ElementoDoble = "ElementoDoble";
        public const string Antorcha = "Antorcha";

        #endregion Constantes

        #region Atributos

        private BarraEstado barraEstado;
        private readonly float resistenciaTotal;
        private bool hayInteraccion;
        private float momentoUltimoGolpe;
        private Efecto efecto;
        private float vibracion;
        private float momentoUltimaVibracion;

        #endregion Atributos

        #region Propiedades

        public float Peso { get; set; }
        public float Resistencia { get; set; }
        private List<Elemento> ElementosComposicion { get; } //Al romperse un obstaculo puede generar otros
        public TgcMesh Mesh { get; set; }
        public float Flexibilidad { get; set; }
        public Color ColorBase { get; set; } = Color.White; //Deberiamos definir un color base para cada tipo de luz

        #endregion Propiedades

        #region Contructores

        public Elemento()
        {
        }

        public Elemento(TgcMesh mesh, float resistencia)
        {
            Mesh = mesh;
            Resistencia = resistencia;
            resistenciaTotal = resistencia;
            barraEstado = null;
            hayInteraccion = false;
            momentoUltimoGolpe = 0;
            Flexibilidad = 0;
            vibracion = 0;
            momentoUltimaVibracion = 0;
            ColorBase = Color.White;
        }

        public Elemento(float peso, float resistencia, TgcMesh mesh) : this(mesh, resistencia)
        {
            Peso = peso;
            ElementosComposicion = new List<Elemento>();
            Flexibilidad = 0;
            vibracion = 0;
            momentoUltimaVibracion = 0;
            ColorBase = Color.White;
        }

        public Elemento(float peso, float resistencia, TgcMesh mesh, Elemento elemento) : this(mesh, resistencia)
        {
            Peso = peso;
            ElementosComposicion = new List<Elemento>();
            agregarElemento(elemento);
            Flexibilidad = 0;
            vibracion = 0;
            momentoUltimaVibracion = 0;
            ColorBase = Color.White;
        }

        public Elemento(float peso, float resistencia, TgcMesh mesh, Elemento elemento, Efecto efecto)
            : this(mesh, resistencia)
        {
            Peso = peso;
            ElementosComposicion = new List<Elemento>();
            agregarElemento(elemento);
            SetEfecto(efecto);
            Flexibilidad = 0;
            vibracion = 0;
            momentoUltimaVibracion = 0;
            ColorBase = Color.White;
        }

        public Elemento(float peso, float resistencia, TgcMesh mesh, Elemento elemento, Efecto efecto, Color color)
            : this(mesh, resistencia)
        {
            Peso = peso;
            ElementosComposicion = new List<Elemento>();
            agregarElemento(elemento);
            SetEfecto(efecto);
            Flexibilidad = 0;
            vibracion = 0;
            momentoUltimaVibracion = 0;
            ColorBase = color;
        }

        public Elemento(float peso, float resistencia, TgcMesh mesh, Efecto efecto) : this(mesh, resistencia)
        {
            Peso = peso;
            ElementosComposicion = new List<Elemento>();
            SetEfecto(efecto);
            Flexibilidad = 0;
            vibracion = 0;
            momentoUltimaVibracion = 0;
            ColorBase = Color.White;
        }

        #endregion Contructores

        #region Comportamientos

        /// <summary>
        ///     TODO. Ver si no aplica poner una interfaz colisionable
        ///     Procesa una colisión cuando la misma es en contra del personaje
        /// </summary>
        public virtual void procesarColision(Personaje personaje, float elapsedTime, List<Elemento> elementos,
            float moveForward, Vector3 movementVector, Vector3 lastPos)
        {
            //TODO. Este metodo tiene muchos parametros que deberian ser del personaje.
            if (moveForward < 0)
            {
                //Si esta caminando para adelante entonces empujamos la caja, sino no hacemos nada.
                if (seMueveConUnaFuerza(personaje.Fuerza))
                {
                    var direccionMovimiento = movementVector;
                    direccionMovimiento.Normalize();
                    direccionMovimiento.Multiply(moveForward * elapsedTime * -0.1f);
                    mover(direccionMovimiento);
                }
                personaje.mesh.playAnimation("Empujar", true);
                personaje.mesh.Position = lastPos;
                personaje.ActualizarEsferas();
            }
            else
            {
                personaje.mesh.Position = lastPos;
                personaje.ActualizarEsferas();
            }
        }

        public virtual void Actualizar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (hayInteraccion)
            {
                if (barraEstado != null)
                {
                    barraEstado.ActualizarEstado(Resistencia);
                }
            }
            else
            {
                if (barraEstado != null)
                {
                    barraEstado.Liberar();
                    barraEstado = null;
                }
            }

            //Preguntamos por el tiempo del ultimo golpe porque queremos que la barra de estado se muestre durante 5 segudos despues de cada golpe,
            //independientemente de que no haya interaccion
            if (SuperoTiempoGolpe(contexto.tiempo))
            {
                //Esto maneja la sincronización, ya que siempre se ejecuta primero las colisiones e interacciones y luego las actualizaciones.
                hayInteraccion = false;
            }
        }

        public virtual void procesarInteraccion(string accion, SuvirvalCraft contexto, float elapsedTime)
        {
            if (barraEstado == null)
            {
                barraEstado = new BarraEstado(Mesh.BoundingBox.PMin,
                    new Vector3(BoundingBox().PMin.X, BoundingBox().PMax.Y, BoundingBox().PMin.Z), resistenciaTotal);
            }
            hayInteraccion = true;
        }

        public virtual void ProcesarColisionConElemento(Elemento elemento)
        {
        }

        /// <summary>
        ///     Aplica el daño que se recibe por parametro y actualiza la barra de estado
        /// </summary>
        /// <returns></returns>
        public void recibirDanio(float danio, float tiempoDeGolpe)
        {
            hayInteraccion = true;
            momentoUltimoGolpe = tiempoDeGolpe;
            Resistencia -= danio;
        }

        public bool estaDestruido()
        {
            return Resistencia <= 0;
        }

        /// <summary>
        ///     Destruye el elemento
        /// </summary>
        public virtual void destruir()
        {
            foreach (var elemento in ElementosComposicion)
            {
                elemento.destruir();
            }
            Mesh.dispose();
            if (barraEstado != null)
            {
                barraEstado.Liberar();
            }
        }

        /// <summary>
        ///     Destruye el elemento pero deveulve una lista con los elementos que contenia
        /// </summary>
        /// <returns></returns>
        public List<Elemento> DestruirSolo()
        {
            var contenido = new List<Elemento>();
            contenido.AddRange(elementosQueContiene());
            elementosQueContiene().Clear();
            destruir();
            return contenido;
        }

        /// <summary>
        ///     Renderiza el objeto
        /// </summary>
        public virtual void renderizar(SuvirvalCraft contexto)
        {
            if (Efecto() != null)
            {
                //Delego en el efecto la responsabilidad del renderizado.
                Efecto().ActualizarRenderizar(contexto, this);
            }
            else
            {
                Mesh.render();
            }
            if (barraEstado != null)
            {
                barraEstado.Render();
            }
        }

        public void ActualizarBarraEstadoCompleta(Vector3 puntoOrigen, Vector3 puntoDestino)
        {
            if (barraEstado != null)
            {
                barraEstado.ActualizarPuntosBase(puntoOrigen, puntoDestino);
            }
        }

        /// <summary>
        ///     Caja que encierra al objeto
        /// </summary>
        /// <returns></returns>
        public TgcBoundingBox BoundingBox()
        {
            return Mesh.BoundingBox;
        }

        public void mover(Vector3 movimiento)
        {
            Mesh.move(movimiento.X, movimiento.Y, movimiento.Z);
        }

        public virtual Vector3 posicion()
        {
            return Mesh.Position;
        }

        public void posicion(Vector3 posicion)
        {
            Mesh.Position = posicion;
        }

        public void liberar()
        {
            Mesh.dispose();
        }

        /// <summary>
        ///     TODO. Aplicar de ser posible ecuaciones fisicas de rosamiento y demás de modo tal que sea más real el movimiento.
        /// </summary>
        /// <param name="fuerza"></param>
        public bool seMueveConUnaFuerza(float fuerza)
        {
            return Peso < fuerza;
        }

        public void agregarElemento(Elemento elemento)
        {
            ElementosComposicion.Add(elemento);
        }

        public void AgregarElementos(List<Elemento> elementos)
        {
            ElementosComposicion.AddRange(elementos);
        }

        public void EliminarElemento(Elemento elemento)
        {
            ElementosComposicion.Remove(elemento);
        }

        public void EliminarElementos(List<Elemento> elementos)
        {
            foreach (var elem in elementos)
            {
                EliminarElemento(elem);
            }
        }

        public List<Elemento> elementosQueContiene()
        {
            return ElementosComposicion;
        }

        /// <summary>
        ///     Determina si un obstáculo fue destruído completamente o puede arrojar sus partes
        /// </summary>
        /// <returns></returns>
        public bool destruccionTotal()
        {
            if (Resistencia <= 0)
            {
                if (Resistencia < -1000)
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
            return Mesh.Name;
        }

        public float distanciaA(Elemento unElemento)
        {
            var aux = new Vector3(unElemento.posicion().X - posicion().X,
                unElemento.posicion().Y - posicion().Y, unElemento.posicion().Z - posicion().Z);
            return aux.Length();
        }

        public float distanciaA(Vector3 posicion)
        {
            var aux = new Vector3(posicion.X - this.posicion().X,
                posicion.Y - this.posicion().Y, posicion.Z - this.posicion().Z);
            return aux.Length();
        }

        public virtual string getAcciones()
        {
            //El elemento generico no posee acciones
            return "";
        }

        public string GetElementos()
        {
            var elementos = "";
            foreach (var elem in ElementosComposicion)
            {
                elementos = elementos + " - " + elem.nombre();
            }
            return elementos;
        }

        public virtual bool AdmiteMultipleColision()
        {
            return false;
        }

        public virtual string GetTipo()
        {
            return General;
        }

        public virtual string GetDescripcion()
        {
            return GetTipo();
        }

        public virtual bool EsDeTipo(string tipo)
        {
            return GetTipo().Equals(tipo);
        }

        /// <summary>
        ///     Retorne el primer elemento de la lista que cumpla con el tipo que se paso por parametro o null
        ///     Deberiamos trabjar con Excepciones
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public Elemento ElementoDeTipo(string tipo)
        {
            Elemento elemento = null;
            foreach (var elem in elementosQueContiene())
            {
                if (elem.GetTipo().Equals(tipo))
                {
                    elemento = elem;
                    break;
                }
            }
            return elemento;
        }

        private bool SuperoTiempoGolpe(float tiempoActual)
        {
            if (momentoUltimoGolpe == 0)
            {
                return true;
            }
            //Sabemos que el tiempo esta en segundos
            return tiempoActual - momentoUltimoGolpe > 5;
        }

        public void SetEfecto(Efecto efecto)
        {
            this.efecto = efecto;
            efecto.Aplicar(Mesh);
        }

        public Efecto Efecto()
        {
            return efecto;
        }

        /// <summary>
        ///     Debe ser confgurable de cada tipo de elemento
        /// </summary>
        /// <returns></returns>

        #region Para configurar la luz

        public virtual ColorValue ColorEmisor()
        {
            return ColorValue.FromColor(Color.Black);
        }

        public virtual ColorValue ColorAmbiente()
        {
            return ColorValue.FromColor(ColorBase);
        }

        public virtual ColorValue ColorDifuso()
        {
            return ColorValue.FromColor(ColorBase);
        }

        public virtual ColorValue ColorEspecular()
        {
            return ColorValue.FromColor(ColorBase);
        }

        public virtual float EspecularEx()
        {
            return 20;
        }

        public virtual void Iluminar(Efecto efecto, Vector3 posicionVision, ColorValue colorEmisor,
            ColorValue colorAmbiente,
            ColorValue colorDifuso, ColorValue colorEspecular, float especularEx)
        {
        }

        public Color GetColorBase()
        {
            return ColorBase;
            /*  if (this.ColorBase != null)
              {
                  return ColorBase;
              }
              return Color.White;*/
        }

        #endregion Para configurar la luz

        public float ObtenerPuntoMedio()
        {
            //Sabemos que la altura de los elementos las estamos manejando sobre el eje Y
            return Mesh.BoundingBox.PMax.Y - Mesh.BoundingBox.PMin.Y;
        }

        public void GenerarVibracion(float tiempo)
        {
            vibracion = 1;
            momentoUltimaVibracion = tiempo;
        }

        public float ObtenerVibracion(float tiempo)
        {
            if (vibracion != 0)
            {
                vibracion -= tiempo - momentoUltimaVibracion;
                if (vibracion < 0)
                {
                    vibracion = 0;
                }
            }
            return vibracion;
        }

        public virtual float GetAlturaAnimacion()
        {
            return 0;
        }

        public virtual void Activar()
        {
        }

        public virtual void Desactivar()
        {
        }

        #endregion Comportamientos
    }
}