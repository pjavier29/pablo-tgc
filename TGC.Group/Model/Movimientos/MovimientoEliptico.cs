using Microsoft.DirectX;
using TGC.Core.SceneLoader;
using TGC.Core.Utils;

namespace TGC.Group.Model.Movimientos
{
    public class MovimientoEliptico
    {
        #region Constructor

        public MovimientoEliptico(Vector3 centro, Vector3 a, Vector3 b, TgcMesh mesh)
        {
            this.centro = centro;
            this.a = a;
            this.b = b;
            this.mesh = mesh;
            posicionActual = 0;
        }

        #endregion Constructor

        #region Atributos

        private readonly TgcMesh mesh;
        private Vector3 a;
        private Vector3 b;
        private Vector3 centro;
        private float posicionActual;

        #endregion Atributos

        #region Comportamientos

        public virtual void Actualizar(float valor)
        {
            posicionActual = valor;
            var x = a.X * FastMath.Cos(posicionActual);
            var y = b.Y * FastMath.Sin(posicionActual);
            mesh.Position = new Vector3(x, y, mesh.Position.Z);
        }

        public float AlturaMaxima()
        {
            return b.Y;
        }

        #endregion Comportamientos
    }
}