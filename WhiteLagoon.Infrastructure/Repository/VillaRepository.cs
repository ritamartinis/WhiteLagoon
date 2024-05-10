using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{

    //Cada vez que preciso de adicionar, atualizar ou remover algo da BD, faço através do repositório genérico. Aqui é só para updates
    public class VillaRepository : RepositoryGeneric<Villa>, IVillaRepository
    {
        //Ligação à BD
        private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Villa entity)
        {
            _db.Update(entity);
        }
    }
}
