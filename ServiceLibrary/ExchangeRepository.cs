using Dapper;
using Npgsql;

namespace ServiceLibrary
{
    public class ExchangeRepository
    {
        public async Task<Exchange?> GetByIdAsync(int Id)
        {
         using (var connection = new NpgsqlConnection("Host=eu-west-1.rdsw.test.morningstar.com;Port=1234;Database=praqw;Username=pwda;Password=Password"))
         {
            connection.Open();
            const string sql = "SELECT Id, MicCodes FROM exchanges WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Exchange>(sql, new { Id });
         }
        }
    }

}