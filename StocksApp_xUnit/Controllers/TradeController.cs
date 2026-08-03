using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using StocksApp_xUnit.Options;
using StocksApp_xUnit.ViewModels;
using System.Text.Json;

namespace StocksApp_xUnit.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnHubService;
        private readonly IStocksService _stocksService;
        private readonly string _defaultStockSymbol;

        public TradeController(IOptions<TradingOptions> tradingOptions,
            IFinnhubService finnHubService, IStocksService stocksService)
        {
            _tradingOptions = tradingOptions.Value;
            _finnHubService = finnHubService;
            _stocksService = stocksService;

            _defaultStockSymbol = "MSFT";
        }

        private async Task<StockTrade> PopulateStockTrade(string symbol)
        {
            var companyProfile = await _finnHubService.GetCompanyProfile(symbol);
            var stockQuote = await _finnHubService.GetStockPriceQuote(symbol);

            StockTrade stockTrade = new StockTrade
            {
                StockSymbol = symbol,
                Quantity = _tradingOptions.DefaultOrderQuantity
            };

            if (companyProfile != null)
            {
                if (companyProfile.TryGetValue("name", out var name))
                    stockTrade.StockName = name?.ToString();
                if (companyProfile.TryGetValue("ticker", out var ticker))
                    stockTrade.StockSymbol = ticker?.ToString();
                if (companyProfile.TryGetValue("logo", out var logo))
                    stockTrade.Logo = logo?.ToString();
            }

            if (stockQuote != null && stockQuote.TryGetValue("c", out var currentPrice))
            {
                if (currentPrice is JsonElement element)
                    stockTrade.Price = element.GetDouble();
            }

            return stockTrade;
        }

        [Route("[action]/{stockSymbol?}")]
        [Route("/")]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            StockTrade stockTrade = await PopulateStockTrade(stockSymbol ?? _defaultStockSymbol);

            return View(stockTrade);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder([FromForm]BuyOrderRequest buyOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                // Re-populate model if validation fails
                var model = await PopulateStockTrade(buyOrderRequest.StockSymbol 
                    ?? _defaultStockSymbol);
                return View("Index", model);
            }

            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            await _stocksService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder([FromForm]SellOrderRequest sellOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                var model = await PopulateStockTrade(sellOrderRequest.StockSymbol
                    ?? _defaultStockSymbol);
                return View("Index", model);
            }

            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            await _stocksService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            Orders orders = new Orders() 
            {
                BuyOrders = await _stocksService.GetAllBuyOrders(),
                SellOrders = await _stocksService.GetAllSellOrders()
            };

            return View(orders);
        }

        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            Orders orders = new Orders()
            {
                BuyOrders = await _stocksService.GetAllBuyOrders(),
                SellOrders = await _stocksService.GetAllSellOrders()
            };

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins(20, 10, 20, 10),
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };
        }
    }
}
