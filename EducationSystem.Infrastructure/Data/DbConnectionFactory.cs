using Microsoft.Data.SqlClient;
using System.Data;

namespace EducationSystem.Infrastructure.Data
{
    public sealed class DbConnectionFactory : IDbConnectionFactory
    {
        private const string DefaultConnectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EducationSystemDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";

        private readonly string _connectionString;

        public DbConnectionFactory()
            : this(DefaultConnectionString)
        {
        }

        public DbConnectionFactory(string connectionString)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
