using Microsoft.DirectX;
using TGC.Core.SceneLoader;
using TGC.Core.SkeletalAnimation;

namespace TGC.Group.Model.Utiles
{
    public class MallaEnvoltura
    {
        #region Atributos

        private readonly TgcMesh tgcMesh;
        private readonly TgcSkeletalMesh tgcSkeletalMesh;

        #endregion Atributos

        #region Contructores

        public MallaEnvoltura(TgcMesh mesh)
        {
            tgcMesh = mesh;
            tgcSkeletalMesh = null;
        }

        public MallaEnvoltura(TgcSkeletalMesh mesh)
        {
            tgcSkeletalMesh = mesh;
            tgcMesh = null;
        }

        #endregion Contructores

        #region Comportamientos

        public void Render()
        {
            if (tgcMesh != null)
            {
                tgcMesh.render();
            }
            else
            {
                tgcSkeletalMesh.render();
            }
        }

        public Vector3 Posicion()
        {
            if (tgcMesh != null)
            {
                return tgcMesh.Position;
            }
            return tgcSkeletalMesh.Position;
        }

        public void Posicion(Vector3 nuevaPosicion)
        {
            if (tgcMesh != null)
            {
                tgcMesh.Position = nuevaPosicion;
            }
            else
            {
                tgcSkeletalMesh.Position = nuevaPosicion;
            }
        }

        public Vector3 MinimoPunto()
        {
            if (tgcMesh != null)
            {
                return tgcMesh.BoundingBox.PMin;
            }
            return tgcSkeletalMesh.BoundingBox.PMin;
        }

        public float FactorCorreccion()
        {
            if (tgcMesh != null)
            {
                return 10;
            }
            return -2;
        }

        #endregion Comportamientos
    }
}