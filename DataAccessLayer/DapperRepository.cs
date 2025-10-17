using Dapper;
using DataAccessLayer;
using LogicLab;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

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
                var properties = typeof(T).GetProperties()
                    .Where(p => p.Name != "Id")
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