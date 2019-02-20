using System.Threading;
using System.Threading.Tasks;
using custom_sample_ui.Services;
using Microsoft.AspNetCore.Mvc;

namespace custom_sample_ui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        DummyServiceCommand _serviceCommand;

        public HomeController(DummyServiceCommand serviceCommand)
        {
            _serviceCommand = serviceCommand;
        }
        // GET api/values
        [HttpGet]
        public async Task<string> Get()
        {
            return await _serviceCommand.ExecuteAsync(CancellationToken.None);
        }
    }
}
