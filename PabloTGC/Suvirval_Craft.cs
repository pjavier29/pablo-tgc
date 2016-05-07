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
        Personaje personaje;
        bool jumping;
        bool jumpingAdelante;
        TgcArrow directionArrow;
        float tiempo;
        bool puedeGolpear;
        bool puedeIncrementarSaludConFuego = false;
        TgcScene scene, scene2, scene3, scene4, scene5, scene6;
        TgcMesh palmera, pino, arbol, banana, fuego, lenia;
        TgcText2d textGameOver;
        Animal oveja;

        Obstaculo puebaFisica;
        MovimientoParabolico movimiento;
        MovimientoParabolico movimientoPersonaje = new MovimientoParabolico();


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


            ///////////////USER VARS//////////////////

            //Crear una UserVar
            GuiController.Instance.UserVars.addVar("salud");
            GuiController.Instance.UserVars.addVar("fuerza");
            GuiController.Instance.UserVars.addVar("velocidad");
            GuiController.Instance.UserVars.addVar("tiempocorriendo");
            GuiController.Instance.UserVars.addVar("x");
            GuiController.Instance.UserVars.addVar("y");
            GuiController.Instance.UserVars.addVar("z");

            GuiController.Instance.UserVars.addVar("dirx");
            GuiController.Instance.UserVars.addVar("diry");
            GuiController.Instance.UserVars.addVar("dirz");

            GuiController.Instance.UserVars.addVar("largoxz");
            GuiController.Instance.UserVars.addVar("anguloxz");
            GuiController.Instance.UserVars.addVar("difx");
            GuiController.Instance.UserVars.addVar("difz");
            GuiController.Instance.UserVars.addVar("porx");
            GuiController.Instance.UserVars.addVar("porz");


            // ------------------------------------------------------------
            // Creo el Heightmap para el terreno:
            terreno = new Terreno();
            terreno.loadHeightmap(alumnoMediaFolder
                    + "Shaders\\WorkshopShaders\\Heighmaps\\" + "HeightmapHawaii.jpg", 100f, 1f, new Vector3(0, 0, 0));
            terreno.loadTexture(alumnoMediaFolder
                    + "Shaders\\WorkshopShaders\\Heighmaps\\" + "TerrainTextureHawaii.jpg");

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
                 + "MeshCreator\\Meshes\\Vegetacion\\ArbolBananas\\ArbolBananas-TgcScene.xml");
            arbol = scene3.Meshes[0];

            scene4 = loader.loadSceneFromFile(alumnoMediaFolder
                + "MeshCreator\\Meshes\\Frutas\\Bananas\\Bananas-TgcScene.xml");
            banana = scene4.Meshes[0];

            scene5 = loader.loadSceneFromFile(alumnoMediaFolder
                + "MeshCreator\\Meshes\\Fuego\\fuego-TgcScene.xml");
            fuego = scene5.Meshes[0];

            scene6 = loader.loadSceneFromFile(alumnoMediaFolder
                 + "MeshCreator\\Meshes\\Leña\\lenia-TgcScene.xml");
            lenia = scene6.Meshes[0];

            obstaculos = new List<Obstaculo>();

            float[] r = { 1850f, 2100f, 2300f, 1800f };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    TgcMesh palmeraNueva = palmera.createMeshInstance(palmera.Name + i);
                    palmeraNueva.Scale = new Vector3(0.5f, 1.5f, 0.5f);
                    float x = r[i] * (float)Math.Cos(Geometry.DegreeToRadian(100 + 10.0f * j));
                    float z = r[i] * (float)Math.Sin(Geometry.DegreeToRadian(100 + 10.0f * j));
                    palmeraNueva.Position = new Vector3(x, terreno.CalcularAltura(x,z), z);
                    obstaculos.Add(new Obstaculo(1000, 140, palmeraNueva));
                }
            }

            TgcMesh bananaMesh;
            float[] ar = { -850f, -600f, -400f, -200f };
            for (int i = 0; i < 4; i++)
            {
                TgcMesh arbolBanana = arbol.createMeshInstance(arbol.Name + i);
                arbolBanana.Scale = new Vector3(1.5f, 3.5f, 1.5f);
                float x = r[i] - 100;
                float z = r[i] -123;
                arbolBanana.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                bananaMesh = banana.createMeshInstance(banana.Name);
                bananaMesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                obstaculos.Add(new Obstaculo(1000, 130, arbolBanana, new Obstaculo(1000, 1000, bananaMesh)));
            }

            TgcMesh fuegoMesh;
            TgcMesh leniaMesh;
            for (int i = 0; i < 4; i++)
            {
                TgcMesh pinoNuevo = pino.createMeshInstance(pino.Name + i);
                pinoNuevo.Scale = new Vector3(0.5f, 1.5f, 0.5f);
                float x = r[i] - 100;
                float z = -(r[i] - 123 );
                pinoNuevo.Position = new Vector3(x, terreno.CalcularAltura(x, z), z);
                fuegoMesh = fuego.createMeshInstance(/*fuego.Name*/"fuego");
                leniaMesh = lenia.createMeshInstance(/*lenia.Name*/"lenia");
                fuegoMesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                leniaMesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                obstaculos.Add(new Obstaculo(1000, 233, pinoNuevo, 
                                (new Obstaculo(1000, 233, leniaMesh, 
                                    new Obstaculo(1000, 233, fuegoMesh)))));
            }


            //Cargar obstaculos y posicionarlos. Los obstáculos se crean con TgcBox en lugar de cargar un modelo.
            TgcBox caja;

            //Caja para tirar
            caja = TgcBox.fromSize(
                new Vector3(0, 0, 0),
                new Vector3(20, 20, 20),
                TgcTexture.createTexture(d3dDevice, alumnoMediaFolder + "Texturas\\baldosaFacultad.jpg"));
            puebaFisica = new Obstaculo(100, 300, caja);
            movimiento = new MovimientoParabolico(caja.Position, new Vector3(0,5,0), 50, puebaFisica.mesh);

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


            //Creamos al animal
            caja = TgcBox.fromSize(
                new Vector3(0, 0, 0),
                new Vector3(80, 150, 80),
                TgcTexture.createTexture(d3dDevice, alumnoMediaFolder + "Texturas\\baldosaFacultad.jpg"));
            oveja = new Animal(5000, 20, caja.toMesh("oveja"));
            caja.Position = new Vector3(200, terreno.CalcularAltura(200,200), 200);
            obstaculos.Add(oveja);

            //Crear piso
            TgcTexture pisoTexture = TgcTexture.createTexture(d3dDevice, alumnoMediaFolder + "Texturas\\Agua.jpg");
            piso = TgcBox.fromExtremes(new Vector3(-3000, -2, -3000), new Vector3(3000, 5, 3000), pisoTexture);


            //Creamos el personaje
            //Cargar personaje con animaciones
            personaje = new Personaje();
            personaje.velocidadCaminar = 150f;
            personaje.velocidadRotacion = 50f;
            personaje.fuerza = 200f;
            personaje.salud = 100f;
            personaje.resistenciaFisica = 30f;
            TgcSkeletalLoader skeletalLoader = new TgcSkeletalLoader();
            personaje.mesh = skeletalLoader.loadMeshAndAnimationsFromFile(
                alumnoMediaFolder + "SkeletalAnimations\\Robot\\" + "Robot-TgcSkeletalMesh.xml",
                alumnoMediaFolder + "SkeletalAnimations\\Robot\\",
                new string[] {
                    alumnoMediaFolder + "SkeletalAnimations\\Robot\\" + "Caminando-TgcSkeletalAnim.xml",
                    alumnoMediaFolder + "SkeletalAnimations\\Robot\\" + "Parado-TgcSkeletalAnim.xml",
                    alumnoMediaFolder + "SkeletalAnimations\\Robot\\" + "Empujar-TgcSkeletalAnim.xml",
                    alumnoMediaFolder + "SkeletalAnimations\\Robot\\" + "Patear-TgcSkeletalAnim.xml",
                    alumnoMediaFolder + "SkeletalAnimations\\Robot\\" + "Pegar-TgcSkeletalAnim.xml",
                });

            //Agregamos el arma al personaje
            TgcSkeletalBoneAttach attachment = new TgcSkeletalBoneAttach();
            TgcBox attachmentBox = TgcBox.fromSize(new Vector3(3, 60, 3), Color.Green);
            attachment.Mesh = attachmentBox.toMesh("attachment");
            attachment.Bone = personaje.mesh.getBoneByName("Bip01 R Hand");
            attachment.Offset = Matrix.Translation(10, 40, 0);
            attachment.updateValues();
            personaje.mesh.Attachments.Add(attachment);

            //Le cambiamos la textura para diferenciarlo un poco
            personaje.mesh.changeDiffuseMaps(new TgcTexture[] { TgcTexture.createTexture(d3dDevice, alumnoMediaFolder + "SkeletalAnimations\\Robot\\Textures\\" + "uvwGreen.jpg") });

            //Configurar animacion inicial
            personaje.mesh.playAnimation("Parado", true);
            //Escalarlo porque es muy grande
            personaje.mesh.Position = new Vector3(0, terreno.CalcularAltura(0, 0), 0);
            //Rotarlo 180° porque esta mirando para el otro lado
            personaje.mesh.rotateY(Geometry.DegreeToRadian(180f));


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
            GuiController.Instance.ThirdPersonCamera.setCamera(personaje.mesh.Position, 200, -300);

            //Texto de Game Over
            textGameOver = new TgcText2d();
            textGameOver.Text = "GAME OVER";
            textGameOver.Color = Color.Red;
            textGameOver.Align = TgcText2d.TextAlign.LEFT;
            textGameOver.Position = new Point(300, 100);
            textGameOver.Size = new Size(300, 100);
            textGameOver.changeFont(new System.Drawing.Font("TimesNewRoman", 40, FontStyle.Bold | FontStyle.Italic));

            //Creamos los modificadores para las acciones
            string[] acciones = new string[] { "Juntar", "Encender"};
            GuiController.Instance.Modifiers.addInterval("Acción", acciones, 0);

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

            //Hacer que la camara siga al personaje en su nueva posicion. Sumamos 100 en el posición de Y porque queremos que la cámara este un poco más alta.
            GuiController.Instance.ThirdPersonCamera.Target = personaje.mesh.Position + new Vector3(0,100,0);

            //Calcular proxima posicion de personaje segun Input
            float moveForward = 0f;
            float rotate = 0;
            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;
            //TODO. Mejorar esta logica de representación de estados
            bool moving = false;
            bool rotating = false;
            bool patear = false;
            bool pegar = false;
            bool tirar = false;
            bool lanzar = false;
            jumping = false;
            jumpingAdelante = false;

            float dirC = 1f;
            float dirR = 1f;

            //Movimiento para adelante
            if (d3dInput.keyDown(Key.W) || d3dInput.keyDown(Key.Up))
            {
                dirC = -1f;
                moveForward = dirC * personaje.velocidadCaminar;
                moving = true;
            }

            //Movimiento para Atras
            if (d3dInput.keyDown(Key.S) || d3dInput.keyDown(Key.Down))
            {
                moveForward = personaje.velocidadCaminar;
                moving = true;
            }

            //Rotar Derecha
            if (d3dInput.keyDown(Key.Right) || d3dInput.keyDown(Key.D))
            {
                rotate = personaje.velocidadRotacion;
                rotating = true;
            }

            //Rotar Izquierda
            if (d3dInput.keyDown(Key.Left) || d3dInput.keyDown(Key.A))
            {
                dirR = -1f;
                rotate = dirR * personaje.velocidadRotacion;
                rotating = true;
            }

            //Si preciono para caminar más rápido
            if (d3dInput.keyDown(Key.RightShift) || d3dInput.keyDown(Key.LeftShift))
            {
                moveForward = dirC * personaje.correr(elapsedTime);
                rotate = dirR * personaje.rotarRapido();
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

            //Tirar un elemento
            if (d3dInput.keyDown(Key.T))
            {
                tirar = true;
            }

            //Lanza un elemento con fuerza
            if (d3dInput.keyDown(Key.C))
            {
                lanzar = true;
            }

            //Saltar
            if (d3dInput.keyDown(Key.Space))
            {
                jumping = true;
            }

            //Saltar
            if (d3dInput.keyDown(Key.Space) && d3dInput.keyDown(Key.R))
            {
                jumpingAdelante = true;
            }

            //Si hubo rotacion
            if (rotating)
            {
                //Rotar personaje y la camara, hay que multiplicarlo por el tiempo transcurrido para no atarse a la velocidad el hardware
                float rotAngle = Geometry.DegreeToRadian(rotate * elapsedTime);
                personaje.mesh.rotateY(rotAngle);
                GuiController.Instance.ThirdPersonCamera.rotateY(rotAngle);
            }

            //Si hubo desplazamiento
            if (moving)
            {
                puedeIncrementarSaludConFuego = false;

                //Activar animacion de caminando
                personaje.mesh.playAnimation("Caminando", true);

                //Aplicar movimiento hacia adelante o atras segun la orientacion actual del Mesh
                Vector3 lastPos = personaje.mesh.Position;

                //Aplicamos el movimiento
                //TODO Ver si es correcta la forma que aplico para representar que se esta a la altura del terreno.
                float xm = FastMath.Sin(personaje.mesh.Rotation.Y) * moveForward;
                float zm = FastMath.Cos(personaje.mesh.Rotation.Y) * moveForward;
                Vector3 movementVector = new Vector3(xm, 0, zm);
                personaje.mesh.move(movementVector * elapsedTime);
                personaje.mesh.Position = new Vector3(personaje.mesh.Position.X, terreno.CalcularAltura(personaje.mesh.Position.X, personaje.mesh.Position.Z), personaje.mesh.Position.Z);


                //Detectar colisiones
                bool collide = false;
                Obstaculo obstaculoColiciono = new Obstaculo();//TODO. Mejorar esta creacion!!!
                foreach (Obstaculo obstaculo in obstaculos)
                {
                    TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(personaje.mesh.BoundingBox, obstaculo.BoundingBox());
                    if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                    {
                        collide = true;
                        obstaculoColiciono = obstaculo;
                        break;
                    }
                }

                //Si hubo colision, restaurar la posicion anterior
                if (collide)
                {
                    string accionElegida = (string)GuiController.Instance.Modifiers["Acción"];

                    if (obstaculoColiciono.nombre().Equals("fuego"))
                    {
                        //TODO. sacar esta flecha que esta al pedo.
                        directionArrow.PStart = personaje.mesh.Position;
                        directionArrow.PEnd = obstaculoColiciono.posicion();
                        directionArrow.updateValues();

                        if (obstaculoColiciono.distanciaA(personaje.mesh.Position) > 20)
                        {
                            puedeIncrementarSaludConFuego = true;
                        }
                        else
                        {
                            personaje.morir();
                        }
                    }

                    else {
                        if (obstaculoColiciono.nombre().Equals("lenia"))
                        {
                            if (accionElegida.Equals("Juntar"))
                            {
                                personaje.juntar(obstaculoColiciono);
                                obstaculos.Remove(obstaculoColiciono);
                            }
                            if (accionElegida.Equals("Encender"))
                            {
                                foreach (Obstaculo obs in obstaculoColiciono.obstaculosQueContiene())
                                {
                                    obs.posicion(obstaculoColiciono.posicion());
                                    obs.mesh.BoundingBox.scaleTranslate(obstaculoColiciono.posicion(), new Vector3(2f,2f,2f));
                                    obstaculos.Add(obs);
                                }
                                obstaculoColiciono.liberar();
                                obstaculos.Remove(obstaculoColiciono);
                                //Movemos el personaje hacia atrás para evitar que se queme con el fuego
                                //float z = (float)Math.Cos((float)personaje.mesh.Rotation.Y) * 30;
                                //float x = (float)Math.Sin((float)personaje.mesh.Rotation.Y) * 30;
                                //personaje.mesh.Position = new Vector3(x, 0, z);

                                float x = movementVector.X * 30;
                                float z = movementVector.Z * 30;
                                personaje.mesh.Position = new Vector3(x, terreno.CalcularAltura(x,z), z);
                            }
                        }

                        else {

                            if (obstaculoColiciono.nombre().Equals("Banana"))
                            {
                                personaje.consumirAlimento();
                                obstaculoColiciono.liberar();
                                obstaculos.Remove(obstaculoColiciono);
                            }
                            else {
                                if (moveForward < 0)
                                {//Si esta caminando para adelante entonces empujamos la caja, sino no hacemos nada.
                                    if (obstaculoColiciono.seMueveConUnaFuerza(personaje.fuerza))
                                    {
                                        /*Vector3 direccionMovimiento = new Vector3((personaje.mesh.Position.X - lastPos.X),
                                            (personaje.mesh.Position.Y - lastPos.Y), (personaje.mesh.Position.Z - lastPos.Z));*/
                                        Vector3 direccionMovimiento = movementVector;
                                        direccionMovimiento.Normalize();
                                        direccionMovimiento.Multiply(moveForward * elapsedTime * -0.1f);
                                        obstaculoColiciono.mover(direccionMovimiento);
                                    }
                                    personaje.mesh.playAnimation("Empujar", true);
                                }
                            }
                        }
                        personaje.mesh.Position = lastPos;
                    }
                }
                else
                {

                }
            }

            //Si no se esta moviendo, activar animacion de Parado, puede estar pateando o pegando
            else
            {
                //TODO. Mejorar esta lógica de estados.
                personaje.mesh.playAnimation("Parado", true);

                //Simulamos el descanso del personaje
                personaje.incrementoResistenciaFisica(elapsedTime);

                if ((pegar || patear))
                {
                    //TODO. Por ahora dejamos estos valores aca ya que dependen del personaje y de la herramienta que este usando
                    int alcance = 0;
                    float fuerzaGolpe = 0;

                    if (patear)
                    {
                        personaje.mesh.playAnimation("Patear", true);
                        alcance = 70;
                        fuerzaGolpe = 66;
                    }

                    if (pegar)
                    {
                        personaje.mesh.playAnimation("Pegar", true);
                        alcance = 50;
                        fuerzaGolpe = 33;
                    }

                    //TODO. Tener en cuenta que la direccion se esta calculando mas arriba, aunque aqui se calcula la direccion si el perosnaje esta quieto. Analizar!!!
                    //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
                    float z = -(float)Math.Cos((float)personaje.mesh.Rotation.Y) * alcance;
                    float x = -(float)Math.Sin((float)personaje.mesh.Rotation.Y) * alcance;
                    //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
                    //el vector salga del personaje.
                    Vector3 direccion = personaje.mesh.Position + new Vector3(x, /*terreno.CalcularAltura(x, z) + */50, z);


                    //TODO. sacar esta flecha que esta al pedo.
                    directionArrow.PStart = personaje.mesh.Position;
                    directionArrow.PEnd = direccion;
                    directionArrow.updateValues();

                    //Buscamos si esta al alcance alguno de los obstáculos
                    foreach (Obstaculo obstaculo in obstaculos)
                    {
                        if (this.isPointInsideAABB(direccion, obstaculo.BoundingBox()))
                        {
                            //Si golpeo un obstáculo deberé esperar 1 segundo para poder golpearlo nuevamente
                            if (puedeGolpear)
                            {
                                puedeGolpear = false;
                                if (!obstaculo.recibirDanio(fuerzaGolpe))
                                {
                                    if (!obstaculo.destruccionTotal())
                                    {
                                        foreach (Obstaculo obs in obstaculo.obstaculosQueContiene())
                                        {
                                            //TODO. Aplicar algun algoritmo de dispersion copado
                                            obs.posicion(obstaculo.posicion());
                                            obstaculos.Add(obs);
                                        }
                                    }
                                    obstaculo.liberar();
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

            if (tirar)
            {
                if (personaje.elementosEnMochila().Count > 0)
                {
                    //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
                    float z = -(float)Math.Cos((float)personaje.mesh.Rotation.Y) * 150;
                    float x = -(float)Math.Sin((float)personaje.mesh.Rotation.Y) * 150;
                    //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
                    //el vector salga del personaje.
                    Vector3 direccion = personaje.mesh.Position + new Vector3(x, terreno.CalcularAltura(x,z), z);

                    Obstaculo elementoATirar = personaje.elementosEnMochila()[0];
                    elementoATirar.posicion(direccion);
                    obstaculos.Add(elementoATirar);
                    personaje.elementosEnMochila().Remove(elementoATirar);
                }
            }

            if (lanzar)
            {
                //TODO. Tener en cuenta que la direccion se esta calculando mas arriba, aunque aqui se calcula la direccion si el perosnaje esta quieto. Analizar!!!
                //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
                float z = -(float)Math.Cos((float)personaje.mesh.Rotation.Y) * 50;
                float x = -(float)Math.Sin((float)personaje.mesh.Rotation.Y) * 50;
                //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
                //el vector salga del personaje.
                Vector3 direccion = personaje.mesh.Position + new Vector3(x, /*terreno.CalcularAltura(x, z) + */1, z);


                directionArrow.PStart = personaje.mesh.Position;
                directionArrow.PEnd = direccion;
                directionArrow.updateValues();

                puebaFisica.mesh.Position = personaje.mesh.Position + new Vector3(0,50,0);

                movimiento = new MovimientoParabolico(personaje.mesh.Position, direccion, 30, puebaFisica.mesh);

            }

            if (jumping)
            {
                movimientoPersonaje = new MovimientoParabolico(personaje.mesh.Position, personaje.mesh.Position + new Vector3(0,1,0), 4, personaje.mesh);
            }

            if (jumpingAdelante)
            {
                //TODO. Agregar para que no tome al mismo tiempo la tecla de avance W con la barra
                //TODO. Tener en cuenta que la direccion se esta calculando mas arriba, aunque aqui se calcula la direccion si el perosnaje esta quieto. Analizar!!!
                //Lo hacemos negativo para invertir hacia donde apunta el vector en 180 grados
                float z = -(float)Math.Cos((float)personaje.mesh.Rotation.Y) * 50;
                float x = -(float)Math.Sin((float)personaje.mesh.Rotation.Y) * 50;
                //Direccion donde apunta el personaje, sumamos las coordenadas obtenidas a la posición del personaje para que
                //el vector salga del personaje.
                Vector3 direccion = personaje.mesh.Position + new Vector3(x, /*terreno.CalcularAltura(x, z) + */1, z);

                movimientoPersonaje = new MovimientoParabolico(personaje.mesh.Position, direccion, 40, personaje.mesh);
            }

            movimientoPersonaje.update(elapsedTime, terreno);
            movimientoPersonaje.render();

            GuiController.Instance.UserVars.setValue("salud", personaje.salud);
            GuiController.Instance.UserVars.setValue("fuerza", personaje.fuerza);
            GuiController.Instance.UserVars.setValue("velocidad", personaje.velocidadCaminar);
            GuiController.Instance.UserVars.setValue("tiempocorriendo", personaje.tiempoCorriendo);

            GuiController.Instance.UserVars.setValue("x", personaje.mesh.Position.X);
            GuiController.Instance.UserVars.setValue("y", personaje.mesh.Position.Y);
            GuiController.Instance.UserVars.setValue("z", personaje.mesh.Position.Z);


            if (tiempo >= 1)
            {
                //Si paso más de un segundo lo reiniciamos.
                personaje.afectarSaludPorTiempo(tiempo);
 
                tiempo = 0;
                puedeGolpear = true;
            }
            tiempo += elapsedTime;

            //Analisis de posibilidad de incrementar o no salud por fuego
            if (puedeIncrementarSaludConFuego)
            {
                personaje.incrementarSaludPorTiempo(elapsedTime);
            }

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

            //Actualizamos el objeto Fisico y lo renderizo
            movimiento.update(elapsedTime, terreno);
            movimiento.render();

            //actualizamos la posición de la oveja
            oveja.update(elapsedTime, terreno);

            //Render Terreno
            terreno.render();

            directionArrow.render();

            //Render piso
            piso.render();

            //Render personaje
            personaje.mesh.animateAndRender();

            //Render obstaculos
            foreach (Obstaculo obstaculo in obstaculos)
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
            foreach (Obstaculo obstaculo in obstaculos)
            {
                obstaculo.destruir();
            }
            textGameOver.dispose();
        }

        private bool isPointInsideAABB(Vector3 point, TgcBoundingBox box)
        {
            return (point.X >= box.PMin.X && point.X <= box.PMax.X) &&
                   (point.Y >= box.PMin.Y && point.Y <= box.PMax.Y) &&
                   (point.Z >= box.PMin.Z && point.Z <= box.PMax.Z);
        }

    }
}
