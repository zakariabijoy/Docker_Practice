using System.Data;
using System.Data.SqlClient;

namespace Api.Data.Context;

public class DapperContext
{
    private readonly IConfiguration _configuration;

    public DapperContext(IConfiguration configuration) => _configuration = configuration;

    public IDbConnection GetDbConnection() => new SqlConnection(_configuration["ConnectionString"]);

}
