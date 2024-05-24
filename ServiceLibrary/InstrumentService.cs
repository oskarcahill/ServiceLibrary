namespace ServiceLibrary
{
   public class InstrumentService
   {
      private static readonly string[] SymbolsToReject = new string[] { "APLT", "NLB", "AC" };
      private const string ConnString = "Host=eu-west-1.rdsw.test.morningstar.com;Port=1234;Database=praqw;Username=pwda;Password=Password";
      private const string BaseAddress = "https://api.exchanges.morningstar.com";

      public static IInstrumentDataAccessWrapper InstrumentDataAccess { get; set; } = new InstrumentDataAccessWrapper();
      public static IExchangeRepositoryWrapper ExchangeRepository { get; set; } = new ExchangeRepositoryWrapper();
      public static IHttpClientWrapper HttpClient { get; set; } = new HttpClientWrapper(new HttpClient { BaseAddress = new Uri(BaseAddress) });

      public async Task AddPriceSnapshot(InstrumentType type, string name, string[] symbols, int exchangeId)
      {
         ValidateInput(name, symbols);
         int instrumentId = await EnsureInstrumentExistsAsync(type, name, symbols, exchangeId, ConnString);
         string micCodes = await GetExchangeMicCodes(exchangeId);
         double price = await FetchPriceAsync(micCodes, instrumentId);

         if (price != 0.0)
         {
            await InstrumentDataAccess.AddInstrumentPriceAsync(instrumentId, price, ConnString);
         }
      }

      private void ValidateInput(string name, string[] symbols)
      {
         if (string.IsNullOrEmpty(name))
         {
            throw new ArgumentNullException(nameof(name));
         }

         if (symbols is null)
         {
            throw new ArgumentNullException(nameof(symbols));
         }

         if (!symbols.Any())
         {
            throw new ArgumentException("Symbols collection cannot be empty.");
         }

         if (symbols.Any(s => s.Length < 3))
         {
            throw new ArgumentException("Symbols must have a length of at least 3 characters.");
         }

         for (int i = 0; i < symbols.Length; i++)
         {
            if (SymbolsToReject.Contains(symbols[i]))
            {
               throw new Exception($"This symbol has been blacklisted {symbols[i]}.");
            }
         }
      }

      private async Task<int> EnsureInstrumentExistsAsync(InstrumentType type, string name, string[] symbols, int exchangeId, string connString)
      {
         var instrumentId = await InstrumentDataAccess.GetInstrumentIdAsync(name, exchangeId, type, connString);

         if (instrumentId == 0)
         {
            await InstrumentDataAccess.AddInstrumentAsync(type, name, symbols, exchangeId, connString);
            instrumentId = await InstrumentDataAccess.GetInstrumentIdAsync(name, exchangeId, type, connString);
         }

         return instrumentId;
      }

      private async Task<string> GetExchangeMicCodes(int exchangeId)
      {
         var (id, micCodes) = await ExchangeRepository.GetByIdAsync(exchangeId);

         if (micCodes is null)
         {
            throw new Exception($" Cannot find exchange for id {exchangeId}");
         }

         return micCodes;
      }

      private async Task<double> FetchPriceAsync(string micCodes, int instrumentId)
      {
         try
         {
            var response = await HttpClient.GetAsync($"/{micCodes}/{instrumentId}/price");
            response.EnsureSuccessStatusCode();
            var priceAsString = await response.Content.ReadAsStringAsync();

            if (!double.TryParse(priceAsString, out var price))
            {
               throw new Exception("Failed to parse price.");
            }

            return price;
         }
         catch (HttpRequestException e)
         {
            Console.WriteLine("\nHttp Exception Caught");
            Console.WriteLine("Message :{0} ", e.Message);
            return 0.0;
         }
      }
   }
}