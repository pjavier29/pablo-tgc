using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
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

        private Effect efectoShader;
        private String tecnica;
        private List<ElementoIluminacion> elementosIluminacion;

        #endregion Atributos

        #region Constructores

        public Efecto(Effect efectoShader, String tecnica)
        {
            this.efectoShader = efectoShader;
            this.tecnica = tecnica;
            this.elementosIluminacion = new List<ElementoIluminacion>();
        }

        public Efecto(Effect efectoShader)
        {
            this.efectoShader = efectoShader;
            this.elementosIluminacion = new List<ElementoIluminacion>();
        }

        #endregion Constructores

        #region Comportamientos

        public void AgregarElementoDeIluminacion(ElementoIluminacion elemento)
        {
            this.elementosIluminacion.Add(elemento);
        }

        public void EliminarElementoDeIluminacion(Elemento elemento)
        {
            ElementoIluminacion elementoABorrar = null;
            foreach (ElementoIluminacion elemAux in this.elementosIluminacion)
            {
                if (elemAux.Elemento == elemento)
                {
                    elementoABorrar = elemAux;
                }
            }
            if (elementoABorrar != null)
            {
                this.elementosIluminacion.Remove(elementoABorrar);
            }
        }

        public List<ElementoIluminacion> GetElementosIluminacion()
        {
            return this.elementosIluminacion;
        }

        public virtual ElementoIluminacion AlguienIluminaAElemento(Elemento elemento)
        {
            foreach (ElementoIluminacion elem in this.elementosIluminacion)
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
            foreach (ElementoIluminacion elem in this.elementosIluminacion)
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
            ElementoIluminacion elemIlumActual = this.elementosIluminacion[0];
            List<ElementoIluminacion> aux = new List<ElementoIluminacion>();
            foreach (ElementoIluminacion elem in this.elementosIluminacion)
            {
                if (contexto.optimizador.ElementosRenderizacion.Contains(elem.Elemento))
                {
                    aux.Add(elem);
                }
            }
            if (aux.Count != 0)
            {
                elemIlumActual = aux[0];
                foreach (ElementoIluminacion elem in aux)
                {
                    if (elem.Elemento.distanciaA(posicion) < elemIlumActual.Elemento.distanciaA(posicion))
                    {
                        elemIlumActual = elem;
                    }
                }
            }
            return elemIlumActual;
        }

        public virtual bool ContieneElementoDeTipo(String tipo)
        {
            foreach (ElementoIluminacion elem in this.elementosIluminacion)
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
            return this.efectoShader;
        }

        public virtual String Tecnica()
        {
            return this.tecnica;
        }

        public virtual void Tecnica(String tecnica)
        {
            this.tecnica = tecnica;
        }

        /// <summary>
        /// Para el caso que el mesh haya cambiado de efecto, debemos aplicar todo de nuevo
        /// </summary>
        /// <param name="mesh"></param>
        public void Aplicar(TgcMesh mesh, SuvirvalCraft contexto)
        {
            mesh.Effect = this.efectoShader;
            mesh.Technique = this.tecnica;
            this.Actualizar(contexto);
        }

        public void Aplicar(TgcMesh mesh)
        {
            mesh.Effect = this.efectoShader;
            if (this.tecnica == null)
            {
                this.tecnica = TgcShaders.Instance.getTgcMeshTechnique(mesh.RenderType);
            }
            mesh.Technique = this.tecnica;
        }

        public void Aplicar(TgcSkeletalMesh mesh)
        {
            mesh.Effect = this.efectoShader;
            if (this.tecnica == null)
            {
                this.tecnica = TgcShaders.Instance.getTgcSkeletalMeshTechnique(mesh.RenderType);
            }
            mesh.Technique = this.tecnica;
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
        /// TODO. Este método no tiene que estar más cuando las armas sean elementos en si mismos
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="arma"></param>
        public virtual void ActualizarRenderizar(SuvirvalCraft contexto, Arma arma)
        {
        }

        public virtual bool HayQueIluminarConElementos(SuvirvalCraft contexto)
        {
            return ((!contexto.dia.EsDeDia()) && this.GetElementosIluminacion().Count > 0);
        }

        #endregion Comportamientos
    }
}