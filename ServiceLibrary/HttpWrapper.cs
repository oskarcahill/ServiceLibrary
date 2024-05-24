﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
   public class HttpClientWrapper : IHttpClientWrapper
   {
      private readonly HttpClient _httpClient;

      public HttpClientWrapper(HttpClient httpClient)
      {
         _httpClient = httpClient;
      }

      public Task<HttpResponseMessage> GetAsync(string requestUri)
      {
         return _httpClient.GetAsync(requestUri);
      }
   }
}
