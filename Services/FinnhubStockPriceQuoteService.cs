using RepositoryContracts;
using ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Services
{
  public class FinnhubStockPriceQuoteService : IFinnhubStockPriceQuoteService
  {
    private readonly IFinnhubRepository _finnhubRepository;
    private readonly ILogger<FinnhubStockPriceQuoteService> _logger;

    public FinnhubStockPriceQuoteService(IFinnhubRepository finnhubRepository, ILogger<FinnhubStockPriceQuoteService> logger)
    {
      _finnhubRepository = finnhubRepository;
      _logger = logger;
    }

    public async Task<Dictionary<string, object>?> GetStockPriceQuote(string? stockSymbol)
    {
      _logger.LogInformation("{ServiceName}.{MethodName}() invoked", nameof(FinnhubStockPriceQuoteService), nameof(GetStockPriceQuote));

      if (string.IsNullOrWhiteSpace(stockSymbol)) return null;

      Dictionary<string, object>? stockPriceQuoteKeys =
          await _finnhubRepository.GetStockPriceQuote(stockSymbol);

      if (stockPriceQuoteKeys == null) return null;

      return stockPriceQuoteKeys;
    }
  }
}
