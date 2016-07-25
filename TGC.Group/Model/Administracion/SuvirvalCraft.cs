using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TGC.Group.Model.Comandos;
using TGC.Group.Model.ElementosDia;
using TGC.Group.Model.ElementosJuego;
using TGC.Group.Model.Movimientos;
using TGC.Group.Model.Utiles;
using TGC.Group.Model.Utiles.Camaras;
using TGC.Group.Model.Utiles.Efectos;
using TGC.Core.SceneLoader;
using TGC.Core.Sound;
using TGC.Core;
using TGC.Core.Direct3D;
using TGC.Core.Terrain;
using TGC.Core.Example;

namespace TGC.Group.Model.Administracion
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class SuvirvalCraft : TgcExample
    {
        //TODO. Refactorizar esta lógica, hay demasiados atributos publicos en la apicacion principal.
        public Terreno terreno;

        public TgcSkyBox skyBox;
        public TgcMesh piso;
        public List<Elemento> elementos;
        public Personaje personaje;
        public float tiempo;
        public TgcText2d estadoJuego;
        public TgcText2d informativo;
        public Animal oveja;
        public Animal gallo;
        public Elemento cajonReal;
        public Elemento cajonOlla;
        public Elemento fuenteAgua;

        public TgcSprite mochila;
        public TgcSprite miniMapa;
        public TgcText2d referenciaMiniMapa;
        public TgcSprite linea;
        public Point coordenadaSuperiorDerecha;
        public Point coordenadaInferiorIzquierda;
        public TgcSprite cajon;
        public TgcText2d mochilaReglon1;
        public TgcText2d cajonReglon1;
        public bool mostrarMenuMochila;
        public bool mostrarMenuCajon;

        public Elemento puebaFisica;
        public MovimientoParabolico movimiento;
        public MovimientoParabolico movimientoPersonaje;

        public ControladorEntradas controladorEntradas;

        public Vector3 esquina;//Con un solo punto arma el cuadrado para definir los limites del mapa

        public Optimizador optimizador;

        public TgcSprite salud;
        public TgcSprite hidratacion;
        public TgcSprite alimentacion;
        public TgcSprite cansancio;
        public TgcSprite saludIcono;
        public TgcSprite hidratacionIcono;
        public TgcSprite alimentacionIcono;
        public TgcSprite cansancioIcono;
        public TgcSprite objetivosIcono;
        public TgcText2d mensajeObjetivo1;
        public float tiempoObjetivo;
        public TgcSprite ayuda;
        public TgcText2d ayudaReglon1;
        public TgcText2d ayudaReglon2;
        public bool mostrarAyuda;
        public TgcText2d temperaturaDia;
        public TgcText2d horaDia;
        public TgcSprite temperaturaDiaIcono;
        public TgcSprite horaDiaIcono;
        public TgcText2d temperaturaPersonaje;
        public TgcSprite temperaturaPersonajeIcono;
        public TgcSprite estadoDiaSolIcono;
        public TgcSprite estadoDiaLunaIcono;
        public TgcSprite estadoDiaLluviaIcono;

        public Camara camara;

        public Dia dia;

        //TODO. Ver si no conviente tener un administrador de efectos
        public Efecto pisoEfecto;

        public Efecto skyboxEfecto;
        public Efecto efectoTerreno;
        public Efecto efectoLuz;
        public Efecto efectoLuz2;
        public Efecto efectoAlgas;
        public Efecto efectoAlgas2;
        public Efecto efectoBotes;
        public Efecto efectoFuego;
        public Efecto efectoFuego2;
        public Efecto efectoArbol;
        public Efecto efectoArbol2;

        public VertexBuffer screenQuadVB;
        public Texture renderTarget2D;
        private Surface pOldRT;
        private Surface pSurf;
        public Microsoft.DirectX.Direct3D.Effect efectoLluvia;

        public TgcStaticSound sonidoGolpePatada;
        public TgcStaticSound sonidoGolpe;
        public TgcStaticSound sonidoLluvia;
        public List<String> musicas;
        public Tgc3dSound sonidoGrillos;
        public Tgc3dSound sonidoGrillos2;

        /// <summary>
        /// Constructor del juego
        /// </summary>
        /// <param name="mediaDir">Ruta donde esta la carpeta con los assets</param>
        /// <param name="shadersDir">Ruta donde esta la carpeta con los shaders</param>
        public SuvirvalCraft(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = "AlumnoEjemplos";
            Name = "Pablo TGC - Suvirval Craft";
            Description = "Juego de supervivencia en donde un personaje debe sobrevivir durante un tiempo determinado."
                + System.Environment.NewLine +
                "Para ello debe hidratarse, y consumir alimentos. Puede eliminar animales y crear su propia hamburguesa, a ver si descubre cómo hacerlo."
                + System.Environment.NewLine +
                "Tambien puede comer frutas desde los árboles."
                + System.Environment.NewLine +
                "Tenga cuidado con la temperatura del dia y de su personaje, si la temperatura del personaje llega a 34° morirá."
                + System.Environment.NewLine +
                "Podría ser últil crear algún fuego para evitar esto."
                + System.Environment.NewLine +
                "Busque los cajones indicados en el mini mapa, encontrará algunas sorpresas."
                + System.Environment.NewLine +
                "Buena Suerte!!!!";
        }
        
        /// <summary>
        /// Método que se llama una sola vez,  al principio cuando se ejecuta el ejemplo.
        /// Escribir aquí todo el código de inicialización: cargar modelos, texturas, modifiers, uservars, etc.
        /// Borrar todo lo que no haga falta
        /// </summary>
        public override void Init()
        {
            Configuracion con = new Configuracion(new ConfiguracionModel(this, D3DDevice.Instance.Device, this.MediaDir));
            con.ShowDialog();
        }

        public override void Update()
        {
            //TODO en la nueva estructura del TGC.Core se desacoplo el render del update logico
        }

        /// <summary>
        /// Método que se llama cada vez que hay que refrescar la pantalla.
        /// Escribir aquí todo el código referido al renderizado.
        /// Borrar todo lo que no haga falta
        /// </summary>
        public override void Render()
        {
            //Device de DirectX para renderizar
            Microsoft.DirectX.Direct3D.Device d3dDevice = D3DDevice.Instance.Device;

            //Reproduccion de sonidos
            this.ReproducirMusica();

            //Actualizamos el dia
            dia.Actualizar(this, ElapsedTime);

            //Surface pSurf = null;
            if (dia.GetLluvia().EstaLloviendo())
            {
                pOldRT = d3dDevice.GetRenderTarget(0);
                pSurf = renderTarget2D.GetSurfaceLevel(0);
                d3dDevice.SetRenderTarget(0, pSurf);
                d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            }

            d3dDevice.BeginScene();

            //TgcD3dInput d3dInput = GuiController.Instance.D3dInput;
            tiempo += ElapsedTime;

            informativo.Text = "Obtener Ayuda (F1)";
            mostrarMenuMochila = false;
            mostrarMenuCajon = false;
            mostrarAyuda = false;

            foreach (Comando comando in controladorEntradas.ProcesarEntradasTeclado())
            {
                comando.Ejecutar(this, ElapsedTime);
            }

            camara.Render(personaje, this);

            optimizador.Actualizar(personaje.mesh.Position);

            //Afectamos salud por paso de tiempo y por la temperatura
            personaje.AfectarSaludPorTiempo(ElapsedTime);
            personaje.AfectarSaludPorTemperatura(dia.TemperaturaActual(), ElapsedTime);

            //Actualizamos los objetos Fisicos y los renderizamos
            if (movimiento != null)
            {
                movimiento.update(ElapsedTime, terreno);
                movimiento.render();
                if (movimiento.Finalizo)
                {
                    movimiento = null;
                }
            }
            else
            {
                puebaFisica.renderizar(this);
            }

            if (movimientoPersonaje != null)
            {
                movimientoPersonaje.update(ElapsedTime, terreno);
                movimientoPersonaje.render();
                if (movimientoPersonaje.Finalizo)
                {
                    movimientoPersonaje = null;
                }
            }

            //Render Terreno
            terreno.Renderizar(this);

            //Render piso
            pisoEfecto.Actualizar(this);
            piso.render();

            //Renderizar SkyBox
            foreach (TgcMesh faces in this.skyBox.Faces)
            {
                skyboxEfecto.Actualizar(this);
                faces.render();
            }

            //Actualiza los elementos
            List<Elemento> aux = new List<Elemento>();
            aux.AddRange(elementos);//TODO. Porque sino con la actualizacion borramos o agregamos elementos de la coleccion y se rompe todo
            foreach (Elemento elemento in aux)
            {
                elemento.Actualizar(this, ElapsedTime);
            }

            foreach (Elemento elem in optimizador.ElementosRenderizacion)
            {
                elem.renderizar(this);
            }

            //dia.GetSol().Mesh.render();

            d3dDevice.EndScene();

            if (dia.GetLluvia().EstaLloviendo())
            {
                //Liberar memoria de surface de Render Target
                pSurf.Dispose();
                //Ahora volvemos a restaurar el Render Target original (osea dibujar a la pantalla)
                d3dDevice.SetRenderTarget(0, pOldRT);

                //Arrancamos la escena
                d3dDevice.BeginScene();

                //Cargamos para renderizar el unico modelo que tenemos, un Quad que ocupa toda la pantalla, con la textura de todo lo dibujado antes
                d3dDevice.VertexFormat = CustomVertex.PositionTextured.Format;
                d3dDevice.SetStreamSource(0, screenQuadVB, 0);

                //Cargamos parametros en el shader de Post-Procesado
                efectoLluvia.SetValue("render_target2D", renderTarget2D);
                efectoLluvia.SetValue("intensidad_ancho", dia.GetLluvia().AnchoLluvia());
                efectoLluvia.SetValue("intensidad_alto", dia.GetLluvia().AltoLluvia());
                efectoLluvia.SetValue("lightIntensityRelitive", dia.GetSol().IntensidadRelativa());

                //Limiamos la pantalla y ejecutamos el render del shader
                d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
                efectoLluvia.Begin(FX.None);
                efectoLluvia.BeginPass(0);
                d3dDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
                efectoLluvia.EndPass();
                efectoLluvia.End();

                //Terminamos el renderizado de la escena
                d3dDevice.EndScene();
            }

            //Personaje muerto
            if (personaje.estaMuerto())
            {
                estadoJuego.Color = Color.Red;
                estadoJuego.Text = "Game Over";
                estadoJuego.render();
            }
            else if (this.tiempo > this.tiempoObjetivo)
            {
                estadoJuego.Color = Color.Green;
                estadoJuego.Text = "Has Ganado";
                estadoJuego.render();
            }

            if (informativo.Text != "")
            {
                informativo.render();
            }

            TgcDrawer2D.Instance.beginDrawSprite();
            saludIcono.render();
            hidratacionIcono.render();
            alimentacionIcono.render();
            cansancioIcono.render();
            salud.Scaling = new Vector2(personaje.PorcentajeDeSalud() * 0.5f, 0.3f);
            salud.render();
            hidratacion.Scaling = new Vector2(personaje.PorcentajeDeHidratacion() * 0.5f, 0.3f);
            hidratacion.render();
            alimentacion.Scaling = new Vector2(personaje.PorcentajeDeAlimentacion() * 0.5f, 0.3f);
            alimentacion.render();
            cansancio.Scaling = new Vector2(personaje.PorcentajeDeCansancio() * 0.5f, 0.3f);
            cansancio.render();
            objetivosIcono.render();
            miniMapa.render();
            TgcDrawer2D.Instance.endDrawSprite();

            referenciaMiniMapa.Position = this.PosicionarReferencia(personaje.mesh.Position);
            referenciaMiniMapa.Color = Color.Orange;
            referenciaMiniMapa.render();
            referenciaMiniMapa.Position = this.PosicionarReferencia(fuenteAgua.posicion());
            referenciaMiniMapa.Color = Color.Blue;
            referenciaMiniMapa.render();
            referenciaMiniMapa.Position = this.PosicionarReferencia(cajonReal.posicion());
            referenciaMiniMapa.Color = Color.Brown;
            referenciaMiniMapa.render();
            referenciaMiniMapa.Position = this.PosicionarReferencia(cajonOlla.posicion());
            referenciaMiniMapa.Color = Color.Brown;
            referenciaMiniMapa.render();

            mensajeObjetivo1.Text = "Sobrevivir " + System.Environment.NewLine + TimeSpan.FromSeconds(this.tiempoObjetivo - this.tiempo).ToString(@"hh\:mm\:ss");
            mensajeObjetivo1.render();

            TgcDrawer2D.Instance.beginDrawSprite();
            linea.Rotation = personaje.mesh.Rotation.Y;
            linea.render();

            horaDia.Text = dia.HoraActualTexto();
            temperaturaDia.Text = dia.TemperaturaActualTexto();
            temperaturaPersonaje.Text = personaje.TemperaturaCorporalTexto();
            horaDia.render();
            temperaturaDia.render();
            temperaturaPersonajeIcono.render();
            temperaturaPersonaje.render();
            temperaturaDiaIcono.render();
            horaDiaIcono.render();
            if (dia.GetLluvia().EstaLloviendo())
            {
                estadoDiaLluviaIcono.render();
            }
            else
            {
                if (dia.EsDeDia())
                {
                    estadoDiaSolIcono.render();
                }
                else
                {
                    estadoDiaLunaIcono.render();
                }
            }
            TgcDrawer2D.Instance.endDrawSprite();

            if (mostrarMenuMochila)
            {
                //Iniciar dibujado de todos los Sprites de la escena (en este caso es solo uno)
                TgcDrawer2D.Instance.beginDrawSprite();
                //Dibujar sprite (si hubiese mas, deberian ir todos aquí)
                mochila.render();
                if (mostrarMenuCajon)
                {
                    //El cajon se muestra siempre junto con la mochila
                    cajon.render();
                }
                //Finalizar el dibujado de Sprites
                TgcDrawer2D.Instance.endDrawSprite();
                //TODO. Por el momento podemos mantener todo en un renglon ya que no imprimimos ninguna imagen de los elementos en cuestion
                mochilaReglon1.Text = "";
                for (int i = 0; i < 9; i++)
                {
                    if (personaje.ContieneElementoEnPosicionDeMochila(i))
                    {
                        mochilaReglon1.Text = mochilaReglon1.Text + (i + 1).ToString() + "    " + personaje.DarElementoEnPosicionDeMochila(i).GetDescripcion() + System.Environment.NewLine;
                    }
                    else
                    {
                        mochilaReglon1.Text = mochilaReglon1.Text + (i + 1).ToString() + "    " + "Disponible" + System.Environment.NewLine;
                    }
                }
                mochilaReglon1.render();
                if (mostrarMenuCajon)
                {
                    //En texto que va en el renglon lo completa el cajon cuando hay una interaccion.
                    cajonReglon1.render();
                }
            }

            if (mostrarAyuda)
            {
                //TODO poner todos los Sprite dentro de una sola apertura y cierre del controlador
                TgcDrawer2D.Instance.beginDrawSprite();
                ayuda.render();
                TgcDrawer2D.Instance.endDrawSprite();
                ayudaReglon1.Position = new Point(((int)(ayuda.Position.X)) + 40, ((int)(ayuda.Position.Y)) + 130);
                ayudaReglon1.render();
                ayudaReglon2.render();
            }

            RenderFPS();
        }

        /// <summary>
        /// Método que se llama cuando termina la ejecución del ejemplo.
        /// Hacer dispose() de todos los objetos creados.
        /// </summary>
        public override void Dispose()
        {
            piso.dispose();
            personaje.Dispose();
            terreno.dispose();
            skyBox.dispose();
            foreach (Elemento elemento in elementos)
            {
                elemento.destruir();
            }
            informativo.dispose();
            estadoJuego.dispose();
            oveja.destruir();
            gallo.destruir();
            mochila.dispose();
            cajon.dispose();
            mochilaReglon1.dispose();
            cajonReglon1.dispose();
            alimentacion.dispose();
            salud.dispose();
            hidratacion.dispose();
            cansancio.dispose();
            mensajeObjetivo1.dispose();
            objetivosIcono.dispose();
            alimentacionIcono.dispose();
            saludIcono.dispose();
            hidratacionIcono.dispose();
            cansancioIcono.dispose();
            ayudaReglon1.dispose();
            ayuda.dispose();
            ayudaReglon2.dispose();
            dia.GetSol().Mesh.dispose();
            temperaturaDia.dispose();
            horaDia.dispose();
            temperaturaDiaIcono.dispose();
            horaDiaIcono.dispose();
            temperaturaPersonaje.dispose();
            temperaturaPersonajeIcono.dispose();
            estadoDiaSolIcono.dispose();
            estadoDiaLunaIcono.dispose();
            miniMapa.dispose();
            referenciaMiniMapa.dispose();
            linea.dispose();
            efectoLluvia.Dispose();
            screenQuadVB.Dispose();
            renderTarget2D.Dispose();
            estadoDiaLluviaIcono.dispose();
            sonidoGolpe.dispose();
            sonidoGolpePatada.dispose();
            sonidoLluvia.dispose();
            sonidoGrillos.dispose();
            sonidoGrillos2.dispose();
        }

        public void ActualizarPosicionSkyBox(Vector3 posicion)
        {
            foreach (TgcMesh faces in this.skyBox.Faces)
            {
                faces.Position += posicion;
            }
        }

        public void ActualizarPosicionSuelo(Vector3 posicion)
        {
            this.piso.Position += posicion;
        }

        private Point PosicionarReferencia(Vector3 posicion)
        {
            float porcentajeX;
            float porcentajeY;
            int coordenadaRelativaX;
            int coordenadaRelativaY;
            int factorCorreccionX = 0;
            int factorCorreccionY = -50;
            //Necesitamos calcular el porcentaje de x que esta recorrido sobre el total del mapa.
            porcentajeX = (esquina.X + posicion.X) / (2 * esquina.X);
            //Necesitamos calcular el porcentaje de Z que esta recorrido sobre el total del mapa.
            porcentajeY = (esquina.Z + posicion.Z) / (2 * esquina.Z);

            //En este caso Y seria Z
            coordenadaRelativaX = (int)(porcentajeX * (coordenadaSuperiorDerecha.X - coordenadaInferiorIzquierda.X));
            coordenadaRelativaY = (int)(porcentajeY * (coordenadaInferiorIzquierda.Y - coordenadaSuperiorDerecha.Y));

            return new Point(coordenadaInferiorIzquierda.X + coordenadaRelativaX + factorCorreccionX, coordenadaInferiorIzquierda.Y - coordenadaRelativaY + factorCorreccionY);
        }

        private void ReproducirMusica()
        {
            TgcMp3Player player = new TgcMp3Player();
            TgcMp3Player.States currentState = player.getStatus();
            if (currentState == TgcMp3Player.States.Open || currentState == TgcMp3Player.States.Stopped)
            {
                player.closeFile();
                player.FileName = this.musicas[FuncionesMatematicas.Instance.NumeroAleatorioIntEntre(0, this.musicas.Count - 1)];
                player.play(false);
            }
        }
    }
}