using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
    //Este é um repositório para as Villas Numbers
    public interface IVillaNumberRepository : IRepositoryGeneric<VillaNumber>
    {
        void Update(VillaNumber entity);
    }
}
