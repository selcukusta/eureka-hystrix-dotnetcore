using Common;
using Common.Data;
using Consumer.Commands;
using Consumer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer.Controllers
{
    public class HomeController : Controller
    {
        ProductServiceCommand _productServiceCommand;

        public HomeController(ProductServiceCommand productServiceCommand)
        {
            _productServiceCommand = productServiceCommand;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var result = await _productServiceCommand.ExecuteAsync(CancellationToken.None);
            if (result == null || !result.Any())
            {
                return View("NoContent");
            }
            return View(result);
        }
    }
}
