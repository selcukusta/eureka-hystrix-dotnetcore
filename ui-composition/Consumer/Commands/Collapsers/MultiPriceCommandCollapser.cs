using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Common.Discovery;
using Common;
using Common.Data;
using System.Collections.Generic;
using System.Linq;

namespace Consumer.Commands.Collapsers
{
    public class MultiPriceCommandCollapser : HystrixCollapser<IList<Price>, Price, int>
    {
        IPriceService _service;
        ILogger<MultiPriceCommandCollapser> _logger;
        ILoggerFactory _logFactory;

        public MultiPriceCommandCollapser(IHystrixCollapserOptions options, IPriceService service, ILogger<MultiPriceCommandCollapser> logger, ILoggerFactory logFactory) : base(options)
        {
            _service = service;
            _logger = logger;
            _logFactory = logFactory;
        }
        public int ProductId { get; set; }
        public override int RequestArgument => ProductId;

        protected override HystrixCommand<IList<Price>> CreateCommand(ICollection<ICollapsedRequest<Price, int>> requests)
        {
            return new MultiPriceServiceCommand(HystrixCommandGroupKeyDefault.AsKey("ServiceGroup"), requests, _service, _logFactory.CreateLogger<MultiPriceServiceCommand>());
        }

        protected override void MapResponseToRequests(IList<Price> batchResponse, ICollection<ICollapsedRequest<Price, int>> requests)
        {
            if (batchResponse == null || !batchResponse.Any())
            {
                SetAllResponsesAsEmpty(requests);
                return;
            }

            foreach (var f in batchResponse)
            {
                foreach (var r in requests)
                {
                    if (r.Argument == f.ProductId)
                    {
                        r.Response = f;
                    }
                }
            }
        }

        private void SetAllResponsesAsEmpty(ICollection<ICollapsedRequest<Price, int>> requests)
        {
            foreach (var r in requests)
            {
                r.Response = null;
            }
        }
    }
}
