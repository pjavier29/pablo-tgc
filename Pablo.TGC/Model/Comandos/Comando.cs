using TGC.Group.Model.Administracion;

namespace TGC.Group.Model.Comandos
{
    public interface Comando
    {
        //TODO. Deberia recibir como paramtro otro objeto que no sea el SuvirvalCraft!!!!
        void Ejecutar(SuvirvalCraft contexto, float elapsedTime);
    }
}