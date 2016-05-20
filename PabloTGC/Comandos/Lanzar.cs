using AlumnoEjemplos.MiGrupo;
using AlumnoEjemplos.PabloTGC.Utiles;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class Lanzar : Comando
    {
        #region Atributos
        private Elemento elemento;
        #endregion

        #region Propiedades
        public MovimientoParabolico Movimiento {get; set;}
        #endregion

        #region Constructores
        public Lanzar()
        {

        }

        #endregion

        #region Comportamientos
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            //TODO. Esto esta muy choto
            this.elemento = contexto.puebaFisica;

            //TODO. Tener en cuenta que la direccion se esta calculando mas arriba, aunque aqui se calcula la direccion si el perosnaje esta quieto. Analizar!!!
            //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
            float z = -(float)Math.Cos((float)contexto.personaje.mesh.Rotation.Y) * 50;
            float x = -(float)Math.Sin((float)contexto.personaje.mesh.Rotation.Y) * 50;
            //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
            //el vector salga del personaje.
            Vector3 direccion = contexto.personaje.mesh.Position + new Vector3(x, /*terreno.CalcularAltura(x, z) + */1, z);

            this.elemento.Mesh.Position = contexto.personaje.mesh.Position + new Vector3(0, 50, 0);

            this.Movimiento = new MovimientoParabolico(contexto.personaje.mesh.Position, direccion, 20, new MallaEnvoltura(this.elemento.Mesh));

            contexto.movimiento = this.Movimiento;

            contexto.personaje.mesh.playAnimation("Arrojar", true);
        }

        #endregion
    }
}
