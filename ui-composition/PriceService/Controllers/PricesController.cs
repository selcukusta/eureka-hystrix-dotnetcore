using Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StarService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Price> Get()
        {
            return NoContent();
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Price>> GetAsync(int productId)
        {
            if (productId <= 0)
            {
                return BadRequest();
            }

            var dataContent = await System.IO.File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "Database", "data.json"));
            var data = JsonConvert.DeserializeObject<IList<Price>>(dataContent);
            if (!data.Any(d => d.ProductId == productId))
            {
                return NoContent();
            }

            return data.FirstOrDefault(d => d.ProductId == productId);
        }
    }
}
