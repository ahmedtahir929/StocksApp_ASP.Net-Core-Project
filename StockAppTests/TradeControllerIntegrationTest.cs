using FluentAssertions;
using HtmlAgilityPack;

namespace StockAppTests
{
    public class TradeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        
        public TradeControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Index_ToReturnView()
        {
            // Arrange
            var stockSymbol = "MSFT";
            var requestUrl = $"/Trade/Index/{stockSymbol}";
            
            // Act
            var response = await _httpClient.GetAsync(requestUrl);
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(responseBody);

            var document = htmlDocument.DocumentNode;

            document.Should().NotBeNull();
        }
    }
}