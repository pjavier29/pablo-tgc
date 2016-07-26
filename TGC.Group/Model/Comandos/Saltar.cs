using System;
using Microsoft.DirectX;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Movimientos;
using TGC.Group.Model.Utiles;

namespace TGC.Group.Model.Comandos
{
    public class Saltar : Comando
    {
        #region Constantes

        public const String Adelante = "Adelante";
        public const String EnLugar = "EnLugar";

        #endregion Constantes

        #region Atributos

        private String tipoSalto;

        #endregion Atributos

        #region Propiedades

        public MovimientoParabolico Movimiento { get; set; }

        #endregion Propiedades

        #region Constructores

        public Saltar(String tipoSalto)
        {
            this.tipoSalto = tipoSalto;
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            Vector3 direccion;
            float velocidad;
            if (this.tipoSalto.Equals(Adelante))
            {
                //TODO. Tener en cuenta que la direccion se esta calculando mas arriba, aunque aqui se calcula la direccion si el perosnaje esta quieto. Analizar!!!
                //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
                float z = -(float)Math.Cos((float)contexto.personaje.mesh.Rotation.Y) * 50;
                float x = -(float)Math.Sin((float)contexto.personaje.mesh.Rotation.Y) * 50;
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

            Movimiento = new MovimientoParabolico(contexto.personaje.mesh.Position, direccion, velocidad, new MallaEnvoltura(contexto.personaje.mesh));

            contexto.movimientoPersonaje = this.Movimiento;
        }

        #endregion Comportamientos
    }
}