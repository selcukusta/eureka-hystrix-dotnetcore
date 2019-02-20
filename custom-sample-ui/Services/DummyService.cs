using System.Net.Http;
using System.Threading.Tasks;
using Steeltoe.Common.Discovery;

namespace custom_sample_ui.Services
{
    public class DummyService : IDummyService
    {
        DiscoveryHttpClientHandler _handler;
        private string API_URL = "https://remote-service/api/home";

        public DummyService(IDiscoveryClient client)
        {
            _handler = new DiscoveryHttpClientHandler(client);
        }

        public async Task<string> GetAddress()
        {
            var client = GetClient();
            var result = await client.GetStringAsync(API_URL);
            return result;
        }

        public async Task<string> GetAddressFallback()
        {
            return await Task.FromResult<string>("Bağcılar, İSTANBUL :)");
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient(_handler, false);
            return client;
        }
    }
}
