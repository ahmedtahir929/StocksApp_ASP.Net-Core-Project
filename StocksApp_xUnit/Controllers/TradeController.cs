using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using ServiceContracts.DTO;
using Services;
using StocksApp_xUnit.Filters.ActionFilters;
using StocksApp_xUnit.ViewModels;

namespace StocksApp_xUnit.Controllers
{
  [Route("[controller]")]
  public class TradeController : Controller
  {
    private readonly IStocksService _stocksService;
    private readonly ILogger<TradeController> _logger;

    public TradeController(IStocksService stocksService, ILogger<TradeController> logger)
    {
      _stocksService = stocksService;
      _logger = logger;
    }
    
    [Route("[action]/{stockSymbol?}")]
    [Route("/")]
    [TypeFilter(typeof(PopulateStockTradeActionFilter))]
    public async Task<IActionResult> Index(string? stockSymbol)
    {
      _logger.LogInformation("{ControllerName}.{ActionName}() invoked with stockSymbol: {stockSymbol}", nameof(TradeController), nameof(Index), stockSymbol);

      StockTrade? stockTrade = HttpContext.Items[nameof(StockTrade)] as StockTrade;

      return View(stockTrade);
    }

    [Route("[action]")]
    [HttpPost]
    [TypeFilter(typeof(ValidateModelStateActionFilter))]
    public async Task<IActionResult> BuyOrder([FromForm] BuyOrderRequest buyOrderRequest)
    {
      _logger.LogInformation("{ControllerName}.{ActionName}() invoked with buyOrderRequest: {buyOrderRequest}", nameof(TradeController), nameof(BuyOrder), buyOrderRequest);

      buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
      await _stocksService.CreateBuyOrder(buyOrderRequest);

      return RedirectToAction("Orders", "Trade");
    }

    [Route("[action]")]
    [HttpPost]
    [TypeFilter(typeof(ValidateModelStateActionFilter))]
    public async Task<IActionResult> SellOrder([FromForm] SellOrderRequest sellOrderRequest)
    {
      _logger.LogInformation("{ControllerName}.{ActionName}() invoked with sellOrderRequest: {sellOrderRequest}", nameof(TradeController), nameof(SellOrder), sellOrderRequest);

      sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
      await _stocksService.CreateSellOrder(sellOrderRequest);

      return RedirectToAction("Orders", "Trade");
    }

    [Route("[action]")]
    public async Task<IActionResult> Orders()
    {
      _logger.LogInformation("{ControllerName}.{ActionName}() invoked", nameof(TradeController), nameof(Orders));

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
      _logger.LogInformation("{ControllerName}.{ActionName}() invoked", nameof(TradeController), nameof(OrdersPDF));

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
