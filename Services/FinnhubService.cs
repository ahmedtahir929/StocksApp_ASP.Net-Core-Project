using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp_xUnit.Options;
using System.Text.Json;

namespace Services
{
    public class FinnhubService : IFinnHubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TradingApiOptions _tradingApiOptions;

        public FinnhubService(IHttpClientFactory httpClientFactory, IOptions<TradingApiOptions> tradingApiOptions)
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
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={symbol}&token={token}"),
                Method = HttpMethod.Get,
            };

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

            StreamReader streamReader = new StreamReader(stream);
            string response = await streamReader.ReadToEndAsync();

            Dictionary<string, object>? companyProfile =
                JsonSerializer.Deserialize<Dictionary<string, object>>(response);

            if (companyProfile == null)
                return null;

            return companyProfile;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient();

            string symbol = stockSymbol.Trim().ToUpperInvariant();
            string token = _tradingApiOptions.ApiKey;

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage() 
            {
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={symbol}&token={token}"),
                Method = HttpMethod.Get,
            };

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

            StreamReader reader = new StreamReader(stream);
            string response = await reader.ReadToEndAsync();

            Dictionary<string, object>? stockPriceQuoteKeys = 
                JsonSerializer.Deserialize<Dictionary<string, object>>(response);

            if (stockPriceQuoteKeys == null) return null;

            return stockPriceQuoteKeys;
        }
    }
}
