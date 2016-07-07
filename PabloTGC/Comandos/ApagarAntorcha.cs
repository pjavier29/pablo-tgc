using AlumnoEjemplos.PabloTGC.Administracion;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class ApagarAntorcha : Comando
    {
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (contexto.personaje.TieneAntorchaSeleccionada())
            {
                contexto.personaje.antorcha.Desactivar();
                contexto.efectoTerreno.EliminarElementoDeIluminacion(contexto.personaje.antorcha);
                contexto.efectoLuz.EliminarElementoDeIluminacion(contexto.personaje.antorcha);
                contexto.efectoAlgas.EliminarElementoDeIluminacion(contexto.personaje.antorcha);
                contexto.efectoAlgas2.EliminarElementoDeIluminacion(contexto.personaje.antorcha);
                contexto.efectoBotes.EliminarElementoDeIluminacion(contexto.personaje.antorcha);
                contexto.efectoArbol.EliminarElementoDeIluminacion(contexto.personaje.antorcha);
                contexto.efectoArbol2.EliminarElementoDeIluminacion(contexto.personaje.antorcha);
                contexto.efectoLuz2.EliminarElementoDeIluminacion(contexto.personaje.antorcha);
            }
        }
    }
}
