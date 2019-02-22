using System;
using System.Threading.Tasks;
using Common;
using Common.Data;
using Steeltoe.Common.Discovery;

public class PriceService : IPriceService
{
    DiscoveryHttpClientHandler _handler;
    private string API_URL = "https://price-service/api/prices";

    public PriceService(IDiscoveryClient client)
    {
        _handler = new DiscoveryHttpClientHandler(client);
    }
    public async Task<Price> GetPriceByProductId(int productId)
    {
        return await ServiceData.GetAsync<Price>(_handler, $"{API_URL}/{productId}");
    }
}