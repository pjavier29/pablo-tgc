using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.PabloTGC.Utiles;
using AlumnoEjemplos.PabloTGC.ElementosJuego.Instrumentos;
using AlumnoEjemplos.PabloTGC.Administracion;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public class EncenderAntorcha : Comando
    {
        public void Ejecutar(SuvirvalCraft contexto, float elapsedTime)
        {
            if (! contexto.efectoTerreno.ContieneElementoDeTipo(Elemento.Antorcha))
            {
                //Si algun efecto contiene una antorcha sabemos entonces que hay ya antocha y no hay que agregar más
                Antorcha antorcha = new Antorcha();
                contexto.personaje.antorcha = antorcha;
                //antorcha.SetPosicion(contexto.personaje.mesh.Position);
                //TODO. ver si es la mejor forma de manejar los elementos de iluminacion
                contexto.efectoTerreno.AgregarElementoDeIluminacion(new ElementoIluminacion(antorcha, 1500));
                contexto.efectoLuz.AgregarElementoDeIluminacion(new ElementoIluminacion(antorcha, 1500));
                contexto.efectoAlgas.AgregarElementoDeIluminacion(new ElementoIluminacion(antorcha, 1500));
                contexto.efectoAlgas2.AgregarElementoDeIluminacion(new ElementoIluminacion(antorcha, 1500));
                contexto.efectoBotes.AgregarElementoDeIluminacion(new ElementoIluminacion(antorcha, 1500));
                //TODO+++++++++++++++++++++++++++++++++

            }
        }
    }
}
