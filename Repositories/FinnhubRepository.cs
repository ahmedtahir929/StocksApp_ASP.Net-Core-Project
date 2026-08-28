using Microsoft.Extensions.Options;
using RepositoryContracts;
using StocksApp_xUnit.Options;
using System.Text.Json;

namespace Repositories
{
  public class FinnhubRepository : IFinnhubRepository
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TradingApiOptions _tradingApiOptions;

    public FinnhubRepository(IHttpClientFactory httpClientFactory, IOptions<TradingApiOptions> tradingApiOptions, IOptions<TradingOptions> tradingOptions)
    {
      _httpClientFactory = httpClientFactory;
      _tradingApiOptions = tradingApiOptions.Value;
    }

    public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
    {
      using HttpClient httpClient = _httpClientFactory.CreateClient();

      string symbol = stockSymbol.Trim().ToUpperInvariant();
      string token = _tradingApiOptions.ApiKey;

      HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
      {
        RequestUri =
          new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={symbol}&token={token}"),
        Method = HttpMethod.Get,
      };

      HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

      Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

      StreamReader reader = new StreamReader(stream);

      string response = await reader.ReadToEndAsync();

      Dictionary<string, object>? companyProfile =
          JsonSerializer.Deserialize<Dictionary<string, object>>(response);

      if (companyProfile == null || companyProfile.Count == 0) return null;

      return companyProfile;
    }

    public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
    {
      using HttpClient httpClient = _httpClientFactory.CreateClient();

      string symbol = stockSymbol.Trim().ToUpperInvariant();
      string token = _tradingApiOptions.ApiKey;

      HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
      {
        RequestUri =
          new Uri($"https://finnhub.io/api/v1/quote?symbol={symbol}&token={token}"),
        Method = HttpMethod.Get,
      };

      HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

      Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

      StreamReader reader = new StreamReader(stream);

      string response = await reader.ReadToEndAsync();

      Dictionary<string, object>? stockPriceQuote =
          JsonSerializer.Deserialize<Dictionary<string, object>>(response);

      if (stockPriceQuote == null) return null;

      return stockPriceQuote;
    }

    public async Task<List<Dictionary<string, string>>?> GetStocks()
    {
      using HttpClient httpClient = _httpClientFactory.CreateClient();

      string token = _tradingApiOptions.ApiKey;

      HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
      {
        RequestUri =
          new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={token}"),
        Method = HttpMethod.Get,
      };

      HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

      Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

      StreamReader reader = new StreamReader(stream);

      string response = await reader.ReadToEndAsync();

      List<Dictionary<string, string>>? stocksList =
          JsonSerializer.Deserialize<List<Dictionary<string, string>>>(response);

      if (stocksList == null) return null;

      return stocksList;
    }

    public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
    {
      using HttpClient httpClient = _httpClientFactory.CreateClient();

      string token = _tradingApiOptions.ApiKey;

      HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
      {
        RequestUri =
          new Uri($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&token={token}"),
        Method = HttpMethod.Get,
      };

      HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

      Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

      StreamReader reader = new StreamReader(stream);

      string response = await reader.ReadToEndAsync();

      Dictionary<string, object>? searchedStock =
          JsonSerializer.Deserialize<Dictionary<string, object>>(response);

      if (searchedStock == null) return null;

      return searchedStock;
    }
  }
}
