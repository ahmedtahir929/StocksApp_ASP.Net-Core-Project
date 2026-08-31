using RepositoryContracts;
using ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Services
{
  public class FinnhubStocksService : IFinnhubStocksService
  {
    private readonly IFinnhubRepository _finnhubRepository;
    private readonly ILogger<FinnhubStocksService> _logger;

    public FinnhubStocksService(IFinnhubRepository finnhubRepository, ILogger<FinnhubStocksService> logger)
    {
      _finnhubRepository = finnhubRepository;
      _logger = logger;
    }

    public async Task<List<Dictionary<string, string>>?> GetStocks()
    {
      _logger.LogInformation("{ServiceName}.{MethodName}() invoked", nameof(FinnhubStocksService), nameof(GetStocks));

      List<Dictionary<string, string>>? stockList = await _finnhubRepository.GetStocks();

      if (stockList == null) return new List<Dictionary<string, string>>();

      return stockList;
    }
  }
}
