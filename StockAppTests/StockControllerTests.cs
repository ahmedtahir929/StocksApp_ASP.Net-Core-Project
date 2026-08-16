using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using ServiceContracts;
using StocksApp_xUnit.Controllers;
using StocksApp_xUnit.Options;
using StocksApp_xUnit.ViewModels;

namespace StockAppTests
{
    public class StockControllerTests
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _options;

        private readonly Mock<IFinnhubService> _finnhubServiceMock;
        private readonly Mock<IOptions<TradingOptions>> _optionsMock;

        public StockControllerTests()
        {
            _finnhubServiceMock = new Mock<IFinnhubService>();
            _finnhubService = _finnhubServiceMock.Object;

            _optionsMock = new Mock<IOptions<TradingOptions>>();
            _optionsMock
                .Setup(o => o.Value)
                .Returns(new TradingOptions
                {
                    DefaultOrderQuantity = 10,
                    Top25PopularStocks = "AAPL,MSFT,GOOG,AMZN,NVDA"
                });

            _options = _optionsMock.Object;
        }

        #region Explore

        [Fact]
        public async Task Explore_ShouldReturnEmptyStockList_WhenGetStocksReturnsNull()
        {
            // Arrange
            _finnhubServiceMock
                .Setup(x => x.GetStocks())
                .ReturnsAsync(new List<Dictionary<string, string>>());

            StockController controller = new StockController(_options, _finnhubService);

            // Act
            IActionResult result = await controller.Explore(null);

            // Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;

            List<Stock> model = viewResult.Model.Should()
                .BeAssignableTo<List<Stock>>()
                .Subject;

            model.Should().BeEmpty();
        }

        [Fact]
        public async Task Explore_ShouldReturnFilteredStocks_WhenStocksExist()
        {
            // Arrange
            List<Dictionary<string, string>> stocks = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>()
                {
                    ["symbol"] = "AAPL",
                    ["description"] = "Apple Inc."
                },
                new Dictionary<string, string>()
                {
                    ["symbol"] = "MSFT",
                    ["description"] = "Microsoft"
                },
                new Dictionary<string, string>()
                {
                    ["symbol"] = "TSLA",
                    ["description"] = "Tesla"
                }
            };

            _finnhubServiceMock
                .Setup(x => x.GetStocks())
                .ReturnsAsync(stocks);

            StockController controller = new StockController(_options, _finnhubService);

            // Act
            IActionResult result = await controller.Explore(null);

            // Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;

            List<Stock> model = viewResult.Model.Should()
                .BeAssignableTo<List<Stock>>()
                .Subject;

            model.Should().HaveCount(2);

            model.Should().Contain(x => x.StockSymbol == "AAPL");
            model.Should().Contain(x => x.StockSymbol == "MSFT");
            model.Should().NotContain(x => x.StockSymbol == "TSLA");
        }

        #endregion

        #region GetStockDetails

        [Fact]
        public void GetStockDetails_ShouldReturnViewComponentResult()
        {
            // Arrange
            StockController controller = new StockController(_options, _finnhubService);

            // Act
            IActionResult result = controller.GetStockDetails("AAPL");

            // Assert
            ViewComponentResult viewComponentResult = 
                result.Should().BeOfType<ViewComponentResult>().Subject;

            viewComponentResult.ViewComponentName.Should().Be("SelectedStock");
        }

        #endregion
    }
}
