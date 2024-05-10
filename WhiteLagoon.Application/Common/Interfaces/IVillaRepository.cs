using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
    //Este é um repositório para as Villas
    public interface IVillaRepository : IRepositoryGeneric<Villa>
    {
        void Update(Villa entity);
    }
}
