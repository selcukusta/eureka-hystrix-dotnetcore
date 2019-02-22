using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Common.Discovery;
using Common;
using Common.Data;
using System.Collections.Generic;
using System.Net.Http;

namespace Consumer.Commands
{
    public class ProductServiceCommand : HystrixCommand<IList<Product>>
    {
        DiscoveryHttpClientHandler _handler;
        private string API_URL = "https://product-service/api/products";
        private ILogger<ProductServiceCommand> _logger;

        public ProductServiceCommand(IHystrixCommandOptions options, ILogger<ProductServiceCommand> logger, IDiscoveryClient client) : base(options)
        {
            _logger = logger;
            _handler = new DiscoveryHttpClientHandler(client);
            this.IsFallbackUserDefined = true;
        }

        protected override async Task<IList<Product>> RunAsync()
        {
            var products = await ServiceData.GetAsync<IList<Product>>(_handler, API_URL);
            return products;
        }

        protected override async Task<IList<Product>> RunFallbackAsync()
        {
            var circuitBreakerStatus = this.IsCircuitBreakerOpen ? bool.TrueString : bool.FalseString;
            _logger.LogCritical($"Circuit breaker status: {circuitBreakerStatus}");
            if (!this.IsCircuitBreakerOpen && this.IsFailedExecution)
            {
                _logger.LogCritical(this.FailedExecutionException, this.FailedExecutionException.Message);
            }
            return await Task.FromResult<IList<Product>>(null);
        }
    }
}
