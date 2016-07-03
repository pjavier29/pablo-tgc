using AlumnoEjemplos.PabloTGC.ElementosDia;
using AlumnoEjemplos.PabloTGC.ElementosJuego;
using AlumnoEjemplos.PabloTGC.ElementosJuego.Instrumentos;
using AlumnoEjemplos.PabloTGC.Utiles;
using AlumnoEjemplos.PabloTGC.Utiles.Camaras;
using AlumnoEjemplos.PabloTGC.Utiles.Efectos;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.Shaders;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.PabloTGC.Administracion
{
    public class ConfiguracionModel
    {
        #region Atributos
        private SuvirvalCraft contexto;
        private Microsoft.DirectX.Direct3D.Device d3dDevice;
        private string recursos;
        private TgcSceneLoader loader;
        private TgcScene scene;
        #endregion

        #region Constructores
        public ConfiguracionModel(SuvirvalCraft contexto)
        {
            this.contexto = contexto;
        }
        #endregion

        #region Comportamientos

        public void IniciarCreacion(int tiempoObjetivo, bool ejecutarPantallaCompleta)
        { 
            //Device de DirectX para crear primitivas
            d3dDevice = GuiController.Instance.D3dDevice;

            //Carpeta de acceso a los recursos
            recursos = GuiController.Instance.AlumnoEjemplosDir + "PabloTGC\\Recursos\\";

            //Crear loader
            loader = new TgcSceneLoader();

            //***********Inicializamos las esquinas************************************
            //Crear objeto propio para manejar los limites.
            contexto.esquina = new Vector3(10000, 0, 10000);
            //***********Inicializamos las esquinas************************************

            //Iniciamos el tiempo
            contexto.tiempo = 0;

            //Iniciamos los elementos
            contexto.elementos = new List<Elemento>();

            contexto.controladorEntradas = new ControladorEntradas();
            //contexto.camara = new CamaraPrimeraPersona(GuiController.Instance.Frustum, GuiController.Instance.D3dDevice);//Por defecto usamos la camara en primera persona

            //¿de donde viene ese 15? bueno, si tiene que andar como mínimo a 30 fps, creo que actualizar los objetos de colision cada 0.5 segundos es razonable.
            contexto.optimizador = new Optimizador(contexto.elementos, 15, 500);

            float aspectRatio = (float)GuiController.Instance.Panel3d.Width / GuiController.Instance.Panel3d.Height;
            float zNearPlaneDistance = 1f;
            float zFarPlaneDistance = 20000f;
            d3dDevice.Transform.Projection =
                Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(45.0f), aspectRatio, zNearPlaneDistance, zFarPlaneDistance);

            contexto.tiempoObjetivo = tiempoObjetivo;

            GuiController.Instance.FullScreenEnable = ejecutarPantallaCompleta;
        }

        public void AdministracionDeEfectos()
        {
            contexto.efectoTerreno = new EfectoTerreno(TgcShaders.loadEffect(recursos + "Shaders\\TerrenoShader.fx"), "RenderScene");
            contexto.pisoEfecto = new EfectoAgua(TgcShaders.loadEffect(recursos + "Shaders\\AguaShader.fx"), "RenderScene");
            contexto.skyboxEfecto = new EfectoSkyBox(TgcShaders.loadEffect(recursos + "Shaders\\SkyBoxShader.fx"), "RenderScene");
            //Cargar Shader personalizado para el efecto del fuego
            contexto.efectoFuego = new EfectoFuego(TgcShaders.loadEffect(recursos + "Shaders\\FuegoShader.fx"), "RenderScene");
            //Cargar Shader personalizado para el efecto de las algas
            contexto.efectoAlgas = new EfectoAlga(TgcShaders.loadEffect(recursos + "Shaders\\AlgaShader.fx"), "RenderScene");
            contexto.efectoAlgas2 = new EfectoAlga(TgcShaders.loadEffect(recursos + "Shaders\\AlgaShader.fx"), "RenderScene2");
            contexto.efectoArbol = new EfectoArbol(TgcShaders.loadEffect(recursos + "Shaders\\ArbolShader.fx"), "RenderScene");
            contexto.efectoArbol2 = new EfectoArbol(TgcShaders.loadEffect(recursos + "Shaders\\ArbolShader.fx"), "RenderScene2");
            contexto.efectoBotes = new EfectoBote(TgcShaders.loadEffect(recursos + "Shaders\\BoteShader.fx"), "RenderScene");
            contexto.efectoLuz = new EfectoLuz(GuiController.Instance.Shaders.TgcMeshPointLightShader);
        }

        public void CrearHeimap()
        {
            Terreno terreno = new Terreno();
            terreno.loadHeightmap(recursos
                    + "Shaders\\WorkshopShaders\\Heighmaps\\" + "HeightmapHawaii.jpg", 400f, 3f, new Vector3(0, 0, 0));
            terreno.loadTexture(recursos
                    + "Shaders\\WorkshopShaders\\Heighmaps\\" + "TerrainTextureHawaii.jpg");
            terreno.SetEfecto(contexto.efectoTerreno);

            contexto.terreno = terreno;
        }

        public void CrearIluminacion(float velocidadTiempo, float relojInterno, float lapsoPrecipitaciones)
        {
            TgcBox lightMesh = TgcBox.fromSize(new Vector3(500, 500, 500), Color.Yellow);
            Sol sol = new Sol();
            sol.Mesh = lightMesh.toMesh("SOL");
            sol.CrearMovimiento();
            contexto.dia = new Dia(velocidadTiempo, sol, relojInterno, new Lluvia(1, lapsoPrecipitaciones));
        }

        public void CrearSkyBox()
        {
            TgcSkyBox skyBox = new TgcSkyBox();
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
            contexto.skyBox = skyBox;
            foreach (TgcMesh faces in skyBox.Faces)
            {
                contexto.skyboxEfecto.Aplicar(faces, contexto);
            }
        }

        public void CrearPalmerasComunes()
        {
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
                    palmeraNueva.Position = new Vector3(x, contexto.terreno.CalcularAltura(x, z), z);

                    Elemento elemNuevo;
                    if (FuncionesMatematicas.Instance.NumeroAleatorioDouble() <= 0.5)
                    {
                        elemNuevo = new Elemento(1000, 1400, palmeraNueva, contexto.efectoArbol);
                    }
                    else
                    {
                        elemNuevo = new Elemento(1000, 1400, palmeraNueva, contexto.efectoArbol2);
                    }
                    elemNuevo.Flexibilidad = FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(0, 0.15F);
                    contexto.elementos.Add(elemNuevo);
                }
            }
        }

        public void CrearArbolesBanana()
        {
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
                    arbolBananaNuevo.Position = new Vector3(x, contexto.terreno.CalcularAltura(x, z), z);
                    bananaMeshNueva = bananaMesh.createMeshInstance("Banana");
                    bananaMeshNueva.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                    Elemento elemNuevo;
                    if (FuncionesMatematicas.Instance.NumeroAleatorioDouble() <= 0.5)
                    {
                        elemNuevo = new Elemento(1000, 1300, arbolBananaNuevo, new Alimento(1000, 1000, bananaMeshNueva, 20, contexto.efectoLuz), contexto.efectoArbol);
                    }
                    else
                    {
                        elemNuevo = new Elemento(1000, 1300, arbolBananaNuevo, new Alimento(1000, 1000, bananaMeshNueva, 20, contexto.efectoLuz), contexto.efectoArbol2);
                    }
                    elemNuevo.Flexibilidad = FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(0, 0.15F);
                    contexto.elementos.Add(elemNuevo);
                }
            }
        }

        public void CrearArbolesDeLenia()
        {
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
            float[] posicionPinoX = { -8000f, -7500f, -7000f, -6500f, -6000f };
            float[] posicionPinoZ = { -4000f, -3500f, -3000f, -2500f, -2000f, -1500f, -1000f, -500f, 0f };
            for (int i = 0; i < posicionPinoX.Length; i++)
            {
                for (int j = 0; j < posicionPinoZ.Length; j++)
                {
                    pinoNuevo = pino.createMeshInstance(pino.Name + i + j);
                    pinoNuevo.Scale = new Vector3(1.5f, 3f, 1.5f);
                    float x = posicionPinoX[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    float z = posicionPinoZ[j] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    pinoNuevo.Position = new Vector3(x, contexto.terreno.CalcularAltura(x, z), z);
                    fuegoMesh = fuego.createMeshInstance(fuego.Name + i + j);
                    leniaMesh = lenia.createMeshInstance(lenia.Name + i + j);
                    fuegoMesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                    leniaMesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                    Elemento elementoNuevo;
                    if (FuncionesMatematicas.Instance.NumeroAleatorioDouble() <= 0.5)
                    {
                        elementoNuevo = new Elemento(1000, 2330, pinoNuevo,
                                (new Madera(1000, 233, leniaMesh,
                                    new Fuego(1000, 233, fuegoMesh, contexto.efectoFuego), contexto.efectoLuz)), contexto.efectoArbol);
                    }
                    else
                    {
                        elementoNuevo = new Elemento(1000, 2330, pinoNuevo,
                                (new Madera(1000, 233, leniaMesh,
                                    new Fuego(1000, 233, fuegoMesh, contexto.efectoFuego), contexto.efectoLuz)), contexto.efectoArbol2);
                    }
                    elementoNuevo.Flexibilidad = FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(0, 0.08F);
                    contexto.elementos.Add(elementoNuevo);
                }
            }
        }

        public void CrearPiedraParaTirar()
        {
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Roca\\Roca-TgcScene.xml");
            TgcMesh piedraMesh = scene.Meshes[0].createMeshInstance("Piedra");
            contexto.puebaFisica = new Elemento(100, 300, piedraMesh);
        }

        public void CrearOvejaYGallo()
        {
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Oveja\\Ovelha-TgcScene.xml");
            TgcMesh ovejaMesh = scene.Meshes[0].createMeshInstance("Oveja");
            scene = loader.loadSceneFromFile(recursos
                 + "MeshCreator\\Meshes\\Alimentos\\CarneCruda\\carnecruda-TgcScene.xml");
            TgcMesh carneCruda = scene.Meshes[0];
            Animal oveja = new Animal(5000, 20, ovejaMesh, contexto.efectoLuz);
            ovejaMesh.Position = new Vector3(200, contexto.terreno.CalcularAltura(200, 200), 200);
            scene = loader.loadSceneFromFile(recursos
                 + "MeshCreator\\Meshes\\Alimentos\\Hamburguesa\\Hamburguesa-TgcScene.xml");
            TgcMesh hamburguesa = scene.Meshes[0];
            Alimento alimento = new Alimento(1000, 1000, carneCruda.createMeshInstance("Carne Cruda"), 10, contexto.efectoLuz);
            alimento.posicion(ovejaMesh.Position);
            TgcMesh hamburguesaOveja = hamburguesa.createMeshInstance("Hambur Oveja");
            hamburguesaOveja.Scale = new Vector3(0.3f, 0.3f, 0.3f);
            alimento.agregarElemento(new Alimento(1000, 1000, hamburguesaOveja, 70, contexto.efectoLuz));
            alimento.BoundingBox().scaleTranslate(alimento.posicion(), new Vector3(2f, 2f, 2f));
            oveja.agregarElemento(alimento);
            contexto.elementos.Add(oveja);
            contexto.oveja = oveja;

            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Gallo\\Gallo-TgcScene.xml");
            TgcMesh galloMesh = scene.Meshes[0].createMeshInstance("Gallo");
            Animal gallo = new Animal(5000, 20, galloMesh, contexto.efectoLuz);
            galloMesh.Position = new Vector3(0, contexto.terreno.CalcularAltura(0, 0), 0);
            alimento = new Alimento(1000, 1000, carneCruda.createMeshInstance("Carne Cruda"), 10, contexto.efectoLuz);
            alimento.posicion(galloMesh.Position);
            TgcMesh hamburguesaGallo = hamburguesa.createMeshInstance("Hambur Gallo");
            hamburguesaGallo.Scale = new Vector3(0.3f, 0.3f, 0.3f);
            alimento.agregarElemento(new Alimento(1000, 1000, hamburguesaGallo, 70, contexto.efectoLuz));
            alimento.BoundingBox().scaleTranslate(alimento.posicion(), new Vector3(2f, 2f, 2f));
            gallo.agregarElemento(alimento);
            contexto.gallo = gallo;
            contexto.elementos.Add(gallo);
        }

        public void CreamosLosCajones()
        {
            #region Creamos el cajon con las manzanas
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Cajon\\cajon-TgcScene.xml");
            TgcMesh cajonRealMesh = scene.Meshes[0].createMeshInstance("Cajon_Manzanas");
            Cajon cajonReal = new Cajon(5000, 1000, cajonRealMesh, contexto.efectoLuz);
            cajonRealMesh.Position = new Vector3(600, contexto.terreno.CalcularAltura(600, 7200), 7200);
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Alimentos\\Frutas\\ManzanaVerde\\manzanaverde-TgcScene.xml");
            TgcMesh manzanaVerde = scene.Meshes[0].createMeshInstance("Manzana Verde");
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Alimentos\\Frutas\\ManzanaRoja\\manzanaroja-TgcScene.xml");
            TgcMesh manzanaRoja = scene.Meshes[0].createMeshInstance("Manzana Roja");
            manzanaVerde.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            cajonReal.agregarElemento(new Alimento(1000, 1000, manzanaVerde, 30, contexto.efectoLuz));
            manzanaRoja.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            cajonReal.agregarElemento(new Alimento(1000, 1000, manzanaRoja, 30, contexto.efectoLuz));
            contexto.elementos.Add(cajonReal);
            contexto.cajonReal = cajonReal;
            #endregion

            #region Creamos el cajon con la olla y con la copa
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Cajon\\cajon-TgcScene.xml");
            TgcMesh cajonOllaMesh = scene.Meshes[0].createMeshInstance("Cajon_Olla");
            Cajon cajonOlla = new Cajon(5000, 1000, cajonOllaMesh, contexto.efectoLuz);
            cajonOllaMesh.Position = new Vector3(-9500, contexto.terreno.CalcularAltura(-9500, -4950), -4950);
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Olla\\Olla-TgcScene.xml");
            TgcMesh ollaMesh = scene.Meshes[0].createMeshInstance("Olla");
            ollaMesh.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\CopaMadera\\CopaMadera-TgcScene.xml");
            TgcMesh copaMesh = scene.Meshes[0].createMeshInstance("Copa");
            copaMesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
            cajonOlla.agregarElemento(new Olla(1000, 1000, ollaMesh, contexto.efectoLuz));
            cajonOlla.agregarElemento(new Copa(1000, 1000, copaMesh, contexto.efectoLuz));
            contexto.elementos.Add(cajonOlla);
            contexto.cajonOlla = cajonOlla;
            #endregion
        }

        public void CrearArbolesGenerales()
        {
            scene = loader.loadSceneFromFile(recursos
            + "MeshCreator\\Meshes\\Vegetacion\\Arbol\\Arbol-TgcScene.xml");
            TgcMesh arbol = scene.Meshes[0];
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\ArbolAmarillo\\arbolamarillo-TgcScene.xml");
            TgcMesh arbolAmarillo = scene.Meshes[0];
            float[] posicionesArbolesX = { -3000f, -2500f, -2000f, -1500f, -1000f, -500f, 00f, 500f, 1000f, 1500f, 2000f, 2500f, 3000f, 3500f,
                4000f, 4500f, 5000f,5500f, 6000f, 6500f, 7000f, 7500f, 8000f };
            float[] posicionesArbolesZ = { -5000f, -5000f, -6000f, -6500f, -7000f, -7500f, -8000f };
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
                    arbolNuevo.Position = new Vector3(x, contexto.terreno.CalcularAltura(x, z), z);
                    Elemento elemNuevo;
                    if (FuncionesMatematicas.Instance.NumeroAleatorioDouble() <= 0.5)
                    {
                        elemNuevo = new Elemento(1000, 1400, arbolNuevo, contexto.efectoArbol);
                    }
                    else
                    {
                        elemNuevo = new Elemento(1000, 1400, arbolNuevo, contexto.efectoArbol2);
                    }
                    elemNuevo.Flexibilidad = FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(0, 0.07F);
                    contexto.elementos.Add(elemNuevo);
                }
            }
        }

        public void CrearFuenteAgua()
        {
            scene = loader.loadSceneFromFile(recursos
             + "MeshCreator\\Meshes\\FuenteAgua\\FuenteAgua-TgcScene.xml");
            TgcMesh fuente = scene.Meshes[0].createMeshInstance("Fuente de Agua");
            fuente.Scale = new Vector3(1.5f, 2.5f, 1.5f);
            fuente.Position = new Vector3(2400, contexto.terreno.CalcularAltura(2400, -3160), -3160);
            FuenteAgua fuenteAgua = new FuenteAgua(1000, 1400, fuente, contexto.efectoLuz);
            contexto.elementos.Add(fuenteAgua);
            contexto.fuenteAgua = fuenteAgua;
        }

        public void CrearAlgas()
        {
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Vegetacion\\Alga\\alga-TgcScene.xml");
            TgcMesh alga = scene.Meshes[0];
            TgcMesh algaNueva;
            float[] posicionesAlgasX = { -9500f, -9000f, -8500f, -8000f, -7500f, -7000f };
            float[] posicionesAlgasZ = { 9500f, 9000f, 8500f, 8000f, 7500f, 7000f, 6500f, 6000f, 5500f, 5000f, 4500f };
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    algaNueva = alga.createMeshInstance(alga.Name + contexto.elementos.Count.ToString());
                    algaNueva.Scale = new Vector3(0.5f, 0.7f, 0.5f);
                    float x = (posicionesAlgasX[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(j * 10f, (j * 50f + 100)));
                    float z = (FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-9500, -8000));
                    algaNueva.Position = new Vector3(x, contexto.terreno.CalcularAltura(x, z), z);
                    if (FuncionesMatematicas.Instance.NumeroAleatorioFloat() > 0.5f)
                    {
                        contexto.elementos.Add(new ElementoSinInteraccion(1000, 1400, algaNueva, contexto.efectoAlgas));
                    }
                    else
                    {
                        contexto.elementos.Add(new ElementoSinInteraccion(1000, 1400, algaNueva, contexto.efectoAlgas2));
                    }
                }
            }
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    algaNueva = alga.createMeshInstance(alga.Name + contexto.elementos.Count.ToString());
                    algaNueva.Scale = new Vector3(0.5f, 0.7f, 0.5f);
                    float x = (FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(6000, 9000));
                    float z = (posicionesAlgasZ[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(j * 10f, (j * 50f + 100)));
                    algaNueva.Position = new Vector3(x, contexto.terreno.CalcularAltura(x, z), z);
                    if (FuncionesMatematicas.Instance.NumeroAleatorioFloat() > 0.5f)
                    {
                        contexto.elementos.Add(new ElementoSinInteraccion(1000, 1400, algaNueva, contexto.efectoAlgas));
                    }
                    else
                    {
                        contexto.elementos.Add(new ElementoSinInteraccion(1000, 1400, algaNueva, contexto.efectoAlgas2));
                    }
                }
            }
        }

        public void CrearPiedrasSobreAgua()
        {
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Roca\\Roca-TgcScene.xml");
            TgcMesh piedraAguaMesh = scene.Meshes[0];
            TgcMesh piedraAguaNuevaMesh;
            float[] posicionesPiedrasZ = { 500f, 1000f, 2000f, 3000f, 3500f, 4000f };
            float aleatorio;
            for (int i = 0; i < 6; i++)
            {
                piedraAguaNuevaMesh = piedraAguaMesh.createMeshInstance(piedraAguaMesh.Name + contexto.elementos.Count.ToString());
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
                float z = (posicionesPiedrasZ[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(100, 300));
                piedraAguaNuevaMesh.Position = new Vector3(x, contexto.terreno.CalcularAltura(x, z), z);
                contexto.elementos.Add(new Elemento(10000, 10000, piedraAguaNuevaMesh, contexto.efectoLuz));
            }
        }

        public void CrearCanoasSobreAgua()
        {
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Botes\\Palangreta\\Palangreta-TgcScene.xml");
            TgcMesh palangreta = scene.Meshes[0].createMeshInstance("Palangreta");
            scene = loader.loadSceneFromFile(recursos
                + "MeshCreator\\Meshes\\Botes\\Canoa\\Canoa-TgcScene.xml");
            TgcMesh canoa = scene.Meshes[0].createMeshInstance("Canoa");
            palangreta.Position = new Vector3(-6500, contexto.terreno.CalcularAltura(-6500, 6000), 6000);
            canoa.Position = new Vector3(-6000, contexto.terreno.CalcularAltura(-6000, 6000) + 30, 6000);
            canoa.Scale = new Vector3(2f, 2f, 2f);
            palangreta.rotateY(Geometry.DegreeToRadian(75f));
            contexto.elementos.Add(new Elemento(10000, 10000, palangreta, contexto.efectoBotes));
            contexto.elementos.Add(new Elemento(10000, 10000, canoa, contexto.efectoBotes));
        }

        public void CrearArbolFrutilla()
        {
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
            float[] posicionArbolFrutillaZ = { -2000f, 0f, 1000f, 2000f };
            for (int i = 0; i < posicionArbolFrutillaX.Length; i++)
            {
                for (int j = 0; j < posicionArbolFrutillaZ.Length; j++)
                {
                    arbolFrutillaMesh = arbolFrutilla.createMeshInstance(arbolFrutilla.Name + i + j);
                    arbolFrutillaVacioMesh = arbolFrutillaVacio.createMeshInstance(arbolFrutillaVacio.Name + i + j);
                    float x = posicionArbolFrutillaX[i] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    float z = posicionArbolFrutillaZ[j] + FuncionesMatematicas.Instance.NumeroAleatorioFloatEntre(-250, 250);
                    arbolFrutillaMesh.Position = new Vector3(x, contexto.terreno.CalcularAltura(x, z), z);
                    arbolFrutillaVacioMesh.Position = arbolFrutillaMesh.Position;
                    nuevoArbolFrutillaCompleto = new ElementoDoble(1000, 2330, arbolFrutillaMesh, arbolFrutillaVacioMesh, contexto.efectoLuz);
                    for (int k = 0; k < 5; k++)
                    {
                        frutillaMesh = frutilla.createMeshInstance(frutilla.Name + i + j + k);
                        frutillaMesh.Position = arbolFrutillaMesh.Position;
                        frutillaMesh.Scale = new Vector3(0.1f, 0.1f, 0.1f);
                        nuevoArbolFrutillaCompleto.agregarElemento(new Alimento(1000, 2330, frutillaMesh, 10, contexto.efectoLuz));
                    }
                    contexto.elementos.Add(nuevoArbolFrutillaCompleto);
                }
            }
        }

        public void CrearPiso()
        {
            TgcTexture pisoTexture = TgcTexture.createTexture(d3dDevice, recursos + "Texturas\\Agua.jpg");
            TgcBox pisoCaja = TgcBox.fromExtremes(new Vector3(-20000, 3, -20000), new Vector3(20000, 15, 20000), pisoTexture);
            contexto.piso = pisoCaja.toMesh("Piso");
            contexto.pisoEfecto.Aplicar(contexto.piso, contexto);
        }

        public void CrearPersonaje(float velocidadCaminar, float velocidadRotacion, float fuerza, Color color)
        {
            TgcMesh hachaMesh;
            TgcMesh palo;
            Personaje personaje;

            //Creamos el personaje
            //Cargar personaje con animaciones
            personaje = new Personaje();
            personaje.VelocidadCaminar = velocidadCaminar;
            personaje.VelocidadRotacion = velocidadRotacion;
            personaje.Fuerza = fuerza;
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
            personaje.mesh.changeDiffuseMaps(new TgcTexture[] { TgcTexture.createTexture(d3dDevice, recursos + "SkeletalAnimations\\Robot\\Textures\\" + "uvw.jpg") });
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
            personaje.mesh.Position = new Vector3(0, contexto.terreno.CalcularAltura(0, 0), 0);

            //Una vez configurado el mesh del personaje iniciamos su bounding esfera y su esfera de alcance de interacción con los elementos
            personaje.IniciarBoundingEsfera();
            personaje.IniciarAlcanceInteraccionEsfera();

            personaje.mesh.setColor(color);

            contexto.personaje = personaje;
        }

        public void CrearHud()
        {
            //****************Crear Sprite de la mochila y del cajon**********************************************
            Size screenSize = GuiController.Instance.Panel3d.Size;

            TgcSprite mochila = new TgcSprite();
            TgcText2d mochilaReglon1 = new TgcText2d();
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

            contexto.mochila = mochila;
            contexto.mochilaReglon1 = mochilaReglon1;

            TgcSprite cajon = new TgcSprite();
            TgcText2d cajonReglon1 = new TgcText2d();
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

            contexto.cajon = cajon;
            contexto.cajonReglon1 = cajonReglon1;
            //****************Crear Sprite de la mochila y del cajon**********************************************

            //*******************Creacion del mini mapa******************
            TgcSprite miniMapa = new TgcSprite();
            miniMapa.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\MiniMapa.png");
            miniMapa.Scaling = new Vector2(0.25f, 0.4f);
            miniMapa.Position = new Vector2(0, screenSize.Height - (miniMapa.Texture.Size.Height * 0.4f));
            //Me guardo las coordenada para que se faciliten los cálculos
            Point coordenadaSuperiorDerecha = new Point((int)(miniMapa.Texture.Size.Width * 0.25f), (int)(screenSize.Height - (miniMapa.Texture.Size.Height * 0.4f)));
            //Multiplicamos por 0.18 porque aproximadamente el 18% de la imagen es la barra de costado
            Point coordenadaInferiorIzquierda = new Point((int)(miniMapa.Texture.Size.Width * 0.25f * 0.18f), screenSize.Height);
            TgcText2d referenciaMiniMapa = new TgcText2d();
            referenciaMiniMapa.changeFont(new System.Drawing.Font("TimesNewRoman", 40, FontStyle.Bold));
            referenciaMiniMapa.Position = new Point(0, 0);
            referenciaMiniMapa.Color = Color.White;
            referenciaMiniMapa.Text = ".";
            referenciaMiniMapa.Align = TgcText2d.TextAlign.CENTER;
            referenciaMiniMapa.Size = new Size(20, 20);

            TgcSprite linea = new TgcSprite();
            linea.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Flecha.png");
            linea.Scaling = new Vector2(0.25f, 0.4f);
            linea.Position = new Vector2(coordenadaInferiorIzquierda.X - 20, coordenadaInferiorIzquierda.Y - 48);
            linea.RotationCenter = new Vector2(0, 13);

            contexto.miniMapa = miniMapa;
            contexto.coordenadaSuperiorDerecha = coordenadaSuperiorDerecha;
            contexto.coordenadaInferiorIzquierda = coordenadaInferiorIzquierda;
            contexto.referenciaMiniMapa = referenciaMiniMapa;
            contexto.linea = linea;

            //*****************Creacion del mini mapa******************

            //****************Crear el texto informativo**********************************************************
            TgcText2d informativo = new TgcText2d();
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
            contexto.informativo = informativo;
            //****************Crear el texto informativo**********************************************************

            //*****Creacion de barra de salud, hidratacion, alimentacion, cansancio y temperatura corporal del persobaje****
            TgcSprite saludIcono = new TgcSprite();
            saludIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Corazon.png");
            saludIcono.Position = new Vector2(20, 20);

            TgcSprite salud = new TgcSprite();
            salud.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\BarraSalud.png");
            salud.Position = new Vector2(100, 40);

            TgcSprite hidratacionIcono = new TgcSprite();
            hidratacionIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Agua.png");
            hidratacionIcono.Position = new Vector2(250, 20);

            TgcSprite hidratacion = new TgcSprite();
            hidratacion.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\BarraHidratacion.png");
            hidratacion.Position = new Vector2(330, 40);

            TgcSprite alimentacionIcono = new TgcSprite();
            alimentacionIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Hamburguesa.png");
            alimentacionIcono.Position = new Vector2(480, 20);

            TgcSprite alimentacion = new TgcSprite();
            alimentacion.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\BarraAlimentacion.png");
            alimentacion.Position = new Vector2(560, 40);

            TgcSprite cansancioIcono = new TgcSprite();
            cansancioIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Cansancio.png");
            cansancioIcono.Position = new Vector2(710, 20);

            TgcSprite cansancio = new TgcSprite();
            cansancio.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\BarraCansancio.png");
            cansancio.Position = new Vector2(790, 40);

            TgcSprite temperaturaPersonajeIcono = new TgcSprite();
            temperaturaPersonajeIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\TemperaturaPersonaje.png");
            temperaturaPersonajeIcono.Position = new Vector2(20, 90);

            TgcText2d temperaturaPersonaje = new TgcText2d();
            temperaturaPersonaje.Color = Color.DarkViolet;
            temperaturaPersonaje.Align = TgcText2d.TextAlign.LEFT;
            temperaturaPersonaje.Position = new Point(100, 100);
            temperaturaPersonaje.Size = new Size(100, 50);
            temperaturaPersonaje.changeFont(new System.Drawing.Font("TimesNewRoman", 30, FontStyle.Bold));

            contexto.saludIcono = saludIcono;
            contexto.salud = salud;
            contexto.hidratacionIcono = hidratacionIcono;
            contexto.hidratacion = hidratacion;
            contexto.alimentacionIcono = alimentacionIcono;
            contexto.alimentacion = alimentacion;
            contexto.cansancioIcono = cansancioIcono;
            contexto.cansancio = cansancio;
            contexto.temperaturaPersonajeIcono = temperaturaPersonajeIcono;
            contexto.temperaturaPersonaje = temperaturaPersonaje;
            //*****Creacion de barra de salud, hidratacion, alimentacion, cansancio y temperatura corporal del persobaje****

            //****************************Creacion de objetivos*********************
            TgcSprite objetivosIcono = new TgcSprite();
            TgcText2d mensajeObjetivo1 = new TgcText2d();
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

            contexto.objetivosIcono = objetivosIcono;
            contexto.mensajeObjetivo1 = mensajeObjetivo1;
            //****************************Creacion de objetivos*********************

            //*****************Creación de texto informativo********************************
            //Texto de Game Over e informativo
            TgcText2d estadoJuego = new TgcText2d();
            estadoJuego.Align = TgcText2d.TextAlign.CENTER;
            estadoJuego.Position = new Point(5, screenSize.Height / 2);
            estadoJuego.Size = new Size(screenSize.Width, 50);
            estadoJuego.changeFont(new System.Drawing.Font("TimesNewRoman", 60, FontStyle.Bold));

            contexto.estadoJuego = estadoJuego;
            //*****************Creación de texto informativo********************************

            //*****************Creación de menu ayuda********************************************************
            TgcSprite ayuda = new TgcSprite();
            ayuda.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\ayuda.jpg");

            //Ubicarlo centrado en la pantalla
            Size ayudaTextureSize = ayuda.Texture.Size;
            TgcText2d ayudaReglon1 = new TgcText2d();
            TgcText2d ayudaReglon2 = new TgcText2d();
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

            contexto.ayuda = ayuda;
            contexto.ayudaReglon1 = ayudaReglon1;
            contexto.ayudaReglon2 = ayudaReglon2;
            //*****************Creación de menu ayuda********************************************************

            //*****************Creación de texto informativo de la temperatura y la hora del dia********************************
            TgcSprite temperaturaDiaIcono = new TgcSprite();
            temperaturaDiaIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\TemperaturaDia.png");
            temperaturaDiaIcono.Position = new Vector2(screenSize.Width - 85, screenSize.Height - 200);
            TgcText2d temperaturaDia = new TgcText2d();
            temperaturaDia.Color = Color.White;
            temperaturaDia.Align = TgcText2d.TextAlign.RIGHT;
            temperaturaDia.Position = new Point(screenSize.Width - 205, screenSize.Height - 180);
            temperaturaDia.Size = new Size(100, 100);
            temperaturaDia.changeFont(new System.Drawing.Font("TimesNewRoman", 20, FontStyle.Bold));
            TgcSprite horaDiaIcono = new TgcSprite();
            horaDiaIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\HoraDelDia.png");
            horaDiaIcono.Position = new Vector2(screenSize.Width - 100, screenSize.Height - 100);
            TgcText2d horaDia = new TgcText2d();
            horaDia.Color = Color.White;
            horaDia.Align = TgcText2d.TextAlign.RIGHT;
            horaDia.Position = new Point(screenSize.Width - 210, screenSize.Height - 85);
            horaDia.Size = new Size(100, 100);
            horaDia.changeFont(new System.Drawing.Font("TimesNewRoman", 20, FontStyle.Bold));
            TgcSprite estadoDiaSolIcono = new TgcSprite();
            estadoDiaSolIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Sol.png");
            estadoDiaSolIcono.Position = new Vector2(screenSize.Width - 100, screenSize.Height - 300);
            TgcSprite estadoDiaLunaIcono = new TgcSprite();
            estadoDiaLunaIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Luna.png");
            estadoDiaLunaIcono.Position = new Vector2(screenSize.Width - 100, screenSize.Height - 300);
            TgcSprite estadoDiaLluviaIcono = new TgcSprite();
            estadoDiaLluviaIcono.Texture = TgcTexture.createTexture(recursos + "\\Texturas\\Hud\\Lluvia.png");
            estadoDiaLluviaIcono.Position = new Vector2(screenSize.Width - 100, screenSize.Height - 300);

            contexto.temperaturaDiaIcono = temperaturaDiaIcono;
            contexto.temperaturaDia = temperaturaDia;
            contexto.horaDiaIcono = horaDiaIcono;
            contexto.horaDia = horaDia;
            contexto.estadoDiaSolIcono = estadoDiaSolIcono;
            contexto.estadoDiaLunaIcono = estadoDiaLunaIcono;
            contexto.estadoDiaLluviaIcono = estadoDiaLluviaIcono;
            //*****************Creación de texto informativo de la temperatura y la hora del dia********************************
        }

        public void CrearPostProcesado()
        {
            //Activamos el renderizado customizado. De esta forma el framework nos delega control total sobre como dibujar en pantalla
            //La responsabilidad cae toda de nuestro lado
            GuiController.Instance.CustomRenderEnabled = true;

            //Se crean 2 triangulos (o Quad) con las dimensiones de la pantalla con sus posiciones ya transformadas
            // x = -1 es el extremo izquiedo de la pantalla, x = 1 es el extremo derecho
            // Lo mismo para la Y con arriba y abajo
            // la Z en 1 simpre
            CustomVertex.PositionTextured[] screenQuadVertices = new CustomVertex.PositionTextured[]
            {
                new CustomVertex.PositionTextured( -1, 1, 1, 0,0),
                new CustomVertex.PositionTextured(1,  1, 1, 1,0),
                new CustomVertex.PositionTextured(-1, -1, 1, 0,1),
                new CustomVertex.PositionTextured(1,-1, 1, 1,1)
            };
            //vertex buffer de los triangulos
            VertexBuffer screenQuadVB = new VertexBuffer(typeof(CustomVertex.PositionTextured),
                    4, d3dDevice, Usage.Dynamic | Usage.WriteOnly,
                        CustomVertex.PositionTextured.Format, Pool.Default);
            screenQuadVB.SetData(screenQuadVertices, 0, LockFlags.None);

            //Creamos un Render Targer sobre el cual se va a dibujar la pantalla
            Texture renderTarget2D = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth
                    , d3dDevice.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget,
                        Format.X8R8G8B8, Pool.Default);


            //Cargar shader con efectos de Post-Procesado
            Microsoft.DirectX.Direct3D.Effect effect = TgcShaders.loadEffect(recursos + "Shaders\\PostProcess.fx");

            //Configurar Technique dentro del shader
            effect.Technique = "RainTechnique";

            contexto.screenQuadVB = screenQuadVB;
            contexto.renderTarget2D = renderTarget2D;
            contexto.efectoLluvia = effect;
        }

        public void IniciarCamaraPrimeraPersona()
        {
            contexto.camara = new CamaraPrimeraPersona(GuiController.Instance.Frustum, GuiController.Instance.D3dDevice);
        }

        public void IniciarCamaraTerceraPersona()
        {
            contexto.camara = new CamaraTerceraPersona(GuiController.Instance.ThirdPersonCamera, contexto.personaje.mesh.Position, GuiController.Instance.Frustum, GuiController.Instance.D3dDevice);
        }

        private String Version()
        {
            return "1.01.001";
        }

        #endregion
    }
}
