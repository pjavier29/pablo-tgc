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

namespace AlumnoEjemplos.MiGrupo
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class Suvirval_Craft : TgcExample
    {
        Terreno terreno;
        TgcBox piso;
        TgcSkeletalMesh personaje;
        TgcBoundingSphere characterSphere;
        bool jumping;

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


            // ------------------------------------------------------------
            // Creo el Heightmap para el terreno:
            /*terreno = new Terreno();
            terreno.loadHeightmap(alumnoMediaFolder
                    + "Shaders\\WorkshopShaders\\Heighmaps\\" + "HeightmapHawaii.jpg", 100f, 1f, new Vector3(0, 0, 0));
            terreno.loadTexture(alumnoMediaFolder
                    + "Shaders\\WorkshopShaders\\Heighmaps\\" + "TerrainTextureHawaii.jpg");*/

            // ------------------------------------------------------------


            //Crear piso
            TgcTexture pisoTexture = TgcTexture.createTexture(d3dDevice, GuiController.Instance.ExamplesMediaDir + "Texturas\\tierra.jpg");
            piso = TgcBox.fromExtremes(new Vector3(-1000, -2, -1000), new Vector3(1000, 0, 1000), pisoTexture);


            //Creamos el personaje
            //Cargar personaje con animaciones
            TgcSkeletalLoader skeletalLoader = new TgcSkeletalLoader();
            personaje = skeletalLoader.loadMeshAndAnimationsFromFile(
                GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Robot\\" + "Robot-TgcSkeletalMesh.xml",
                GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Robot\\",
                new string[] {
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Robot\\" + "Caminando-TgcSkeletalAnim.xml",
                    GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Robot\\" + "Parado-TgcSkeletalAnim.xml",
                });

            //Le cambiamos la textura para diferenciarlo un poco
            personaje.changeDiffuseMaps(new TgcTexture[] { TgcTexture.createTexture(d3dDevice, GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\Robot\\Textures\\" + "uvwGreen.jpg") });

            //Configurar animacion inicial
            personaje.playAnimation("Parado", true);
            //Escalarlo porque es muy grande
            personaje.Position = new Vector3(0, 0, 0);
            //Rotarlo 180° porque esta mirando para el otro lado
            personaje.rotateY(Geometry.DegreeToRadian(180f));


            //BoundingSphere que va a usar el personaje
            personaje.AutoUpdateBoundingBox = false;
            characterSphere = new TgcBoundingSphere(personaje.BoundingBox.calculateBoxCenter(), personaje.BoundingBox.calculateBoxRadius());
            jumping = false;

            //Configurar camara en Tercer Persona
            GuiController.Instance.ThirdPersonCamera.Enable = true;
            GuiController.Instance.ThirdPersonCamera.setCamera(personaje.Position, 200, -300);

            ///////////////USER VARS//////////////////

            //Crear una UserVar
            GuiController.Instance.UserVars.addVar("variablePrueba");

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



            ///////////////LISTAS EN C#//////////////////
            //crear
            List<string> lista = new List<string>();

            //agregar elementos
            lista.Add("elemento1");
            lista.Add("elemento2");

            //obtener elementos
            string elemento1 = lista[0];

            //bucle foreach
            foreach (string elemento in lista)
            {
                //Loggear por consola del Framework
                GuiController.Instance.Logger.log(elemento);
            }

            //bucle for
            for (int i = 0; i < lista.Count; i++)
            {
                string element = lista[i];
            }


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
            bool moving = false;
            bool rotating = false;

            float velocidadCaminar = 100f;
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
                moveForward *= 1.7f;
                rotate *= 1.7f;
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
                /*bool collide = false;
                foreach (TgcBox obstaculo in obstaculos)
                {
                    TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(personaje.BoundingBox, obstaculo.BoundingBox);
                    if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                    {
                        collide = true;
                        break;
                    }
                }

                //Si hubo colision, restaurar la posicion anterior
                if (collide)
                {
                    personaje.Position = lastPos;
                }
                else
                {

                }*/
            }

            //Si no se esta moviendo, activar animacion de Parado
            else
            {
                personaje.playAnimation("Parado", true);
            }


            //Render Terreno
            //terreno.render();

            //Render piso
            piso.render();

            //Render personaje
            personaje.animateAndRender();


            //Obtener valor de UserVar (hay que castear)
            int valor = (int)GuiController.Instance.UserVars.getValue("variablePrueba");


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
        }

    }
}
