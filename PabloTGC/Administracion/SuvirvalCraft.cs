using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using AlumnoEjemplos.PabloTGC;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSkeletalAnimation;
using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcKeyFrameLoader;
using AlumnoEjemplos.PabloTGC.ElementosJuego;
using AlumnoEjemplos.PabloTGC.Instrumentos;
using AlumnoEjemplos.PabloTGC.Utiles;
using AlumnoEjemplos.PabloTGC.Comandos;
using AlumnoEjemplos.PabloTGC.Administracion;

namespace AlumnoEjemplos.MiGrupo
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class SuvirvalCraft : TgcExample
    {
        public Terreno terreno;
        public TgcSkyBox skyBox;
        TgcBox piso;
        public List<Elemento> elementos;
        public Personaje personaje;
        //bool jumping;
        //bool jumpingAdelante;
        TgcArrow directionArrow;
        float tiempo;
        //bool puedeGolpear;
        //bool puedeIncrementarSaludConFuego = false;
        TgcScene scene, scene2, scene3, scene4, scene5, scene6, scene7, scene8, scene9, scene10;
        TgcMesh palmera, pino, arbol, banana, fuego, lenia, carneCruda, cajon, manzanaVerde, manzanaRoja;
        TgcText2d textGameOver;
        public TgcText2d informativo;
        TgcText2d elementosEnMochila;
        Animal oveja;
        Animal gallo;
        TgcMesh hachaMesh;
        TgcMesh palo;

        public Elemento puebaFisica;
        public MovimientoParabolico movimiento;
        public MovimientoParabolico movimientoPersonaje;
        List<MovimientoParabolico> animacones = new List<MovimientoParabolico>();

        //public Lanzar lanzar;
        //public Saltar saltar;
        //public Golpear golpear;

        public ControladorEntradas controladorEntradas;

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
            return "MiIdea - Descripcion de la idea";
        }

        /// <summary>
        /// Método que se llama una sola vez,  al principio cuando se ejecuta el ejemplo.
        /// Escribir aquí todo el código de inicialización: cargar modelos, texturas, modifiers, uservars, etc.
        /// Borrar todo lo que no haga falta
        /// </summary>
        public override void init()
        {
            //GuiController.Instance: acceso principal a todas las herramientas del Framework

            //Device de DirectX para crear primitivas
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            //Carpeta de acceso a los recursos
            string recursos = GuiController.Instance.AlumnoEjemplosDir + "PabloTGC\\Recursos\\";

            //Crear loader
            TgcSceneLoader loader = new TgcSceneLoader();

            ///////////////USER VARS//////////////////

            //Crear una UserVar
            GuiController.Instance.UserVars.addVar("salud");
            GuiController.Instance.UserVars.addVar("fuerza");
            GuiController.Instance.UserVars.addVar("velocidad");
            GuiController.Instance.UserVars.addVar("tiempocorriendo");
            GuiController.Instance.UserVars.addVar("x");
            GuiController.Instance.UserVars.addVar("y");
            GuiController.Instance.UserVars.addVar("z");

            GuiController.Instance.UserVars.addVar("fuerzaGolpe");


            // ------------------------------------------------------------
            // Creo el Heightmap para el terreno:
            terreno = new Terreno();
            terreno.loadHeightmap(recursos
                    + "Shaders\\WorkshopShaders\\Heighmaps\\" + "HeightmapHawaii.jpg", 100f, 1f, new Vector3(0, 0, 0));
            terreno.loadTexture(recursos
                    + "Shaders\\WorkshopShaders\\Heighmaps\\" + "TerrainTextureHawaii.jpg");

            // ------------------------------------------------------------

            // Crear SkyBox:
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 0, 0);
            skyBox.Size = new Vector3(10000, 10000, 10000);
            string texturesPath = recursos + "Texturas\\Quake\\SkyBox1\\";
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "phobos_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "phobos_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "phobos_lf.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "phobos_rt.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "phobos_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "phobos_ft.jpg");
            skyBox.SkyEpsilon = 10f;
            skyBox.updateValues();

            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
            palmera = scene.Meshes[0];

            scene2 = loader.loadSceneFromFile(recursos
                 + "MeshCreator\\Meshes\\Vegetacion\\Pino\\Pino-TgcScene.xml");
            pino = scene2.Meshes[0];

            scene3 = loader.loadSceneFromFile(recursos
                 + "MeshCreator\\Meshes\\Vegetacion\\ArbolBananas\\ArbolBananas-TgcScene.xml");
            arbol = scene3.Meshes[0];

            scene4 = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Alimentos\\Frutas\\Bananas\\Bananas-TgcScene.xml");
            banana = scene4.Meshes[0];

            scene5 = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Fuego\\fuego-TgcScene.xml");
            fuego = scene5.Meshes[0];

            scene6 = loader.loadSceneFromFile(recursos
                 + "MeshCreator\\Meshes\\Leña\\lenia-TgcScene.xml");
            lenia = scene6.Meshes[0];

            scene7 = loader.loadSceneFromFile(recursos
                 + "MeshCreator\\Meshes\\Alimentos\\CarneCruda\\carnecruda-TgcScene.xml");
            carneCruda = scene7.Meshes[0];

            scene8 = loader.loadSceneFromFile(recursos
                 + "MeshCreator\\Meshes\\Cajon\\cajon-TgcScene.xml");
            cajon = scene8.Meshes[0];

            scene9 = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Alimentos\\Frutas\\ManzanaVerde\\manzanaverde-TgcScene.xml");
            manzanaVerde = scene9.Meshes[0];

            scene10 = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Alimentos\\Frutas\\ManzanaRoja\\manzanaroja-TgcScene.xml");
            manzanaRoja = scene10.Meshes[0];

            elementos = new List<Elemento>();

            float[] r = { 1850f, 2100f, 2300f, 1800f };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    TgcMesh palmeraNueva = palmera.createMeshInstance(palmera.Name + i);
                    palmeraNueva.Scale = new Vector3(0.5f, 1.5f, 0.5f);
                    float x = r[i] * (float)Math.Cos(Geometry.DegreeToRadian(100 + 10.0f * j));
                    float z = r[i] * (float)Math.Sin(Geometry.DegreeToRadian(100 + 10.0f * j));
                    palmeraNueva.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                    elementos.Add(new Elemento(1000, 1400, palmeraNueva));
                }
            }

            TgcMesh bananaMesh;
            float[] ar = { -850f, -600f, -400f, -200f };
            for (int i = 0; i < 4; i++)
            {
                TgcMesh arbolBanana = arbol.createMeshInstance(arbol.Name + i);
                arbolBanana.Scale = new Vector3(1.5f, 3.5f, 1.5f);
                float x = r[i] - 100;
                float z = r[i] - 123;
                arbolBanana.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                bananaMesh = banana.createMeshInstance(banana.Name);
                bananaMesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                elementos.Add(new Elemento(1000, 1300, arbolBanana, new Alimento(1000, 1000, bananaMesh)));
            }

            TgcMesh fuegoMesh;
            TgcMesh leniaMesh;
            for (int i = 0; i < 4; i++)
            {
                TgcMesh pinoNuevo = pino.createMeshInstance(pino.Name + i);
                pinoNuevo.Scale = new Vector3(0.5f, 1.5f, 0.5f);
                float x = r[i] - 100;
                float z = -(r[i] - 123);
                pinoNuevo.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                fuegoMesh = fuego.createMeshInstance(/*fuego.Name*/"fuego");
                leniaMesh = lenia.createMeshInstance(/*lenia.Name*/"lenia");
                fuegoMesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                leniaMesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                elementos.Add(new Elemento(1000, 2330, pinoNuevo,
                                (new Madera(1000, 233, leniaMesh,
                                    new Fuego(1000, 233, fuegoMesh)))));
            }

            //Cargar obstaculos y posicionarlos. Los obstáculos se crean con TgcBox en lugar de cargar un modelo.
            TgcBox caja;
            //Caja para tirar
            caja = TgcBox.fromSize(
                new Vector3(0, 0, 0),
                new Vector3(20, 20, 20),
                TgcTexture.createTexture(d3dDevice, recursos + "Texturas\\baldosaFacultad.jpg"));
            puebaFisica = new Elemento(100, 300, caja);
            //movimiento = new MovimientoParabolico(caja.Position, new Vector3(0, 5, 0), 50, new MallaEnvoltura(puebaFisica.Mesh));

            //Creamos los animales
            //Creamos la oveja
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Oveja\\Ovelha-TgcScene.xml");
            TgcMesh ovejaMesh = scene.Meshes[0];
            oveja = new Animal(5000, 20, ovejaMesh.createMeshInstance("Oveja"));
            oveja.agregarElemento(new Alimento(1000, 1000, carneCruda.createMeshInstance("CarneCruda")));
            ovejaMesh.Position = new Vector3(200, terreno.CalcularAltura(200, 200), 200);
            elementos.Add(oveja);

            //Creamos el gallo
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Gallo\\Gallo-TgcScene.xml");
            TgcMesh galloMesh = scene.Meshes[0].createMeshInstance("Gallo");
            gallo = new Animal(5000, 20, galloMesh);
            gallo.agregarElemento(new Alimento(1000, 1000, carneCruda.createMeshInstance("CarneCruda")));
            galloMesh.Position = new Vector3(0, terreno.CalcularAltura(0, 0), 0);
            elementos.Add(gallo);

            //Creamos el cajon con sus elementos
            TgcMesh cajonRealMesh = cajon.createMeshInstance("Cajon_1");
            Elemento cajonReal = new Cajon(5000, 1000, cajonRealMesh);
            cajonRealMesh.Position = new Vector3(233, terreno.CalcularAltura(233, 233), 233);
            cajonReal.agregarElemento(new Alimento(1000,1000, manzanaVerde.createMeshInstance("Manzana Verde")));
            cajonReal.agregarElemento(new Alimento(1000, 1000, manzanaRoja.createMeshInstance("Manzana Roja")));
            elementos.Add(cajonReal);

            //Crear piso
            TgcTexture pisoTexture = TgcTexture.createTexture(d3dDevice, recursos + "Texturas\\Agua.jpg");
            piso = TgcBox.fromExtremes(new Vector3(-5000, -50, -5000), new Vector3(10000, 10, 10000), pisoTexture);


            //Creamos el personaje
            //Cargar personaje con animaciones
            personaje = new Personaje();
            personaje.velocidadCaminar = 150f;
            personaje.velocidadRotacion = 50f;
            personaje.fuerza = 1f;
            personaje.salud = 100f;
            personaje.resistenciaFisica = 30f;

            //Creamos las animaciones del mesh del personaje******
            //Paths para archivo XML de la malla
            string pathMesh = recursos + "SkeletalAnimations\\Robot\\Robot-TgcSkeletalMesh.xml";

            //Path para carpeta de texturas de la malla
            string mediaPath = recursos + "SkeletalAnimations\\Robot\\";

            //Lista de animaciones disponibles
            string[] animationList = new string[]{
                "Parado",
                "Caminando",
                "Correr",
                "PasoDerecho",
                "PasoIzquierdo",
                "Empujar",
                "Patear",
                "Pegar",
                "Arrojar",
            };

            //Crear rutas con cada animacion
            string[] animationsPath = new string[animationList.Length];
            for (int i = 0; i < animationList.Length; i++)
            {
                animationsPath[i] = mediaPath + animationList[i] + "-TgcSkeletalAnim.xml";
            }

            //Cargar mesh y animaciones
            TgcSkeletalLoader skeletalLoader = new TgcSkeletalLoader();
            personaje.mesh = skeletalLoader.loadMeshAndAnimationsFromFile(pathMesh, mediaPath, animationsPath);
            //Le cambiamos la textura para diferenciarlo un poco
            personaje.mesh.changeDiffuseMaps(new TgcTexture[] { TgcTexture.createTexture(d3dDevice, recursos + "SkeletalAnimations\\Robot\\Textures\\" + "uvwGreen.jpg") });
            //****************************************************

            //Agregamos el arma al personaje
            //Hacha
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Armas\\Hacha\\Hacha-TgcScene.xml");
            hachaMesh = scene.Meshes[0];
            //Palo
            palo = TgcBox.fromSize(new Vector3(3, 60, 3), Color.Green).toMesh("Palo");

            //Carmamos las armas
            personaje.agregarInstrumento(new Arma(100, 50, Matrix.Translation(10, 40, 0), palo));
            personaje.agregarInstrumento(new Arma(1000, 75, Matrix.Translation(10, 20, -20), hachaMesh));

            TgcSkeletalBoneAttach attachment = new TgcSkeletalBoneAttach();
            attachment.Bone = personaje.mesh.getBoneByName("Bip01 R Hand");
            personaje.mesh.Attachments.Add(attachment);

            //por defecto inicia con el hacha
            personaje.seleccionarInstrumentoManoDerecha(1);

            //Configurar animacion inicial
            personaje.mesh.playAnimation("Parado", true);
            //Escalarlo porque es muy grande
            personaje.mesh.Position = new Vector3(0, terreno.CalcularAltura(0, 0), 0);
            //Rotarlo 180° porque esta mirando para el otro lado
            personaje.mesh.rotateY(Geometry.DegreeToRadian(180f));

            //Una vez configurado el mesh del personaje iniciamos su bounding esfera y su esfera de alcance de interacción con los elementos
            personaje.IniciarBoundingEsfera();
            personaje.IniciarAlcanceInteraccionEsfera();

            //Crear linea para mostrar la direccion del movimiento del personaje
            directionArrow = new TgcArrow();
            directionArrow.BodyColor = Color.Red;
            directionArrow.HeadColor = Color.Green;
            directionArrow.Thickness = 1;
            directionArrow.HeadSize = new Vector2(10, 20);

            //Configurar camara en Tercer Persona
            GuiController.Instance.ThirdPersonCamera.Enable = true;
            GuiController.Instance.ThirdPersonCamera.setCamera(personaje.mesh.Position, 200, -300);

            //Texto de Game Over e informativo
            textGameOver = new TgcText2d();
            textGameOver.Text = "GAME OVER";
            textGameOver.Color = Color.Red;
            textGameOver.Align = TgcText2d.TextAlign.LEFT;
            textGameOver.Position = new Point(300, 100);
            textGameOver.Size = new Size(300, 100);
            textGameOver.changeFont(new System.Drawing.Font("TimesNewRoman", 40, FontStyle.Bold | FontStyle.Italic));

            informativo = new TgcText2d();
            informativo.Color = Color.DarkGreen;
            informativo.Align = TgcText2d.TextAlign.LEFT;
            informativo.Position = new Point(400, 200);
            informativo.Size = new Size(500, 500);
            informativo.changeFont(new System.Drawing.Font("TimesNewRoman", 40, FontStyle.Bold | FontStyle.Italic));

            elementosEnMochila = new TgcText2d();
            elementosEnMochila.Color = Color.Blue;
            elementosEnMochila.Text = "Mochila: ";
            elementosEnMochila.Align = TgcText2d.TextAlign.LEFT;
            elementosEnMochila.Position = new Point(10, 20);
            elementosEnMochila.Size = new Size(500, 500);
            elementosEnMochila.changeFont(new System.Drawing.Font("TimesNewRoman", 20, FontStyle.Bold | FontStyle.Italic));

            ///////////////MODIFIERS//////////////////

            //Crear un modifier para un valor FLOAT
            GuiController.Instance.Modifiers.addFloat("valorFloat", -50f, 200f, 0f);

            //Crear un modifier para un ComboBox con opciones
            string[] opciones = new string[]{"opcion1", "opcion2", "opcion3"};
            GuiController.Instance.Modifiers.addInterval("valorIntervalo", opciones, 0);

            //Crear un modifier para modificar un vértice
            GuiController.Instance.Modifiers.addVertex3f("valorVertice", new Vector3(-100, -100, -100), new Vector3(50, 50, 50), new Vector3(0, 0, 0));

            tiempo = 0;
            //puedeGolpear = true;

            controladorEntradas = new ControladorEntradas();
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

            //Hacer que la camara siga al personaje en su nueva posicion. Sumamos 100 en el posición de Y porque queremos que la cámara este un poco más alta.
            GuiController.Instance.ThirdPersonCamera.Target = personaje.mesh.Position + new Vector3(0,100,0);

            //Calcular proxima posicion de personaje segun Input
            //float moveForward = 0f;
            //float rotate = 0;
            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;
            //TODO. Mejorar esta logica de representación de estados
            //bool moving = false;
            //bool rotating = false;
            //bool patear = false;
            //bool pegar = false;
            //bool tirar = false;
            //bool lanzar = false;
            //bool correr = false;
            //bool juntar = false;
            //bool encender = false;
            //bool consumir = false;
            //bool abrir = false;
           // bool juntarTodo = false;
            //bool dejarElemento = false;
            //jumping = false;
            //jumpingAdelante = false;

            //bool hayInteraccion = true;
            //Comando accion = null;
            ///Comando rotar = null;
           // Comando tirar = null;
            //Comando interactuar = null;
           // bool saltoAdelante = false;

            //Saltar adelante
       //     if (/*d3dInput.keyDown(Key.Space) && d3dInput.keyDown(Key.W)*/d3dInput.keyDown(Key.E))
        //    {
                //jumpingAdelante = true;
        //        hayInteraccion = false;
        //        saltoAdelante = true;
        //        if (saltar == null)
         //       {
       //             saltar = new Saltar(Saltar.Adelante);
       //             saltar.Ejecutar(this, elapsedTime);
        //        }
        //        if ((saltar.Movimiento.Finalizo))
       //         {
      //              saltar.Ejecutar(this, elapsedTime);
      //          }
      //      }

            //Movimiento para adelante
            //if (d3dInput.keyDown(Key.W) || d3dInput.keyDown(Key.Up))
           // {
                /*dirC = -1f;
                moveForward = dirC * personaje.velocidadCaminar;
                moving = true;*/
             //   if(!saltoAdelante)
          //      { 
              //      accion = new Mover(-1f);
             //       accion.Ejecutar(this, elapsedTime);
            //        hayInteraccion = false;
          //      }
         //   }

       //     //Movimiento para Atras
         //   if (d3dInput.keyDown(Key.S) || d3dInput.keyDown(Key.Down))
         //   {
         //       /*moveForward = personaje.velocidadCaminar;
         //       moving = true;*/
         //       accion = new Mover(1f);
         //       accion.Ejecutar(this, elapsedTime);
         //       hayInteraccion = false;
         //   }

            //Rotar Derecha
      //      if (d3dInput.keyDown(Key.Right) || d3dInput.keyDown(Key.D))
      //      {
                /*rotate = personaje.velocidadRotacion;
                rotating = true;*/
     //           rotar = new Girar(1f);
     //           rotar.Ejecutar(this, elapsedTime);
    //            hayInteraccion = false;
   //         }

            //Rotar Izquierda
     //       if (d3dInput.keyDown(Key.Left) || d3dInput.keyDown(Key.A))
     //       {
                /*dirR = -1f;
                rotate = dirR * personaje.velocidadRotacion;
                rotating = true;*/
     //           rotar = new Girar(-1f);
    //            rotar.Ejecutar(this, elapsedTime);
     //           hayInteraccion = false;
     //       }

            //Si preciono para caminar más rápido
    //        if (d3dInput.keyDown(Key.RightShift) || d3dInput.keyDown(Key.LeftShift))
    //        {
                //moveForward = dirC * personaje.correr(elapsedTime);
                //rotate = dirR * personaje.rotarRapido();
                //correr = true;
              /*  if (accion != null)
                {
                    Mover mover = (Mover)accion;
                    mover.MovimientoRapido = true;
                    mover.Ejecutar(this, elapsedTime);
                }*/
    //            if (rotar != null)
    //            {
    //                Girar girar = (Girar)rotar;
    //                girar.MovimientoRapido = true;
    //                girar.Ejecutar(this, elapsedTime);
    //            }
    //            hayInteraccion = false;
    //        }

        /*    //Pegar una piña
            if (d3dInput.keyDown(Key.RightControl))
            {
                //pegar = true;
                if (golpear == null)
                {
                    golpear = new Golpear(Golpear.Pegar);
                }
                else
                {
                    golpear.GolpeActual = Golpear.Pegar;
                }
                golpear.Ejecutar(this, elapsedTime);
                hayInteraccion = false;
            }

            //Pegar una patada
            if (d3dInput.keyDown(Key.LeftControl))
            {
                //patear = true;
                if (golpear == null)
                {
                    golpear = new Golpear(Golpear.Patear);
                }
                else
                {
                    golpear.GolpeActual = Golpear.Patear;
                }
                golpear.Ejecutar(this, elapsedTime);
                hayInteraccion = false;
            }*/

      /*      //Tirar un elemento
            if (d3dInput.keyDown(Key.T))
            {
                // tirar = true;
                tirar = new Tirar();
                tirar.Ejecutar(this, elapsedTime);
                hayInteraccion = false;
            }*/

            //Lanza un elemento con fuerza
      //      if (d3dInput.keyDown(Key.C))
     //       {
                //lanzar = true;
      //          if (!(lanzar != null && !(lanzar.Movimiento.Finalizo)))
      //          {
      //              lanzar = new Lanzar(puebaFisica);
           //         lanzar.Ejecutar(this, elapsedTime);
      //          }
                //hayInteraccion = false;
     //       }

            //Saltar
      //      if (d3dInput.keyDown(Key.Space))
       //     {
                //jumping = true;
       //         hayInteraccion = false;
      //          if (saltar == null)
      //          {
       //             saltar = new Saltar(Saltar.EnLugar);
       //             saltar.Ejecutar(this, elapsedTime);
       //         }
       //         if ((saltar.Movimiento.Finalizo))
       //         {
        //            saltar.Ejecutar(this, elapsedTime);
       //         }
         //   }

         /*   //Seleccion de Arma palo
            if (d3dInput.keyDown(Key.D1))
            {
                personaje.seleccionarInstrumentoManoDerecha(0);
            }

            //Seleccion de Arma Hacha
            if (d3dInput.keyDown(Key.D2))
            {
                personaje.seleccionarInstrumentoManoDerecha(1);
            }

            //Seleccion Juntar
            if (d3dInput.keyDown(Key.R))
            {
                //juntar = true;
                if (interactuar == null)
                {
                    interactuar = new Interactuar(Interactuar.Juntar);
                }
            }

            //Seleccion Encender
            if (d3dInput.keyDown(Key.E))
            {
                //encender = true;
                if (interactuar == null)
                {
                    interactuar = new Interactuar(Interactuar.Encender);
                }
            }

            //Seleccion Consumir
            if (d3dInput.keyDown(Key.U))
            {
                //consumir = true;
                if (interactuar == null)
                {
                    interactuar = new Interactuar(Interactuar.Consumir);
                }
            }

            //Abrir
            if (d3dInput.keyDown(Key.B))
            {
                //abrir = true;
                if (interactuar == null)
                {
                    interactuar = new Interactuar(Interactuar.Abrir);
                }
            }

            //Juntar todo
            if (d3dInput.keyDown(Key.J))
            {
                //juntarTodo = true;
                if (interactuar == null)
                {
                    interactuar = new Interactuar(Interactuar.JuntarTodo);
                }
            }

            //Dejar Elemento
            if (d3dInput.keyDown(Key.H))
            {
                //dejarElemento = true;
                if (interactuar == null)
                {
                    interactuar = new Interactuar(Interactuar.DejarElemento);
                }
            }*/

            informativo.Text = "";
            /*if (interactuar == null)
            {
                interactuar = new Interactuar(Interactuar.Parado);
            }*/

            //if (hayInteraccion)
            //{
             //   interactuar.Ejecutar(this, elapsedTime);
            //}

            foreach (Comando comando in controladorEntradas.ProcesarEntradasTeclado())
            {
                comando.Ejecutar(this, elapsedTime);
            }

            //Si hubo rotacion
            /*if (rotating)
            {
                //Rotar personaje y la camara, hay que multiplicarlo por el tiempo transcurrido para no atarse a la velocidad el hardware
                float rotAngle = Geometry.DegreeToRadian(rotate * elapsedTime);
                personaje.mesh.rotateY(rotAngle);
                GuiController.Instance.ThirdPersonCamera.rotateY(rotAngle);
            }*/

            //Si hubo desplazamiento
            //if (moving)
            // {
            //puedeIncrementarSaludConFuego = false;

            //Aplicar movimiento hacia adelante o atras segun la orientacion actual del Mesh
            //   Vector3 lastPos = personaje.mesh.Position;

            //Aplicamos el movimiento
            //TODO Ver si es correcta la forma que aplico para representar que se esta a la altura del terreno.
            //         float xm = FastMath.Sin(personaje.mesh.Rotation.Y) * moveForward;
            //         float zm = FastMath.Cos(personaje.mesh.Rotation.Y) * moveForward;
            //        Vector3 movementVector = new Vector3(xm, 0, zm);
            //        personaje.mesh.move(movementVector * elapsedTime);
            //        personaje.mesh.Position = new Vector3(personaje.mesh.Position.X, terreno.CalcularAltura(personaje.mesh.Position.X, personaje.mesh.Position.Z), personaje.mesh.Position.Z);

            //         personaje.ActualizarBoundingEsfera();

            //Detectar colisiones
            //           bool collide = false;
            //         foreach (Elemento obstaculo in obstaculos)
            //         {
            /*TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(personaje.mesh.BoundingBox, obstaculo.BoundingBox());
            if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)*/
            //             if(this.EsferaColisionaCuadrado(personaje.GetBoundingEsfera(), obstaculo.BoundingBox()))
            //               {
            //                 collide = true;

            //TODO. Cuidado que esto se ejecuta una sola vez a menos que el personaje se mueva continuemente. Cuando aparece un fuego en la partida
            //este deberia realizar sus propias colisiones
            //TODO. Tener en cuenta que la direccion del movimiento y si esta yendo para adelante o para atrás deberían ser atributos del personaje
            //                   obstaculo.procesarColision(personaje, elapsedTime, obstaculos, moveForward, movementVector, lastPos);
            //                   break;
            //               }
            //           }

            //            if (!collide)
            //           {
            //              if (correr)
            //               {
            //Activar animacion de corriendo
            //                     personaje.mesh.playAnimation("Correr", true);
            //                  }
            //              else if (moving)
            //               {
            //                   //Activar animacion de caminando
            //                   personaje.mesh.playAnimation("Caminando", true);
            //              }
            //           }

            //Si hubo mivimiento actualizamos el centro del SkyBox para simular que es infinito
            //           if (!personaje.mesh.Position.Equals(lastPos))
            //           {
            //Si no hubo colisiones y el personaje se movio finalmente
            //                skyBox.Center += new Vector3((personaje.mesh.Position.X - lastPos.X), 0, (personaje.mesh.Position.Z - lastPos.Z));
            //              skyBox.updateValues();
            //           }
            //       }

            //Si no se esta moviendo, activar animacion de Parado, puede estar pateando o pegando
            //         else
            //       {
            //TODO. Mejorar esta lógica de estados.

            //    if ((pegar || patear))
            //       {
            //           float alcance = 0;
            //          float fuerzaGolpe = 0;

            //          if (patear)
            //            {
            //               personaje.mesh.playAnimation("Patear", true);
            //                 alcance = this.personaje.alcancePatada();
            //                fuerzaGolpe = this.personaje.fuerzaPatada();
            //            }

            //              if (pegar)
            //             {
            //                personaje.mesh.playAnimation("Pegar", true);
            //                alcance = this.personaje.alcanceGolpe();
            //                fuerzaGolpe = this.personaje.fuerzaGolpe();
            //            }

            //             //Si golpeo un obstáculo deberé esperar 2 segundos para poder golpearlo nuevamente
            //             if (puedeGolpear)
            //              {
            //Buscamos si esta al alcance alguno de los obstáculos
            //                  foreach (Elemento obstaculo in obstaculos)
            //                  {
            //                     if (this.EsferaColisionaCuadrado(personaje.GetAlcanceInteraccionEsfera(), obstaculo.BoundingBox()))
            //                     {
            //                          obstaculo.recibirDanio(fuerzaGolpe);
            //                          if (obstaculo.estaDestruido())
            //                         {
            //                           if (!obstaculo.destruccionTotal())
            //                            {
            //                                foreach (Elemento obs in obstaculo.elementosQueContiene())
            //                                {
            //TODO. Aplicar algun algoritmo de dispersion copado
            //                                    obs.posicion(obstaculo.posicion());
            //                                    obstaculos.Add(obs);
            //                           }
            //                              }
            //                              obstaculo.liberar();
            ////                              obstaculos.Remove(obstaculo);
            //                         }
            //En principio solo se puede golpear un obstaculo a la vez.
            //Tener en cuenta que estamos borrando un elemento de una colección que se esta recorriendo.
            //                          puedeGolpear = false;
            //                          break;
            //                      }
            //                  }
            //              }
            //           }
            //           else
            //        {
            //           personaje.mesh.playAnimation("Parado", true);
            //          }
            //            }

            //Simulamos el descanso del personaje
            //        personaje.incrementoResistenciaFisica(elapsedTime);

            //          informativo.Text = "";
            //          Elemento obstaculoInteractuar = null;
            //          foreach (Elemento elem in obstaculos)
            //           {
            //TODO. Optimizar esto para solo objetos cernanos!!!!!!!!
            //              if (this.EsferaColisionaCuadrado(personaje.GetAlcanceInteraccionEsfera(), elem.BoundingBox()))
            //              {
            //                  obstaculoInteractuar = elem;
            //                  break;
            //              }
            //         }
            //           if (obstaculoInteractuar != null)
            //          {
            //             obstaculoInteractuar.renderizarBarraEstado();

            //Pedimos lista de acciones al elemento
            //               informativo.Text = obstaculoInteractuar.getAcciones();
            //              if (consumir)
            //             {
            //                 obstaculoInteractuar.procesarInteraccion("Consumir", personaje, obstaculos, elapsedTime);
            //             }
            //              if (encender)
            //              {
            //                   obstaculoInteractuar.procesarInteraccion("Encender", personaje, obstaculos, elapsedTime);
            //               }
            //              if (juntar)
            //              {
            //                  obstaculoInteractuar.procesarInteraccion("Juntar", personaje, obstaculos, elapsedTime);
            //              }
            //            if (abrir)
            //           {
            //             obstaculoInteractuar.procesarInteraccion("Abrir", personaje, obstaculos, elapsedTime);
            //           }
            //       if (juntarTodo)
            //         {
            //       obstaculoInteractuar.procesarInteraccion("Juntar Todo", personaje, obstaculos, elapsedTime);
            //     }
            //       if (dejarElemento)
            //     {
            //         obstaculoInteractuar.procesarInteraccion("Dejar Elemento", personaje, obstaculos, elapsedTime);
            //     }
            //Accion generica
            //    obstaculoInteractuar.procesarInteraccion("Parado", personaje, obstaculos, elapsedTime);
            // }

            /* if (tirar)
             {
                 if (personaje.elementosEnMochila().Count > 0)
                 {
                     //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
                     float z = -(float)Math.Cos((float)personaje.mesh.Rotation.Y) * 150;
                     float x = -(float)Math.Sin((float)personaje.mesh.Rotation.Y) * 150;
                     //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
                     //el vector salga del personaje.
                     Vector3 direccion = personaje.mesh.Position + new Vector3(x, 0, z);
                     direccion.Y = terreno.CalcularAltura(direccion.X, direccion.Z);

                     Elemento elementoATirar = personaje.elementosEnMochila()[0];
                     elementoATirar.posicion(direccion);
                     obstaculos.Add(elementoATirar);
                     personaje.elementosEnMochila().Remove(elementoATirar);
                 }
             }*/

            // if (lanzar)
            //  {
            //TODO. Tener en cuenta que la direccion se esta calculando mas arriba, aunque aqui se calcula la direccion si el perosnaje esta quieto. Analizar!!!
            //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
            //        float z = -(float)Math.Cos((float)personaje.mesh.Rotation.Y) * 50;
            //         float x = -(float)Math.Sin((float)personaje.mesh.Rotation.Y) * 50;
            //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
            //el vector salga del personaje.
            //          Vector3 direccion = personaje.mesh.Position + new Vector3(x, /*terreno.CalcularAltura(x, z) + */1, z);


            //         directionArrow.PStart = personaje.mesh.Position;
            //           directionArrow.PEnd = direccion;
            //          directionArrow.updateValues();

            //         puebaFisica.Mesh.Position = personaje.mesh.Position + new Vector3(0,50,0);

            //      movimiento = new MovimientoParabolico(personaje.mesh.Position, direccion, 20, new MallaEnvoltura(puebaFisica.Mesh));

            //          personaje.mesh.playAnimation("Arrojar", true);
            //       }

            /* if (jumping)
             {
                 movimientoPersonaje = new MovimientoParabolico(personaje.mesh.Position, personaje.mesh.Position + new Vector3(0,1,0), 4, 
                     new MallaEnvoltura(personaje.mesh));
             }*/

            // if (jumpingAdelantefalse)
            //     {
            //TODO. Agregar para que no tome al mismo tiempo la tecla de avance W con la barra
            //TODO. Tener en cuenta que la direccion se esta calculando mas arriba, aunque aqui se calcula la direccion si el perosnaje esta quieto. Analizar!!!
            //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
            //          float z = -(float)Math.Cos((float)personaje.mesh.Rotation.Y) * 50;
            //          float x = -(float)Math.Sin((float)personaje.mesh.Rotation.Y) * 50;
            //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
            //el vector salga del personaje.
            //          Vector3 direccion = personaje.mesh.Position + new Vector3(x, /*terreno.CalcularAltura(x, z) + */1, z);

            //           movimientoPersonaje = new MovimientoParabolico(personaje.mesh.Position, direccion, 40, new MallaEnvoltura(personaje.mesh));
            //       }

            GuiController.Instance.UserVars.setValue("salud", personaje.salud);
            GuiController.Instance.UserVars.setValue("fuerza", personaje.fuerza);
            GuiController.Instance.UserVars.setValue("velocidad", personaje.velocidadCaminar);
            GuiController.Instance.UserVars.setValue("tiempocorriendo", personaje.tiempoCorriendo);

            GuiController.Instance.UserVars.setValue("x", personaje.mesh.Position.X);
            GuiController.Instance.UserVars.setValue("y", personaje.mesh.Position.Y);
            GuiController.Instance.UserVars.setValue("z", personaje.mesh.Position.Z);


            if (tiempo >= 2)
            {
                //Si paso más de dos segundos lo reiniciamos.
                personaje.afectarSaludPorTiempo(tiempo);
 
                tiempo = 0;
                //puedeGolpear = true;
            }
            tiempo += elapsedTime;

            //Analisis de posibilidad de incrementar o no salud por fuego
            /*if (puedeIncrementarSaludConFuego)
            {
                personaje.incrementarSaludPorTiempo(elapsedTime);
            }*/

            float offsetHeight;
            if (d3dInput.keyDown(Key.O))
            {
                offsetHeight = GuiController.Instance.ThirdPersonCamera.OffsetHeight;
                GuiController.Instance.ThirdPersonCamera.OffsetHeight = offsetHeight + 1;
            }
            if (d3dInput.keyDown(Key.P))
            {
                offsetHeight = GuiController.Instance.ThirdPersonCamera.OffsetHeight;
                GuiController.Instance.ThirdPersonCamera.OffsetHeight = offsetHeight - 1;
            }
            float offsetForward;
            if (d3dInput.keyDown(Key.K))
            {
                offsetForward = GuiController.Instance.ThirdPersonCamera.OffsetForward;
                GuiController.Instance.ThirdPersonCamera.OffsetForward = offsetForward + 1;
            }
            if (d3dInput.keyDown(Key.L))
            {
                offsetForward = GuiController.Instance.ThirdPersonCamera.OffsetForward;
                GuiController.Instance.ThirdPersonCamera.OffsetForward = offsetForward - 1;
            }

            //Actualizamos los objetos Fisicos y los renderizamos
            if (movimiento != null)
            {
                movimiento.update(elapsedTime, terreno);
                movimiento.render();
                if (movimiento.Finalizo)
                {
                    movimiento = null;
                    //lanzar = null;
                }
            }
            else
            {
                puebaFisica.renderizar();
            }

            if(movimientoPersonaje != null)
            {
                movimientoPersonaje.update(elapsedTime, terreno);
                movimientoPersonaje.render();
                if (movimientoPersonaje.Finalizo)
                {
                    movimientoPersonaje = null;
                    //saltar = null;
                }
            }

            //actualizamos la posición de los animales
            oveja.update(elapsedTime, terreno);
            gallo.update(elapsedTime, terreno);

            //Render Terreno
            terreno.render();

            directionArrow.render();

            //Render piso
            piso.render();

            //Render personaje
            personaje.mesh.animateAndRender();

            //Render obstaculos
            foreach (Elemento obstaculo in elementos)
            {
                obstaculo.renderizar();
                //obstaculo.BoundingBox.render();
            }

            //Renderizar SkyBox
            skyBox.render();

            //Personaje muerto
            if (personaje.estaMuerto())
            {
                textGameOver.render();
            }

            if (informativo.Text != "")
            {
                informativo.render();
            }

            String elementosActuales = "";
            foreach (Elemento elem in personaje.elementosEnMochila())
            {
                elementosActuales = elementosActuales + elem.nombre() + ", ";
            }
            elementosEnMochila.Text = "Mochila: " + elementosActuales;
            elementosEnMochila.render();

            //Obtener valor de UserVar (hay que castear)
            // int valor = (int)GuiController.Instance.UserVars.getValue("variablePrueba");


            //Obtener valores de Modifiers
            float valorFloat = (float)GuiController.Instance.Modifiers["valorFloat"];
            string opcionElegida = (string)GuiController.Instance.Modifiers["valorIntervalo"];
            Vector3 valorVertice = (Vector3)GuiController.Instance.Modifiers["valorVertice"];


            ///////////////INPUT//////////////////
            //conviene deshabilitar ambas camaras para que no haya interferencia

            //Capturar Input teclado 
            if (GuiController.Instance.D3dInput.keyPressed(Microsoft.DirectX.DirectInput.Key.F))
            {
                //Tecla F apretada
            }

            //Capturar Input Mouse
            if (GuiController.Instance.D3dInput.buttonPressed(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                //Boton izq apretado
            }

        }

        /// <summary>
        /// Método que se llama cuando termina la ejecución del ejemplo.
        /// Hacer dispose() de todos los objetos creados.
        /// </summary>
        public override void close()
        {
            piso.dispose();
            personaje.mesh.dispose();
            terreno.dispose();
            skyBox.dispose();
            foreach (Elemento obstaculo in elementos)
            {
                obstaculo.destruir();
            }
            textGameOver.dispose();
        }

        /*private bool isPointInsideAABB(Vector3 point, TgcBoundingBox box)
        {
            return (point.X >= box.PMin.X && point.X <= box.PMax.X) &&
                   (point.Y >= box.PMin.Y && point.Y <= box.PMax.Y) &&
                   (point.Z >= box.PMin.Z && point.Z <= box.PMax.Z);
        }

        private bool EsferaColisionaCuadrado(TgcSphere esfera, TgcBoundingBox cuadrado)
        {
            // Hacemos el test
            float s = 0;
            float d = 0;

            // Comprobamos si el centro de la esfera está dentro del AABB
            if (this.isPointInsideAABB(esfera.Position, cuadrado))
            {
                return true;
            }

            // Comprobamos si la esfera y el AABB se intersectan
            if (esfera.Position.X < cuadrado.PMin.X)
            {
                s = esfera.Position.X - cuadrado.PMin.X;
                d += s * s;
            }
            else if (esfera.Position.X > cuadrado.PMax.X)
            {
                s = esfera.Position.X - cuadrado.PMax.X;
                d += s * s;
            }

            if (esfera.Position.Y < cuadrado.PMin.Y)
            {
                s = esfera.Position.Y - cuadrado.PMin.Y;
                d += s * s;
            }
            else if (esfera.Position.Y > cuadrado.PMax.Y)
            {
                s = esfera.Position.Y - cuadrado.PMax.Y;
                d += s * s;
            }

            if (esfera.Position.Z < cuadrado.PMin.Z)
            {
                s = esfera.Position.Z - cuadrado.PMin.Z;
                d += s * s;
            }
            else if (esfera.Position.Z > cuadrado.PMax.Z)
            {
                s = esfera.Position.Z - cuadrado.PMax.Z;
                d += s * s;
            }

            return (d <= esfera.Radius * esfera.Radius);
        }*/

    }
}
