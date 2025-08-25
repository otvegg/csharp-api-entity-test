using workshop.wwwapi.Models;

namespace workshop.wwwapi.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> Get();
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
        Task<T?> Delete(object id);
        Task Save();
        Task<T?> GetById(int id);
        Task<T?> GetById(int id1, int id2, Func<IQueryable<T>, IQueryable<T>> includeQuery);
        Task<T?> GetById(int id, Func<IQueryable<T>, IQueryable<T>> includeQuery);
        Task<IEnumerable<T>> GetWithIncludes(Func<IQueryable<T>, IQueryable<T>> includeQuery);


    }
}
