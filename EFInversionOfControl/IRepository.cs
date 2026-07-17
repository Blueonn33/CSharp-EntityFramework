using System.Linq.Expressions;

namespace EFInversionOfControl
{
    public interface IRepository
    {
        // with Entity Tracker
        IQueryable<T> All<T>()
            where T : class;

        // without Entity Tracker
        IQueryable<T> AllReadOnly<T>()
            where T : class;

        Task<T?> GetById<T>(object id)
            where T : class;

        Task AddAsync<T>(T entity)
            where T : class;

        Task AddRangeAsync<T>(IEnumerable<T> entities)
            where T : class;

        void Delete<T>(T entity)
            where T : class;

        Task ExecuteBulkDelete<T>(Expression<Func<T, bool>> filter)
            where T : class;

        Task<int> SaveChangesAsync();
    }
}
