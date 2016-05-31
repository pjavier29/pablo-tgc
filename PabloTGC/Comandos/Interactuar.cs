using AlumnoEjemplos.MiGrupo;
using AlumnoEjemplos.PabloTGC.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Terrain;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    //TODO. Revisar si se justifica crear un comando por cada constante que esta clase define. Hay que buecar bien la vuelta entre los comandos y las acciones que
    //definen los elementos,
    public class Interactuar : Comando
    {
        #region Constantes
        public const String Consumir = "Consumir";
        public const String Encender = "Encender";
        public const String Juntar = "Juntar";
        public const String Abrir = "Abrir";
        public const String JuntarTodo = "Juntar Todo";
        public const String DejarTodo = "Dejar Todo";
        public const String Parado = "Parado";//Accion generica
        #endregion

        #region Atributos
        #endregion

        #region Propiedades
        public String Accion { get; set; }
        public List<Elemento> ObstaculosInteractuar { get; set; }
        #endregion

        #region Constructores
        public Interactuar(String accion)
        {
            this.Accion = accion;
            this.ObstaculosInteractuar = new List<Elemento>();
        }

        #endregion

        #region Comportamientos
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            contexto.personaje.mesh.playAnimation("Parado", true);
            //Simulamos el descanso del personaje
            contexto.personaje.incrementoResistenciaFisica(elapsedTime);

            this.ObstaculosInteractuar.Clear();
            foreach (Elemento elem in contexto.optimizador.ElementosColision)
            {
                //TODO. Optimizar esto para solo objetos cernanos!!!!!!!!
                if (ControladorColisiones.EsferaColisionaCuadrado(contexto.personaje.GetAlcanceInteraccionEsfera(), elem.BoundingBox()))
                {
                    this.ObstaculosInteractuar.Add(elem);
                    if (! elem.AdmiteMultipleColision())
                    {
                        break;
                    }                
                }
            }
            foreach (Elemento elem in this.ObstaculosInteractuar)
            {
                elem.procesarInteraccion(this.Accion, contexto, elapsedTime);
                contexto.informativo.Text = elem.getAcciones();
            }
        }

        #endregion
    }
}
