using AlumnoEjemplos.MiGrupo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Terrain;

namespace AlumnoEjemplos.PabloTGC.Comandos
{
    public interface Comando
    {
        //TODO. Deberia recibir como paramtro otro objeto que no sea el SuvirvalCraft!!!!
        void Ejecutar(SuvirvalCraft contexto, float elapsedTime);
    }
}
