using domain.Entities;

namespace Aplication.Interfaces
{
    public interface IUsersApplication
    {
        List<Users> Listar();
        void Guardar(Users entidad);
        void Modificar(Users entidad);
        void Borrar(Users entidad);
    }
}