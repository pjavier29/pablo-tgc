using TGC.Group.Model.Administracion;
using TGC.Group.Model.ElementosJuego;
using TGC.Group.Model.Utiles;

namespace TGC.Group.Model.Comandos
{
    public class Golpear : Comando
    {
        #region Constructores

        public Golpear(string tipoDeGolpe)
        {
            GolpeActual = tipoDeGolpe;
            momentoUltimoGolpe = 0;
            elementoEnColision = null;
        }

        #endregion Constructores

        #region Propiedades

        public string GolpeActual { get; set; }

        #endregion Propiedades

        #region Atributo;

        private float momentoUltimoGolpe;
        private Elemento elementoEnColision;

        #endregion Atributo;

        #region Constantes

        public const string Patear = "Patear";
        public const string Pegar = "Pegar";

        #endregion Constantes

        #region Comportamientos

        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            float alcance = 0;
            float fuerzaGolpe = 0;

            BuscarColisionConGolpe(contexto);

            if (GolpeActual.Equals(Patear))
            {
                if (elementoEnColision != null)
                {
                    contexto.sonidoGolpePatada.play(false);
                    elementoEnColision.GenerarVibracion(contexto.tiempo);
                }
                contexto.personaje.mesh.playAnimation("Patear", true);
                alcance = contexto.personaje.alcancePatada();
                fuerzaGolpe = contexto.personaje.fuerzaPatada();
            }

            if (GolpeActual.Equals(Pegar))
            {
                if (elementoEnColision != null)
                {
                    contexto.sonidoGolpe.play(false);
                    elementoEnColision.GenerarVibracion(contexto.tiempo);
                }
                contexto.personaje.mesh.playAnimation("Pegar", true);
                alcance = contexto.personaje.alcanceGolpe();
                fuerzaGolpe = contexto.personaje.fuerzaGolpe();
            }

            //Si golpeo un obstáculo deberé esperar 2 segundos para poder golpearlo nuevamente
            if (PuedeGolpear(contexto.tiempo) && elementoEnColision != null)
            {
                //Buscamos si esta al alcance alguno de los obstáculos
                //foreach (Elemento elem in contexto.optimizador.ElementosColision)
                //{
                //if (ControladorColisiones.EsferaColisionaCuadrado(contexto.personaje.GetAlcanceInteraccionEsfera(), elem.BoundingBox()))
                //{
                //Si golpeo actualizamos el tiempo local
                momentoUltimoGolpe = contexto.tiempo;
                elementoEnColision.recibirDanio(fuerzaGolpe, contexto.tiempo);
                if (elementoEnColision.estaDestruido())
                {
                    if (!elementoEnColision.destruccionTotal())
                    {
                        foreach (var obs in elementoEnColision.elementosQueContiene())
                        {
                            //TODO. Aplicar algun algoritmo de dispersion copado
                            obs.posicion(elementoEnColision.posicion());
                            contexto.elementos.Add(obs);
                        }
                    }
                    elementoEnColision.liberar();
                    contexto.elementos.Remove(elementoEnColision);
                    contexto.optimizador.ForzarActualizacionElementosColision();
                }

                //En principio solo se puede golpear un obstaculo a la vez.
                // break;
                //}
                //}
            }
        }

        private bool PuedeGolpear(float tiempo)
        {
            if (momentoUltimoGolpe == 0)
            {
                return true;
            }
            return tiempo - momentoUltimoGolpe > 1.5f;
        }

        private void BuscarColisionConGolpe(SuvirvalCraft contexto)
        {
            elementoEnColision = null;
            //Buscamos si esta al alcance alguno de los obstáculos
            foreach (var elem in contexto.optimizador.ElementosColision)
            {
                if (ControladorColisiones.EsferaColisionaCuadrado(contexto.personaje.GetAlcanceInteraccionEsfera(),
                    elem.BoundingBox()))
                {
                    elementoEnColision = elem;
                    //En principio solo se puede golpear un obstaculo a la vez.
                    break;
                }
            }
        }

        #endregion Comportamientos
    }
}