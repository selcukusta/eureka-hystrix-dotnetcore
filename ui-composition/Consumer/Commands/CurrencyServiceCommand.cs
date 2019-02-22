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
    public class CurrencyServiceCommand : HystrixCommand<IList<Currency>>
    {
        DiscoveryHttpClientHandler _handler;
        private string API_URL = "https://currency-service/api/currencies";
        private ILogger<CurrencyServiceCommand> _logger;

        public CurrencyServiceCommand(IHystrixCommandOptions options, ILogger<CurrencyServiceCommand> logger, IDiscoveryClient client) : base(options)
        {
            _logger = logger;
            _handler = new DiscoveryHttpClientHandler(client);
            this.IsFallbackUserDefined = true;
        }

        protected override string CacheKey => "CurrencyServiceCacheKey";
        protected override bool IsRequestCachingEnabled => true;

        protected override async Task<IList<Currency>> RunAsync()
        {
            var currencies = await ServiceData.GetAsync<IList<Currency>>(_handler, API_URL);
            return currencies;
        }

        protected override async Task<IList<Currency>> RunFallbackAsync()
        {
            var circuitBreakerStatus = this.IsCircuitBreakerOpen ? bool.TrueString : bool.FalseString;
            _logger.LogCritical($"Circuit breaker status: {circuitBreakerStatus}");
            if (!this.IsCircuitBreakerOpen && this.IsFailedExecution)
            {
                _logger.LogCritical(this.FailedExecutionException, this.FailedExecutionException.Message);
            }
            return await Task.FromResult<IList<Currency>>(null);
        }
    }
}
