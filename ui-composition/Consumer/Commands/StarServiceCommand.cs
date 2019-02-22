using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Common.Discovery;
using Common;
using Common.Data;
using System.Collections.Generic;

namespace Consumer.Commands
{
    public class StarServiceCommand : HystrixCommand<Star>
    {
        DiscoveryHttpClientHandler _handler;
        private string API_URL = "https://star-service/api/stars";
        private ILogger<StarServiceCommand> _logger;

        public int ProductId { get; set; }

        public StarServiceCommand(IHystrixCommandOptions options, ILogger<StarServiceCommand> logger, IDiscoveryClient client) : base(options)
        {
            _logger = logger;
            _handler = new DiscoveryHttpClientHandler(client);
            this.IsFallbackUserDefined = true;
        }

        protected override async Task<Star> RunAsync()
        {
            var star = await ServiceData.GetAsync<Star>(_handler, $"{API_URL}/{ProductId}");
            return star;
        }

        protected override async Task<Star> RunFallbackAsync()
        {
            var circuitBreakerStatus = this.IsCircuitBreakerOpen ? bool.TrueString : bool.FalseString;
            _logger.LogCritical($"Circuit breaker status: {circuitBreakerStatus}");
            if (!this.IsCircuitBreakerOpen && this.IsFailedExecution)
            {
                _logger.LogCritical(this.FailedExecutionException, this.FailedExecutionException.Message);
            }
            return await Task.FromResult<Star>(null);
        }
    }
}
