using System;
using System.Collections.Generic;
using Microsoft.DirectX.DirectInput;
using TGC.Core.Input;
using TGC.Group.Model.Comandos;

namespace TGC.Group.Model.Administracion
{
    public class ControladorEntradas
    {
        #region Atributos

        private List<Comando> comandosSeleccionados;
        private TgcD3dInput d3dInput;

        private Golpear golpear;//TODO esto esta para poder solucionar la sincronizacion, pensar mejor!!!
        private Saltar saltar;//TODO esto esta para poder solucionar la sincronizacion, pensar mejor!!!
        private Lanzar lanzar;//TODO esto esta para poder solucionar la sincronizacion, pensar mejor!!!

        #endregion Atributos

        #region Cnstructores

        public ControladorEntradas(SuvirvalCraft contexto)
        {
            this.comandosSeleccionados = new List<Comando>();
            this.d3dInput = contexto.Input;
        }

        #endregion Cnstructores

        #region Comportamientos

        public List<Comando> ProcesarEntradasTeclado()
        {
            this.comandosSeleccionados.Clear();

            #region Paquete de fuciones (Fx)

            //Si preciono para ver el menú de ayuda
            if (d3dInput.keyDown(Key.F1))
            {
                this.comandosSeleccionados.Add(new Ayuda(this.TextoDeComandos()));
                return this.comandosSeleccionados;
            }
            //Si preciono para usar camara en primera persona
            if (d3dInput.keyDown(Key.F2))
            {
                this.comandosSeleccionados.Add(new CambiarCamara(CambiarCamara.PrimeraPersona));
                return this.comandosSeleccionados;
            }
            //Si preciono para usar camara en tercera persona
            if (d3dInput.keyDown(Key.F3))
            {
                this.comandosSeleccionados.Add(new CambiarCamara(CambiarCamara.TerceraPersona));
                return this.comandosSeleccionados;
            }
            //Si preciono para bajar la camara
            if (d3dInput.keyDown(Key.F5))
            {
                this.comandosSeleccionados.Add(new MoverCamara(MoverCamara.BajarCamara));
                return this.comandosSeleccionados;
            }
            //Si preciono para subir la camara
            if (d3dInput.keyDown(Key.F6))
            {
                this.comandosSeleccionados.Add(new MoverCamara(MoverCamara.SubirCamara));
                return this.comandosSeleccionados;
            }
            //Si preciono para Acercar la camara
            if (d3dInput.keyDown(Key.F7))
            {
                this.comandosSeleccionados.Add(new MoverCamara(MoverCamara.AcercarCamara));
                return this.comandosSeleccionados;
            }
            //Si preciono para alajar la camara
            if (d3dInput.keyDown(Key.F8))
            {
                this.comandosSeleccionados.Add(new MoverCamara(MoverCamara.AlejarCamara));
                return this.comandosSeleccionados;
            }

            #endregion Paquete de fuciones (Fx)

            #region Paquete de movimientos principales acelerados

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

            #endregion Paquete de movimientos principales acelerados

            #region Paquete de movimientos principales

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

            #endregion Paquete de movimientos principales

            #region Paquete de acciones con 2 teclas

            //Tirar un elemento
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D1))
            {
                Comando accion = new Tirar(Tirar.Uno);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D2))
            {
                Comando accion = new Tirar(Tirar.Dos);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D3))
            {
                Comando accion = new Tirar(Tirar.Tres);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D4))
            {
                Comando accion = new Tirar(Tirar.Cuatro);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D5))
            {
                Comando accion = new Tirar(Tirar.Cinco);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D6))
            {
                Comando accion = new Tirar(Tirar.Seis);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D7))
            {
                Comando accion = new Tirar(Tirar.Siete);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D8))
            {
                Comando accion = new Tirar(Tirar.Ocho);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }
            if (d3dInput.keyDown(Key.T) && d3dInput.keyDown(Key.D9))
            {
                Comando accion = new Tirar(Tirar.Nueve);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }

            #endregion Paquete de acciones con 2 teclas

            #region Paquete de acciones con 1 tecla

            //Seleccion de Arma palo
            if (d3dInput.keyDown(Key.D1))
            {
                this.comandosSeleccionados.Add(new ApagarAntorcha());
                Comando accion = new Seleccionar(Seleccionar.NumeroUno);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }

            //Seleccion de Arma Hacha
            if (d3dInput.keyDown(Key.D2))
            {
                this.comandosSeleccionados.Add(new ApagarAntorcha());
                Comando accion = new Seleccionar(Seleccionar.NumeroDos);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }

            //Seleccion Juntar
            if (d3dInput.keyDown(Key.J))
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
            if (d3dInput.keyDown(Key.C))
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
            if (d3dInput.keyDown(Key.Y))
            {
                Comando accion = new Interactuar(Interactuar.JuntarTodo);
                this.comandosSeleccionados.Add(accion);
                return this.comandosSeleccionados;
            }

            //Dejar Todo
            if (d3dInput.keyDown(Key.U))
            {
                Comando accion = new Interactuar(Interactuar.DejarTodo);
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
                    golpear.GolpeActual = Golpear.Pegar;
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
                    golpear.GolpeActual = Golpear.Patear;
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
            if (d3dInput.keyDown(Key.P))
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

            //Saltar adelante
            if (d3dInput.keyDown(Key.Z))
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

            //Imprimir menu de mochila del personaje
            if (d3dInput.keyDown(Key.M))
            {
                this.comandosSeleccionados.Add(new Menu(Menu.Mochila));
                return this.comandosSeleccionados;
            }

            //Encender una antorcha
            if (d3dInput.keyDown(Key.L))
            {
                this.comandosSeleccionados.Add(new EncenderAntorcha());
                return this.comandosSeleccionados;
            }

            this.comandosSeleccionados.Add(new Interactuar(Interactuar.Parado));
            return this.comandosSeleccionados;

            #endregion Paquete de acciones con 1 tecla
        }

        private String TextoDeComandos()
        {
            String comandos = "";
            comandos += "- Presione F1 para obtener ayuda" + System.Environment.NewLine;
            comandos += "- Presione F2 para usar la cámara en primera persona | F3 para la cámara en tercera persona" + System.Environment.NewLine;
            comandos += "- Presione W o Flecha Arriba para avanzar" + System.Environment.NewLine;
            comandos += "- Presione S o Flecha Abajo para retroceder" + System.Environment.NewLine;
            comandos += "- Presione D o Flecha Derecha para girar a la derecha" + System.Environment.NewLine;
            comandos += "- Presione A o Flecha Izquierda para girar a la izquierda" + System.Environment.NewLine;
            comandos += "- Presione Shift Derecho o Izquierdo para movimientos acelerados" + System.Environment.NewLine;
            comandos += "- Presione T y el número que desee (1-9) para dejar elementos de la mochila" + System.Environment.NewLine;
            comandos += "- Presione Número (1-9) para seleccionar arma configurada" + System.Environment.NewLine;
            comandos += "- Presione J para juntar un elemento del suelo" + System.Environment.NewLine;
            comandos += "- Presione E para encender un leño" + System.Environment.NewLine;//TODO. Que sean acciones en general, no una tecla para cada accion.
            comandos += "- Presione C para consumir algún alimento" + System.Environment.NewLine;
            comandos += "- Presione B para abrir los cajones" + System.Environment.NewLine;
            comandos += "- Presione Y para juntar cuando haya más de un elemento" + System.Environment.NewLine;
            comandos += "- Presione U para dejar cuando haya más de un elemento" + System.Environment.NewLine;
            comandos += "- Presione Control Derecho para golpear con el arma seleccionada" + System.Environment.NewLine;
            comandos += "- Presione Control Izquierdo para golpear con una patada" + System.Environment.NewLine;
            comandos += "- Presione la Barra Espaciadora para saltar" + System.Environment.NewLine;
            comandos += "- Presione Z para saltar hacia adelante" + System.Environment.NewLine;
            comandos += "- Presione P para lanzar una piedra" + System.Environment.NewLine;
            comandos += "- Presione L para encender una antorcha" + System.Environment.NewLine;
            comandos += "- Presione M para mostrar la mochila" + System.Environment.NewLine;
            comandos += "- Presione F5 para bajar la cámara, F6 para subir la cámara" + System.Environment.NewLine;
            comandos += "- Presione F7 para acercar la cámara (Válido solo para cámara en tercera persona)" + System.Environment.NewLine;
            comandos += "- Presione F8 para alejar la cámara (Válido solo para cámara en tercera persona)" + System.Environment.NewLine;
            return comandos;
        }

        #endregion Comportamientos
    }
}