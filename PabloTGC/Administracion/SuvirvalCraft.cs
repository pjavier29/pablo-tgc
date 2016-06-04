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
using AlumnoEjemplos.PabloTGC.Utiles.Camaras;
using TgcViewer.Utils.Shaders;
using AlumnoEjemplos.PabloTGC.Movimientos;
using AlumnoEjemplos.PabloTGC.Dia;

namespace AlumnoEjemplos.MiGrupo
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class SuvirvalCraft : TgcExample
    {
        public Terreno terreno;
        TgcSkyBox skyBox;
        public TgcBox piso;
        public List<Elemento> elementos;
        public Personaje personaje;
        public float tiempo;
        TgcText2d estadoJuego;
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

        public Vector3 esquina;//Con un solo punto arma el cuadrado para definir los limites del mapa

        public Optimizador optimizador;

        TgcSprite salud;
        TgcSprite hidratacion;
        TgcSprite alimentacion;
        TgcSprite cansancio;
        TgcSprite saludIcono;
        TgcSprite hidratacionIcono;
        TgcSprite alimentacionIcono;
        TgcSprite cansancioIcono;
        TgcSprite objetivosIcono;
        TgcText2d mensajeObjetivo1;
        float tiempoObjetivo;
        TgcSprite ayuda;
        public TgcText2d ayudaReglon1;
        public TgcText2d ayudaReglon2;
        public bool mostrarAyuda;
        TgcText2d temperaturaDia;
        TgcText2d horaDia;
        TgcSprite temperaturaDiaIcono;
        TgcSprite horaDiaIcono;
        TgcText2d temperaturaPersonaje;
        TgcSprite temperaturaPersonajeIcono;
        TgcSprite estadoDiaSolIcono;
        TgcSprite estadoDiaLunaIcono;


        public Camara camara;

        public Dia dia;

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
            GuiController.Instance.UserVars.addVar("x");
            GuiController.Instance.UserVars.addVar("y");
            GuiController.Instance.UserVars.addVar("z");

            //***********Inicializamos las esquinas************************************
            //Crear objeto propio para manejar los limites.
            esquina = new Vector3(10000, 0, 10000);
            //***********Inicializamos las esquinas************************************

            // ------------------------------------------------------------
            // Creo el Heightmap para el terreno:
            terreno = new Terreno();
            terreno.loadHeightmap(recursos
                    + "Shaders\\WorkshopShaders\\Heighmaps\\" + "HeightmapHawaii.jpg", 400f, 3f, new Vector3(0, 0, 0));
            terreno.loadTexture(recursos
                    + "Shaders\\WorkshopShaders\\Heighmaps\\" + "TerrainTextureHawaii.jpg");
            terreno.SetEfecto(TgcShaders.loadEffect(recursos + "Shaders\\TerrenoShader.fx"));
            // ------------------------------------------------------------

            // *********************Crear SkyBox*********************************
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 0, 0);
            skyBox.Size = new Vector3(20000, 20000, 20000);
            string texturesPath = recursos + "Texturas\\Quake\\SkyBox1\\";
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "phobos_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "phobos_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "phobos_lf.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "phobos_rt.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "phobos_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "phobos_ft.jpg");
            skyBox.SkyEpsilon = 50f;
            skyBox.updateValues();
            Microsoft.DirectX.Direct3D.Effect effectSkybox = TgcShaders.loadEffect(recursos + "Shaders\\SkyBoxShader.fx");
            foreach (TgcMesh faces in this.skyBox.Faces)
            {
                faces.Effect = effectSkybox;
                faces.Technique = "RenderScene";
            }
            // *********************Crear SkyBox*********************************

            TgcScene scene;
            elementos = new List<Elemento>();

            #region Creamos las palmeras comunes
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
            TgcMesh palmera = scene.Meshes[0];
            TgcMesh palmeraNueva;
            float[] posicionPalmerasX = { -6000f, -5500f, -5000f, -4500f };
            float[] posicionPalmerasZ = { 3000f, 3500f, 4000f, 4500f };
            for (int i = 0; i < posicionPalmerasX.Length; i++)
            {
                for (int j = 0; j < posicionPalmerasZ.Length; j++)
                {
                    palmeraNueva = palmera.createMeshInstance(palmera.Name + i + j);
                    palmeraNueva.Scale = new Vector3(0.5f, 1.5f, 0.5f);
                    float x = posicionPalmerasX[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    float z = posicionPalmerasZ[j] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    palmeraNueva.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                    elementos.Add(new Elemento(1000, 1400, palmeraNueva));
                }
            }
            #endregion

            #region Creamos los arboles de Banana
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\ArbolBananas\\ArbolBananas-TgcScene.xml");
            TgcMesh arbolBanana = scene.Meshes[0];
            scene = loader.loadSceneFromFile(recursos
                 + "MeshCreator\\Meshes\\Alimentos\\Frutas\\Bananas\\Bananas-TgcScene.xml");
            TgcMesh bananaMesh = scene.Meshes[0];
            TgcMesh arbolBananaNuevo;
            TgcMesh bananaMeshNueva;
            float[] posicionBananasX = { -1000f, -500f, 0000f, 500f, 1000f, 1500f, 2000f, 2500f, 3000f, 3500f, 4000f };
            float[] posicionBananasZ = { 4000f, 4500f, 5000f, 5500f, 6000f };
            for (int i = 0; i < posicionBananasX.Length; i++)
            {
                for (int j = 0; j < posicionBananasZ.Length; j++)
                {
                    arbolBananaNuevo = arbolBanana.createMeshInstance(arbolBanana.Name + i + j);
                    arbolBananaNuevo.Scale = new Vector3(1.5f, 3.5f, 1.5f);
                    float x = posicionBananasX[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    float z = posicionBananasZ[j] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    arbolBananaNuevo.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                    bananaMeshNueva = bananaMesh.createMeshInstance("Banana");
                    bananaMeshNueva.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                    elementos.Add(new Elemento(1000, 1300, arbolBananaNuevo, new Alimento(1000, 1000, bananaMeshNueva, 20)));
                }
            }
            #endregion

            #region Creamos los arboles que van a proveer leña para luego hacer fuego
            //Cargar Shader personalizado para el efecto del fuego
            Microsoft.DirectX.Direct3D.Effect effect = TgcShaders.loadEffect(recursos + "Shaders\\FuegoShader.fx");

            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\Pino\\Pino-TgcScene.xml");
            TgcMesh pino = scene.Meshes[0];
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Fuego\\fuego-TgcScene.xml");
            TgcMesh fuego = scene.Meshes[0];
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Leña\\lenia-TgcScene.xml");
            TgcMesh lenia = scene.Meshes[0];
            TgcMesh fuegoMesh;
            TgcMesh leniaMesh;
            TgcMesh pinoNuevo;
            float[] posicionPinoX = { -8000f, -7500f, -7000f ,-6500f, -6000f};
            float[] posicionPinoZ = { -4000f, -3500f, -3000f, -2500f, -2000f, -1500f, -1000f, -500f, 0f };
            for (int i = 0; i < posicionPinoX.Length; i++)
            {
                for (int j = 0; j < posicionPinoZ.Length; j++)
                { 
                    pinoNuevo = pino.createMeshInstance(pino.Name + i + j);
                    pinoNuevo.Scale = new Vector3(0.5f, 1.5f, 0.5f);
                    float x = posicionPinoX[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    float z = posicionPinoZ[j] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    pinoNuevo.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                    fuegoMesh = fuego.createMeshInstance(fuego.Name + i + j);
                    //fuegoMesh.Technique = "RenderScene";
                    leniaMesh = lenia.createMeshInstance(lenia.Name + i + j);
                    fuegoMesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                    leniaMesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                    elementos.Add(new Elemento(1000, 2330, pinoNuevo,
                                (new Madera(1000, 233, leniaMesh,
                                    new Fuego(1000, 233, fuegoMesh/*, effect.Clone(d3dDevice)*/)))));
                }
            }
            #endregion

            #region Creamos la piedra para tirar
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Roca\\Roca-TgcScene.xml");
            TgcMesh piedraMesh = scene.Meshes[0].createMeshInstance("Piedra");
            puebaFisica = new Elemento(100, 300, piedraMesh);
            #endregion

            #region Creamos la oveja
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Oveja\\Ovelha-TgcScene.xml");
            TgcMesh ovejaMesh = scene.Meshes[0].createMeshInstance("Oveja");
            scene = loader.loadSceneFromFile(recursos
                 + "MeshCreator\\Meshes\\Alimentos\\CarneCruda\\carnecruda-TgcScene.xml");
            TgcMesh carneCruda = scene.Meshes[0];
            oveja = new Animal(5000, 20, ovejaMesh);
            ovejaMesh.Position = new Vector3(200, terreno.CalcularAltura(200, 200), 200);
            scene = loader.loadSceneFromFile(recursos
                 + "MeshCreator\\Meshes\\Alimentos\\Hamburguesa\\Hamburguesa-TgcScene.xml");
            TgcMesh hamburguesa = scene.Meshes[0];
            Alimento alimento = new Alimento(1000, 1000, carneCruda.createMeshInstance("Carne Cruda"), 10);
            alimento.posicion(ovejaMesh.Position);
            TgcMesh hamburguesaOveja = hamburguesa.createMeshInstance("Hambur Oveja");
            hamburguesaOveja.Scale = new Vector3(0.3f, 0.3f, 0.3f);
            alimento.agregarElemento(new Alimento(1000, 1000, hamburguesaOveja, 70));
            alimento.BoundingBox().scaleTranslate(alimento.posicion(), new Vector3(2f, 2f, 2f));
            oveja.agregarElemento(alimento);
            elementos.Add(oveja);
            #endregion

            #region Creamos el gallo
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Gallo\\Gallo-TgcScene.xml");
            TgcMesh galloMesh = scene.Meshes[0].createMeshInstance("Gallo");
            gallo = new Animal(5000, 20, galloMesh);
            galloMesh.Position = new Vector3(0, terreno.CalcularAltura(0, 0), 0);
            alimento = new Alimento(1000, 1000, carneCruda.createMeshInstance("Carne Cruda"), 10);
            alimento.posicion(galloMesh.Position);
            TgcMesh hamburguesaGallo = hamburguesa.createMeshInstance("Hambur Gallo");
            hamburguesaGallo.Scale = new Vector3(0.3f, 0.3f, 0.3f);
            alimento.agregarElemento(new Alimento(1000, 1000, hamburguesaGallo, 70));
            alimento.BoundingBox().scaleTranslate(alimento.posicion(), new Vector3(2f, 2f, 2f));
            gallo.agregarElemento(alimento);
            elementos.Add(gallo);
            #endregion

            #region Creamos el cajon con las manzanas
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Cajon\\cajon-TgcScene.xml");
            TgcMesh cajonRealMesh = scene.Meshes[0].createMeshInstance("Cajon_Manzanas");
            Elemento cajonReal = new Cajon(5000, 1000, cajonRealMesh);
            cajonRealMesh.Position = new Vector3(1600, terreno.CalcularAltura(1600, 8500), 8500);
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Alimentos\\Frutas\\ManzanaVerde\\manzanaverde-TgcScene.xml");
            TgcMesh manzanaVerde = scene.Meshes[0].createMeshInstance("Manzana Verde");
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Alimentos\\Frutas\\ManzanaRoja\\manzanaroja-TgcScene.xml");
            TgcMesh manzanaRoja = scene.Meshes[0].createMeshInstance("Manzana Roja");
            manzanaVerde.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            cajonReal.agregarElemento(new Alimento(1000,1000, manzanaVerde, 30));
            manzanaRoja.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            cajonReal.agregarElemento(new Alimento(1000, 1000, manzanaRoja, 30));
            elementos.Add(cajonReal);
            #endregion

            #region Creamos el cajon con la olla y con la copa
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Cajon\\cajon-TgcScene.xml");
            TgcMesh cajonOllaMesh = scene.Meshes[0].createMeshInstance("Cajon_Olla");
            Elemento cajonOlla = new Cajon(5000, 1000, cajonOllaMesh);
            cajonOllaMesh.Position = new Vector3(-9500, terreno.CalcularAltura(-9500, -4950), -4950);
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
            #endregion

            #region Creamos los arboles generales
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\Arbol\\Arbol-TgcScene.xml");
            TgcMesh arbol = scene.Meshes[0];
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\ArbolAmarillo\\arbolamarillo-TgcScene.xml");
            TgcMesh arbolAmarillo = scene.Meshes[0];
            float[] posicionesArbolesX = { -3000f, -2500f, -2000f, -1500f, -1000f, -500f, 00f, 500f, 1000f, 1500f, 2000f, 2500f, 3000f, 3500f,
                4000f, 4500f, 5000f,5500f, 6000f, 6500f, 7000f, 7500f, 8000f };
            float[] posicionesArbolesZ = { -5000f, -5000f, -6000f, -6500f, -7000f, -7500f, - 8000f };
            for (int i = 0; i < posicionesArbolesX.Length; i++)
            {
                for (int j = 1; j < posicionesArbolesZ.Length; j++)
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
                    float x = posicionesArbolesX[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    float z = posicionesArbolesZ[j] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    arbolNuevo.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                    elementos.Add(new Elemento(1000, 1400, arbolNuevo));
                }
            }
            #endregion

            #region Creamos la fuente de agua
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\FuenteAgua\\FuenteAgua-TgcScene.xml");
            TgcMesh fuente = scene.Meshes[0].createMeshInstance("Fuente de Agua");
            fuente.Scale = new Vector3(1.5f, 2.5f, 1.5f);
            fuente.Position = new Vector3(2400, terreno.CalcularAltura(2400, -3160), -3160);
            elementos.Add(new FuenteAgua(1000, 1400, fuente));
            #endregion

            #region Creamos las algas
            //Cargar Shader personalizado para el efecto de las algas
            Microsoft.DirectX.Direct3D.Effect efectoAlgas = TgcShaders.loadEffect(recursos + "Shaders\\AlgaShader.fx");
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\Alga\\alga-TgcScene.xml");
            TgcMesh alga = scene.Meshes[0];
            TgcMesh algaNueva;
            float[] posicionesAlgasX = { -9500f, -9000f, -8500f, -8000f, -7500f, -7000f};
            float[] posicionesAlgasZ = { 9500f, 9000f, 8500f, 8000f, 7500f, 7000f, 6500f, 6000f, 5500f, 5000f, 4500f };
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    algaNueva = alga.createMeshInstance(alga.Name + elementos.Count.ToString());
                    algaNueva.Scale = new Vector3(0.5f, 0.7f, 0.5f);
                    float x = (posicionesAlgasX[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(j * 10f, (j * 50f + 100)));
                    float z = (FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-9500, -8000));
                    algaNueva.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                    //algaNueva.Effect = efectoAlgas.Clone(d3dDevice);
                    if (FuncionesMatematicas.Instance.NumeroAleatorioFloat() > 0.5f)
                    {
                        //algaNueva.Technique = "RenderScene";
                    }
                    else
                    {
                        //algaNueva.Technique = "RenderScene2";
                    }
                    elementos.Add(new ElementoSinInteraccion(1000, 1400, algaNueva/*, efectoAlgas.Clone(d3dDevice)*/));
                }
            }
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    algaNueva = alga.createMeshInstance(alga.Name + elementos.Count.ToString());
                    algaNueva.Scale = new Vector3(0.5f, 0.7f, 0.5f);
                    float x = (FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(6000, 9000));
                    float z = (posicionesAlgasZ[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(j * 10f, (j * 50f + 100)));
                    algaNueva.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                    if (FuncionesMatematicas.Instance.NumeroAleatorioFloat() > 0.5f)
                    {
                        //algaNueva.Technique = "RenderScene";
                    }
                    else
                    {
                        //algaNueva.Technique = "RenderScene2";
                    }
                    elementos.Add(new ElementoSinInteraccion(1000, 1400, algaNueva/*, efectoAlgas.Clone(d3dDevice)*/));
                }
            }
            #endregion

            #region Creamos algunas piedras sobre el agua
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Roca\\Roca-TgcScene.xml");
            TgcMesh piedraAguaMesh = scene.Meshes[0];
            TgcMesh piedraAguaNuevaMesh;
            float[] posicionesPiedrasZ = { 500f, 1000f, 2000f, 3000f, 3500f, 4000f };
            float aleatorio;
            for (int i = 0; i < 6; i++)
            {
                piedraAguaNuevaMesh = piedraAguaMesh.createMeshInstance(piedraAguaMesh.Name + elementos.Count.ToString());
                aleatorio = FuncionesMatematicas.Instance.NumeroAleatorioFloat();
                if (aleatorio < 0.3f)
                {
                    piedraAguaNuevaMesh.Scale = new Vector3(6f, 6f, 6f);
                }
                else
                {
                    if (aleatorio < 0.6f)
                    {
                        piedraAguaNuevaMesh.Scale = new Vector3(12f, 12f, 12f);
                    }
                    else
                    {
                        piedraAguaNuevaMesh.Scale = new Vector3(8f, 5f, 12f);
                    }
                }
                
                float x = (FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(8000, 9000));
                float z = (posicionesPiedrasZ[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(100,300));
                piedraAguaNuevaMesh.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                elementos.Add(new Elemento(10000, 10000, piedraAguaNuevaMesh));
            }
            #endregion

            #region Creamos las canoas sobre el agua
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Botes\\Palangreta\\Palangreta-TgcScene.xml");
            TgcMesh palangreta = scene.Meshes[0].createMeshInstance("Palangreta");
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Botes\\Canoa\\Canoa-TgcScene.xml");
            TgcMesh canoa = scene.Meshes[0].createMeshInstance("Canoa");
            palangreta.Position = new Vector3(-6500, terreno.CalcularAltura(-6500, 6000), 6000);
            canoa.Position = new Vector3(-6000, terreno.CalcularAltura(-6000, 6000) + 30, 6000);
            canoa.Scale = new Vector3(2f,2f,2f);
            //canoa.Technique = "RenderScene";
            //palangreta.Technique = "RenderScene";
            palangreta.rotateY(Geometry.DegreeToRadian(75f));
            elementos.Add(new Elemento(10000, 10000, palangreta/*, TgcShaders.loadEffect(recursos + "Shaders\\BoteShader.fx")*/));
            elementos.Add(new Elemento(10000, 10000, canoa/*, TgcShaders.loadEffect(recursos + "Shaders\\BoteShader.fx")*/));
            #endregion

            #region Creamos las algas limitrofes del mapa
            //Creamos estos elementos para que haya más cantidad de objetos y poder probar el algoritmo de optimizacion
            int limiteX = (int)esquina.X;
            int limiteZ = (int)esquina.Z;
            for (int i = -limiteZ; i < limiteZ; i += 200)
            {
                algaNueva = alga.createMeshInstance(alga.Name + elementos.Count.ToString());
                algaNueva.Position = new Vector3(limiteX, terreno.CalcularAltura(limiteX, i), i);
                elementos.Add(new ElementoSinInteraccion(1000, 1400, algaNueva));
                algaNueva = alga.createMeshInstance(alga.Name + elementos.Count.ToString());
                algaNueva.Position = new Vector3(-limiteX, terreno.CalcularAltura(-limiteX, i), i);
                elementos.Add(new ElementoSinInteraccion(1000, 1400, algaNueva));
            }
            for (int i = -limiteX; i < limiteX; i += 200)
            {
                algaNueva = alga.createMeshInstance(alga.Name + elementos.Count.ToString());
                algaNueva.Position = new Vector3(i, terreno.CalcularAltura(i, limiteZ), limiteZ);
                elementos.Add(new ElementoSinInteraccion(1000, 1400, algaNueva));
                algaNueva = alga.createMeshInstance(alga.Name + elementos.Count.ToString());
                algaNueva.Position = new Vector3(i, terreno.CalcularAltura(i, -limiteZ), -limiteZ);
                elementos.Add(new ElementoSinInteraccion(1000, 1400, algaNueva));
            }
            #endregion

            #region Creamos los arboles de frutillas
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\ArbolFrutilla\\ArbolFrutilla-TgcScene.xml");
            TgcMesh arbolFrutilla = scene.Meshes[0];
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\ArbolFrutillaVacio\\ArbolFrutillaVacio-TgcScene.xml");
            TgcMesh arbolFrutillaVacio = scene.Meshes[0];
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Alimentos\\Frutas\\Frutilla\\Frutilla-TgcScene.xml");
            TgcMesh frutilla = scene.Meshes[0];
            TgcMesh arbolFrutillaMesh;
            TgcMesh arbolFrutillaVacioMesh;
            TgcMesh frutillaMesh;
            ElementoDoble nuevoArbolFrutillaCompleto;

            float[] posicionArbolFrutillaX = { 3000f, 4000f, 5000f };
            float[] posicionArbolFrutillaZ = { -2000f, 0f, 1000f, 2000f};
            for (int i = 0; i < posicionArbolFrutillaX.Length; i++)
            {
                for (int j = 0; j < posicionArbolFrutillaZ.Length; j++)
                {
                    arbolFrutillaMesh = arbolFrutilla.createMeshInstance(arbolFrutilla.Name + i + j);
                    arbolFrutillaVacioMesh = arbolFrutillaVacio.createMeshInstance(arbolFrutillaVacio.Name + i + j);
                    float x = posicionArbolFrutillaX[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    float z = posicionArbolFrutillaZ[j] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    arbolFrutillaMesh.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                    arbolFrutillaVacioMesh.Position = arbolFrutillaMesh.Position;
                    nuevoArbolFrutillaCompleto = new ElementoDoble(1000, 2330, arbolFrutillaMesh, arbolFrutillaVacioMesh);
                    for (int k = 0; k < 5; k++)
                    {
                        frutillaMesh = frutilla.createMeshInstance(frutilla.Name + i + j + k);
                        frutillaMesh.Position = arbolFrutillaMesh.Position;
                        frutillaMesh.Scale = new Vector3(0.1f, 0.1f, 0.1f);
                        nuevoArbolFrutillaCompleto.agregarElemento(new Alimento(1000, 2330, frutillaMesh, 10));
                    }
                    elementos.Add(nuevoArbolFrutillaCompleto);
                }
            }
            #endregion

            #region  Crear piso
            TgcTexture pisoTexture = TgcTexture.createTexture(d3dDevice, recursos + "Texturas\\Agua.jpg");
            piso = TgcBox.fromExtremes(new Vector3(-20000, 3, -20000), new Vector3(20000, 15, 20000), pisoTexture);
            piso.Effect = TgcShaders.loadEffect(recursos + "Shaders\\AguaShader.fx");
            piso.Technique = "RenderScene";
            #endregion

            //Creamos el personaje
            //Cargar personaje con animaciones
            personaje = new Personaje();
            personaje.VelocidadCaminar = 1500f;
            personaje.VelocidadRotacion = 50f;
            personaje.Fuerza = 1f;
            personaje.ResistenciaFisica = 30f;
            personaje.Hidratacion = 100f;
            personaje.Alimentacion = 100f;

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
            personaje.mesh.Scale = new Vector3(1f, 1f, 1f);
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

            //Una vez configurado el mesh del personaje iniciamos su bounding esfera y su esfera de alcance de interacción con los elementos
            personaje.IniciarBoundingEsfera();
            personaje.IniciarAlcanceInteraccionEsfera();

            tiempo = 0;
            controladorEntradas = new ControladorEntradas();
            camara = new CamaraPrimeraPersona(GuiController.Instance.Frustum, GuiController.Instance.D3dDevice);//Por defecto usamos la camara en primera persona

            //****************Crear Sprite de la mochila y del cajon**********************************************
            Size screenSize = GuiController.Instance.Panel3d.Size;

            mochila = new TgcSprite();
            mochilaReglon1 = new TgcText2d();
            mochila.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Mochila.jpg");
            Size textureSize = mochila.Texture.Size;
            if (screenSize.Width >= 1024)
            {
                mochila.Position = new Vector2(20, (screenSize.Height - textureSize.Height / 2) / 2);
                mochila.Scaling = new Vector2(0.5f, 0.5f);
                mochilaReglon1.changeFont(new System.Drawing.Font("TimesNewRoman", 30, FontStyle.Bold));
            }
            else
            {
                mochila.Position = new Vector2(20, (screenSize.Height - (textureSize.Height * 2 / 5)) / 2);
                mochila.Scaling = new Vector2(0.4f, 0.4f);
                mochilaReglon1.changeFont(new System.Drawing.Font("TimesNewRoman", 20, FontStyle.Bold));
            }
            mochilaReglon1.Position = new Point((int)(mochila.Position.X) + 11,
                (int)(mochila.Position.Y) + 95);
            mochilaReglon1.Color = Color.Black;
            mochilaReglon1.Text = "";
            mochilaReglon1.Align = TgcText2d.TextAlign.LEFT;
            mochilaReglon1.Size = new Size(textureSize.Width, textureSize.Height);

            cajon = new TgcSprite();
            cajonReglon1 = new TgcText2d();
            cajon.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Cajon.jpg");
            Size textureCacjonSize = cajon.Texture.Size;
            if (screenSize.Width >= 1024)
            {
                cajon.Position = new Vector2(screenSize.Width - 20 - (textureCacjonSize.Width / 2), (screenSize.Height - textureCacjonSize.Height / 2) / 2);
                cajon.Scaling = new Vector2(0.5f, 0.5f);
                cajonReglon1.changeFont(new System.Drawing.Font("TimesNewRoman", 30, FontStyle.Bold));
            }
            else
            {
                cajon.Position = new Vector2(screenSize.Width - 20 - ((textureCacjonSize.Width * 2) / 5), 
                    (screenSize.Height - textureCacjonSize.Height * 2 / 5) / 2);
                cajon.Scaling = new Vector2(0.4f, 0.4f);
                cajonReglon1.changeFont(new System.Drawing.Font("TimesNewRoman", 20, FontStyle.Bold));
            }
            cajonReglon1.Color = Color.White;
            cajonReglon1.Text = "";
            cajonReglon1.Align = TgcText2d.TextAlign.LEFT;
            cajonReglon1.Position = new Point((int)(cajon.Position.X) + 11, (int)(cajon.Position.Y) + 85);
            cajonReglon1.Size = new Size(textureSize.Width, textureSize.Height);

            //****************Crear Sprite de la mochila y del cajon**********************************************

            //****************Crear el texto informativo**********************************************************
            informativo = new TgcText2d();
            informativo.Color = Color.Yellow;
            informativo.Align = TgcText2d.TextAlign.CENTER;
            informativo.Size = new Size(screenSize.Width, 50);
            if (screenSize.Width >= 1024)
            {
                informativo.changeFont(new System.Drawing.Font("TimesNewRoman", 40, FontStyle.Bold));
                informativo.Position = new Point(10, screenSize.Height - 75);
            }
            else
            {
                informativo.changeFont(new System.Drawing.Font("TimesNewRoman", 20, FontStyle.Bold));
                informativo.Position = new Point(10, screenSize.Height - 50);
            }
            //****************Crear el texto informativo**********************************************************

            #region Creacion de barra de salud, hidratacion, alimentacion, cansancio y temperatura corporal del persobaje
            saludIcono = new TgcSprite();
            saludIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Corazon.png");
            saludIcono.Position = new Vector2(20, 20);

            salud = new TgcSprite();
            salud.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\BarraSalud.png");
            salud.Position = new Vector2(100, 40);

            hidratacionIcono = new TgcSprite();
            hidratacionIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Agua.png");
            hidratacionIcono.Position = new Vector2(250, 20);

            hidratacion = new TgcSprite();
            hidratacion.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\BarraHidratacion.png");
            hidratacion.Position = new Vector2(330, 40);

            alimentacionIcono = new TgcSprite();
            alimentacionIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Hamburguesa.png");
            alimentacionIcono.Position = new Vector2(480, 20);

            alimentacion = new TgcSprite();
            alimentacion.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\BarraAlimentacion.png");
            alimentacion.Position = new Vector2(560, 40);

            cansancioIcono = new TgcSprite();
            cansancioIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Cansancio.png");
            cansancioIcono.Position = new Vector2(710, 20);

            cansancio = new TgcSprite();
            cansancio.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\BarraCansancio.png");
            cansancio.Position = new Vector2(790, 40);

            temperaturaPersonajeIcono = new TgcSprite();
            temperaturaPersonajeIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\TemperaturaPersonaje.png");
            temperaturaPersonajeIcono.Position = new Vector2(20, 90);

            temperaturaPersonaje = new TgcText2d();
            temperaturaPersonaje.Color = Color.DarkViolet;
            temperaturaPersonaje.Align = TgcText2d.TextAlign.LEFT;
            temperaturaPersonaje.Position = new Point(100, 100);
            temperaturaPersonaje.Size = new Size(100, 50);
            temperaturaPersonaje.changeFont(new System.Drawing.Font("TimesNewRoman", 30, FontStyle.Bold));
            #endregion

            //****************************Creacion de objetivos*********************
            objetivosIcono = new TgcSprite();
            mensajeObjetivo1 = new TgcText2d();
            objetivosIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Objetivos.png");
            if (screenSize.Width >= 1024)
            {
                objetivosIcono.Position = new Vector2(screenSize.Width - 300, 25);
                mensajeObjetivo1.Position = new Point(screenSize.Width - 200, 25);
            }
            else
            {
                objetivosIcono.Position = new Vector2(20, 200);
                mensajeObjetivo1.Position = new Point(100, 200);
            }
            mensajeObjetivo1.Color = Color.Red;
            mensajeObjetivo1.Align = TgcText2d.TextAlign.LEFT;
            mensajeObjetivo1.Size = new Size(200, 50);
            mensajeObjetivo1.changeFont(new System.Drawing.Font("TimesNewRoman", 20, FontStyle.Bold));
            tiempoObjetivo = 1200;//Segundos que forman 20 minutos
                                  //****************************Creacion de objetivos*********************

            //*****************Creación de texto informativo********************************
            //Texto de Game Over e informativo
            estadoJuego = new TgcText2d();
            estadoJuego.Align = TgcText2d.TextAlign.CENTER;
            estadoJuego.Position = new Point(5, screenSize.Height / 2);
            estadoJuego.Size = new Size(screenSize.Width, 50);
            estadoJuego.changeFont(new System.Drawing.Font("TimesNewRoman", 60, FontStyle.Bold));
            //*****************Creación de texto informativo********************************

            //*****************Creación de menu ayuda********************************************************
            ayuda = new TgcSprite();
            ayuda.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\ayuda.jpg");

            //Ubicarlo centrado en la pantalla
            Size ayudaTextureSize = ayuda.Texture.Size;
            ayudaReglon1 = new TgcText2d();
            ayudaReglon2 = new TgcText2d();
            if (screenSize.Width >= 1024)
            {
                //Sabemos que la textura tiene 1024
                ayuda.Position = new Vector2((screenSize.Width - ayudaTextureSize.Width) / 2, 
                    (screenSize.Height - ayudaTextureSize.Height / 2) / 2);
                ayuda.Scaling = new Vector2(1f, 0.6f);
                ayudaReglon1.changeFont(new System.Drawing.Font("TimesNewRoman", 11, FontStyle.Bold));
                ayudaReglon2.changeFont(new System.Drawing.Font("TimesNewRoman", 12, FontStyle.Bold));
                ayudaReglon2.Position = new Point(((int)(ayuda.Position.X)) + 840, ((int)(ayuda.Position.Y)) + 580);
            }
            else
            {
                //Sabemos que la textura tiene 1024
                ayuda.Position = new Vector2((ayudaTextureSize.Width - screenSize.Width) / 2,
                    ((ayudaTextureSize.Height / 2) - screenSize.Height) / 2);
                ayuda.Scaling = new Vector2(0.9f, 0.5f);
                ayudaReglon1.changeFont(new System.Drawing.Font("TimesNewRoman", 8, FontStyle.Bold));
                ayudaReglon2.changeFont(new System.Drawing.Font("TimesNewRoman", 8, FontStyle.Bold));
                ayudaReglon2.Position = new Point(((int)(ayuda.Position.X)) + 800, ((int)(ayuda.Position.Y)) + 480);
            }
            ayudaReglon1.Color = Color.Black;
            ayudaReglon1.Text = "";
            ayudaReglon1.Align = TgcText2d.TextAlign.LEFT;
            ayudaReglon1.Position = new Point(0, 0);
            ayudaReglon1.Size = new Size(ayudaTextureSize.Width, ayudaTextureSize.Height);
            ayudaReglon2.Color = Color.DarkViolet;
            ayudaReglon2.Text = "Versión: " + this.Version();
            ayudaReglon2.Align = TgcText2d.TextAlign.LEFT;
            ayudaReglon2.Size = new Size(200, 50);
            //*****************Creación de menu ayuda********************************************************

            //*****************Creación de texto informativo de la temperatura y la hora del dia********************************
            temperaturaDiaIcono = new TgcSprite();
            temperaturaDiaIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\TemperaturaDia.png");
            temperaturaDiaIcono.Position = new Vector2(screenSize.Width - 85, screenSize.Height - 200);
            temperaturaDia = new TgcText2d();
            temperaturaDia.Color = Color.White;
            temperaturaDia.Align = TgcText2d.TextAlign.RIGHT;
            temperaturaDia.Position = new Point(screenSize.Width - 205, screenSize.Height -180);
            temperaturaDia.Size = new Size(100, 100);
            temperaturaDia.changeFont(new System.Drawing.Font("TimesNewRoman", 20, FontStyle.Bold));
            horaDiaIcono = new TgcSprite();
            horaDiaIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\HoraDelDia.png");
            horaDiaIcono.Position = new Vector2(screenSize.Width - 100, screenSize.Height - 100);
            horaDia = new TgcText2d();
            horaDia.Color = Color.White;
            horaDia.Align = TgcText2d.TextAlign.RIGHT;
            horaDia.Position = new Point(screenSize.Width - 210, screenSize.Height - 85);
            horaDia.Size = new Size(100, 100);
            horaDia.changeFont(new System.Drawing.Font("TimesNewRoman", 20, FontStyle.Bold));
            estadoDiaSolIcono = new TgcSprite();
            estadoDiaSolIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Sol.png");
            estadoDiaSolIcono.Position = new Vector2(screenSize.Width - 100, screenSize.Height - 300);
            estadoDiaLunaIcono = new TgcSprite();
            estadoDiaLunaIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Luna.png");
            estadoDiaLunaIcono.Position = new Vector2(screenSize.Width - 100, screenSize.Height - 300);
            //*****************Creación de texto informativo de la temperatura y la hora del dia********************************

            //¿de donde viene ese 15? bueno, si tiene que andar como mínimo a 30 fps, creo que actualizar los objetos de colision cada 0.5 segundos es razonable.
            optimizador = new Optimizador(elementos, 15, 500);

            #region Iluminacion
            TgcBox lightMesh = TgcBox.fromSize(new Vector3(500, 500, 500), Color.Yellow);
            Sol sol = new Sol();
            sol.Mesh = lightMesh.toMesh("SOL");
            sol.CrearMovimiento();
            dia = new Dia(200, sol);
            #endregion


            float aspectRatio = (float)GuiController.Instance.Panel3d.Width / GuiController.Instance.Panel3d.Height;
            float zNearPlaneDistance = 1f;
            float zFarPlaneDistance = 20000f;
            d3dDevice.Transform.Projection =
                Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(45.0f), aspectRatio, zNearPlaneDistance, zFarPlaneDistance);
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

            camara.Render(personaje);

            optimizador.Actualizar(personaje.mesh.Position);

            GuiController.Instance.UserVars.setValue("x", elementos.Count);
            GuiController.Instance.UserVars.setValue("y", optimizador.ElementosColision.Count);
            GuiController.Instance.UserVars.setValue("z", optimizador.ElementosRenderizacion.Count);

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
                puebaFisica.renderizar();
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
            terreno.GetEfecto().SetValue("time", tiempo);
            terreno.GetEfecto().SetValue("lightIntensity", dia.GetSol().IntensidadRelativa());
            terreno.executeRender(terreno.GetEfecto());

            //Render piso
            piso.Effect.SetValue("time", tiempo);
            piso.Effect.SetValue("lightIntensity", dia.GetSol().IntensidadRelativa());
            piso.render();

            //Renderizar SkyBox
            // if (dia.EsDeDia())
            // {
            foreach (TgcMesh faces in this.skyBox.Faces)
            {
                faces.Effect.SetValue("time", tiempo);
                faces.Effect.SetValue("lightIntensity", dia.GetSol().IntensidadRelativa());
                faces.render();
            }
            //skyBox.render();
           // }
         //   else
          //  {
          //      skyBoxNoche.render();
         //   }

            //Actualiza los elementos
            List<Elemento> aux = new List<Elemento>();
            aux.AddRange(elementos);//TODO. Porque sino con la actualziacion borramos elementos de la coleccion de elementos y se rompe todo
            foreach (Elemento elemento in aux)
            {
                elemento.Actualizar(this, elapsedTime);
            }

            foreach (Elemento elem in optimizador.ElementosRenderizacion)
            {
                elem.renderizar();
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
            GuiController.Instance.Drawer2D.endDrawSprite();
            mensajeObjetivo1.Text = "Sobrevivir " + System.Environment.NewLine + TimeSpan.FromSeconds(this.tiempoObjetivo - this.tiempo).ToString(@"hh\:mm\:ss");
            mensajeObjetivo1.render();

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
                     mochilaReglon1.Text = mochilaReglon1.Text + (i+1).ToString() + "    " + personaje.DarElementoEnPosicionDeMochila(i).GetDescripcion() + System.Environment.NewLine;
                    }
                    else
                    {
                        mochilaReglon1.Text = mochilaReglon1.Text + (i + 1).ToString() + "    "  + "Disponible" + System.Environment.NewLine;
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
                ayudaReglon1.Position = new Point(((int)(ayuda.Position.X)) + 40 , ((int)(ayuda.Position.Y)) + 130);
                ayudaReglon1.render();
                ayudaReglon2.render();
            }

            //Actualziamos el dia
            dia.Actualizar(this, elapsedTime);

            GuiController.Instance.Drawer2D.beginDrawSprite();
            horaDia.Text = dia.HoraActualTexto();
            temperaturaDia.Text = dia.TemperaturaActualTexto();
            temperaturaPersonaje.Text = personaje.TemperaturaCorporalTexto();
            horaDia.render();
            temperaturaDia.render();
            temperaturaPersonajeIcono.render();
            temperaturaPersonaje.render();
            temperaturaDiaIcono.render();
            horaDiaIcono.render();
            if (dia.EsDeDia())
            {
                estadoDiaSolIcono.render();
            }
            else
            {
                estadoDiaLunaIcono.render();
            }
            GuiController.Instance.Drawer2D.endDrawSprite();

            #region Render de Luces
            Microsoft.DirectX.Direct3D.Effect currentShader;
            currentShader = GuiController.Instance.Shaders.TgcMeshPointLightShader;

            //Aplicar a cada mesh el shader actual
            foreach (Elemento elem in optimizador.ElementosRenderizacion)
            {
                elem.Mesh.Effect = currentShader;
                //El Technique depende del tipo RenderType del mesh
                elem.Mesh.Technique = GuiController.Instance.Shaders.getTgcMeshTechnique(elem.Mesh.RenderType);
            }

            //Renderizar meshes
            foreach (Elemento elem in optimizador.ElementosRenderizacion)
            {
                dia.GetSol().Iluminar(elem, personaje);
                elem.renderizar();
            }

            dia.GetSol().Mesh.render();

            #endregion
            
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
        }

        private String Version()
        {
            return "0.01.001";
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
    }
}
