using ServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
   public class InstrumentDataAccessWrapper : IInstrumentDataAccessWrapper
   {
      public Task<int> GetInstrumentIdAsync(string name, int exchangeId, InstrumentType type, string connString)
      {
         return InstrumentDataAccess.GetInstrumentIdAsync(name, exchangeId, type, connString);
      }

      public Task AddInstrumentAsync(InstrumentType type, string name, string[] symbols, int exchangeId, string connString)
      {
         return InstrumentDataAccess.AddInstrumentAsync(type, name, symbols, exchangeId, connString);
      }

      public Task AddInstrumentPriceAsync(int instrumentId, double price, string connString)
      {
         return InstrumentDataAccess.AddInstrumentPriceAsync(instrumentId, price, connString);
      }
   }
}
