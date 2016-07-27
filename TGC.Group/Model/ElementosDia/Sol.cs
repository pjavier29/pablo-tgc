using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Core.SceneLoader;
using TGC.Core.Utils;
using TGC.Group.Model.Movimientos;
using TGC.Group.Model.Utiles;
using TGC.Group.Model.Utiles.Efectos;

namespace TGC.Group.Model.ElementosDia
{
    public class Sol
    {
        #region Constructores

        public Sol()
        {
            colorDeLuz = ColorValue.FromColor(Color.LightYellow);
            intensidadDeLuzSol = 1000f;
            atenuacionDeLuz = 0.1f;
            alturaPuestaSol = 0;
        }

        #endregion Constructores

        #region Propiedades

        public TgcMesh Mesh { get; set; }

        #endregion Propiedades

        #region Atributos

        private readonly ColorValue colorDeLuz;
        private readonly float atenuacionDeLuz;
        public float intensidadDeLuzSol;
        private MovimientoEliptico movimientoSol;
        private float alturaPuestaSol;
        private float atenuacionMaxima;
        private float intesidadLuzMinima;

        #endregion Atributos

        #region Comportamientos

        public void CrearMovimiento()
        {
            movimientoSol = new MovimientoEliptico(new Vector3(0f, 0f, 0f), new Vector3(12000f, 0f, 0f),
                new Vector3(0f, 5000f, 0f), Mesh);
            alturaPuestaSol = (Mesh.BoundingBox.PMax.Y - Mesh.BoundingBox.PMin.Y) / 2;
            //Lo colocamos en atributos porque son valores fijos, de esta forma nos evitamos hacer la cuenta en cada render
            //De un calculo que siempre dara igual.
            atenuacionMaxima = atenuacionDeLuz * movimientoSol.AlturaMaxima() / alturaPuestaSol;
            ;
            intesidadLuzMinima = intensidadDeLuzSol * alturaPuestaSol / movimientoSol.AlturaMaxima();
        }

        public void Actualizar(float valor)
        {
            movimientoSol.Actualizar(valor);
        }

        public void Iluminar(Vector3 posicionVision, Efecto efecto, ColorValue colorEmisor, ColorValue colorAmbiente,
            ColorValue colorDifuso, ColorValue colorEspecular, float especularEx)
        {
            efecto.GetEfectoShader().SetValue("lightColor", GetColorLuz());
            efecto.GetEfectoShader().SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(Mesh.Position));
            efecto.GetEfectoShader().SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(posicionVision));
            efecto.GetEfectoShader().SetValue("lightIntensity", IntensidadDeLuz());
            efecto.GetEfectoShader().SetValue("lightAttenuation", Atenuacion());
            efecto.GetEfectoShader().SetValue("materialEmissiveColor", colorEmisor);
            efecto.GetEfectoShader().SetValue("materialAmbientColor", colorAmbiente);
            efecto.GetEfectoShader().SetValue("materialDiffuseColor", colorDifuso);
            efecto.GetEfectoShader().SetValue("materialSpecularColor", colorEspecular);
            efecto.GetEfectoShader().SetValue("materialSpecularExp", especularEx);
        }

        public float Atenuacion()
        {
            if (EsDeNoche())
            {
                return atenuacionMaxima;
            }
            return atenuacionDeLuz * movimientoSol.AlturaMaxima() / Mesh.Position.Y;
        }

        public float IntensidadDeLuz()
        {
            if (EsDeNoche())
            {
                return intesidadLuzMinima;
            }
            return intensidadDeLuzSol * Mesh.Position.Y / movimientoSol.AlturaMaxima();
        }

        /// <summary>
        ///     retorna la intendidad actual de la luz pero llevada a porcentaje entre 0 y 1
        /// </summary>
        /// <returns></returns>
        public float IntensidadRelativa()
        {
            return FuncionesMatematicas.Instance.PorcentajeRelativo(intesidadLuzMinima, intensidadDeLuzSol,
                IntensidadDeLuz());
        }

        public ColorValue GetColorLuz()
        {
            return colorDeLuz;
        }

        public bool EsDeDia()
        {
            return Mesh.Position.Y > alturaPuestaSol;
        }

        public bool EsDeNoche()
        {
            return !EsDeDia();
        }

        public void ActualizarIntensidadMaximaLuz(float nuevaIntensidad)
        {
            intensidadDeLuzSol = nuevaIntensidad;
            intesidadLuzMinima = intensidadDeLuzSol * alturaPuestaSol / movimientoSol.AlturaMaxima();
        }

        public ColorValue GetColorAmanecerAnochecer()
        {
            var color = new ColorValue();
            if (Mesh.BoundingBox.PMax.Y > 0 && EsDeNoche())
            {
                color.Red = 1f;
                color.Green = 0f;
                color.Blue = 0f;
                return color;
            }
            var aux = Mesh.Position.Y - alturaPuestaSol;
            //Si despues de salir el sol su altura no supera mas de 200 la puesta del sol
            if (aux > 0 && aux < 300f)
            {
                color.Red = 0.7f;
                color.Green = 0.7f;
                color.Blue = 0f;
                return color;
            }
            color.Red = 0f;
            color.Green = 0f;
            color.Blue = 0f;
            return color;
        }

        #endregion Comportamientos
    }
}