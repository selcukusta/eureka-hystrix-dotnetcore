using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Common.Discovery;
using Common;
using Common.Data;
using System.Collections.Generic;

namespace Consumer.Commands
{
    public class CategoryServiceCommand : HystrixCommand<Category>
    {
        DiscoveryHttpClientHandler _handler;
        private string API_URL = "https://category-service/api/categories";
        private ILogger<CategoryServiceCommand> _logger;

        public int CategoryId { get; set; }

        public CategoryServiceCommand(IHystrixCommandOptions options, ILogger<CategoryServiceCommand> logger, IDiscoveryClient client) : base(options)
        {
            _logger = logger;
            _handler = new DiscoveryHttpClientHandler(client);
            this.IsFallbackUserDefined = true;
        }

        protected override async Task<Category> RunAsync()
        {
            var category = await ServiceData.GetAsync<Category>(_handler, $"{API_URL}/{CategoryId}");
            return category;
        }

        protected override async Task<Category> RunFallbackAsync()
        {
            var circuitBreakerStatus = this.IsCircuitBreakerOpen ? bool.TrueString : bool.FalseString;
            _logger.LogCritical($"Circuit breaker status: {circuitBreakerStatus}");
            if (!this.IsCircuitBreakerOpen && this.IsFailedExecution)
            {
                _logger.LogCritical(this.FailedExecutionException, this.FailedExecutionException.Message);
            }
            return await Task.FromResult<Category>(null);
        }
    }
}
