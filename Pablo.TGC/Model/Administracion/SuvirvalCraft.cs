using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.SceneLoader;
using TGC.Core.Sound;
using TGC.Core.Terrain;
using TGC.Core.Text;
using TGC.Group.Model.ElementosDia;
using TGC.Group.Model.ElementosJuego;
using TGC.Group.Model.Movimientos;
using TGC.Group.Model.Utiles;
using TGC.Group.Model.Utiles.Camaras;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.Administracion
{
    /// <summary>
    ///     Ejemplo del alumno
    /// </summary>
    public class SuvirvalCraft : TgcExample
    {
        public CustomSprite alimentacion;
        public CustomSprite alimentacionIcono;
        public CustomSprite ayuda;
        public TgcText2D ayudaReglon1;
        public TgcText2D ayudaReglon2;
        public CustomSprite cajon;
        public Elemento cajonOlla;
        public Elemento cajonReal;
        public TgcText2D cajonReglon1;

        public Camara camara;
        public CustomSprite cansancio;
        public CustomSprite cansancioIcono;

        public ControladorEntradas controladorEntradas;
        public Point coordenadaInferiorIzquierda;
        public Point coordenadaSuperiorDerecha;

        public Dia dia;
        public Efecto efectoAlgas;
        public Efecto efectoAlgas2;
        public Efecto efectoArbol;
        public Efecto efectoArbol2;
        public Efecto efectoBotes;
        public Efecto efectoFuego;
        public Efecto efectoFuego2;
        public Effect efectoLluvia;
        public Efecto efectoLuz;
        public Efecto efectoLuz2;
        public Efecto efectoTerreno;
        public List<Elemento> elementos;

        public Vector3 esquina; //Con un solo punto arma el cuadrado para definir los limites del mapa
        public CustomSprite estadoDiaLluviaIcono;
        public CustomSprite estadoDiaLunaIcono;
        public CustomSprite estadoDiaSolIcono;
        public TgcText2D estadoJuego;
        public Elemento fuenteAgua;
        public Animal gallo;
        public CustomSprite hidratacion;
        public CustomSprite hidratacionIcono;
        public TgcText2D horaDia;
        public CustomSprite horaDiaIcono;
        public TgcText2D informativo;
        public CustomSprite linea;
        public TgcText2D mensajeObjetivo1;
        public CustomSprite miniMapa;

        public CustomSprite mochila;
        public TgcText2D mochilaReglon1;
        public bool mostrarAyuda;
        public bool mostrarMenuCajon;
        public bool mostrarMenuMochila;
        public MovimientoParabolico movimiento;
        public MovimientoParabolico movimientoPersonaje;
        public List<string> musicas;
        public CustomSprite objetivosIcono;

        public Optimizador optimizador;
        public Animal oveja;
        public Personaje personaje;
        public TgcMesh piso;

        //TODO. Ver si no conviente tener un administrador de efectos
        public Efecto pisoEfecto;

        private Surface pOldRT;
        private Surface pSurf;

        public Elemento puebaFisica;
        public TgcText2D referenciaMiniMapa;
        public Texture renderTarget2D;

        public CustomSprite salud;
        public CustomSprite saludIcono;

        public VertexBuffer screenQuadVB;

        public TgcSkyBox skyBox;

        public Efecto skyboxEfecto;
        public TgcStaticSound sonidoGolpe;

        public TgcStaticSound sonidoGolpePatada;
        public Tgc3dSound sonidoGrillos;
        public Tgc3dSound sonidoGrillos2;
        public TgcStaticSound sonidoLluvia;
        public TgcText2D temperaturaDia;
        public CustomSprite temperaturaDiaIcono;
        public TgcText2D temperaturaPersonaje;
        public CustomSprite temperaturaPersonajeIcono;

        //TODO. Refactorizar esta lógica, hay demasiados atributos publicos en la apicacion principal.
        public Terreno terreno;

        public float tiempo;
        public float tiempoObjetivo;

        /// <summary>
        ///     Constructor del juego
        /// </summary>
        /// <param name="mediaDir">Ruta donde esta la carpeta con los assets</param>
        /// <param name="shadersDir">Ruta donde esta la carpeta con los shaders</param>
        public SuvirvalCraft(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = "AlumnoEjemplos";
            Name = "Pablo TGC - Suvirval Craft";
            Description = "Juego de supervivencia en donde un personaje debe sobrevivir durante un tiempo determinado."
                          + Environment.NewLine +
                          "Para ello debe hidratarse, y consumir alimentos. Puede eliminar animales y crear su propia hamburguesa, a ver si descubre cómo hacerlo."
                          + Environment.NewLine +
                          "Tambien puede comer frutas desde los árboles."
                          + Environment.NewLine +
                          "Tenga cuidado con la temperatura del dia y de su personaje, si la temperatura del personaje llega a 34° morirá."
                          + Environment.NewLine +
                          "Podría ser últil crear algún fuego para evitar esto."
                          + Environment.NewLine +
                          "Busque los cajones indicados en el mini mapa, encontrará algunas sorpresas."
                          + Environment.NewLine +
                          "Buena Suerte!!!!";
        }

        public Drawer2D Drawer2D { get; set; }

        /// <summary>
        ///     Método que se llama una sola vez,  al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, modifiers, uservars, etc.
        ///     Borrar todo lo que no haga falta
        /// </summary>
        public override void Init()
        {
            var con = new Configuracion(new ConfiguracionModel(this, D3DDevice.Instance.Device, MediaDir));
            con.ShowDialog();
            Drawer2D = new Drawer2D();
        }

        public override void Update()
        {
            PreUpdate();
            //TODO en la nueva estructura del TGC.Core se desacoplo el render del update logico
        }

        /// <summary>
        ///     Método que se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        ///     Borrar todo lo que no haga falta
        /// </summary>
        public override void Render()
        {
            ClearTextures();
            //Device de DirectX para renderizar
            var d3dDevice = D3DDevice.Instance.Device;

            //Reproduccion de sonidos
            ReproducirMusica();

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

            foreach (var comando in controladorEntradas.ProcesarEntradasTeclado())
            {
                comando.Ejecutar(this, ElapsedTime);
            }

            camara.Render(personaje, this);

            optimizador.Actualizar(personaje.mesh.Position, this);

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
            foreach (var faces in skyBox.Faces)
            {
                skyboxEfecto.Actualizar(this);
                faces.render();
            }

            //Actualiza los elementos
            var aux = new List<Elemento>();
            aux.AddRange(elementos);
            //TODO. Porque sino con la actualizacion borramos o agregamos elementos de la coleccion y se rompe todo
            foreach (var elemento in aux)
            {
                elemento.Actualizar(this, ElapsedTime);
            }

            foreach (var elem in optimizador.ElementosRenderizacion)
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
            else if (tiempo > tiempoObjetivo)
            {
                estadoJuego.Color = Color.Green;
                estadoJuego.Text = "Has Ganado";
                estadoJuego.render();
            }

            if (informativo.Text != "")
            {
                informativo.render();
            }

            Drawer2D.BeginDrawSprite();
            Drawer2D.DrawSprite(saludIcono);
            Drawer2D.DrawSprite(hidratacionIcono);
            Drawer2D.DrawSprite(alimentacionIcono);
            Drawer2D.DrawSprite(cansancioIcono);
            salud.Scaling = new Vector2(personaje.PorcentajeDeSalud() * 0.5f, 0.3f);
            Drawer2D.DrawSprite(salud);
            hidratacion.Scaling = new Vector2(personaje.PorcentajeDeHidratacion() * 0.5f, 0.3f);
            Drawer2D.DrawSprite(hidratacion);
            alimentacion.Scaling = new Vector2(personaje.PorcentajeDeAlimentacion() * 0.5f, 0.3f);
            Drawer2D.DrawSprite(alimentacion);
            cansancio.Scaling = new Vector2(personaje.PorcentajeDeCansancio() * 0.5f, 0.3f);
            Drawer2D.DrawSprite(cansancio);
            Drawer2D.DrawSprite(objetivosIcono);
            Drawer2D.DrawSprite(miniMapa);
            Drawer2D.EndDrawSprite();

            referenciaMiniMapa.Position = PosicionarReferencia(personaje.mesh.Position);
            referenciaMiniMapa.Color = Color.Orange;
            referenciaMiniMapa.render();
            referenciaMiniMapa.Position = PosicionarReferencia(fuenteAgua.posicion());
            referenciaMiniMapa.Color = Color.Blue;
            referenciaMiniMapa.render();
            referenciaMiniMapa.Position = PosicionarReferencia(cajonReal.posicion());
            referenciaMiniMapa.Color = Color.Brown;
            referenciaMiniMapa.render();
            referenciaMiniMapa.Position = PosicionarReferencia(cajonOlla.posicion());
            referenciaMiniMapa.Color = Color.Brown;
            referenciaMiniMapa.render();

            mensajeObjetivo1.Text = "Sobrevivir " + Environment.NewLine +
                                    TimeSpan.FromSeconds(tiempoObjetivo - tiempo).ToString(@"hh\:mm\:ss");
            mensajeObjetivo1.render();

            Drawer2D.BeginDrawSprite();
            linea.Rotation = personaje.mesh.Rotation.Y;
            Drawer2D.DrawSprite(linea);

            horaDia.Text = dia.HoraActualTexto();
            temperaturaDia.Text = dia.TemperaturaActualTexto();
            temperaturaPersonaje.Text = personaje.TemperaturaCorporalTexto();
            horaDia.render();
            temperaturaDia.render();
            Drawer2D.DrawSprite(temperaturaPersonajeIcono);
            temperaturaPersonaje.render();
            Drawer2D.DrawSprite(temperaturaDiaIcono);
            Drawer2D.DrawSprite(horaDiaIcono);
            if (dia.GetLluvia().EstaLloviendo())
            {
                Drawer2D.DrawSprite(estadoDiaLluviaIcono);
            }
            else
            {
                if (dia.EsDeDia())
                {
                    Drawer2D.DrawSprite(estadoDiaSolIcono);
                }
                else
                {
                    Drawer2D.DrawSprite(estadoDiaLunaIcono);
                }
            }
            Drawer2D.EndDrawSprite();

            if (mostrarMenuMochila)
            {
                //Iniciar dibujado de todos los Sprites de la escena (en este caso es solo uno)
                Drawer2D.BeginDrawSprite();
                //Dibujar sprite (si hubiese mas, deberian ir todos aquí)
                Drawer2D.DrawSprite(mochila);
                if (mostrarMenuCajon)
                {
                    //El cajon se muestra siempre junto con la mochila
                    Drawer2D.DrawSprite(cajon);
                }
                //Finalizar el dibujado de Sprites
                Drawer2D.EndDrawSprite();
                //TODO. Por el momento podemos mantener todo en un renglon ya que no imprimimos ninguna imagen de los elementos en cuestion
                mochilaReglon1.Text = "";
                for (var i = 0; i < 9; i++)
                {
                    if (personaje.ContieneElementoEnPosicionDeMochila(i))
                    {
                        mochilaReglon1.Text = mochilaReglon1.Text + (i + 1) + "    " +
                                              personaje.DarElementoEnPosicionDeMochila(i).GetDescripcion() +
                                              Environment.NewLine;
                    }
                    else
                    {
                        mochilaReglon1.Text = mochilaReglon1.Text + (i + 1) + "    " + "Disponible" +
                                              Environment.NewLine;
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
                Drawer2D.BeginDrawSprite();
                Drawer2D.DrawSprite(ayuda);
                Drawer2D.EndDrawSprite();
                ayudaReglon1.Position = new Point((int)ayuda.Position.X + 40, (int)ayuda.Position.Y + 130);
                ayudaReglon1.render();
                ayudaReglon2.render();
            }

            RenderFPS();
            D3DDevice.Instance.Device.Present();
        }

        /// <summary>
        ///     Método que se llama cuando termina la ejecución del ejemplo.
        ///     Hacer dispose() de todos los objetos creados.
        /// </summary>
        public override void Dispose()
        {
            piso.dispose();
            personaje.Dispose();
            terreno.dispose();
            skyBox.dispose();
            foreach (var elemento in elementos)
            {
                elemento.destruir();
            }
            informativo.Dispose();
            estadoJuego.Dispose();
            oveja.destruir();
            gallo.destruir();
            mochila.Dispose();
            cajon.Dispose();
            mochilaReglon1.Dispose();
            cajonReglon1.Dispose();
            alimentacion.Dispose();
            salud.Dispose();
            hidratacion.Dispose();
            cansancio.Dispose();
            mensajeObjetivo1.Dispose();
            objetivosIcono.Dispose();
            alimentacionIcono.Dispose();
            saludIcono.Dispose();
            hidratacionIcono.Dispose();
            cansancioIcono.Dispose();
            ayudaReglon1.Dispose();
            ayuda.Dispose();
            ayudaReglon2.Dispose();
            dia.GetSol().Mesh.dispose();
            temperaturaDia.Dispose();
            horaDia.Dispose();
            temperaturaDiaIcono.Dispose();
            horaDiaIcono.Dispose();
            temperaturaPersonaje.Dispose();
            temperaturaPersonajeIcono.Dispose();
            estadoDiaSolIcono.Dispose();
            estadoDiaLunaIcono.Dispose();
            miniMapa.Dispose();
            referenciaMiniMapa.Dispose();
            linea.Dispose();
            efectoLluvia.Dispose();
            screenQuadVB.Dispose();
            renderTarget2D.Dispose();
            estadoDiaLluviaIcono.Dispose();
            sonidoGolpe.dispose();
            sonidoGolpePatada.dispose();
            sonidoLluvia.dispose();
            sonidoGrillos.dispose();
            sonidoGrillos2.dispose();
        }

        public void ActualizarPosicionSkyBox(Vector3 posicion)
        {
            foreach (var faces in skyBox.Faces)
            {
                faces.Position += posicion;
            }
        }

        public void ActualizarPosicionSuelo(Vector3 posicion)
        {
            piso.Position += posicion;
        }

        private Point PosicionarReferencia(Vector3 posicion)
        {
            float porcentajeX;
            float porcentajeY;
            int coordenadaRelativaX;
            int coordenadaRelativaY;
            var factorCorreccionX = 0;
            var factorCorreccionY = -50;
            //Necesitamos calcular el porcentaje de x que esta recorrido sobre el total del mapa.
            porcentajeX = (esquina.X + posicion.X) / (2 * esquina.X);
            //Necesitamos calcular el porcentaje de Z que esta recorrido sobre el total del mapa.
            porcentajeY = (esquina.Z + posicion.Z) / (2 * esquina.Z);

            //En este caso Y seria Z
            coordenadaRelativaX = (int)(porcentajeX * (coordenadaSuperiorDerecha.X - coordenadaInferiorIzquierda.X));
            coordenadaRelativaY = (int)(porcentajeY * (coordenadaInferiorIzquierda.Y - coordenadaSuperiorDerecha.Y));

            return new Point(coordenadaInferiorIzquierda.X + coordenadaRelativaX + factorCorreccionX,
                coordenadaInferiorIzquierda.Y - coordenadaRelativaY + factorCorreccionY);
        }

        private void ReproducirMusica()
        {
            var player = new TgcMp3Player();
            var currentState = player.getStatus();
            if (currentState == TgcMp3Player.States.Open || currentState == TgcMp3Player.States.Stopped)
            {
                player.closeFile();
                player.FileName = musicas[FuncionesMatematicas.Instance.NumeroAleatorioIntEntre(0, musicas.Count - 1)];
                player.play(false);
            }
        }
    }
}