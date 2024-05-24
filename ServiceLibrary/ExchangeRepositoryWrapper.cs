using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
   public class ExchangeRepositoryWrapper : IExchangeRepositoryWrapper
   {
      public Task<Exchange?> GetByIdAsync(int exchangeId)
      {
         return new ExchangeRepository().GetByIdAsync(exchangeId);
      }
   }
}
