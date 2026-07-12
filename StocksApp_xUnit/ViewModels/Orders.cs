using ServiceContracts.DTO;

namespace StocksApp_xUnit.ViewModels
{
    public class Orders
    {
        public List<BuyOrderResponse>? BuyOrders { get; set; }
        public List<SellOrderResponse>? SellOrders { get; set; }
    }
}
