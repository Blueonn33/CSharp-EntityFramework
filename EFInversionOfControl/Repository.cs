using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EFInversionOfControl
{
    public class Repository : IRepository
    {
        private readonly DbContext context;

        public Repository(DbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            await context.Set<T>().AddRangeAsync(entities);
        }

        public IQueryable<T> All<T>() where T : class
        {
            return context.Set<T>();
        }

        public IQueryable<T> AllReadOnly<T>() where T : class
        {
            return context.Set<T>().AsNoTracking();
        }

        public async Task ExecuteBulkDelete<T>(Expression<Func<T, bool>> filter) where T : class
        {
            await context.Set<T>()
                .Where(filter)
                .ExecuteDeleteAsync();
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Set<T>().Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public async Task<T?> GetById<T>(object id) where T : class
        {
            return await context.Set<T>().FindAsync(id);
        }
    }
}