using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DapperCrud
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
