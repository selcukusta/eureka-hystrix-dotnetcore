using Consumer.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer.ViewComponents
{
    public class StarViewComponent : ViewComponent
    {
        StarServiceCommand _starServiceCommand;

        public StarViewComponent(StarServiceCommand starServiceCommand)
        {
            _starServiceCommand = starServiceCommand;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            _starServiceCommand.ProductId = productId;
            var result = await _starServiceCommand.ExecuteAsync(CancellationToken.None);
            return View(result);
        }
    }
}
