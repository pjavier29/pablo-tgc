﻿using Microsoft.DirectX.Direct3D;
using TGC.Group.Model.Administracion;
using TGC.Group.Model.ElementosJuego;
using TGC.Group.Model.ElementosJuego.Instrumentos;

namespace TGC.Group.Model.Utiles.Efectos
{
    public class EfectoLuz : Efecto
    {
        #region Constructores

        public EfectoLuz(Effect efectoShader, string tecnica) : base(efectoShader, tecnica)
        {
        }

        public EfectoLuz(Effect efectoShader) : base(efectoShader)
        {
            Tecnica(null);
        }

        #endregion Constructores

        #region Comportamientos

        public override void ActualizarRenderizar(SuvirvalCraft contexto, Elemento elemento)
        {
            if (HayQueIluminarConElementos(contexto))
            {
                var iluminador = AlguienIluminaAElemento(elemento);
                if (iluminador != null)
                {
                    //this.Tecnica(GuiController.Instance.Shaders.getTgcMeshTechnique(elemento.Mesh.RenderType));
                    //this.Aplicar(elemento.Mesh);
                    //Setea primero aquellos parámetros que son propios del efecto en cuestión.
                    iluminador.Iluminar(this, contexto.personaje.mesh.Position, elemento.ColorEmisor(),
                        elemento.ColorAmbiente(),
                        elemento.ColorDifuso(), elemento.ColorEspecular(), elemento.EspecularEx());
                    elemento.Mesh.render();
                }
                else
                {
                    contexto.dia.GetSol()
                        .Iluminar(contexto.personaje.mesh.Position, this, elemento.ColorEmisor(),
                            elemento.ColorAmbiente(),
                            elemento.ColorDifuso(), elemento.ColorEspecular(), elemento.EspecularEx());
                    elemento.Mesh.render();
                }
            }
            else
            {
                //this.Tecnica(GuiController.Instance.Shaders.getTgcMeshTechnique(elemento.Mesh.RenderType));
                //this.Aplicar(elemento.Mesh);
                contexto.dia.GetSol()
                    .Iluminar(contexto.personaje.mesh.Position, this, elemento.ColorEmisor(), elemento.ColorAmbiente(),
                        elemento.ColorDifuso(), elemento.ColorEspecular(), elemento.EspecularEx());
                elemento.Mesh.render();
            }
        }

        public override void ActualizarRenderizar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (HayQueIluminarConElementos(contexto))
            {
                var iluminador = AlguienIluminaAElemento(contexto.personaje.mesh.Position);
                if (iluminador != null)
                {
                    //Setea primero aquellos parámetros que son propios del efecto en cuestión.
                    iluminador.Iluminar(this, contexto.personaje.mesh.Position, contexto.personaje.ColorEmisor(),
                        contexto.personaje.ColorAmbiente(),
                        contexto.personaje.ColorDifuso(), contexto.personaje.ColorEspecular(),
                        contexto.personaje.EspecularEx());
                    contexto.personaje.mesh.animateAndRender(elapsedTime);
                }
                else
                {
                    contexto.dia.GetSol()
                        .Iluminar(contexto.personaje.mesh.Position, this, contexto.personaje.ColorEmisor(),
                            contexto.personaje.ColorAmbiente(),
                            contexto.personaje.ColorDifuso(), contexto.personaje.ColorEspecular(),
                            contexto.personaje.EspecularEx());
                    contexto.personaje.mesh.animateAndRender(elapsedTime);
                }
            }
            else
            {
                contexto.dia.GetSol()
                    .Iluminar(contexto.personaje.mesh.Position, this, contexto.personaje.ColorEmisor(),
                        contexto.personaje.ColorAmbiente(),
                        contexto.personaje.ColorDifuso(), contexto.personaje.ColorEspecular(),
                        contexto.personaje.EspecularEx());
                contexto.personaje.mesh.animateAndRender(elapsedTime);
            }
        }

        public override void ActualizarRenderizar(SuvirvalCraft contexto, Arma arma)
        {
            if (HayQueIluminarConElementos(contexto))
            {
                //Porque el arma no tiene posicion, tiene matriz de translación. De todas formas siempre esta al lado del personaje.
                var iluminador = AlguienIluminaAElemento(contexto.personaje.mesh.Position);
                if (iluminador != null)
                {
                    //Setea primero aquellos parámetros que son propios del efecto en cuestión.
                    iluminador.Iluminar(this, contexto.personaje.mesh.Position, arma.ColorEmisor(), arma.ColorAmbiente(),
                        arma.ColorDifuso(), arma.ColorEspecular(), arma.EspecularEx());
                    arma.mesh.render();
                }
                else
                {
                    contexto.dia.GetSol()
                        .Iluminar(contexto.personaje.mesh.Position, this, arma.ColorEmisor(), arma.ColorAmbiente(),
                            arma.ColorDifuso(), arma.ColorEspecular(), arma.EspecularEx());
                    arma.mesh.render();
                }
            }
            else
            {
                contexto.dia.GetSol()
                    .Iluminar(contexto.personaje.mesh.Position, this, arma.ColorEmisor(), arma.ColorAmbiente(),
                        arma.ColorDifuso(), arma.ColorEspecular(), arma.EspecularEx());
                arma.mesh.render();
            }
        }

        #endregion Comportamientos
    }
}