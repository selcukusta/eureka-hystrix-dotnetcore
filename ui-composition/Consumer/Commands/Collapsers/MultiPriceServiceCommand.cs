using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Common.Discovery;
using Common;
using Common.Data;
using System.Collections.Generic;

namespace Consumer.Commands
{
    public class MultiPriceServiceCommand : HystrixCommand<IList<Price>>
    {
        ILogger<MultiPriceServiceCommand> _logger;
        ICollection<ICollapsedRequest<Price, int>> _requests;
        IPriceService _service;

        public MultiPriceServiceCommand(IHystrixCommandGroupKey groupKey,
            ICollection<ICollapsedRequest<Price, int>> requests, IPriceService service,
            ILogger<MultiPriceServiceCommand> logger) : base(groupKey)
        {
            _logger = logger;
            _service = service;
            _requests = requests;
        }

        protected override async Task<IList<Price>> RunAsync()
        {
            var prices = new List<Price>();
            foreach (var request in _requests)
            {
                if (request.Argument > 0)
                {
                    prices.Add(await _service.GetPriceByProductId(request.Argument));
                }
            }
            return prices;
        }

        protected override async Task<IList<Price>> RunFallbackAsync()
        {
            var circuitBreakerStatus = this.IsCircuitBreakerOpen ? bool.TrueString : bool.FalseString;
            _logger.LogCritical($"Circuit breaker status: {circuitBreakerStatus}");
            if (!this.IsCircuitBreakerOpen && this.IsFailedExecution)
            {
                _logger.LogCritical(this.FailedExecutionException, this.FailedExecutionException.Message);
            }
            return await Task.FromResult<IList<Price>>(null);
        }
    }
}
