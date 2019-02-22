using Consumer.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        CategoryServiceCommand _categoryServiceCommand;

        public CategoryViewComponent(CategoryServiceCommand categoryServiceCommand)
        {
            _categoryServiceCommand = categoryServiceCommand;
        }
        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            _categoryServiceCommand.CategoryId = categoryId;
            var result = await _categoryServiceCommand.ExecuteAsync(CancellationToken.None);
            return View(result);
        }
    }
}
