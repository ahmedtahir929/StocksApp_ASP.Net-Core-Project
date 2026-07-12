using Entities;
using ServiceContracts.DTO;
using Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class StocksService : IStockService
    {
        private readonly StockMarketDbContext _db;

        public StocksService(StockMarketDbContext ordersDbContext)
        {
            _db = ordersDbContext;
        }

        private static BuyOrderResponse ConvertToBuyOrderResponse(BuyOrder buyOrder)
        {
            BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();
            buyOrderResponse.TradeAmount = buyOrderResponse.Price * buyOrderResponse.Quantity;

            return buyOrderResponse;
        }

        private static SellOrderResponse ConvertToSellOrderResponse(SellOrder sellOrder)
        {
            SellOrderResponse sellOrderResponse = sellOrder.ToSellOrderResponse();
            sellOrderResponse.TradeAmount = sellOrderResponse.Price * sellOrderResponse.Quantity;

            return sellOrderResponse;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            ArgumentNullException.ThrowIfNull(buyOrderRequest, nameof(buyOrderRequest));

            //Checks if a valid model object was passed
            ModelValidationHelper.ModelValidation(buyOrderRequest);

            //Convert BuyOrderRequest obj to BuyOrder
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //Generate BuyOrderID
            buyOrder.BuyOrderID = Guid.NewGuid();

            //Save BuyOrder obj to the data source
            _db.Add(buyOrder);
            await _db.SaveChangesAsync();

            return ConvertToBuyOrderResponse(buyOrder);
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            ArgumentNullException.ThrowIfNull(sellOrderRequest, nameof(sellOrderRequest));

            //Checks if a valid model object was passed
            ModelValidationHelper.ModelValidation(sellOrderRequest);

            //Convert to SellOrder & assign SellOrderID
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            //Adding to sell order list
            _db.Add(sellOrder);
            await _db.SaveChangesAsync();

            return ConvertToSellOrderResponse(sellOrder);
        }

        public async Task<List<BuyOrderResponse>> GetAllBuyOrders()
        {
            return await _db.BuyOrders
                .Select(temp => ConvertToBuyOrderResponse(temp))
                .ToListAsync();
        }

        public async Task<List<SellOrderResponse>> GetAllSellOrders()
        {
            return await _db.SellOrders
                .Select(temp => ConvertToSellOrderResponse(temp))
                .ToListAsync();
        }
    }
}
