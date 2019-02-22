using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Data
{
    public static class ServiceData
    {
        public static async Task<T> GetAsync<T>(HttpClientHandler handler, string url,
            CancellationToken cancelToken = default(CancellationToken))
        {
            using (HttpClient httpClient = new HttpClient(handler, false))
            {
                HttpResponseMessage response;
                try
                {
                    response = await httpClient.GetAsync(url);
                }
                catch (Exception)
                {
                    response = null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(responseContent, settings));
            }
        }
    }
}
