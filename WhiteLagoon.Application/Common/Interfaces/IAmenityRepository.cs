using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
	//Este é um repositório para as Amenities
	public interface IAmenityRepository : IRepositoryGeneric<Amenity>
	{
		void Update(Amenity entity);
	}
}

