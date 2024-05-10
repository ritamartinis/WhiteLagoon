using System.Linq.Expressions;

namespace WhiteLagoon.Application.Common.Interfaces
{
    //A Letra T refere-se a algo generico, comummente
    public interface IRepositoryGeneric<T> where T : class 
    {
        //Este nome é um nome qualquer que nós queremos dar. no caso, demos "getall"
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);            //nome da var é "filter" e o valor por defeito é null.
        //Pode vir a nulo e, então, recebe todas as vilas. se o filter não for nulo, recebe o id. se a string não for nula, recebe o include.   //expression é um tipo de dados

        T Get(Expression<Func<T, bool>>? filter, string? includeProperties = null);

        void Add(T entity);

        bool Any(Expression<Func<T, bool>> filter);
        void Remove(T entity);

        //o professor não gosta de usar o Update e o Save num Interface genérico, então prefere fazer nos interfaces específicos do modelo.
        //Aqui, ficam estes.
    }
}
