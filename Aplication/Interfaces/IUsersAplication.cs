using domain.Entities;

namespace application.Interfaces
{
    public interface IUsersApplication
    {
        List<Users> Listar();
        void Guardar(Users entidad);
        void Modificar(Users entidad);
        void Borrar(Users entidad);
    }
}