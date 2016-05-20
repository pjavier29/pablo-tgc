using AlumnoEjemplos.PabloTGC.Comandos;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Input;

namespace AlumnoEjemplos.PabloTGC.Administracion
{
    public class ControladorEntradas
    {
        #region Atributos
        private List<Comando> comandosSeleccionados;
        private TgcD3dInput d3dInput;

        private Golpear golpear;//TODO esto esta para poder solucionar la sincronizacion, pensar mejor!!!
        private Saltar saltar;//TODO esto esta para poder solucionar la sincronizacion, pensar mejor!!!
        private Lanzar lanzar;//TODO esto esta para poder solucionar la sincronizacion, pensar mejor!!!
        #endregion

        #region Cnstructores
        public ControladorEntradas()
        {
            this.comandosSeleccionados = new List<Comando>();
            this.d3dInput = GuiController.Instance.D3dInput;
        }
        #endregion

        #region Comportamientos

        public List<Comando> ProcesarEntradasTeclado()
        {
            this.comandosSeleccionados.Clear();

            //*************Comando mas prioritario**************
            //Saltar adelante
            if (/*d3dInput.keyDown(Key.Space) && d3dInput.keyDown(Key.W)*/d3dInput.keyDown(Key.E))
            {
                if (saltar == null)
                {
                    saltar = new Saltar(Saltar.Adelante);
                }
                else if ((saltar.Movimiento.Finalizo))
                {
                    saltar = new Saltar(Saltar.Adelante);
                }
                this.comandosSeleccionados.Add(saltar);
                return this.comandosSeleccionados;
            }
            //*************Comando mas prioritario**************

            //**********************Paquete de movimientos principales acelerados***********************
            //Si preciono para caminar más rápido para adelante
            if (d3dInput.keyDown(Key.RightShift) || d3dInput.keyDown(Key.LeftShift) && (d3dInput.keyDown(Key.W) || d3dInput.keyDown(Key.Up)))
            {
                Mover mover = new Mover(-1f);
                mover.MovimientoRapido = true;
                this.comandosSeleccionados.Add(mover);
                //return this.comandosSeleccionados;
            }

            //Si preciono para caminar más rápido para atras
            if (d3dInput.keyDown(Key.RightShift) || d3dInput.keyDown(Key.LeftShift) && (d3dInput.keyDown(Key.S) || d3dInput.keyDown(Key.Down)))
            {
                Mover mover = new Mover(1f);
                mover.MovimientoRapido = true;
                this.comandosSeleccionados.Add(mover);
                //return this.comandosSeleccionados;
            }

            //Si preciono para rotar más rápido para la derecha
            if (d3dInput.keyDown(Key.RightShift) || d3dInput.keyDown(Key.LeftShift) && (d3dInput.keyDown(Key.Right) || d3dInput.keyDown(Key.D)))
            {
                Girar rotar = new Girar(1f);
                rotar.MovimientoRapido = true;
                this.comandosSeleccionados.Add(rotar);
                //return this.comandosSeleccionados;
            }

            //Si preciono para rotar más rápido para la izquierda
            if (d3dInput.keyDown(Key.RightShift) || d3dInput.keyDown(Key.LeftShift) && (d3dInput.keyDown(Key.Left) || d3dInput.keyDown(Key.A)))
            {
                Girar rotar = new Girar(-1f);
                rotar.MovimientoRapido = true;
                this.comandosSeleccionados.Add(rotar);
                //return this.comandosSeleccionados;
            }
            if (this.comandosSeleccionados.Count > 0)
            {
                return this.comandosSeleccionados;
            }
            //**********************Paquete de movimientos principales acelerados***********************

            //**********************Paquete de movimientos principales***********************
            //Movimiento para adelante
            if (d3dInput.keyDown(Key.W) || d3dInput.keyDown(Key.Up))
            {
                Comando accion = new Mover(-1f);
                this.comandosSeleccionados.Add(accion);
                //return this.comandosSeleccionados;
            }

            //Movimiento para Atras
            if (d3dInput.keyDown(Key.S) || d3dInput.keyDown(Key.Down))
            {
                Comando accion = new Mover(1f);
                this.comandosSeleccionados.Add(accion);
                //return this.comandosSeleccionados;
            }

            //Rotar Derecha
            if (d3dInput.keyDown(Key.Right) || d3dInput.keyDown(Key.D))
            {
                Comando rotar = new Girar(1f);
                this.comandosSeleccionados.Add(rotar);
                //return this.comandosSeleccionados;
            }

            //Rotar Izquierda
            if (d3dInput.keyDown(Key.Left) || d3dInput.keyDown(Key.A))
            {
                Comando rotar = new Girar(-1f);
                this.comandosSeleccionados.Add(rotar);
                //return this.comandosSeleccionados;
            }
            if (this.comandosSeleccionados.Count > 0)
            {
                return this.comandosSeleccionados;
            }
            //**********************Paquete de movimientos principales***********************

            //Seleccion de Arma palo
            if (d3dInput.keyDown(Key.D1))
            {
                Comando accion = new Seleccionar(Seleccionar.NumeroUno);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }

            //Seleccion de Arma Hacha
            if (d3dInput.keyDown(Key.D2))
            {
                Comando accion = new Seleccionar(Seleccionar.NumeroDos);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }

            //Seleccion Juntar
            if (d3dInput.keyDown(Key.R))
            {
                Comando accion = new Interactuar(Interactuar.Juntar);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }

            //Seleccion Encender
            if (d3dInput.keyDown(Key.E))
            {
                Comando accion = new Interactuar(Interactuar.Encender);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }

            //Seleccion Consumir
            if (d3dInput.keyDown(Key.U))
            {
                Comando accion = new Interactuar(Interactuar.Consumir);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }

            //Abrir
            if (d3dInput.keyDown(Key.B))
            {
                Comando accion = new Interactuar(Interactuar.Abrir);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }

            //Juntar todo
            if (d3dInput.keyDown(Key.J))
            {
                Comando accion = new Interactuar(Interactuar.JuntarTodo);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }

            //Dejar Elemento
            if (d3dInput.keyDown(Key.H))
            {
                Comando accion = new Interactuar(Interactuar.DejarElemento);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }

            //Tirar un elemento
            if (d3dInput.keyDown(Key.T))
            {
                Comando accion = new Tirar();
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
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
                    if (golpear.PuedeGolpear)
                    {
                        golpear.GolpeActual = Golpear.Pegar;
                    }    
                }
                this.comandosSeleccionados.Add(golpear);
                return this.comandosSeleccionados;
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
                    if (golpear.PuedeGolpear)
                    {
                        golpear.GolpeActual = Golpear.Patear;
                    }
                }
                this.comandosSeleccionados.Add(golpear);
                return this.comandosSeleccionados;
            }

            //Saltar
            if (d3dInput.keyDown(Key.Space))
            {
                if (saltar == null)
                {
                    saltar = new Saltar(Saltar.EnLugar);
                }
                else if ((saltar.Movimiento.Finalizo))
                {
                    saltar = new Saltar(Saltar.EnLugar);
                }
                this.comandosSeleccionados.Add(saltar);
                return this.comandosSeleccionados;
            }

            //Lanza un elemento con fuerza
            if (d3dInput.keyDown(Key.C))
            {
                if (lanzar == null)
                {
                    lanzar = new Lanzar();
                    this.comandosSeleccionados.Add(lanzar);
                }
                else if (lanzar.Movimiento != null && lanzar.Movimiento.Finalizo)
                {
                    lanzar = null;
                    lanzar = new Lanzar();
                    this.comandosSeleccionados.Add(lanzar);
                }
                return this.comandosSeleccionados;
            }

            this.comandosSeleccionados.Add(new Interactuar(Interactuar.Parado));
            return this.comandosSeleccionados;
        }

        #endregion
    }
}
