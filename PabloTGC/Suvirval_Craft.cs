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

namespace AlumnoEjemplos.MiGrupo
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class Suvirval_Craft : TgcExample
    {
        Terreno terreno;
        TgcSkyBox skyBox;
        TgcBox piso;
        List<Obstaculo> obstaculos;
        TgcSkeletalMesh personaje;
        TgcBoundingSphere characterSphere;
        bool jumping;
        TgcArrow directionArrow;
        float tiempo;
        bool puedeGolpear;
        TgcScene scene, scene2, scene3;
        TgcMesh palmera, pino, arbol;
        List<TgcMesh> palmeras, arboles, pinos;



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

            //Carpeta de archivos Media del alumno
            string alumnoMediaFolder = GuiController.Instance.AlumnoEjemplosMediaDir;

            //Crear loader
            TgcSceneLoader loader = new TgcSceneLoader();

            // ------------------------------------------------------------
            // Creo el Heightmap para el terreno:
            /*terreno = new Terreno();
            terreno.loadHeightmap(alumnoMediaFolder
                    + "Shaders\\WorkshopShaders\\Heighmaps\\" + "HeightmapHawaii.jpg", 100f, 1f, new Vector3(0, 0, 0));
            terreno.loadTexture(alumnoMediaFolder
                    + "Shaders\\WorkshopShaders\\Heighmaps\\" + "TerrainTextureHawaii.jpg");*/

            // ------------------------------------------------------------

            // Crear SkyBox:
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 0, 0);
            skyBox.Size = new Vector3(8000, 8000, 8000);
            string texturesPath = alumnoMediaFolder + "Texturas\\Quake\\SkyBox1\\";
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "phobos_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "phobos_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "phobos_lf.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "phobos_rt.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "phobos_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "phobos_ft.jpg");
            skyBox.SkyEpsilon = 50f;
            skyBox.updateValues();

            //Creamos el ambiente selvático
            scene = loader.loadSceneFromFile(alumnoMediaFolder
                + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
            palmera = scene.Meshes[0];

            scene2 = loader.loadSceneFromFile(alumnoMediaFolder
                 + "MeshCreator\\Meshes\\Vegetacion\\Pino\\Pino-TgcScene.xml");
            pino = scene2.Meshes[0];

            scene3 = loader.loadSceneFromFile(alumnoMediaFolder
                 + "MeshCreator\\Meshes\\Vegetacion\\ArbolSelvatico\\ArbolSelvatico-TgcScene.xml");
            arbol = scene3.Meshes[0];


            Vector3 size = palmera.BoundingBox.calculateSize();
            palmeras = new List<TgcMesh>();
            float[] r = { 1850f, 2100f, 2300f, 1800f };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    TgcMesh palmeraNueva = palmera.createMeshInstance(palmera.Name + i);
                    palmeraNueva.Scale = new Vector3(0.5f, 1.5f, 0.5f);
                    float x = r[i] * (float)Math.Cos(Geometry.DegreeToRadian(100 + 10.0f * j));
                    float z = r[i] * (float)Math.Sin(Geometry.DegreeToRadian(100 + 10.0f * j));
                    palmeraNueva.Position = new Vector3(x, 0, z);
                    palmeras.Add(palmeraNueva);
                }
            }

            size = arbol.BoundingBox.calculateSize();
            arboles = new List<TgcMesh>();
            float[] ar = { -850f, -600f, -400f, -200f };
            for (int i = 0; i < 4; i++)
            {
                TgcMesh arbolNuevo = arbol.createMeshInstance(arbol.Name + i);
                arbolNuevo.Scale = new Vector3(0.5f, 1.5f, 0.5f);
                float x = r[i] - 100;
                float z = r[i] -123;
                arbolNuevo.Position = new Vector3(x, 0, z);
                arboles.Add(arbolNuevo);
            }

            size = pino.BoundingBox.calculateSize();
            pinos = new List<TgcMesh>();
            for (int i = 0; i < 4; i++)
            {
                TgcMesh pinoNuevo = pino.createMeshInstance(pino.Name + i);
                pinoNuevo.Scale = new Vector3(0.5f, 1.5f, 0.5f);
                float x = r[i] - 100;
                float z = -(r[i] - 123 );
                pinoNuevo.Position = new Vector3(x, 0, z);
                pinos.Add(pinoNuevo);
            }


            //Cargar obstaculos y posicionarlos. Los obstáculos se crean con TgcBox en lugar de cargar un modelo.
            obstaculos = new List<Obstaculo>();
            TgcBox caja;

            //Obstaculo 1
            caja = TgcBox.fromSize(
                new Vector3(-100, 0, 0),
                new Vector3(80, 150, 80),
                TgcTexture.createTexture(d3dDevice, alumnoMediaFolder + "Texturas\\baldosaFacultad.jpg"));
            obstaculos.Add(new Obstaculo(100, 300, caja));

            //Obstaculo 2
            caja = TgcBox.fromSize(
                new Vector3(50, 0, 200),
                new Vector3(80, 300, 80),
                TgcTexture.createTexture(d3dDevice, alumnoMediaFolder + "Texturas\\madera.jpg"));
            obstaculos.Add(new Obstaculo(100, 300, caja));

            //Obstaculo 3
            caja = TgcBox.fromSize(
                new Vector3(300, 0, 100),
                new Vector3(80, 100, 150),
                TgcTexture.createTexture(d3dDevice, alumnoMediaFolder + "Texturas\\granito.jpg"));
            obstaculos.Add(new Obstaculo(233, 300, caja));


            //Crear piso
            TgcTexture pisoTexture = TgcTexture.createTexture(d3dDevice, alumnoMediaFolder + "Texturas\\pasto.jpg");
            piso = TgcBox.fromExtremes(new Vector3(-3000, -2, -3000), new Vector3(3000, 0, 3000), pisoTexture);


            //Creamos el personaje
            //Cargar personaje con animaciones
            TgcSkeletalLoader skeletalLoader = new TgcSkeletalLoader();
            personaje = skeletalLoader.loadMeshAndAnimationsFromFile(
                alumnoMediaFolder + "SkeletalAnimations\\Robot\\" + "Robot-TgcSkeletalMesh.xml",
                alumnoMediaFolder + "SkeletalAnimations\\Robot\\",
                new string[] {
                    alumnoMediaFolder + "SkeletalAnimations\\Robot\\" + "Caminando-TgcSkeletalAnim.xml",
                    alumnoMediaFolder + "SkeletalAnimations\\Robot\\" + "Parado-TgcSkeletalAnim.xml",
                    alumnoMediaFolder + "SkeletalAnimations\\Robot\\" + "Empujar-TgcSkeletalAnim.xml",
                    alumnoMediaFolder + "SkeletalAnimations\\Robot\\" + "Patear-TgcSkeletalAnim.xml",
                    alumnoMediaFolder + "SkeletalAnimations\\Robot\\" + "Pegar-TgcSkeletalAnim.xml",
                });

            //Le cambiamos la textura para diferenciarlo un poco
            personaje.changeDiffuseMaps(new TgcTexture[] { TgcTexture.createTexture(d3dDevice, alumnoMediaFolder + "SkeletalAnimations\\Robot\\Textures\\" + "uvwGreen.jpg") });

            //Configurar animacion inicial
            personaje.playAnimation("Parado", true);
            //Escalarlo porque es muy grande
            personaje.Position = new Vector3(0, 0, 0);
            //Rotarlo 180° porque esta mirando para el otro lado
            personaje.rotateY(Geometry.DegreeToRadian(180f));

            //Crear linea para mostrar la direccion del movimiento del personaje
            directionArrow = new TgcArrow();
            directionArrow.BodyColor = Color.Red;
            directionArrow.HeadColor = Color.Green;
            directionArrow.Thickness = 1;
            directionArrow.HeadSize = new Vector2(10, 20);



            //BoundingSphere que va a usar el personaje
            //personaje.AutoUpdateBoundingBox = false;
            //characterSphere = new TgcBoundingSphere(personaje.BoundingBox.calculateBoxCenter(), personaje.BoundingBox.calculateBoxRadius());
            //jumping = false;

            //Configurar camara en Tercer Persona
            GuiController.Instance.ThirdPersonCamera.Enable = true;
            GuiController.Instance.ThirdPersonCamera.setCamera(personaje.Position, 200, -300);

            ///////////////USER VARS//////////////////

            //Crear una UserVar
            GuiController.Instance.UserVars.addVar("variablePrueba");
            GuiController.Instance.UserVars.addVar("variablePrueba2");
            GuiController.Instance.UserVars.addVar("variablePrueba3");

            //Cargar valor en UserVar
            GuiController.Instance.UserVars.setValue("variablePrueba", 5451);



            ///////////////MODIFIERS//////////////////

            //Crear un modifier para un valor FLOAT
            GuiController.Instance.Modifiers.addFloat("valorFloat", -50f, 200f, 0f);

            //Crear un modifier para un ComboBox con opciones
            string[] opciones = new string[]{"opcion1", "opcion2", "opcion3"};
            GuiController.Instance.Modifiers.addInterval("valorIntervalo", opciones, 0);

            //Crear un modifier para modificar un vértice
            GuiController.Instance.Modifiers.addVertex3f("valorVertice", new Vector3(-100, -100, -100), new Vector3(50, 50, 50), new Vector3(0, 0, 0));



            ///////////////CONFIGURAR CAMARA ROTACIONAL//////////////////
            //Es la camara que viene por default, asi que no hace falta hacerlo siempre
            //GuiController.Instance.RotCamera.Enable = true;
            //Configurar centro al que se mira y distancia desde la que se mira
            //.Instance.RotCamera.setCamera(new Vector3(0, 0, 0), 100);


            /*
            ///////////////CONFIGURAR CAMARA PRIMERA PERSONA//////////////////
            //Camara en primera persona, tipo videojuego FPS
            //Solo puede haber una camara habilitada a la vez. Al habilitar la camara FPS se deshabilita la camara rotacional
            //Por default la camara FPS viene desactivada
            GuiController.Instance.FpsCamera.Enable = true;
            //Configurar posicion y hacia donde se mira
            GuiController.Instance.FpsCamera.setCamera(new Vector3(0, 0, -20), new Vector3(0, 0, 0));
            */

            tiempo = 0;
            puedeGolpear = true;
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

            //Hacer que la camara siga al personaje en su nueva posicion
            GuiController.Instance.ThirdPersonCamera.Target = personaje.Position;

            //Calcular proxima posicion de personaje segun Input
            float moveForward = 0f;
            float rotate = 0;
            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;
            //TODO. Mejorar esta logica de representación de estados
            bool moving = false;
            bool rotating = false;
            bool patear = false;
            bool pegar = false;

            float velocidadCaminar = 150f;
            float velocidadRotacion = 50f;

            //Movimiento para adelante
            if (d3dInput.keyDown(Key.W) || d3dInput.keyDown(Key.Up))
            {
                moveForward = -velocidadCaminar;
                moving = true;
            }

            //Movimiento para Atras
            if (d3dInput.keyDown(Key.S) || d3dInput.keyDown(Key.Down))
            {
                moveForward = velocidadCaminar;
                moving = true;
            }

            //Rotar Derecha
            if (d3dInput.keyDown(Key.Right) || d3dInput.keyDown(Key.D))
            {
                rotate = velocidadRotacion;
                rotating = true;
            }

            //Rotar Izquierda
            if (d3dInput.keyDown(Key.Left) || d3dInput.keyDown(Key.A))
            {
                rotate = -velocidadRotacion;
                rotating = true;
            }

            //Si preciono para caminar más rápido
            if (d3dInput.keyDown(Key.RightShift) || d3dInput.keyDown(Key.LeftShift))
            {
                //Valor harcodeado. Ademas seria interesanta agregar que no se pueda corrar en forma infinita.
                moveForward *= 2f;
                rotate *= 2f;
            }

            //Pegar una piña
            if (d3dInput.keyDown(Key.RightControl))
            {
                pegar = true;
            }

            //Pegar una patada
            if (d3dInput.keyDown(Key.RightAlt))
            {
                patear = true;
            }

            //Si hubo rotacion
            if (rotating)
            {
                //Rotar personaje y la camara, hay que multiplicarlo por el tiempo transcurrido para no atarse a la velocidad el hardware
                float rotAngle = Geometry.DegreeToRadian(rotate * elapsedTime);
                personaje.rotateY(rotAngle);
                GuiController.Instance.ThirdPersonCamera.rotateY(rotAngle);
            }

            //Si hubo desplazamiento
            if (moving)
            {
                //Activar animacion de caminando
                personaje.playAnimation("Caminando", true);

                //Aplicar movimiento hacia adelante o atras segun la orientacion actual del Mesh
                Vector3 lastPos = personaje.Position;

                //La velocidad de movimiento tiene que multiplicarse por el elapsedTime para hacerse independiente de la velocida de CPU
                //Ver Unidad 2: Ciclo acoplado vs ciclo desacoplado
                personaje.moveOrientedY(moveForward * elapsedTime);

                //Detectar colisiones
                bool collide = false;
                Obstaculo obstaculoColiciono = new Obstaculo();//TODO. Mejorar esta creacion!!!
                foreach (Obstaculo obstaculo in obstaculos)
                {
                    TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(personaje.BoundingBox, obstaculo.caja.BoundingBox);
                    if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                    {
                        collide = true;
                        obstaculoColiciono = obstaculo;
                        break;
                    }
                }

                int fuerzaEmpuje = 200;//Estos deberias ser atributos del personaje

                //Si hubo colision, restaurar la posicion anterior
                if (collide)
                {
                    if (moveForward < 0)
                    {
                        if (obstaculoColiciono.seMueveConUnaFuerza(fuerzaEmpuje))
                        {
                            //Si esta caminando para adelante entonces empujamos la caja, sino no hacemos nada.
                            Vector3 direccionMovimiento = new Vector3((personaje.Position.X - lastPos.X),
                                (personaje.Position.Y - lastPos.Y), (personaje.Position.Z - lastPos.Z));
                            direccionMovimiento.Normalize();
                            direccionMovimiento.Multiply(moveForward * elapsedTime * -0.1f);
                            obstaculoColiciono.caja.move(direccionMovimiento);
                        }
                        personaje.playAnimation("Empujar", true);
                    }

                    personaje.Position = lastPos;
                }
                else
                {

                }
            }

            //Si no se esta moviendo, activar animacion de Parado, puede estar pateando o pegando
            else
            {
                //TODO. Mejorar esta lógica de estados.
                personaje.playAnimation("Parado", true);

                if ((pegar || patear))
                {
                    int alcance = 0;
                    float fuerzaGolpe = 0;

                    if (patear)
                    {
                        personaje.playAnimation("Patear", true);
                        alcance = 70;
                        fuerzaGolpe = 66;
                    }

                    if (pegar)
                    {
                        personaje.playAnimation("Pegar", true);
                        alcance = 50;
                        fuerzaGolpe = 33;
                    }

                    //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
                    float z = -(float)Math.Cos((float)personaje.Rotation.Y) * alcance;
                    float x = -(float)Math.Sin((float)personaje.Rotation.Y) * alcance;

                    //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
                    //el vector salga del personaje.
                    Vector3 direccion = personaje.Position + new Vector3(x, 0, z);

                    //TODO. sacar esta flecha que esta al pedo.
                    directionArrow.PStart = personaje.Position;
                    directionArrow.PEnd = direccion;
                    directionArrow.updateValues();

                    //Buscamos si esta al alcance alguno de los obstáculos
                    foreach (Obstaculo obstaculo in obstaculos)
                    {
                        if (this.isPointInsideAABB(direccion, obstaculo.caja.BoundingBox))
                        {
                            //Si golpeo un obstáculo deberé esperar 1 segundo para poder golpearlo nuevamente
                            if (puedeGolpear)
                            {
                                puedeGolpear = false;
                                if (!obstaculo.recibirDanio(fuerzaGolpe))
                                {
                                    //TODO. Ver que hacer después con las partes del obtáculo destruído!!!!
                                    obstaculos.Remove(obstaculo);
                                    //En principio solo se puede golpear un obstaculo a la vez.
                                    //Tener en cuenta que estamos borrando un elemento de una colección que se esta recorriendo.
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            GuiController.Instance.UserVars.setValue("variablePrueba", obstaculos[0].resistencia);
            GuiController.Instance.UserVars.setValue("variablePrueba2", obstaculos[1].resistencia);
            GuiController.Instance.UserVars.setValue("variablePrueba3", obstaculos[2].resistencia);

            if (tiempo >= 1)
            {
                //Si paso más de un segundo lo reiniciamos.
                tiempo = 0;
                puedeGolpear = true;
            }
            tiempo += elapsedTime;

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

            //Render Terreno
            //terreno.render();

            directionArrow.render();

            //Render piso
            piso.render();

            //Render personaje
            personaje.animateAndRender();

            //Render obstaculos
            foreach (Obstaculo obstaculo in obstaculos)
            {
                obstaculo.caja.render();
                //obstaculo.BoundingBox.render();
            }

            //Renderizar SkyBox
            skyBox.render();

            //Renderizo las palmeras
            foreach (TgcMesh palmera in palmeras)
            {
                palmera.render();
            }
            foreach (TgcMesh arbol in arboles)
            {
                arbol.render();
            }
            foreach (TgcMesh pino in pinos)
            {
                pino.render();
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
            personaje.dispose();
            // terreno.dispose();
            skyBox.dispose();
            foreach (Obstaculo obstaculo in obstaculos)
            {
                obstaculo.caja.dispose();
            }
            foreach (TgcMesh palmera in palmeras)
            {
                palmera.dispose();
            }
            foreach (TgcMesh arbol in arboles)
            {
                arbol.dispose();
            }
            foreach (TgcMesh pino in pinos)
            {
                pino.dispose();
            }
        }

        private bool isPointInsideAABB(Vector3 point, TgcBoundingBox box)
        {
            return (point.X >= box.PMin.X && point.X <= box.PMax.X) &&
                   (point.Y >= box.PMin.Y && point.Y <= box.PMax.Y) &&
                   (point.Z >= box.PMin.Z && point.Z <= box.PMax.Z);
        }

    }
}
