using System.Data;
using Laquila.Integrations.Core.Infra.Interfaces;
using Microsoft.Data.SqlClient;

namespace Laquila.Integrations.Core.Infra.Repositories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}