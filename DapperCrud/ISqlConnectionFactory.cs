using System.Data.SqlClient;

namespace DapperCrud
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateSqlConnection();
    }
}
