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
        public static bool isPointInsideAABB(Vector3 point, TgcBoundingBox box)
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
            if (ControladorColisiones.isPointInsideAABB(esfera.Position, cuadrado))
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
    }
}
