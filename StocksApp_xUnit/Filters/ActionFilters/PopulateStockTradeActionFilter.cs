using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp_xUnit.Options;
using StocksApp_xUnit.ViewModels;
using System.Text.Json;

namespace StocksApp_xUnit.Filters.ActionFilters
{
  public class PopulateStockTradeActionFilter : IAsyncActionFilter
  {
    private readonly ILogger<PopulateStockTradeActionFilter> _logger;
    private readonly IFinnhubService _finnhubService;
    private readonly TradingOptions _tradingOptions;

    public PopulateStockTradeActionFilter(ILogger<PopulateStockTradeActionFilter> logger, IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions)
    {
      _logger = logger;
      _finnhubService = finnhubService;
      _tradingOptions = tradingOptions.Value;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      //TO DO: before logic
      _logger.LogInformation("{ActionFilterName}.{MethodName}() invoked - before logic", nameof(PopulateStockTradeActionFilter), nameof(OnActionExecutionAsync));

      var stockSymbol =
        context.ActionArguments.TryGetValue("stockSymbol", out var value)
        ? value as string ?? "MSFT"
        : "MSFT";

      var companyProfile = await _finnhubService.GetCompanyProfile(stockSymbol);
      var stockQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);

      StockTrade stockTrade = new StockTrade
      {
        StockSymbol = stockSymbol,
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

      context.HttpContext.Items[nameof(StockTrade)] = stockTrade;

      await next();

      //TO DO: after logic
      _logger.LogInformation("{ActionFilterName}.{MethodName}() invoked - after logic", nameof(PopulateStockTradeActionFilter), nameof(OnActionExecutionAsync));
    }
  }
}
