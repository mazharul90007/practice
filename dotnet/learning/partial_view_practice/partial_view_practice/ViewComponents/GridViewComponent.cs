using Microsoft.AspNetCore.Mvc;
using Models;

namespace ViewComponents
{
    public class GridViewComponent: ViewComponent
    {
      public async Task<IViewComponentResult> InvokeAsync(ProductGrid products)
        {
            // ProductGrid ProductGridModel = new ProductGrid()
            // {
            //     GridTitle = "Product List",
            //     products = new List<Product>()
            //     {
            //         new Product(){ProductId = 1, ProductName = "Iphone", ProductPrice = 5000},
            //         new Product(){ProductId = 2, ProductName = "Laptop", ProductPrice = 12000},
            //         new Product(){ProductId = 3, ProductName = "TV", ProductPrice = 20000}
            //     }
            // };

            // ViewData["Grid"] = model;

            return View("Default", products); //invoked a partial view. [location: Views/Shared/Components/Grid/Default.cshtml]
        }
    }
}