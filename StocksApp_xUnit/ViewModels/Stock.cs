namespace StocksApp_xUnit.ViewModels
{
    public class Stock
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }

        // Added to support the Selected Stock ViewComponent based on Finnhub's GetCompanyProfile
        public string? Logo { get; set; }
        public string? FinnhubIndustry { get; set; }
        public string? Exchange { get; set; }

        // Note: You will likely need to populate this from IFinnhubRepository.GetQuote() 
        // since GetCompanyProfile() does not return the live price.
        public double Price { get; set; }
    }
}