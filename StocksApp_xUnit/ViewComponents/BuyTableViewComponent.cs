using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;

namespace StocksApp_xUnit.ViewComponents
{
    public class BuyTableViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<BuyOrderResponse> buyOrders)
        {
            return View(buyOrders);
        }
    }
}
