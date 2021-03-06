﻿using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.ElementosJuego;
using TGC.Group.Model.Utiles;

namespace TGC.Group.Model.Comandos
{
    public class Tirar : Comando
    {
        #region Atributos

        private readonly int numeroATirar;

        #endregion Atributos

        #region Constructores

        public Tirar(int numero)
        {
            numeroATirar = numero;
        }

        #endregion Constructores

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (contexto.personaje.ContieneElementoEnPosicionDeMochila(numeroATirar))
            {
                //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
                var z = -(float)Math.Cos(contexto.personaje.mesh.Rotation.Y) * 150;
                var x = -(float)Math.Sin(contexto.personaje.mesh.Rotation.Y) * 150;
                //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
                //el vector salga del personaje.
                var posicionElemento = contexto.personaje.mesh.Position + new Vector3(x, 0, z);
                posicionElemento.Y = contexto.terreno.CalcularAltura(posicionElemento.X, posicionElemento.Z);

                var elementoATirar = contexto.personaje.DarElementoEnPosicionDeMochila(numeroATirar);
                elementoATirar.posicion(posicionElemento);

                var posiblesColisiones = new List<Elemento>();
                foreach (var elem in contexto.optimizador.ElementosColision)
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
                foreach (var elem in posiblesColisiones)
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
    }
}