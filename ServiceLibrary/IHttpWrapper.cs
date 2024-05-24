using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
   public interface IHttpClientWrapper
   {
      Task<HttpResponseMessage> GetAsync(string requestUri);
   }
}
