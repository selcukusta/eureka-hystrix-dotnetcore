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
    public class CurrenciesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IList<Currency>>> GetAsync()
        {
            var dataContent = await System.IO.File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "Database", "data.json"));
            var data = JsonConvert.DeserializeObject<IList<Currency>>(dataContent);
            return data.ToList();
        }
    }
}
