using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.PabloTGC.Utiles
{
    public class ControladorColisiones
    {
        public static bool IsPointInsideAABB(Vector3 point, TgcBoundingBox box)
        {
            return (point.X >= box.PMin.X && point.X <= box.PMax.X) &&
                   (point.Y >= box.PMin.Y && point.Y <= box.PMax.Y) &&
                   (point.Z >= box.PMin.Z && point.Z <= box.PMax.Z);
        }

        public static bool EsferaColisionaCuadrado(TgcSphere esfera, TgcBoundingBox cuadrado)
        {
            // Hacemos el test
            float s = 0;
            float d = 0;

            // Comprobamos si el centro de la esfera está dentro del AABB
            if (ControladorColisiones.IsPointInsideAABB(esfera.Position, cuadrado))
            {
                return true;
            }

            // Comprobamos si la esfera y el AABB se intersectan
            if (esfera.Position.X < cuadrado.PMin.X)
            {
                s = esfera.Position.X - cuadrado.PMin.X;
                d += s * s;
            }
            else if (esfera.Position.X > cuadrado.PMax.X)
            {
                s = esfera.Position.X - cuadrado.PMax.X;
                d += s * s;
            }

            if (esfera.Position.Y < cuadrado.PMin.Y)
            {
                s = esfera.Position.Y - cuadrado.PMin.Y;
                d += s * s;
            }
            else if (esfera.Position.Y > cuadrado.PMax.Y)
            {
                s = esfera.Position.Y - cuadrado.PMax.Y;
                d += s * s;
            }

            if (esfera.Position.Z < cuadrado.PMin.Z)
            {
                s = esfera.Position.Z - cuadrado.PMin.Z;
                d += s * s;
            }
            else if (esfera.Position.Z > cuadrado.PMax.Z)
            {
                s = esfera.Position.Z - cuadrado.PMax.Z;
                d += s * s;
            }

            return (d <= esfera.Radius * esfera.Radius);
        }

        /// <summary>
        /// Este método verifica la colision entre dos cuadrados pero proyectanto los cuadrados, asumiendo que ambos estan alineados sobre un eje.
        /// </summary>
        /// <param name="cuadrado1"></param>
        /// <param name="cuadrado2"></param>
        /// <returns></returns>
        public static bool CuadradoColisionaCuadrano(TgcBoundingBox cuadrado1, TgcBoundingBox cuadrado2)
        {
            return (cuadrado1.PMin.X <= cuadrado2.PMax.X && cuadrado1.PMax.X >= cuadrado2.PMin.X) &&
                   (cuadrado1.PMin.Y <= cuadrado2.PMax.Y && cuadrado1.PMax.Y >= cuadrado2.PMin.Y) &&
                   (cuadrado1.PMin.Z <= cuadrado2.PMax.Z && cuadrado1.PMax.Z >= cuadrado2.PMin.Z);
        }

        public static bool FrustumColisionaCuadrado(TgcFrustum frustum, TgcBoundingBox boundingBox)
        {
            TgcCollisionUtils.FrustumResult c = TgcCollisionUtils.classifyFrustumAABB(frustum, boundingBox);
            return (c == TgcCollisionUtils.FrustumResult.INSIDE || c == TgcCollisionUtils.FrustumResult.INTERSECT);
        }

    }
}
