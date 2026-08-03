using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly StockMarketDbContext _stockMarketDbContext;

        public StocksRepository(StockMarketDbContext stockMarketDbContext)
        {
            _stockMarketDbContext = stockMarketDbContext;
        }

        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            await _stockMarketDbContext.BuyOrders.AddAsync(buyOrder);
            await _stockMarketDbContext.SaveChangesAsync();

            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            await _stockMarketDbContext.SellOrders.AddAsync(sellOrder);
            await _stockMarketDbContext.SaveChangesAsync();

            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetAllBuyOrders()
        {
            List<BuyOrder> buyOrders = await _stockMarketDbContext.BuyOrders.ToListAsync();

            return buyOrders;
        }

        public async Task<List<SellOrder>> GetAllSellOrders()
        {
            List<SellOrder> sellOrders = await _stockMarketDbContext.SellOrders.ToListAsync();

            return sellOrders;
        }
    }
}
