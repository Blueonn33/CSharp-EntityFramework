using System.Reflection;

namespace MiniORM
{
    public class ChangeTracker<T>
        where T : class, new()

    {
        private ICollection<T> allEntities;
        private ICollection<T> added;
        private ICollection<T> removed;

        private ChangeTracker()
        {
            this.added = new List<T>();
            this.removed = new List<T>();
        }
        public ChangeTracker(IEnumerable<T> entities) : this()
        {
            this.allEntities = CloneEntities(entities);
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
    }
}