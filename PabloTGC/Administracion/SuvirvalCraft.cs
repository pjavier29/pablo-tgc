using System;
using System.Collections.Generic;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils._2D;
using AlumnoEjemplos.PabloTGC.Utiles;
using AlumnoEjemplos.PabloTGC.Comandos;
using AlumnoEjemplos.PabloTGC.Utiles.Camaras;
using TgcViewer.Utils.Shaders;
using AlumnoEjemplos.PabloTGC.Utiles.Efectos;
using AlumnoEjemplos.PabloTGC.ElementosDia;
using TgcViewer.Utils;
using TgcViewer.Utils.Sound;

namespace AlumnoEjemplos.PabloTGC.Administracion
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
        public Efecto efectoAlgas;
        public Efecto efectoAlgas2;
        public Efecto efectoBotes;
        public Efecto efectoFuego;
        public Efecto efectoFuego2;
        public Efecto efectoArbol;
        public Efecto efectoArbol2;

        public VertexBuffer screenQuadVB;
        public Texture renderTarget2D;
        Surface pOldRT;
        Surface pSurf;
        public Microsoft.DirectX.Direct3D.Effect efectoLluvia;


        public TgcStaticSound sonidoGolpePatada;
        public TgcStaticSound sonidoGolpe;
        public TgcStaticSound sonidoLluvia;
        public List<String> musicas;
        public Tgc3dSound sonidoGrillos;
        public Tgc3dSound sonidoGrillos2;

        /// <summary>
        /// Categoría a la que pertenece el ejemplo.
        /// Influye en donde se va a haber en el árbol de la derecha de la pantalla.
        /// </summary>
        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        /// <summary>
        /// Completar nombre del grupo en formato Grupo NN
        /// </summary>
        public override string getName()
        {
            return "Pablo TGC - Suvirval Craft";
        }

        /// <summary>
        /// Completar con la descripción del TP
        /// </summary>
        public override string getDescription()
        {
            return "Juego de supervivencia en donde un personaje debe sobrevivir durante un tiempo determinado."
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
        public override void init()
        {
            Configuracion con = new Configuracion(new ConfiguracionModel(this));
            con.ShowDialog();
        }


        /// <summary>
        /// Método que se llama cada vez que hay que refrescar la pantalla.
        /// Escribir aquí todo el código referido al renderizado.
        /// Borrar todo lo que no haga falta
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el último frame</param>
        public override void render(float elapsedTime)
        {
            //Device de DirectX para renderizar
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            //Reproduccion de sonidos
            this.ReproducirMusica();

            //Actualizamos el dia
            dia.Actualizar(this, elapsedTime);

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
            tiempo += elapsedTime;

            informativo.Text = "Obtener Ayuda (F1)";
            mostrarMenuMochila = false;
            mostrarMenuCajon = false;
            mostrarAyuda = false;

            foreach (Comando comando in controladorEntradas.ProcesarEntradasTeclado())
            {
                comando.Ejecutar(this, elapsedTime);
            }

            camara.Render(personaje, this);

            optimizador.Actualizar(personaje.mesh.Position);

            //Afectamos salud por paso de tiempo y por la temperatura
            personaje.AfectarSaludPorTiempo(elapsedTime);
            personaje.AfectarSaludPorTemperatura(dia.TemperaturaActual(), elapsedTime);

            //Actualizamos los objetos Fisicos y los renderizamos
            if (movimiento != null)
            {
                movimiento.update(elapsedTime, terreno);
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
                movimientoPersonaje.update(elapsedTime, terreno);
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
                elemento.Actualizar(this, elapsedTime);
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

            GuiController.Instance.Drawer2D.beginDrawSprite();
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
            GuiController.Instance.Drawer2D.endDrawSprite();

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

            GuiController.Instance.Drawer2D.beginDrawSprite();

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

            GuiController.Instance.Drawer2D.endDrawSprite();

            if (mostrarMenuMochila)
            {
                //Iniciar dibujado de todos los Sprites de la escena (en este caso es solo uno)
                GuiController.Instance.Drawer2D.beginDrawSprite();
                //Dibujar sprite (si hubiese mas, deberian ir todos aquí)
                mochila.render();
                if (mostrarMenuCajon)
                {
                    //El cajon se muestra siempre junto con la mochila
                    cajon.render();
                }
                //Finalizar el dibujado de Sprites
                GuiController.Instance.Drawer2D.endDrawSprite();
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
                GuiController.Instance.Drawer2D.beginDrawSprite();
                ayuda.render();
                GuiController.Instance.Drawer2D.endDrawSprite();
                ayudaReglon1.Position = new Point(((int)(ayuda.Position.X)) + 40, ((int)(ayuda.Position.Y)) + 130);
                ayudaReglon1.render();
                ayudaReglon2.render();
            }

            //Como estamos en modo CustomRenderEnabled, tenemos que dibujar todo nosotros, incluso el contador de FPS
            GuiController.Instance.Text3d.drawText("FPS: " + HighResolutionTimer.Instance.FramesPerSecond, 0, 0, Color.Yellow);

        }

        /// <summary>
        /// Método que se llama cuando termina la ejecución del ejemplo.
        /// Hacer dispose() de todos los objetos creados.
        /// </summary>
        public override void close()
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
            TgcMp3Player player = GuiController.Instance.Mp3Player;
            TgcMp3Player.States currentState = player.getStatus();
            if (currentState == TgcMp3Player.States.Open || currentState == TgcMp3Player.States.Stopped)
            {
                player.closeFile();
                player.FileName = this.musicas[FuncionesMatematicas.Instance.NumeroAleatorioIntEntre(0,this.musicas.Count - 1)];
                player.play(false);
            }
        }
    }
}
