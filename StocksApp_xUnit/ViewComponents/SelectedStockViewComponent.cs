using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using StocksApp_xUnit.ViewModels;

public class SelectedStockViewComponent : ViewComponent
{
    private readonly IFinnhubService _finnhubService;

    public SelectedStockViewComponent(IFinnhubService finnhubService)
    {
        _finnhubService = finnhubService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
    {
        Dictionary<string, object>? stockCompanyProfile =
            await _finnhubService.GetCompanyProfile(stockSymbol);

        //Update 'GetStockPriceQuote' to actual method name for fetching prices
        Dictionary<string, object>? stockQuote =
            await _finnhubService.GetStockPriceQuote(stockSymbol);

        Stock stock = new Stock();

        if (stockCompanyProfile == null || !stockCompanyProfile.ContainsKey("ticker"))
        {
            stock.StockSymbol = stockSymbol;
            stock.StockName = "Profile Not Found";
            return View(stock);
        }

        //Using .ToString() instead of Convert.ToString() to safely handle JsonElement
        stock.StockSymbol = stockCompanyProfile.ContainsKey("ticker") && stockCompanyProfile["ticker"] != null
            ? stockCompanyProfile["ticker"].ToString()
            : stockSymbol;

        stock.StockName = stockCompanyProfile.ContainsKey("name") && stockCompanyProfile["name"] != null
            ? stockCompanyProfile["name"].ToString()
            : "N/A";

        stock.Logo = stockCompanyProfile.ContainsKey("logo") && stockCompanyProfile["logo"] != null
            ? stockCompanyProfile["logo"].ToString()
            : string.Empty;

        stock.FinnhubIndustry = stockCompanyProfile.ContainsKey("finnhubIndustry") && stockCompanyProfile["finnhubIndustry"] != null
            ? stockCompanyProfile["finnhubIndustry"].ToString()
            : string.Empty;

        stock.Exchange = stockCompanyProfile.ContainsKey("exchange") && stockCompanyProfile["exchange"] != null
            ? stockCompanyProfile["exchange"].ToString()
            : string.Empty;

        //Convert the JsonElement's string representation to a double
        if (stockQuote != null && stockQuote.ContainsKey("c") && stockQuote["c"] != null)
        {
            if (double.TryParse(stockQuote["c"].ToString(), out double parsedPrice))
            {
                stock.Price = parsedPrice;
            }
        }

        return View(stock);
    }
}