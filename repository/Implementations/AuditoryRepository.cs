using domain.Entities;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Implementations
{
    public class AuditoryRepository: BaseRepository<Auditories, long>, IAuditoryRepository
    {
        public AuditoryRepository(DbConexion conexion) : base(conexion)
        {
        }
    }
}
