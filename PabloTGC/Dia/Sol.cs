﻿using AlumnoEjemplos.MiGrupo;
using AlumnoEjemplos.PabloTGC.Movimientos;
using AlumnoEjemplos.PabloTGC.Utiles;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.Dia
{
    public class Sol
    {
        #region Atributos
        private ColorValue colorDeLuz;
        private float intensidadDeLuz;
        private float atenuacionDeLuz;
        private MovimientoEliptico movimientoSol;
        private float alturaPuestaSol;
        private float atenuacionMaxima;
        private float intesidadLuzMinima;
        #endregion

        #region Propiedades
        public TgcMesh Mesh { get; set; }
        #endregion

        #region Constructores
        public Sol()
        {
            this.colorDeLuz = ColorValue.FromColor(Color.White);
            this.intensidadDeLuz = 15000f;
            this.atenuacionDeLuz = 0.1f;
            this.alturaPuestaSol = 0;
        }
        #endregion

        #region Comportamientos
        public void CrearMovimiento()
        {
            movimientoSol = new MovimientoEliptico(new Vector3(0f, 0f, 0f), new Vector3(12000f, 0f, 0f), new Vector3(0f, 5000f, 0f), this.Mesh);
            this.alturaPuestaSol = (this.Mesh.BoundingBox.PMax.Y - this.Mesh.BoundingBox.PMin.Y) / 2;
            //Lo colocamos en atributos porque son valores fijos, de esta forma nos evitamos hacer la cuenta en cada render
            //De un calculo que siempre dara igual.
            this.atenuacionMaxima = (this.atenuacionDeLuz * this.movimientoSol.AlturaMaxima()) / this.alturaPuestaSol; ;
            this.intesidadLuzMinima = (this.intensidadDeLuz * this.alturaPuestaSol) / this.movimientoSol.AlturaMaxima();
    }

        public void Actualizar(float valor)
        {
            this.movimientoSol.Actualizar(valor);
        }

        public void Iluminar(Elemento elemento, Personaje personaje)
        {
            //elemento.ActualizarParametrosEfectoIluminacion(this.colorDeLuz, this.Mesh.Position, this.IntensidadDeLuz(), this.Atenuacion());
        }

        public float Atenuacion()
        {
            if (this.EsDeNoche())
            {
                return this.atenuacionMaxima;
            }
            return (this.atenuacionDeLuz * this.movimientoSol.AlturaMaxima()) / this.Mesh.Position.Y;
        }

        public float IntensidadDeLuz()
        {
            if (this.EsDeNoche())
            {
                return this.intesidadLuzMinima;
            }
            return (this.intensidadDeLuz * this.Mesh.Position.Y) / this.movimientoSol.AlturaMaxima();
        }

        /// <summary>
        /// retorna la intendidad actual de la luz pero llevada a porcentaje entre 0 y 1
        /// </summary>
        /// <returns></returns>
        public float IntensidadRelativa()
        {
            return FuncionesMatematicas.Instance.PorcentajeRelativo(this.intesidadLuzMinima, this.intensidadDeLuz, this.IntensidadDeLuz());
        }

        public bool EsDeDia()
        {
            return this.Mesh.Position.Y > this.alturaPuestaSol;
        }

        public bool EsDeNoche()
        {
            return ! this.EsDeDia();
        }

        public ColorValue GetColorAmanecerAnochecer()
        {
            ColorValue color = new ColorValue();
            if (this.Mesh.BoundingBox.PMax.Y > 0 && this.EsDeNoche())
            {
                color.Red = 1f;
                color.Green = 0f;
                color.Blue = 0f;
                return color;
            }
            float aux = this.Mesh.Position.Y - this.alturaPuestaSol;
            //Si despues de salir el sol su altura no supera mas de 200 la puesta del sol
            if (aux > 0 && aux < 300f)
            {
                color.Red = 1f;
                color.Green = 1f;
                color.Blue = 0f;
                return color;
            }
            color.Red = 0f;
            color.Green = 0f;
            color.Blue = 0f;
            return color;
        }

        #endregion

    }
}