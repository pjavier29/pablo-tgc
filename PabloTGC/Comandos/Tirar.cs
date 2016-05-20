using AlumnoEjemplos.MiGrupo;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Terrain;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class Tirar : Comando
    {
        #region Atributos
        #endregion

        #region Propiedades
        #endregion

        #region Constructores
        public Tirar()
        {

        }

        #endregion

        #region Comportamientos
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (contexto.personaje.elementosEnMochila().Count > 0)
            {
                //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
                float z = -(float)Math.Cos((float)contexto.personaje.mesh.Rotation.Y) * 150;
                float x = -(float)Math.Sin((float)contexto.personaje.mesh.Rotation.Y) * 150;
                //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
                //el vector salga del personaje.
                Vector3 direccion = contexto.personaje.mesh.Position + new Vector3(x, 0, z);
                direccion.Y = contexto.terreno.CalcularAltura(direccion.X, direccion.Z);

                Elemento elementoATirar = contexto.personaje.elementosEnMochila()[0];
                elementoATirar.posicion(direccion);
                contexto.elementos.Add(elementoATirar);
                contexto.personaje.elementosEnMochila().Remove(elementoATirar);
            }
        }
        #endregion
    }
}
