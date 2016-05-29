using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.PabloTGC.Utiles
{
    public class FuncionesMatematicas
    {
        #region Atributos
        private Random aleatorio;
        private static FuncionesMatematicas instance = null;
        #endregion

        #region Constructores
        protected FuncionesMatematicas()
        {
            this.aleatorio = new Random();
        }

        public static FuncionesMatematicas Instance
        {
            get
            {
                if (instance == null)
                    instance = new FuncionesMatematicas();

                return instance;
            }
        }
        #endregion

        #region Comportamientos
        public double NumeroAleatorioDouble()
        {
            return this.aleatorio.NextDouble();
        }

        public float NumeroAleatorioFloat()
        {
            return (float) this.aleatorio.NextDouble();
        }

        public double NumeroAleatorioDoubleEntre(double minimo, double maximo)
        {
            if (minimo > maximo)
            {
                throw new Exception("El número mínimo no puede ser superior la número máximo");
            }
            return ((maximo - minimo) * this.NumeroAleatorioDouble()) + minimo;
        }

        public float NumeroAleatorioFloatEntre(float minimo, float maximo)
        {
            if (minimo > maximo)
            {
                throw new Exception("El número mínimo no puede ser superior la número máximo");
            }
            return ((maximo - minimo) * this.NumeroAleatorioFloat()) + minimo;
        }

        public float DistanciaEntrePuntos(Vector3 origen, Vector3 destino)
        {
            return FastMath.Sqrt(FastMath.Pow2(destino.X - origen.X) + FastMath.Pow2(destino.Y - origen.Y) + FastMath.Pow2(destino.Z - origen.Z));
        }

        /// <summary>
        /// Determina si un punto esta dentro del un cuadrado, arma el cuadrado en base al punto esquina. Replica el punto segun los ejes cartesianos
        /// </summary>
        /// <param name="punto"></param>
        /// <param name="esquina"></param>
        /// <returns></returns>
        public bool EstaDentroDelCuadrado(Vector3 punto, Vector3 esquina)
        {
            float xp = punto.X;
            float zp = punto.Z;
            float xe = esquina.X;
            float ze = esquina.Z;
            if (xp < 0) { xp *= -1; }
            if (zp < 0) { zp *= -1; }
            if (xe < 0) { xe *= -1; }
            if (ze < 0) { ze *= -1; }
            return xp < xe && zp < ze;

        }
        #endregion
    }
}
