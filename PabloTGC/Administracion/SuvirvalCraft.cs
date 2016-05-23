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
        public List<Elemento> elementosSinInteraccion;
        public Personaje personaje;
        TgcArrow directionArrow;
        float tiempo;
        TgcScene scene, scene2, scene3, scene4, scene5, scene6, scene7;
        TgcMesh palmera, pino, arbol, banana, fuego, lenia, carneCruda;
        TgcText2d textGameOver;
        public TgcText2d informativo;
        Animal oveja;
        Animal gallo;
        TgcMesh hachaMesh;
        TgcMesh palo;

        TgcSprite mochila;
        TgcSprite cajon;
        TgcText2d mochilaReglon1;
        public TgcText2d cajonReglon1;
        public bool mostrarMenuMochila;
        public bool mostrarMenuCajon;

        public Elemento puebaFisica;
        public MovimientoParabolico movimiento;
        public MovimientoParabolico movimientoPersonaje;

        public ControladorEntradas controladorEntradas;

        TgcSprite salud;
        TgcSprite hidratacion;
        TgcSprite alimentacion;
        TgcSprite cansancio;

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

            elementos = new List<Elemento>();
            elementosSinInteraccion = new List<Elemento>();

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
            ovejaMesh.Position = new Vector3(200, terreno.CalcularAltura(200, 200), 200);
            scene = loader.loadSceneFromFile(recursos
                 + "MeshCreator\\Meshes\\Alimentos\\Hamburguesa\\Hamburguesa-TgcScene.xml");
            TgcMesh hamburguesa = ovejaMesh = scene.Meshes[0];
            Alimento alimento = new Alimento(1000, 1000, carneCruda.createMeshInstance("CarneCruda"));
            alimento.posicion(ovejaMesh.Position);
            alimento.agregarElemento(new Alimento(1000, 1000, hamburguesa.createMeshInstance("Hamburguesa_1")));
            alimento.BoundingBox().scaleTranslate(alimento.posicion(), new Vector3(2f, 2f, 2f));
            oveja.agregarElemento(alimento);
            elementos.Add(oveja);

            //Creamos el gallo
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Gallo\\Gallo-TgcScene.xml");
            TgcMesh galloMesh = scene.Meshes[0].createMeshInstance("Gallo");
            gallo = new Animal(5000, 20, galloMesh);
            galloMesh.Position = new Vector3(0, terreno.CalcularAltura(0, 0), 0);
            alimento = new Alimento(1000, 1000, carneCruda.createMeshInstance("CarneCruda"));
            alimento.posicion(galloMesh.Position);
            alimento.agregarElemento(new Alimento(1000, 1000, hamburguesa.createMeshInstance("Hamburguesa_2")));
            alimento.BoundingBox().scaleTranslate(alimento.posicion(), new Vector3(2f, 2f, 2f));
            gallo.agregarElemento(alimento);
            elementos.Add(gallo);

            //**************Creamos el cajon con las manzanas*********************
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Cajon\\cajon-TgcScene.xml");
            TgcMesh cajonRealMesh = scene.Meshes[0].createMeshInstance("Cajon_Manzanas");
            Elemento cajonReal = new Cajon(5000, 1000, cajonRealMesh);
            cajonRealMesh.Position = new Vector3(233, terreno.CalcularAltura(233, 233), 233);
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Alimentos\\Frutas\\ManzanaVerde\\manzanaverde-TgcScene.xml");
            TgcMesh manzanaVerde = scene.Meshes[0].createMeshInstance("Manzana Verde");
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Alimentos\\Frutas\\ManzanaRoja\\manzanaroja-TgcScene.xml");
            TgcMesh manzanaRoja = scene.Meshes[0].createMeshInstance("Manzana Roja");
            manzanaVerde.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            cajonReal.agregarElemento(new Alimento(1000,1000, manzanaVerde));
            manzanaRoja.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            cajonReal.agregarElemento(new Alimento(1000, 1000, manzanaRoja));
            elementos.Add(cajonReal);
            //********************************************************************

            //**************Creamos el cajon con la olla*********************
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Cajon\\cajon-TgcScene.xml");
            TgcMesh cajonOllaMesh = scene.Meshes[0].createMeshInstance("Cajon_Olla");
            Elemento cajonOlla = new Cajon(5000, 1000, cajonOllaMesh);
            cajonOllaMesh.Position = new Vector3(-233, terreno.CalcularAltura(-233, -233), -233);
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Olla\\Olla-TgcScene.xml");
            TgcMesh ollaMesh = scene.Meshes[0].createMeshInstance("Olla");
            ollaMesh.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\CopaMadera\\CopaMadera-TgcScene.xml");
            TgcMesh copaMesh = scene.Meshes[0].createMeshInstance("Copa");
            copaMesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
            cajonOlla.agregarElemento(new Olla(1000, 1000, ollaMesh));
            cajonOlla.agregarElemento(new Copa(1000, 1000, copaMesh));
            elementos.Add(cajonOlla);
            //********************************************************************

            //*******************Creamos los arboles y la fuente de agua**********************
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\Arbol\\Arbol-TgcScene.xml");
            arbol = scene.Meshes[0];
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\ArbolAmarillo\\arbolamarillo-TgcScene.xml");
            TgcMesh arbolAmarillo = scene.Meshes[0];
            float[] posicionesArboles = { 500f, 700f, 900f, 1100f };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    TgcMesh arbolNuevo;
                    if (FuncionesMatematicas.Instance.NumeroAleatorioDouble() <= 0.8)
                    {
                        arbolNuevo = arbol.createMeshInstance(arbol.Name + i);
                    }
                    else
                    {
                        arbolNuevo = arbolAmarillo.createMeshInstance(arbolAmarillo.Name + i);
                    }
                    arbolNuevo.Scale = new Vector3(2f, 3f, 2f);
                    float x = (posicionesArboles[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(j * 100f, (j * 100f + 30)));
                    float z = (posicionesArboles[i]  * FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-1.4f, -1));
                    arbolNuevo.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                    elementos.Add(new Elemento(1000, 1400, arbolNuevo));
                }
            }
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\FuenteAgua\\FuenteAgua-TgcScene.xml");
            TgcMesh fuente = scene.Meshes[0].createMeshInstance("Fuente de Agua");
            fuente.Scale = new Vector3(1.5f, 2.5f, 1.5f);
            //Sabemos que en la posicion 400x;-400z no hay ningun arbol
            fuente.Position = new Vector3(400, terreno.CalcularAltura(400, -400), -400);
            elementos.Add(new FuenteAgua(1000, 1400, fuente));
            //*********************************************************************************

            //***************Creamos las algas***********************************************************
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\Alga\\alga-TgcScene.xml");
            arbol = scene.Meshes[0];
            float[] posicionesArbustos = { 200f, 300f, 400f, 500f, 600f, 700f, 800f, 900f, 1000f };
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    TgcMesh arbusto = arbol.createMeshInstance(arbol.Name + i);
                    arbusto.Scale = new Vector3(0.5f, 0.7f, 0.5f);
                    float x = (posicionesArbustos[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(j * 100f, (j * 100f + 30)));
                    float z = (-3000 + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-100, 100));
                    arbusto.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                    elementosSinInteraccion.Add(new Elemento(1000, 1400, arbusto));
                }
            }
            //*****************************************************************************************************************************************

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

            //****************Crear Sprite de la mochila y del cajon**********************************************
            mochila = new TgcSprite();
            mochila.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Mochila.jpg");

            //Ubicarlo centrado en la pantalla
            Size screenSize = GuiController.Instance.Panel3d.Size;
            Size textureSize = mochila.Texture.Size;
            mochila.Position = new Vector2(20, (screenSize.Height - textureSize.Height / 2) / 2);
            mochila.Scaling = new Vector2(0.5f, 0.5f);

            mochilaReglon1 = new TgcText2d();
            mochilaReglon1.Color = Color.Black;
            mochilaReglon1.Text = "";
            mochilaReglon1.Align = TgcText2d.TextAlign.LEFT;
            mochilaReglon1.Position = new Point(0, 0);
            //Imagen 1024, escalada 0.5 son 512 menos 70 de la parte de arriba quedarian 442 para los 9 renglones
            //largo es como la imagen
            mochilaReglon1.Size = new Size(512, 50);
            mochilaReglon1.changeFont(new System.Drawing.Font("TimesNewRoman", 30, FontStyle.Bold));

            cajon = new TgcSprite();
            cajon.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Cajon.jpg");

            //Ubicarlo centrado en la pantalla
            Size textureCacjonSize = cajon.Texture.Size;
            cajon.Position = new Vector2(screenSize.Width - 20 - (textureCacjonSize.Width / 2), (screenSize.Height - textureCacjonSize.Height / 2) / 2);
            cajon.Scaling = new Vector2(0.5f, 0.5f);

            cajonReglon1 = new TgcText2d();
            cajonReglon1.Color = Color.White;
            cajonReglon1.Text = "";
            cajonReglon1.Align = TgcText2d.TextAlign.LEFT;
            cajonReglon1.Position = new Point(0, 0);
            //Imagen 1024, escalada 0.5 son 512 menos 70 de la parte de arriba quedarian 442 para los 9 renglones
            //largo es como la imagen
            cajonReglon1.Size = new Size(512, 50);
            cajonReglon1.changeFont(new System.Drawing.Font("TimesNewRoman", 30, FontStyle.Bold));

            //****************Crear Sprite de la mochila y del cajon**********************************************

            //****************Crear el texto informativo**********************************************************
            informativo = new TgcText2d();
            informativo.Color = Color.Yellow;
            informativo.Align = TgcText2d.TextAlign.CENTER;
            informativo.Position = new Point(10, screenSize.Height - 75);
            informativo.Size = new Size(screenSize.Width, 50);
            informativo.changeFont(new System.Drawing.Font("TimesNewRoman", 40, FontStyle.Bold));
            //****************Crear el texto informativo**********************************************************

            //****************Creacion de barra de salud, hidratacion, alimentacion y cansancio*******************
            salud = new TgcSprite();
            salud.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\BarraSalud.png");
            salud.Position = new Vector2(20, 20);
            salud.Scaling = new Vector2(0.5f, 0.5f);

            hidratacion = new TgcSprite();
            hidratacion.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\BarraHidratacion.png");
            hidratacion.Position = new Vector2(200, 20);
            hidratacion.Scaling = new Vector2(0.5f, 0.5f);

            alimentacion = new TgcSprite();
            alimentacion.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\BarraAlimentacion.png");
            alimentacion.Position = new Vector2(380, 20);
            alimentacion.Scaling = new Vector2(0.5f, 0.5f);

            cansancio = new TgcSprite();
            cansancio.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\BarraCansancio.png");
            cansancio.Position = new Vector2(560, 20);
            cansancio.Scaling = new Vector2(0.5f, 0.5f);
            //****************Creacion de barra de salud, hidratacion, alimentacion y cansancio*******************
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

            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;

            informativo.Text = "";
            mostrarMenuMochila = false;
            mostrarMenuCajon = false;

            foreach (Comando comando in controladorEntradas.ProcesarEntradasTeclado())
            {
                comando.Ejecutar(this, elapsedTime);
            }

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

            //Actualiza los elementos
            List<Elemento> aux = new List<Elemento>();
            aux.AddRange(elementos);//TODO. Porque sino con la actualziacion borramos elementos de la colecsion de elemetos y se rompe todo
            foreach (Elemento elemento in aux)
            {
                elemento.Actualizar(this, elapsedTime);
            }

            //Render elementos
            foreach (Elemento elemento in elementos)
            {
                elemento.renderizar();
                elemento.BoundingBox().render();
            }

            //Render elementos sin interaccion
            foreach (Elemento elemento in elementosSinInteraccion)
            {
                elemento.renderizar();
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

            GuiController.Instance.Drawer2D.beginDrawSprite();
            salud.Scaling = new Vector2(personaje.PorcentajeDeSalud() * 0.5f, 0.5f);
            salud.render();
            hidratacion.render();
            hidratacion.Scaling = new Vector2(personaje.PorcentajeDeSalud() * 0.5f, 0.5f);//TODO poner valor real
            alimentacion.render();
            alimentacion.Scaling = new Vector2(personaje.PorcentajeDeSalud() * 0.5f, 0.5f);//TODO poner valor real
            cansancio.Scaling = new Vector2(personaje.PorcentajeDeCansancio() * 0.5f, 0.5f);
            cansancio.render();
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
                     mochilaReglon1.Text = mochilaReglon1.Text + (i+1).ToString() + "    " + personaje.DarElementoEnPosicionDeMochila(i).GetTipo() + System.Environment.NewLine;
                    }
                    else
                    {
                        mochilaReglon1.Text = mochilaReglon1.Text + (i + 1).ToString() + "    "  + "Disponible" + System.Environment.NewLine;
                    }
                }
                int x = (int)(mochila.Position.X);
                int y = (int)(mochila.Position.Y);
                mochilaReglon1.Position = new Point(x + 11, y + 95);
                mochilaReglon1.render();
                if (mostrarMenuCajon)
                {
                    //En texto que va en el renglon lo completa el cajon cuando hay una interaccion.
                    x = (int)(cajon.Position.X);
                    y = (int)(cajon.Position.Y);
                    cajonReglon1.Position = new Point(x + 11, y + 85);
                    cajonReglon1.render();
                }
            }

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
            foreach (Elemento elemento in elementos)
            {
                elemento.destruir();
            }
            foreach (Elemento elemento in elementosSinInteraccion)
            {
                elemento.destruir();
            }
            informativo.dispose();
            textGameOver.dispose();
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
        }

    }
}
