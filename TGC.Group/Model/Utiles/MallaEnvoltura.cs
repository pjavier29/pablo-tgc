using Microsoft.DirectX;
using TGC.Core.SceneLoader;
using TGC.Core.SkeletalAnimation;

namespace TGC.Group.Model.Utiles
{
    public class MallaEnvoltura
    {
        #region Atributos

        private TgcMesh tgcMesh;
        private TgcSkeletalMesh tgcSkeletalMesh;

        #endregion Atributos

        #region Contructores

        public MallaEnvoltura(TgcMesh mesh)
        {
            this.tgcMesh = mesh;
            this.tgcSkeletalMesh = null;
        }

        public MallaEnvoltura(TgcSkeletalMesh mesh)
        {
            this.tgcSkeletalMesh = mesh;
            this.tgcMesh = null;
        }

        #endregion Contructores

        #region Comportamientos

        public void Render()
        {
            if (tgcMesh != null) { this.tgcMesh.render(); } else { this.tgcSkeletalMesh.render(); }
        }

        public Vector3 Posicion()
        {
            if (tgcMesh != null) { return this.tgcMesh.Position; } else { return this.tgcSkeletalMesh.Position; }
        }

        public void Posicion(Vector3 nuevaPosicion)
        {
            if (tgcMesh != null) { this.tgcMesh.Position = nuevaPosicion; } else { this.tgcSkeletalMesh.Position = nuevaPosicion; }
        }

        public Vector3 MinimoPunto()
        {
            if (tgcMesh != null) { return this.tgcMesh.BoundingBox.PMin; } else { return this.tgcSkeletalMesh.BoundingBox.PMin; }
        }

        public float FactorCorreccion()
        {
            if (tgcMesh != null) { return 10; } else { return -2; }
        }

        #endregion Comportamientos
    }
}