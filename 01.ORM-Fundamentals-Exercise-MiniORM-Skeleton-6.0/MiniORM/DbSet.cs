using System.Collections;

namespace MiniORM
{
    public class DbSet<TEntity> : ICollection<TEntity>
        where TEntity : class, new()
    {
        internal DbSet(IEnumerable<TEntity> entities)
        {
            this.Entities = entities.ToList();
            this.ChangeTracker = new ChangeTracker<TEntity>(entities);
        }

        internal IList<TEntity> Entities
        {
            get;
        }

        internal ChangeTracker<TEntity> ChangeTracker
        {
            get;
        }

        public int Count
            => this.Entities.Count();

        public bool IsReadOnly
            => this.Entities.IsReadOnly;

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Added entity cannot be null");
            }

            this.Entities.Add(entity);

            this.ChangeTracker.Add(entity);
        }

        public bool RemoveRange(IEnumerable<TEntity> entities)
        {
            bool result = true;

            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), "Entity collection to remove cannot be null.");
            }

            foreach (TEntity entity in entities)
            {
                result &= this.Remove(entity);
            }

            return result;
        }

        public void Clear()
        {
            while (this.Count > 0)
            {
                TEntity entityToRemove = this.Entities.First();
                this.Remove(entityToRemove);
            }
        }

        public bool Contains(TEntity entity)
            => this.Entities.Contains(entity);

        public void CopyTo(TEntity[] array, int arrayIndex)
            => this.Entities.CopyTo(array, arrayIndex);

        public bool Remove(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Removed entity cannot be null");
            }

            bool removedSuccessfully = this.Entities.Remove(entity);

            if (removedSuccessfully)
            {
                this.ChangeTracker.Remove(entity);
            }

            return removedSuccessfully;
        }

        public IEnumerator<TEntity> GetEnumerator()
            => this.Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}