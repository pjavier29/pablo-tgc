using Microsoft.DirectX;
using System;
using TGC.Core.Utils;

namespace TGC.Group.Model.Utiles
{
    public class FuncionesMatematicas
    {
        #region Atributos

        private readonly Random aleatorio;
        private static FuncionesMatematicas instance;

        #endregion Atributos

        #region Constructores

        protected FuncionesMatematicas()
        {
            aleatorio = new Random();
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

        #endregion Constructores

        #region Comportamientos

        public double NumeroAleatorioDouble()
        {
            return aleatorio.NextDouble();
        }

        public float NumeroAleatorioFloat()
        {
            return (float)aleatorio.NextDouble();
        }

        public int NumeroAleatorioInt()
        {
            return aleatorio.Next();
        }

        public int NumeroAleatorioIntEntre(int minimo, int maximo)
        {
            if (minimo > maximo)
            {
                throw new Exception("El número mínimo no puede ser superior la número máximo");
            }
            return aleatorio.Next(minimo, maximo);
        }

        public double NumeroAleatorioDoubleEntre(double minimo, double maximo)
        {
            if (minimo > maximo)
            {
                throw new Exception("El número mínimo no puede ser superior la número máximo");
            }
            return (maximo - minimo) * NumeroAleatorioDouble() + minimo;
        }

        public float NumeroAleatorioFloatEntre(float minimo, float maximo)
        {
            if (minimo > maximo)
            {
                throw new Exception("El número mínimo no puede ser superior la número máximo");
            }
            return (maximo - minimo) * NumeroAleatorioFloat() + minimo;
        }

        public float DistanciaEntrePuntos(Vector3 origen, Vector3 destino)
        {
            return
                FastMath.Sqrt(FastMath.Pow2(destino.X - origen.X) + FastMath.Pow2(destino.Y - origen.Y) +
                              FastMath.Pow2(destino.Z - origen.Z));
        }

        /// <summary>
        ///     Determina si un punto esta dentro del un cuadrado, arma el cuadrado en base al punto esquina. Replica el punto
        ///     segun los ejes cartesianos
        /// </summary>
        /// <param name="punto"></param>
        /// <param name="esquina"></param>
        /// <returns></returns>
        public bool EstaDentroDelCuadrado(Vector3 punto, Vector3 esquina)
        {
            var xp = punto.X;
            var zp = punto.Z;
            var xe = esquina.X;
            var ze = esquina.Z;
            if (xp < 0)
            {
                xp *= -1;
            }
            if (zp < 0)
            {
                zp *= -1;
            }
            if (xe < 0)
            {
                xe *= -1;
            }
            if (ze < 0)
            {
                ze *= -1;
            }
            return xp < xe && zp < ze;
        }

        public float PorcentajeRelativo(float minimo, float maximo, float actual)
        {
            if (minimo > maximo)
            {
                throw new Exception("El número mínimo no puede ser superior la número máximo");
            }
            if (minimo > actual || actual > maximo)
            {
                throw new Exception(
                    "El número actual no puede ser superior la número máximo o inferior al número mímino");
            }
            return actual / (maximo - minimo);
        }

        #endregion Comportamientos
    }
}