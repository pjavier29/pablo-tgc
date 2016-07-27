using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.SkeletalAnimation;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.ElementosJuego;
using TGC.Group.Model.ElementosJuego.Instrumentos;

namespace TGC.Group.Model.Utiles.Efectos
{
    public class Efecto
    {
        #region Atributos

        private readonly Effect efectoShader;
        private string tecnica;
        private readonly List<ElementoIluminacion> elementosIluminacion;

        #endregion Atributos

        #region Constructores

        public Efecto(Effect efectoShader, string tecnica)
        {
            this.efectoShader = efectoShader;
            this.tecnica = tecnica;
            elementosIluminacion = new List<ElementoIluminacion>();
        }

        public Efecto(Effect efectoShader)
        {
            this.efectoShader = efectoShader;
            elementosIluminacion = new List<ElementoIluminacion>();
        }

        #endregion Constructores

        #region Comportamientos

        public void AgregarElementoDeIluminacion(ElementoIluminacion elemento)
        {
            elementosIluminacion.Add(elemento);
        }

        public void EliminarElementoDeIluminacion(Elemento elemento)
        {
            ElementoIluminacion elementoABorrar = null;
            foreach (var elemAux in elementosIluminacion)
            {
                if (elemAux.Elemento == elemento)
                {
                    elementoABorrar = elemAux;
                }
            }
            if (elementoABorrar != null)
            {
                elementosIluminacion.Remove(elementoABorrar);
            }
        }

        public List<ElementoIluminacion> GetElementosIluminacion()
        {
            return elementosIluminacion;
        }

        public virtual ElementoIluminacion AlguienIluminaAElemento(Elemento elemento)
        {
            foreach (var elem in elementosIluminacion)
            {
                if (elem.IluminoAElemento(elemento))
                {
                    return elem;
                }
            }
            return null;
        }

        public virtual ElementoIluminacion AlguienIluminaAElemento(Vector3 posicion)
        {
            foreach (var elem in elementosIluminacion)
            {
                if (elem.IluminoAElemento(posicion))
                {
                    return elem;
                }
            }
            return null;
        }

        public virtual ElementoIluminacion IluminadorMasCercanoA(Vector3 posicion, SuvirvalCraft contexto)
        {
            var elemIlumActual = elementosIluminacion[0];
            var aux = new List<ElementoIluminacion>();
            foreach (var elem in elementosIluminacion)
            {
                if (contexto.optimizador.ElementosRenderizacion.Contains(elem.Elemento))
                {
                    aux.Add(elem);
                }
            }
            if (aux.Count != 0)
            {
                elemIlumActual = aux[0];
                foreach (var elem in aux)
                {
                    if (elem.Elemento.distanciaA(posicion) < elemIlumActual.Elemento.distanciaA(posicion))
                    {
                        elemIlumActual = elem;
                    }
                }
            }
            return elemIlumActual;
        }

        public virtual bool ContieneElementoDeTipo(string tipo)
        {
            foreach (var elem in elementosIluminacion)
            {
                if (elem.Elemento.EsDeTipo(tipo))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual Effect GetEfectoShader()
        {
            return efectoShader;
        }

        public virtual string Tecnica()
        {
            return tecnica;
        }

        public virtual void Tecnica(string tecnica)
        {
            this.tecnica = tecnica;
        }

        /// <summary>
        ///     Para el caso que el mesh haya cambiado de efecto, debemos aplicar todo de nuevo
        /// </summary>
        /// <param name="mesh"></param>
        public void Aplicar(TgcMesh mesh, SuvirvalCraft contexto)
        {
            mesh.Effect = efectoShader;
            mesh.Technique = tecnica;
            Actualizar(contexto);
        }

        public void Aplicar(TgcMesh mesh)
        {
            mesh.Effect = efectoShader;
            if (tecnica == null)
            {
                tecnica = TgcShaders.Instance.getTgcMeshTechnique(mesh.RenderType);
            }
            mesh.Technique = tecnica;
        }

        public void Aplicar(TgcSkeletalMesh mesh)
        {
            mesh.Effect = efectoShader;
            if (tecnica == null)
            {
                tecnica = TgcShaders.Instance.getTgcSkeletalMeshTechnique(mesh.RenderType);
            }
            mesh.Technique = tecnica;
        }

        public virtual void Actualizar(SuvirvalCraft contexto)
        {
        }

        public virtual void Actualizar(SuvirvalCraft contexto, Elemento elemento)
        {
        }

        public virtual void ActualizarRenderizar(SuvirvalCraft contexto, Elemento elemento)
        {
        }

        public virtual void ActualizarRenderizar(SuvirvalCraft contexto, float elapsedTime)
        {
        }

        public virtual void ActualizarRenderizar(SuvirvalCraft contexto, Terreno terreno)
        {
        }

        /// <summary>
        ///     TODO. Este método no tiene que estar más cuando las armas sean elementos en si mismos
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="arma"></param>
        public virtual void ActualizarRenderizar(SuvirvalCraft contexto, Arma arma)
        {
        }

        public virtual bool HayQueIluminarConElementos(SuvirvalCraft contexto)
        {
            return !contexto.dia.EsDeDia() && GetElementosIluminacion().Count > 0;
        }

        #endregion Comportamientos
    }
}