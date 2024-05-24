using Dapper;
using Npgsql;
using System.Data;

namespace ServiceLibrary
{
   /*
    * You MUST NOT make this class and it's members non-static. 
    */
   public static class InstrumentDataAccess
   {
      public static async Task<int> GetInstrumentIdAsync(string name, int exchangeId, InstrumentType type, string connectionString)
      {
         using (var connection = new NpgsqlConnection(connectionString))
         {
            connection.Open();
            const string sql = "SELECT Id FROM instruments WHERE Name = @name AND ExchangeId = @exchangeId AND Type = @type";
            var result = await connection.QuerySingleOrDefaultAsync<int>(sql, new { name, exchangeId, type });
            return result;
         }

      }
      public static async Task AddInstrumentAsync(InstrumentType type, string name, string[] symbols, int exchangeId, string connectionString)
      {
         using (var connection = new NpgsqlConnection(connectionString))
         {
            connection.Open();
            await connection.ExecuteAsync("AddInstrument", new { type, name, symbols, exchangeId }, commandType: CommandType.StoredProcedure);
         }
      }

      public static async Task AddInstrumentPriceAsync(int instrumentId, double price, string connectionString)
      {
         using (var connection = new NpgsqlConnection(connectionString))
         {
            connection.Open();
            await connection.ExecuteAsync("AddInstrumentPrice", new { instrumentId, price }, commandType: CommandType.StoredProcedure);
         }
      }
   }

}