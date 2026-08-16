using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public StocksService(IStocksRepository stocksRepository, ILogger<StocksService> logger,
            IDiagnosticContext diagnosticContext)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            _logger.LogInformation("{ServiceName}.{MethodName}() invoked", nameof(StocksService), nameof(CreateBuyOrder));

            ArgumentNullException.ThrowIfNull(buyOrderRequest, nameof(buyOrderRequest));

            //Checks if a valid model object was passed
            ModelValidationHelper.ModelValidation(buyOrderRequest);

            //Convert BuyOrderRequest obj to BuyOrder
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //Generate BuyOrderID
            buyOrder.BuyOrderID = Guid.NewGuid();

            //Save BuyOrder obj to the data store
            await _stocksRepository.CreateBuyOrder(buyOrder);

            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            _logger.LogInformation("{ServiceName}.{MethodName}() invoked", nameof(StocksService), nameof(CreateSellOrder));

            ArgumentNullException.ThrowIfNull(sellOrderRequest, nameof(sellOrderRequest));

            //Checks if a valid model object was passed
            ModelValidationHelper.ModelValidation(sellOrderRequest);

            //Convert to SellOrder & assign SellOrderID
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            //Save SellOrder obj to the data store
            await _stocksRepository.CreateSellOrder(sellOrder);

            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetAllBuyOrders()
        {
            _logger.LogInformation("{ServiceName}.{MethodName}() invoked", nameof(StocksService), nameof(GetAllBuyOrders));

            List<BuyOrder> buyOrders = await _stocksRepository.GetAllBuyOrders();

            if (buyOrders == null)
                return new List<BuyOrderResponse>();

            List<BuyOrderResponse> buyOrderResponses =
                buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();

            _diagnosticContext.Set("BuyOrders", buyOrderResponses);

            return buyOrderResponses.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();
        }

        public async Task<List<SellOrderResponse>> GetAllSellOrders()
        {
            List<SellOrder> sellOrders = await _stocksRepository.GetAllSellOrders();

            if (sellOrders == null)
                return new List<SellOrderResponse>();

            List<SellOrderResponse> sellOrderResponses =
                sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();

            _diagnosticContext.Set("SellOrders", sellOrderResponses);

            return sellOrderResponses.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();
        }
    }
}