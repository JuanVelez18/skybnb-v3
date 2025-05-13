using domain.Entities;
namespace lib_aplicaciones.Interfaces
{
    public interface IRolesAplication
    {
        void Configuration(string StringConexion);
        List<Users> PorEstudiante(Users? entidad);
        List<Users> Listar();
        Users? Guardar(Users? entidad);
        Users? Modificar(Users? entidad);
        Users? Borrar(Users? entidad);
    }
}