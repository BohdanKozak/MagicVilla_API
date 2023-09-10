using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, bool tracked = true, string? includeProperties = null);

        Task RemoveAsync(T entity);
        Task SaveAsync();

    }
}
