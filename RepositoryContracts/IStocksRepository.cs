using Entities;

namespace RepositoryContracts
{
    public interface IStocksRepository
    {
        /// <summary>
        /// Creates the BuyOrder obj and stores in the data store
        /// </summary>
        /// <param name="buyOrder">BuyOrder obj to create</param>
        /// <returns>Returns the same BuyOrder obj after storing in the data store</returns>
        public Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);

        /// <summary>
        /// Creates the SellOrder obj and stores in the DB
        /// </summary>
        /// <param name="sellOrder">SellOrder obj to create</param>
        /// <returns>Returns the same SellOrder obj after storing in the data store</returns>
        public Task<SellOrder> CreateSellOrder(SellOrder sellOrder);

        /// <summary>
        /// Fetches all the stored BuyOrder objs from the data store
        /// </summary>
        /// <returns>Returns a list of BuyOrder fetched from the data store</returns>
        public Task<List<BuyOrder>> GetAllBuyOrders();

        /// <summary>
        /// Fetches all the stored SellOrder objs from the data store
        /// </summary>
        /// <returns>Returns a list of SellOrder fetched from the data store</returns>
        public Task<List<SellOrder>> GetAllSellOrders();
    }
}
