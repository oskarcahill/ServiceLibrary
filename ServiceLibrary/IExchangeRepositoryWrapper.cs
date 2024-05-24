using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
   public interface IExchangeRepositoryWrapper
   {
      Task<Exchange?> GetByIdAsync(int exchangeId);
   }
}
