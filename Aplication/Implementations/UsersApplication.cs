using Aplication.Interfaces;
using domain.Entities;
using repository.Implementations;

namespace Aplication.Implementations
{
    public class UsersApplication: IUsersApplication
    {
        private readonly DbConexion _conexion;

        public UsersApplication(DbConexion conexion)
        {
            _conexion = conexion;
        }

        public List<Users> Listar()
        {
            return _conexion.Users
                .Where(user => user.IsActive)
                .ToList();
        }

        public void Guardar(Users entidad)
        {
            _conexion.Add(entidad);
            _conexion.SaveChanges();
        }

        public void Modificar(Users entidad)
        {
            _conexion.Update(entidad);
            _conexion.SaveChanges();
        }

        public void Borrar(Users entidad)
        {
            entidad.IsActive = false;
            _conexion.Update(entidad);
            _conexion.SaveChanges();
        }
    }
}
