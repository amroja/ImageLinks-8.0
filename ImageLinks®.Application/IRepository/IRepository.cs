using System.Linq.Expressions;
using System.Threading.Tasks;
namespace ImageLinks_.Application.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll(CancellationToken ct = default,Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false);
        Task<T> Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        void Add(T entity);
        bool Any(Expression<Func<T, bool>> filter);
        void Remove(T entity);
    }
}
