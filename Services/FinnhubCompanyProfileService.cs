using RepositoryContracts;
using ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Services
{
  public class FinnhubCompanyProfileService : IFinnhubCompanyProfileService
  {
    private readonly IFinnhubRepository _finnhubRepository;
    private readonly ILogger<FinnhubCompanyProfileService> _logger;

    public FinnhubCompanyProfileService(IFinnhubRepository finnhubRepository, ILogger<FinnhubCompanyProfileService> logger)
    {
      _finnhubRepository = finnhubRepository;
      _logger = logger;
    }

    public async Task<Dictionary<string, object>?> GetCompanyProfile(string? stockSymbol)
    {
      _logger.LogInformation("{ServiceName}.{MethodName}() invoked", nameof(FinnhubCompanyProfileService), nameof(GetCompanyProfile));

      if (string.IsNullOrWhiteSpace(stockSymbol)) { return null; }

      Dictionary<string, object>? companyProfile =
          await _finnhubRepository.GetCompanyProfile(stockSymbol);

      return companyProfile;
    }
  }
}
