using Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IList<Category>>> GetAsync()
        {
            var dataContent = await System.IO.File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "Database", "data.json"));
            var data = JsonConvert.DeserializeObject<IList<Category>>(dataContent);
            return data.ToList();
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<Category>> GetAsync(int categoryId)
        {
            if (categoryId <= 0)
            {
                return BadRequest();
            }

            var dataContent = await System.IO.File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "Database", "data.json"));
            var data = JsonConvert.DeserializeObject<IList<Category>>(dataContent);
            if (!data.Any(d => d.Id == categoryId))
            {
                return NoContent();
            }

            return data.FirstOrDefault(d => d.Id == categoryId);
        }
    }
}
