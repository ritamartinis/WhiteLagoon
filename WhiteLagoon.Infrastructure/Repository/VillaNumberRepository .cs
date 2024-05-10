using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{

    //Cada vez que preciso de adicionar, atualizar ou remover algo da BD, faço através do repositório.
    public class VillaNumberRepository : RepositoryGeneric<VillaNumber>, IVillaNumberRepository
    {
        //Ligação à BD
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(VillaNumber entity)
        {
            _db.Update(entity);
        }
    }
}
