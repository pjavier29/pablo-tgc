using AlumnoEjemplos.MiGrupo;
using AlumnoEjemplos.PabloTGC.Utiles;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.PabloTGC.Movimientos
{
    public class MovimientoEliptico
    {
        #region Atributos
        private TgcMesh mesh;
        private Vector3 a;
        private Vector3 b;
        private Vector3 centro;
        private float posicionActual;
        #endregion

        #region Constructor
        public MovimientoEliptico(Vector3 centro, Vector3 a, Vector3 b, TgcMesh mesh)
        {
            this.centro = centro;
            this.a = a;
            this.b = b;
            this.mesh = mesh;
            this.posicionActual = 0;
        }
        #endregion

        #region Comportamientos
        public virtual void Actualizar(float valor)
        {
            this.posicionActual = valor;
            float x = this.a.X * FastMath.Cos(this.posicionActual);
            float y = this.b.Y * FastMath.Sin(this.posicionActual);
            this.mesh.Position = new Vector3(x,y, this.mesh.Position.Z);
        }

        public float AlturaMaxima()
        {
            return this.b.Y;
        }
        #endregion
    }
}
