using Dapper;
using DataAccessLayer;
using LogicLab;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LogicLibrary
{
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public DapperRepository(string tableName, string connectionString)
        {
            _connectionString = connectionString;
            _tableName = tableName;
        }

        public void Add(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Если ID = 0, генерируем новый
                if (entity.Id == 0)
                {
                    entity.Id = GetNextId(connection);
                }

                var properties = typeof(T).GetProperties()
                    .Where(p => p.Name != "Id") // Исключаем Id если он автоинкрементный
                    .Select(p => p.Name);

                var columns = string.Join(", ", properties);
                var parameters = string.Join(", ", properties.Select(p => "@" + p));

                var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({parameters})";
                connection.Execute(sql, entity);
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var affectedRows = connection.Execute(
                    $"DELETE FROM {_tableName} WHERE Id = @Id",
                    new { Id = id });

                return affectedRows > 0;
            }
        }

        public IEnumerable<T> ReadAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<T>($"SELECT * FROM {_tableName}");
            }
        }

        public T ReadById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<T>(
                    $"SELECT * FROM {_tableName} WHERE Id = @Id",
                    new { Id = id });
            }
        }

        private int GetNextId(SqlConnection connection)
        {
            var maxId = connection.ExecuteScalar<int?>($"SELECT MAX(Id) FROM {_tableName}");
            return (maxId ?? 0) + 1;
        }

        public void Update(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var properties = typeof(T).GetProperties()
                    .Where(p => p.Name != "Id")
                    .Select(p => $"{p.Name} = @{p.Name}");

                var setClause = string.Join(", ", properties);
                var sql = $"UPDATE {_tableName} SET {setClause} WHERE Id = @Id";
                connection.Execute(sql, entity);
            }
        }

        public void SaveChanges()
        {

        }
    }
}