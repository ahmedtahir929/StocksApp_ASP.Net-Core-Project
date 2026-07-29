using ServiceContracts.DTO;

namespace Services
{
    public interface IStocksService
    {
        /// <summary>
        /// Creates a new buy order.
        /// </summary>
        /// <param name="buyOrderRequest">
        /// The details of the buy order to create.
        /// </param>
        /// <returns>
        /// A <see cref="BuyOrderResponse"/> containing the created buy order,
        /// including the generated BuyOrderID.
        /// </returns>
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

        /// <summary>
        /// Creates a new sell order.
        /// </summary>
        /// <param name="sellOrderRequest">
        /// The details of the sell order to create.
        /// </param>
        /// <returns>
        /// A <see cref="SellOrderResponse"/> containing the created sell order,
        /// including the generated SellOrderID.
        /// </returns>
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

        /// <summary>
        /// Retrieves all buy orders.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="BuyOrderResponse"/> objects representing all buy orders.
        /// </returns>
        Task<List<BuyOrderResponse>> GetAllBuyOrders();

        /// <summary>
        /// Retrieves all sell orders.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="SellOrderResponse"/> objects representing all sell orders.
        /// </returns>
        Task<List<SellOrderResponse>> GetAllSellOrders();
    }
}