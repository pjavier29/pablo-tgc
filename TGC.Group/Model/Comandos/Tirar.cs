using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.ElementosJuego;
using TGC.Group.Model.Utiles;

namespace TGC.Group.Model.Comandos
{
    public class Tirar : Comando
    {
        #region Constantes

        public const int Uno = 0;
        public const int Dos = 1;
        public const int Tres = 2;
        public const int Cuatro = 3;
        public const int Cinco = 4;
        public const int Seis = 5;
        public const int Siete = 6;
        public const int Ocho = 7;
        public const int Nueve = 8;

        #endregion Constantes

        #region Atributos

        private int numeroATirar;

        #endregion Atributos



        #region Constructores

        public Tirar(int numero)
        {
            this.numeroATirar = numero;
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (contexto.personaje.ContieneElementoEnPosicionDeMochila(this.numeroATirar))
            {
                //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
                float z = -(float)Math.Cos((float)contexto.personaje.mesh.Rotation.Y) * 150;
                float x = -(float)Math.Sin((float)contexto.personaje.mesh.Rotation.Y) * 150;
                //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
                //el vector salga del personaje.
                Vector3 posicionElemento = contexto.personaje.mesh.Position + new Vector3(x, 0, z);
                posicionElemento.Y = contexto.terreno.CalcularAltura(posicionElemento.X, posicionElemento.Z);

                Elemento elementoATirar = contexto.personaje.DarElementoEnPosicionDeMochila(this.numeroATirar);
                elementoATirar.posicion(posicionElemento);

                List<Elemento> posiblesColisiones = new List<Elemento>();
                foreach (Elemento elem in contexto.optimizador.ElementosColision)
                {
                    //TODO. Optimizar esto para solo objetos cernanos!!!!!!!!
                    if (ControladorColisiones.CuadradoColisionaCuadrano(elementoATirar.BoundingBox(), elem.BoundingBox()))
                    {
                        posiblesColisiones.Add(elem);
                        if (!elem.AdmiteMultipleColision())
                        {
                            break;
                        }
                    }
                }
                foreach (Elemento elem in posiblesColisiones)
                {
                    elem.ProcesarColisionConElemento(elementoATirar);
                }

                //De todas formas se procesa el tirado del elemento.
                contexto.elementos.Add(elementoATirar);
                contexto.optimizador.ForzarActualizacionElementosColision();
                contexto.personaje.Dejar(elementoATirar);
            }
        }

        #endregion Comportamientos
    }
}