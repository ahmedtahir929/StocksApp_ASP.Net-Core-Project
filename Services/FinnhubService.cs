using RepositoryContracts;
using ServiceContracts;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string? stockSymbol)
        {
            if (string.IsNullOrWhiteSpace(stockSymbol)) { return null; }

            Dictionary<string, object>? companyProfile = 
                await _finnhubRepository.GetCompanyProfile(stockSymbol);

            return companyProfile;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string? stockSymbol)
        {
            if (string.IsNullOrWhiteSpace(stockSymbol)) return null;

            Dictionary<string, object>? stockPriceQuoteKeys =
                await _finnhubRepository.GetStockPriceQuote(stockSymbol);

            if (stockPriceQuoteKeys == null) return null;

            return stockPriceQuoteKeys;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            List<Dictionary<string, string>>? stockList = await _finnhubRepository.GetStocks();

            if (stockList == null) return new List<Dictionary<string, string>>();

            return stockList;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string? stockSymbolToSearch)
        {
            if (string.IsNullOrWhiteSpace(stockSymbolToSearch)) return null;

            Dictionary<string, object> ? stock =
                await _finnhubRepository.SearchStocks(stockSymbolToSearch);

            if (stock == null) return null;

            return stock;
        }
    }
}
