﻿using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using TGC.Core.Input;
using TGC.Group.Model.Comandos;

namespace TGC.Group.Model.Administracion
{
    public class ControladorEntradas
    {
        #region Cnstructores

        public ControladorEntradas(SuvirvalCraft contexto)
        {
            comandosSeleccionados = new List<Comando>();
            d3dInput = contexto.Input;
        }

        #endregion Cnstructores

        #region Atributos

        private readonly List<Comando> comandosSeleccionados;
        private readonly TgcD3dInput d3dInput;

        private Golpear golpear; //TODO esto esta para poder solucionar la sincronizacion, pensar mejor!!!
        private Saltar saltar; //TODO esto esta para poder solucionar la sincronizacion, pensar mejor!!!
        private Lanzar lanzar; //TODO esto esta para poder solucionar la sincronizacion, pensar mejor!!!

        #endregion Atributos

        #region Comportamientos

        public List<Comando> ProcesarEntradasTeclado()
        {
            comandosSeleccionados.Clear();

            #region Paquete de fuciones (Fx)

            //Si preciono para ver el menú de ayuda
            if (d3dInput.keyDown(Key.F1))
            {
                comandosSeleccionados.Add(new Ayuda(TextoDeComandos()));
                return comandosSeleccionados;
            }
            //Si preciono para usar camara en primera persona
            if (d3dInput.keyDown(Key.F2))
            {
                comandosSeleccionados.Add(new CambiarCamara(CambiarCamara.PrimeraPersona));
                return comandosSeleccionados;
            }
            //Si preciono para usar camara en tercera persona
            if (d3dInput.keyDown(Key.F3))
            {
                comandosSeleccionados.Add(new CambiarCamara(CambiarCamara.TerceraPersona));
                return comandosSeleccionados;
            }
            //Si preciono para bajar la camara
            if (d3dInput.keyDown(Key.F5))
            {
                comandosSeleccionados.Add(new MoverCamara(MoverCamara.BajarCamara));
                return comandosSeleccionados;
            }
            //Si preciono para subir la camara
            if (d3dInput.keyDown(Key.F6))
            {
                comandosSeleccionados.Add(new MoverCamara(MoverCamara.SubirCamara));
                return comandosSeleccionados;
            }
            //Si preciono para Acercar la camara
            if (d3dInput.keyDown(Key.F7))
            {
                comandosSeleccionados.Add(new MoverCamara(MoverCamara.AcercarCamara));
                return comandosSeleccionados;
            }
            //Si preciono para alajar la camara
            if (d3dInput.keyDown(Key.F8))
            {
                comandosSeleccionados.Add(new MoverCamara(MoverCamara.AlejarCamara));
                return comandosSeleccionados;
            }

            #endregion Paquete de fuciones (Fx)

            #region Paquete de movimientos principales acelerados

            //Si preciono para caminar más rápido para adelante
            if (d3dInput.keyDown(Key.RightShift) ||
                d3dInput.keyDown(Key.LeftShift) && (d3dInput.keyDown(Key.W) || d3dInput.keyDown(Key.Up)))
            {
                var mover = new Mover(-1f);
                mover.MovimientoRapido = true;
                comandosSeleccionados.Add(mover);
                //return this.comandosSeleccionados;
            }

            //Si preciono para caminar más rápido para atras
            if (d3dInput.keyDown(Key.RightShift) ||
                d3dInput.keyDown(Key.LeftShift) && (d3dInput.keyDown(Key.S) || d3dInput.keyDown(Key.Down)))
            {
                var mover = new Mover(1f);
                mover.MovimientoRapido = true;
                comandosSeleccionados.Add(mover);
                //return this.comandosSeleccionados;
            }

            //Si preciono para rotar más rápido para la derecha
            if (d3dInput.keyDown(Key.RightShift) ||
                d3dInput.keyDown(Key.LeftShift) && (d3dInput.keyDown(Key.Right) || d3dInput.keyDown(Key.D)))
            {
                var rotar = new Girar(1f);
                rotar.MovimientoRapido = true;
                comandosSeleccionados.Add(rotar);
                //return this.comandosSeleccionados;
            }

            //Si preciono para rotar más rápido para la izquierda
            if (d3dInput.keyDown(Key.RightShift) ||
                d3dInput.keyDown(Key.LeftShift) && (d3dInput.keyDown(Key.Left) || d3dInput.keyDown(Key.A)))
            {
                var rotar = new Girar(-1f);
                rotar.MovimientoRapido = true;
                comandosSeleccionados.Add(rotar);
                //return this.comandosSeleccionados;
            }
            if (comandosSeleccionados.Count > 0)
            {
                return comandosSeleccionados;
            }

            #endregion Paquete de movimientos principales acelerados

            #region Paquete de movimientos principales

            //Movimiento para adelante
            if (d3dInput.keyDown(Key.W) || d3dInput.keyDown(Key.Up))
            {
                Comando accion = new Mover(-1f);
                comandosSeleccionados.Add(accion);
                //return this.comandosSeleccionados;
            }

            //Movimiento para Atras
            if (d3dInput.keyDown(Key.S) || d3dInput.keyDown(Key.Down))
            {
                Comando accion = new Mover(1f);
                comandosSeleccionados.Add(accion);
                //return this.comandosSeleccionados;
            }

            //Rotar Derecha
            if (d3dInput.keyDown(Key.Right) || d3dInput.keyDown(Key.D))
            {
                Comando rotar = new Girar(1f);
                comandosSeleccionados.Add(rotar);
                //return this.comandosSeleccionados;
            }

            //Rotar Izquierda
            if (d3dInput.keyDown(Key.Left) || d3dInput.keyDown(Key.A))
            {
                Comando rotar = new Girar(-1f);
                comandosSeleccionados.Add(rotar);
                //return this.comandosSeleccionados;
            }
            if (comandosSeleccionados.Count > 0)
            {
                return comandosSeleccionados;
            }

            #endregion Paquete de movimientos principales

            #region Paquete de acciones con 2 teclas

            //Tirar un elemento
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D1))
            {
                Comando accion = new Tirar(Tirar.Uno);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D2))
            {
                Comando accion = new Tirar(Tirar.Dos);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D3))
            {
                Comando accion = new Tirar(Tirar.Tres);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D4))
            {
                Comando accion = new Tirar(Tirar.Cuatro);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D5))
            {
                Comando accion = new Tirar(Tirar.Cinco);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D6))
            {
                Comando accion = new Tirar(Tirar.Seis);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D7))
            {
                Comando accion = new Tirar(Tirar.Siete);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D8))
            {
                Comando accion = new Tirar(Tirar.Ocho);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D9))
            {
                Comando accion = new Tirar(Tirar.Nueve);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }

            #endregion Paquete de acciones con 2 teclas

            #region Paquete de acciones con 1 tecla

            //Seleccion de Arma palo
            if (d3dInput.keyDown(Key.D1))
            {
                comandosSeleccionados.Add(new ApagarAntorcha());
                Comando accion = new Seleccionar(Seleccionar.NumeroUno);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }

            //Seleccion de Arma Hacha
            if (d3dInput.keyDown(Key.D2))
            {
                comandosSeleccionados.Add(new ApagarAntorcha());
                Comando accion = new Seleccionar(Seleccionar.NumeroDos);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }

            //Seleccion Juntar
            if (d3dInput.keyDown(Key.J))
            {
                Comando accion = new Interactuar(Interactuar.Juntar);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }

            //Seleccion Encender
            if (d3dInput.keyDown(Key.E))
            {
                Comando accion = new Interactuar(Interactuar.Encender);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }

            //Seleccion Consumir
            if (d3dInput.keyDown(Key.C))
            {
                Comando accion = new Interactuar(Interactuar.Consumir);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }

            //Abrir
            if (d3dInput.keyDown(Key.B))
            {
                Comando accion = new Interactuar(Interactuar.Abrir);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }

            //Juntar todo
            if (d3dInput.keyDown(Key.Y))
            {
                Comando accion = new Interactuar(Interactuar.JuntarTodo);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }

            //Dejar Todo
            if (d3dInput.keyDown(Key.U))
            {
                Comando accion = new Interactuar(Interactuar.DejarTodo);
                comandosSeleccionados.Add(accion);
                return comandosSeleccionados;
            }

            //Pegar una piña
            if (d3dInput.keyDown(Key.RightControl))
            {
                if (golpear == null)
                {
                    golpear = new Golpear(Golpear.Pegar);
                }
                else
                {
                    golpear.GolpeActual = Golpear.Pegar;
                }
                comandosSeleccionados.Add(golpear);
                return comandosSeleccionados;
            }

            //Pegar una patada
            if (d3dInput.keyDown(Key.LeftControl))
            {
                if (golpear == null)
                {
                    golpear = new Golpear(Golpear.Patear);
                }
                else
                {
                    golpear.GolpeActual = Golpear.Patear;
                }
                comandosSeleccionados.Add(golpear);
                return comandosSeleccionados;
            }

            //Saltar
            if (d3dInput.keyDown(Key.Space))
            {
                if (saltar == null)
                {
                    saltar = new Saltar(Saltar.EnLugar);
                }
                else if (saltar.Movimiento.Finalizo)
                {
                    saltar = new Saltar(Saltar.EnLugar);
                }
                comandosSeleccionados.Add(saltar);
                return comandosSeleccionados;
            }

            //Lanza un elemento con fuerza
            if (d3dInput.keyDown(Key.P))
            {
                if (lanzar == null)
                {
                    lanzar = new Lanzar();
                    comandosSeleccionados.Add(lanzar);
                }
                else if (lanzar.Movimiento != null && lanzar.Movimiento.Finalizo)
                {
                    lanzar = null;
                    lanzar = new Lanzar();
                    comandosSeleccionados.Add(lanzar);
                }
                return comandosSeleccionados;
            }

            //Saltar adelante
            if (d3dInput.keyDown(Key.Z))
            {
                if (saltar == null)
                {
                    saltar = new Saltar(Saltar.Adelante);
                }
                else if (saltar.Movimiento.Finalizo)
                {
                    saltar = new Saltar(Saltar.Adelante);
                }
                comandosSeleccionados.Add(saltar);
                return comandosSeleccionados;
            }

            //Imprimir menu de mochila del personaje
            if (d3dInput.keyDown(Key.M))
            {
                comandosSeleccionados.Add(new Menu(Menu.Mochila));
                return comandosSeleccionados;
            }

            //Encender una antorcha
            if (d3dInput.keyDown(Key.L))
            {
                comandosSeleccionados.Add(new EncenderAntorcha());
                return comandosSeleccionados;
            }

            comandosSeleccionados.Add(new Interactuar(Interactuar.Parado));
            return comandosSeleccionados;

            #endregion Paquete de acciones con 1 tecla
        }

        private string TextoDeComandos()
        {
            var comandos = "";
            comandos += "- Presione F1 para obtener ayuda" + Environment.NewLine;
            comandos += "- Presione F2 para usar la cámara en primera persona | F3 para la cámara en tercera persona" +
                        Environment.NewLine;
            comandos += "- Presione W o Flecha Arriba para avanzar" + Environment.NewLine;
            comandos += "- Presione S o Flecha Abajo para retroceder" + Environment.NewLine;
            comandos += "- Presione D o Flecha Derecha para girar a la derecha" + Environment.NewLine;
            comandos += "- Presione A o Flecha Izquierda para girar a la izquierda" + Environment.NewLine;
            comandos += "- Presione Shift Derecho o Izquierdo para movimientos acelerados" + Environment.NewLine;
            comandos += "- Presione T y el número que desee (1-9) para dejar elementos de la mochila" +
                        Environment.NewLine;
            comandos += "- Presione Número (1-9) para seleccionar arma configurada" + Environment.NewLine;
            comandos += "- Presione J para juntar un elemento del suelo" + Environment.NewLine;
            comandos += "- Presione E para encender un leño" + Environment.NewLine;
            //TODO. Que sean acciones en general, no una tecla para cada accion.
            comandos += "- Presione C para consumir algún alimento" + Environment.NewLine;
            comandos += "- Presione B para abrir los cajones" + Environment.NewLine;
            comandos += "- Presione Y para juntar cuando haya más de un elemento" + Environment.NewLine;
            comandos += "- Presione U para dejar cuando haya más de un elemento" + Environment.NewLine;
            comandos += "- Presione Control Derecho para golpear con el arma seleccionada" + Environment.NewLine;
            comandos += "- Presione Control Izquierdo para golpear con una patada" + Environment.NewLine;
            comandos += "- Presione la Barra Espaciadora para saltar" + Environment.NewLine;
            comandos += "- Presione Z para saltar hacia adelante" + Environment.NewLine;
            comandos += "- Presione P para lanzar una piedra" + Environment.NewLine;
            comandos += "- Presione L para encender una antorcha" + Environment.NewLine;
            comandos += "- Presione M para mostrar la mochila" + Environment.NewLine;
            comandos += "- Presione F5 para bajar la cámara, F6 para subir la cámara" + Environment.NewLine;
            comandos += "- Presione F7 para acercar la cámara (Válido solo para cámara en tercera persona)" +
                        Environment.NewLine;
            comandos += "- Presione F8 para alejar la cámara (Válido solo para cámara en tercera persona)" +
                        Environment.NewLine;
            return comandos;
        }

        #endregion Comportamientos
    }
}