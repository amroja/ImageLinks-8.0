using ImageLinks_.Application.IRepository;
using ImageLinks_.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ImageLinks_.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public bool Any(Expression<Func<T, bool>> filter)
        {
            return dbSet.Any(filter);
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return await query.FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<List<T>> GetAll(CancellationToken ct, Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false)
        {
            try
            {
                IQueryable<T> query;
                if (tracked)
                {
                    query = dbSet;
                }
                else
                {
                    query = dbSet.AsNoTracking();
                }
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProp in includeProperties
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp.Trim());
                    }
                }
                return await query.ToListAsync(ct);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use any logging framework you prefer)
                Console.WriteLine($"An error occurred while fetching data: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }
        }
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}