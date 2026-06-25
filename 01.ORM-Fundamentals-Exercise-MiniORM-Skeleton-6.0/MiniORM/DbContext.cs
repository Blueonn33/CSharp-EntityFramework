using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        private IEnumerable<TEntity> LoadTableEntities<TEntity>()
            where TEntity : class, new()
        {
            string tableName = GetTableName(typeof(TEntity));
            string[] entityColumns = GetEntityTableColumns(typeof(TEntity))
                .ToArray();

            IEnumerable<TEntity> fetchedEntityRows = this.connection
                .FetchResultSet<TEntity>(tableName, entityColumns);

            return fetchedEntityRows;
        }

        private string GetTableName(Type entityType)
        {
            string? tableName = entityType.GetCustomAttribute<TableAttribute>()?.Name;

            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableName = this.dbSetProperties[entityType].Name;
            }

            return tableName;
        }

        private IEnumerable<string> GetEntityTableColumns(Type entityType)
        {
            string tableName = GetTableName(entityType);
            IEnumerable<string> dbColumnNames = this.connection
                .FetchColumnNames(tableName);

            IEnumerable<string> entityColumnNames = entityType
                .GetProperties()
                .Where(pi =>
                    dbColumnNames.Contains(pi.Name) && pi.HasAttribute<NotMappedAttribute>() &&
                    AllowedSqlTypes.Contains(pi.PropertyType))
                .Select(pi => pi.Name)
                .ToArray();

            return entityColumnNames;
        }

        private void MapAllRelations()
        {
            foreach ((Type dbSetType, PropertyInfo dbSetProperty) in this.dbSetProperties)
            {
                object? dbSetInstance = dbSetProperty.GetValue(this);

                if (dbSetInstance == null)
                {
                    throw new InvalidOperationException($"DbSet<{dbSetType.Name}> cannot be null");
                }

                MethodInfo mapRelationsMethod = typeof(DbContext)
                    .GetMethod(nameof(MapRelations), BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(dbSetType) ?? throw new InvalidOperationException("A general error occured while initializing DbSet data!");

                mapRelationsMethod.Invoke(this, new object?[] { dbSetInstance });
            }
        }

        private void MapRelations<TEntity>(DbSet<TEntity> dbSetInstance)
            where TEntity : class, new()
        {
            Type entityType = typeof(TEntity);

            MapNavigationProperties(dbSetInstance);

            PropertyInfo[] navigationCollections = entityType
                .GetProperties()
                .Where(pi => pi.PropertyType.IsGenericType &&
                             pi.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                .ToArray();

            foreach (PropertyInfo navCollectionProp in navigationCollections)
            {
                Type collectionEntityType = navCollectionProp
                    .PropertyType
                    .GetGenericArguments()
                    .Single();

                MethodInfo mapCollectionMethod = typeof(DbContext)
                    .GetMethod(nameof(MapCollection), BindingFlags.Instance | BindingFlags.NonPublic)?
                    .MakeGenericMethod(entityType, collectionEntityType) ?? throw new InvalidOperationException("A general error occured while initializing DbSet data!");

                mapCollectionMethod.Invoke(this, new object?[] { dbSetInstance, navCollectionProp });
            }
        }

        private void MapNavigationProperties<TEntity>(DbSet<TEntity> dbSetInstance)
            where TEntity : class, new()
        {
            Type entityType = typeof(TEntity);
            PropertyInfo[] foreignKeyProps = entityType
                .GetProperties()
                .Where(pi => pi.HasAttribute<ForeignKeyAttribute>())
                .ToArray();

            foreach (PropertyInfo foreignKeyPropInfo in foreignKeyProps)
            {
                string navPropertyName = foreignKeyPropInfo
                    .GetCustomAttribute<ForeignKeyAttribute>()!.Name;
                PropertyInfo navPropertyInfo = entityType
                                                   .GetProperty(navPropertyName) ??
                                               throw new InvalidOperationException(
                                                   $"Nav prop: {navPropertyName} not found on entity type {entityType.Name}");
                if (navPropertyInfo == null)
                {
                    continue;
                }

                object navEntityDbSet = this.dbSetProperties[navPropertyInfo.PropertyType]
                                            .GetValue(this) ??
                                        throw new InvalidOperationException(
                                            $"DbSet<{navPropertyInfo}> is not initialized");

                PropertyInfo navEntityPrimaryKeyProp = navPropertyInfo.PropertyType
                    .GetProperties()
                    .Single(pi => pi.HasAttribute<KeyAttribute>());

                foreach (TEntity entity in dbSetInstance)
                {
                    object? foreignKeyValue = foreignKeyPropInfo.GetValue(entity);
                }
            }
        }

        private void MapCollection<TEntity, TCollection>(DbSet<TEntity> dbSetInstance, PropertyInfo navCollectionPropInfo)
            where TEntity : class, new()
            where TCollection : class, new()
        {

        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
