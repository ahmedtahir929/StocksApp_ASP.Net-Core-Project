using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;

namespace StocksApp_xUnit.ViewComponents
{
    public class SellTableViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<SellOrderResponse> sellOrders)
        {
            return View(sellOrders);
        }
    }
}
