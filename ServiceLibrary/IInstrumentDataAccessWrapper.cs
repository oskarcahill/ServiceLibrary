using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
   public interface IInstrumentDataAccessWrapper
   {
      Task<int> GetInstrumentIdAsync(string name, int exchangeId, InstrumentType type, string connString);
      Task AddInstrumentAsync(InstrumentType type, string name, string[] symbols, int exchangeId, string connString);
      Task AddInstrumentPriceAsync(int instrumentId, double price, string connString);
   }
}
