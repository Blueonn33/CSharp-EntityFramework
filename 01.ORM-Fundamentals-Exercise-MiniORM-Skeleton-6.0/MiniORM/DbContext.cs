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
            foreach (var in this.dbSetProperties)
            {

            }
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
