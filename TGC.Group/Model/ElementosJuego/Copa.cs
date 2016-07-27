using System;
using TGC.Core.SceneLoader;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosJuego
{
    public class Copa : Elemento
    {
        #region Atributos

        private string mensajeInformativo;

        #endregion Atributos

        #region Constructores

        public Copa(float peso, float resistencia, TgcMesh mesh, Efecto efecto) : base(peso, resistencia, mesh, efecto)
        {
            mensajeInformativo = "";
        }

        public Copa(float peso, float resistencia, TgcMesh mesh, Elemento elemento)
            : base(peso, resistencia, mesh, elemento)
        {
            mensajeInformativo = "";
        }

        #endregion Constructores

        #region Comportamientos

        public override void procesarInteraccion(string accion, SuvirvalCraft contexto, float elapsedTime)
        {
            mensajeInformativo = "Juntar (J)";
            base.procesarInteraccion(accion, contexto, elapsedTime);
            if (accion.Equals("Juntar"))
            {
                //TODO. Esta validacion es porque se ejecuta muchas veces al presionar la tecla. Se deberia solucioanr cuando implementemos los comandos
                if (!contexto.personaje.ContieneElementoEnMochila(this))
                {
                    try
                    {
                        contexto.personaje.juntar(this);
                        contexto.elementos.Remove(this);
                        contexto.optimizador.ForzarActualizacionElementosColision();
                    }
                    catch (Exception ex)
                    {
                        mensajeInformativo = ex.Message;
                    }
                }
            }
        }

        public override string getAcciones()
        {
            //TODO. Mejorar esta lógica
            return mensajeInformativo;
        }

        public override string GetTipo()
        {
            return Copa;
        }

        #endregion Comportamientos
    }
}