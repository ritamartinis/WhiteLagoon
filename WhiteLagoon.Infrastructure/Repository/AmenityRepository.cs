using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
	//Cada vez que preciso de adicionar, atualizar ou remover algo da BD, faço através do repositório genérico. Aqui é só para updates
	public class AmenityRepository : RepositoryGeneric<Amenity>, IAmenityRepository
	{
		//Ligação à BD
		private readonly ApplicationDbContext _db;

		public AmenityRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Amenity entity)
		{
			_db.Update(entity);
		}
	}
}
