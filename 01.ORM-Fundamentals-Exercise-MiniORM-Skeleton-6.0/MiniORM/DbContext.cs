using System.Reflection;

namespace MiniORM
{
    public abstract class DbContext : IDisposable
    {
        private readonly DatabaseConnection connection;

        private readonly IDictionary<Type, PropertyInfo> dbSetProperties;

        internal static readonly Type[] AllowedSqlTypes =
        {
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(bool),
            typeof(string),
            typeof(char),
            typeof(DateTime)
        };

        public DbContext(string connectionString)
        {
            this.connection = new DatabaseConnection(connectionString);
            this.dbSetProperties = this.DiscoverDbSetProperties();

            using (ConnectionManager connectionManager = new ConnectionManager(this.connection))
            {
                InitializeDbSets();
            }

            MapAllRelations();
        }

        private IDictionary<Type, PropertyInfo> DiscoverDbSetProperties()
            => this.GetType()
                .GetProperties()
                .Where(pi => pi.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .ToDictionary(pi => pi.PropertyType.GetGenericArguments().Single(), pi => pi);

        private void InitializeDbSets()
        {
            foreach (KeyValuePair<Type, PropertyInfo> dbSetKvp in this.dbSetProperties)
            {
                Type dbSetType = dbSetKvp.Key;
                PropertyInfo dbSetProperty = dbSetKvp.Value;

                MethodInfo populateDbSetMethod = typeof(DbContext)
                    .GetMethod(nameof(PopulateDbSet), BindingFlags.Instance | BindingFlags.NonPublic)?
                    .MakeGenericMethod(dbSetType) ?? throw new InvalidOperationException("A general error occured while initializing DbSet data!");

                populateDbSetMethod.Invoke(this, new[] { dbSetProperty });
            }
        }

        private void PopulateDbSet<TEntity>(PropertyInfo dbSetProperty)
            where TEntity : class, new()
        {
            IEnumerable<TEntity> entities = LoadTableEntities<TEntity>();
            DbSet<TEntity> dbSetInstance = new DbSet<TEntity>(entities);

            ReflectionHelper.ReplaceBackingField(this, dbSetProperty.Name, dbSetInstance);
        }



        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
