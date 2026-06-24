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

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

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
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
