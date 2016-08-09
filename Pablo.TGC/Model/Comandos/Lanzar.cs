using Microsoft.DirectX;
using System;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.ElementosJuego;
using TGC.Group.Model.Movimientos;
using TGC.Group.Model.Utiles;

namespace TGC.Group.Model.Comandos
{
    public class Lanzar : Comando
    {
        #region Atributos

        private Elemento elemento;

        #endregion Atributos



        #region Propiedades

        public MovimientoParabolico Movimiento { get; set; }

        #endregion Propiedades

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            //TODO. Esto esta muy choto
            elemento = contexto.puebaFisica;

            //TODO. Tener en cuenta que la direccion se esta calculando mas arriba, aunque aqui se calcula la direccion si el perosnaje esta quieto. Analizar!!!
            //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
            var z = -(float)Math.Cos(contexto.personaje.mesh.Rotation.Y) * 50;
            var x = -(float)Math.Sin(contexto.personaje.mesh.Rotation.Y) * 50;
            //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
            //el vector salga del personaje.
            var direccion = contexto.personaje.mesh.Position + new Vector3(x, /*terreno.CalcularAltura(x, z) + */1, z);

            elemento.Mesh.Position = contexto.personaje.mesh.Position + new Vector3(0, 50, 0);

            Movimiento = new MovimientoParabolico(contexto.personaje.mesh.Position, direccion, 20,
                new MallaEnvoltura(elemento.Mesh));

            contexto.movimiento = Movimiento;

            contexto.personaje.mesh.playAnimation("Arrojar", true);
        }

        #endregion Comportamientos
    }
}