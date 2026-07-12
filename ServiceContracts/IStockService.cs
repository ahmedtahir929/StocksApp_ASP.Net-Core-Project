using ServiceContracts.DTO;

namespace Services
{
    public interface IStockService
    {
        /// <summary>
        /// Creates a buy order
        /// </summary>
        /// <param name="buyOrderRequest">buy order request to create</param>
        /// <returns>Returns the same buy order details along with
        /// the newly generated BuyOrderID as BuyOrderResponse
        /// </returns>
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        /// <summary>
        /// Creates a sell order
        /// </summary>
        /// <param name="sellOrderRequest">sell order request to create</param>
        /// <returns>Returns the same details of sell order along with newly generated SellOrderID as 
        /// SellOrderResponse
        /// </returns>
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);
        /// <summary>
        /// To get all buy orders stored in the list
        /// </summary>
        /// <returns>Returns a list of BuyOrderResponse</returns>
        Task<List<BuyOrderResponse>> GetAllBuyOrders();
        /// <summary>
        /// To get all sell orders stored in the list
        /// </summary>
        /// <returns>Returns a list of SellOrderResponse</returns>
        Task<List<SellOrderResponse>> GetAllSellOrders();
    }
}