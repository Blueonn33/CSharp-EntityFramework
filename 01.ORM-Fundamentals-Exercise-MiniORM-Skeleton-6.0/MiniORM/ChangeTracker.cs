using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MiniORM
{
    public class ChangeTracker<T>
        where T : class, new()

    {
        private ICollection<T> allEntities;
        private ICollection<T> added;
        private ICollection<T> removed;

        public ChangeTracker(IEnumerable<T> entities)
        {
            this.added = new List<T>();
            this.removed = new List<T>();

            this.allEntities = CloneEntities(entities);
        }

        public IReadOnlyCollection<T> AllEntities
            => this.allEntities.ToList().AsReadOnly();

        public IReadOnlyCollection<T> Added
            => this.added.ToList().AsReadOnly();

        public IReadOnlyCollection<T> Removed
            => this.removed.ToList().AsReadOnly();

        public void Add(T entity)
        {
            this.added.Add(entity);
        }

        public void Remove(T entity)
        {
            this.removed.Add(entity);
        }

        public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet)
        {
            ICollection<T> modifiedEntities = new List<T>();

            PropertyInfo[] primaryKeys = typeof(T)
                .GetProperties()
                .Where(p => p.HasAttribute<KeyAttribute>())
                .ToArray();

            foreach (T proxyEntity in AllEntities)
            {
                object[] primaryKeyValues = GetPrimaryKeyValues(primaryKeys, proxyEntity);


            }
        }

        private static ICollection<T> CloneEntities(IEnumerable<T> entities)
        {
            ICollection<T> clonedEntities = new List<T>();
            PropertyInfo[] propertiesToClone = typeof(T)
                .GetProperties()
                .Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
                .ToArray();

            foreach (T entity in entities)
            {
                T clonedEntity = Activator.CreateInstance<T>();

                foreach (PropertyInfo cloneableProp in propertiesToClone)
                {
                    object? propValue = cloneableProp.GetValue(entity);
                    cloneableProp.SetValue(clonedEntity, propValue
                    );
                }

                clonedEntities.Add(clonedEntity);
            }

            return clonedEntities;
        }

        private static IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKeyProperties, T entity)
            => primaryKeyProperties
                .Select(pk => pk.GetValue(entity)!)
                .ToArray();
    }
}