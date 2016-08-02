using TGC.Group.Model.Administracion;
using TGC.Group.Model.Utiles;

namespace TGC.Group.Model.Comandos
{
    public class EncenderAntorcha : Comando
    {
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (!contexto.personaje.TieneAntorchaSeleccionada())
            {
                //Seleccionamos la antorcha, sabemos que esta en la posicion 2 de las armas (eso esta muy feo)
                contexto.personaje.seleccionarInstrumentoManoDerecha(2);
                contexto.personaje.antorcha.Activar();
                contexto.efectoTerreno.AgregarElementoDeIluminacion(new ElementoIluminacion(
                    contexto.personaje.antorcha, 1500));
                contexto.efectoLuz.AgregarElementoDeIluminacion(new ElementoIluminacion(contexto.personaje.antorcha,
                    1500));
                contexto.efectoAlgas.AgregarElementoDeIluminacion(new ElementoIluminacion(contexto.personaje.antorcha,
                    1500));
                contexto.efectoAlgas2.AgregarElementoDeIluminacion(new ElementoIluminacion(contexto.personaje.antorcha,
                    1500));
                contexto.efectoBotes.AgregarElementoDeIluminacion(new ElementoIluminacion(contexto.personaje.antorcha,
                    1500));
                contexto.efectoArbol.AgregarElementoDeIluminacion(new ElementoIluminacion(contexto.personaje.antorcha,
                    1500));
                contexto.efectoArbol2.AgregarElementoDeIluminacion(new ElementoIluminacion(contexto.personaje.antorcha,
                    1500));
                contexto.efectoLuz2.AgregarElementoDeIluminacion(new ElementoIluminacion(contexto.personaje.antorcha,
                    1500));
            }
        }
    }
}