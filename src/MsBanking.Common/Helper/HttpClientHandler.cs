using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsBanking.Common.Helper
{
    public class HttpClientHandler : IHttpClientHandler
    {
        private readonly HttpClient _httpClient;

        public HttpClientHandler()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetStringAsync(string url)
        {
            return await _httpClient.GetStringAsync(url);
        }
    }
}
