using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;

namespace custom_sample_ui.Services
{
    public class DummyServiceCommand : HystrixCommand<string>
    {
        private IDummyService _service;
        private ILogger<DummyServiceCommand> _logger;

        public DummyServiceCommand(IHystrixCommandOptions options, IDummyService dummyService, ILogger<DummyServiceCommand> logger) : base(options)
        {
            _service = dummyService;
            _logger = logger;
            this.IsFallbackUserDefined = true;
        }

        protected override async Task<string> RunAsync()
        {
            return await _service.GetAddress();
        }

        protected override async Task<string> RunFallbackAsync()
        {
            var circuitBreakerStatus = this.IsCircuitBreakerOpen ? bool.TrueString : bool.FalseString;
            _logger.LogCritical($"Circuit breaker status: {circuitBreakerStatus}");
            if (!this.IsCircuitBreakerOpen && this.IsFailedExecution)
            {
                _logger.LogCritical(this.FailedExecutionException, this.FailedExecutionException.Message);
            }
            return await _service.GetAddressFallback();
        }
    }
}
