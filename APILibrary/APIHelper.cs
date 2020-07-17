using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace APILibrary
{
    public class APIHelper
    {
        public HttpClient apiClient { get; set; }

        public void InitializeClient ()
        {
            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri("https://api.exchangeratesapi.io/");
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
