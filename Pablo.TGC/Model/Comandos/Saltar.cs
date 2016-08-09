using Microsoft.DirectX;
using System;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Movimientos;
using TGC.Group.Model.Utiles;

namespace TGC.Group.Model.Comandos
{
    public class Saltar : Comando
    {
        #region Atributos

        private readonly string tipoSalto;

        #endregion Atributos

        #region Constructores

        public Saltar(string tipoSalto)
        {
            this.tipoSalto = tipoSalto;
        }

        #endregion Constructores

        #region Propiedades

        public MovimientoParabolico Movimiento { get; set; }

        #endregion Propiedades

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            Vector3 direccion;
            float velocidad;
            if (tipoSalto.Equals(Adelante))
            {
                //TODO. Tener en cuenta que la direccion se esta calculando mas arriba, aunque aqui se calcula la direccion si el perosnaje esta quieto. Analizar!!!
                //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
                var z = -(float)Math.Cos(contexto.personaje.mesh.Rotation.Y) * 50;
                var x = -(float)Math.Sin(contexto.personaje.mesh.Rotation.Y) * 50;
                //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
                //el vector salga del personaje.
                direccion = contexto.personaje.mesh.Position + new Vector3(x, 75, z);
                velocidad = 6;
            }
            else
            {
                //Por defecto el salto es en el lugar!!!
                direccion = contexto.personaje.mesh.Position + new Vector3(0, 1, 0);
                velocidad = 4;
            }

            Movimiento = new MovimientoParabolico(contexto.personaje.mesh.Position, direccion, velocidad,
                new MallaEnvoltura(contexto.personaje.mesh));

            contexto.movimientoPersonaje = Movimiento;
        }

        #endregion Comportamientos

        #region Constantes

        public const string Adelante = "Adelante";
        public const string EnLugar = "EnLugar";

        #endregion Constantes
    }
}