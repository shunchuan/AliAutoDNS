using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
namespace CaoSCAutoDNS.Methods
{
    class HttpUtility
    {
        public static string Get(string url)
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.None };
            using (var httpClient = new HttpClient(handler))
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var httpResponseMessage = httpClient.GetAsync(url).Result;
                var result= httpResponseMessage.Content.ReadAsStringAsync().Result;
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return result;
                }
                else throw new HttpRequestException(httpResponseMessage.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
