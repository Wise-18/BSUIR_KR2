using Microsoft.AspNetCore.Mvc;

namespace BSUIR_KR1.UI.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(new CartViewModel { TotalPrice = 0, ItemsCount = 0 });
        }
    }

    public class CartViewModel
    {
        public decimal TotalPrice { get; set; }
        public int ItemsCount { get; set; }
    }
}