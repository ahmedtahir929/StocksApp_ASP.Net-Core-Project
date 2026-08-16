using RepositoryContracts;
using ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubService> _logger;

        public FinnhubService(IFinnhubRepository finnhubRepository, ILogger<FinnhubService> logger)
        {
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string? stockSymbol)
        {
            _logger.LogInformation("{ServiceName}.{MethodName}() invoked", nameof(FinnhubService), nameof(GetCompanyProfile));

            if (string.IsNullOrWhiteSpace(stockSymbol)) { return null; }

            Dictionary<string, object>? companyProfile = 
                await _finnhubRepository.GetCompanyProfile(stockSymbol);

            return companyProfile;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string? stockSymbol)
        {
            _logger.LogInformation("{ServiceName}.{MethodName}() invoked", nameof(FinnhubService), nameof(GetStockPriceQuote));

            if (string.IsNullOrWhiteSpace(stockSymbol)) return null;

            Dictionary<string, object>? stockPriceQuoteKeys =
                await _finnhubRepository.GetStockPriceQuote(stockSymbol);

            if (stockPriceQuoteKeys == null) return null;

            return stockPriceQuoteKeys;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            _logger.LogInformation("{ServiceName}.{MethodName}() invoked", nameof(FinnhubService), nameof(GetStocks));

            List<Dictionary<string, string>>? stockList = await _finnhubRepository.GetStocks();

            if (stockList == null) return new List<Dictionary<string, string>>();

            return stockList;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string? stockSymbolToSearch)
        {
            _logger.LogInformation("{ServiceName}.{MethodName}() invoked", nameof(FinnhubService), nameof(SearchStocks));

            if (string.IsNullOrWhiteSpace(stockSymbolToSearch)) return null;

            Dictionary<string, object> ? stock =
                await _finnhubRepository.SearchStocks(stockSymbolToSearch);

            if (stock == null) return null;

            return stock;
        }
    }
}
