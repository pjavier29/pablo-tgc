using System.Collections.Generic;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.ElementosJuego;
using TGC.Group.Model.Utiles;

namespace TGC.Group.Model.Comandos
{
    //TODO. Revisar si se justifica crear un comando por cada constante que esta clase define. Hay que buecar bien la vuelta entre los comandos y las acciones que
    //definen los elementos,
    public class Interactuar : Comando
    {
        #region Constructores

        public Interactuar(string accion)
        {
            Accion = accion;
            ObstaculosInteractuar = new List<Elemento>();
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            contexto.personaje.mesh.playAnimation("Parado", true);
            //Simulamos el descanso del personaje
            contexto.personaje.incrementoResistenciaFisica(elapsedTime);

            ObstaculosInteractuar.Clear();
            foreach (var elem in contexto.optimizador.ElementosColision)
            {
                //TODO. Optimizar esto para solo objetos cernanos!!!!!!!!
                if (ControladorColisiones.EsferaColisionaCuadrado(contexto.personaje.GetAlcanceInteraccionEsfera(),
                    elem.BoundingBox()))
                {
                    ObstaculosInteractuar.Add(elem);
                    if (!elem.AdmiteMultipleColision())
                    {
                        break;
                    }
                }
            }
            foreach (var elem in ObstaculosInteractuar)
            {
                elem.procesarInteraccion(Accion, contexto, elapsedTime);
                contexto.informativo.Text = elem.getAcciones();
            }
        }

        #endregion Comportamientos

        #region Constantes

        public const string Consumir = "Consumir";
        public const string Encender = "Encender";
        public const string Juntar = "Juntar";
        public const string Abrir = "Abrir";
        public const string JuntarTodo = "Juntar Todo";
        public const string DejarTodo = "Dejar Todo";
        public const string Parado = "Parado"; //Accion generica

        #endregion Constantes

        #region Propiedades

        public string Accion { get; set; }
        public List<Elemento> ObstaculosInteractuar { get; set; }

        #endregion Propiedades
    }
}