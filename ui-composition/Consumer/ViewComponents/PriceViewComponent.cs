using Consumer.Commands;
using Consumer.Commands.Collapsers;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Consumer.ViewComponents
{
    public class PriceViewComponent : ViewComponent
    {
        MultiPriceCommandCollapser _collapser;
        CurrencyServiceCommand _currencyServiceCommand;

        public PriceViewComponent(MultiPriceCommandCollapser collapser, CurrencyServiceCommand currencyServiceCommand)
        {
            _collapser = collapser;
            _currencyServiceCommand = currencyServiceCommand;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            _collapser.ProductId = productId;
            var result = await _collapser.ExecuteAsync(CancellationToken.None);
            var currencyCookie = Request.Cookies.FirstOrDefault(x => x.Key == "SelectedCurrency" && !string.IsNullOrWhiteSpace(x.Value));
            if (currencyCookie.Equals(default(KeyValuePair<string, string>)))
            {
                return View(result);
            }

            var currencies = await _currencyServiceCommand.ExecuteAsync(CancellationToken.None);
            if (currencies != null && currencies.Any(x => x.Id == currencyCookie.Value))
            {
                var usd = currencies.First(x => x.Id == currencyCookie.Value);
                result.Amount = result.Amount / usd.Bid;
                result.Currency = usd.Symbol;
            }

            return View(result);
        }
    }
}
