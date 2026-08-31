using RepositoryContracts;
using ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Services
{
  public class FinnhubSearchStocksService : IFinnhubSearchStocksService
  {
    private readonly IFinnhubRepository _finnhubRepository;
    private readonly ILogger<FinnhubSearchStocksService> _logger;

    public FinnhubSearchStocksService(IFinnhubRepository finnhubRepository, ILogger<FinnhubSearchStocksService> logger)
    {
      _finnhubRepository = finnhubRepository;
      _logger = logger;
    }

    public async Task<Dictionary<string, object>?> SearchStocks(string? stockSymbolToSearch)
    {
      _logger.LogInformation("{ServiceName}.{MethodName}() invoked", nameof(FinnhubSearchStocksService), nameof(SearchStocks));

      if (string.IsNullOrWhiteSpace(stockSymbolToSearch)) return null;

      Dictionary<string, object>? stock =
          await _finnhubRepository.SearchStocks(stockSymbolToSearch);

      if (stock == null) return null;

      return stock;
    }
  }
}
