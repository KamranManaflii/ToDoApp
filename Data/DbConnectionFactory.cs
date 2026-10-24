using Microsoft.Data.SqlClient;
using System.Data;

namespace AspNetCoreMvc.Data;

public class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Default")!;
    }

    public IDbConnection Create() => new SqlConnection(_connectionString);
}
