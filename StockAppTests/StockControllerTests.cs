using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    private readonly IFinnhubStocksService _finnhubStocksService;
    private readonly IOptions<TradingOptions> _options;
    private readonly ILogger<StockController> _logger;
    private readonly Mock<ILogger<StockController>> _loggerMock;
    private readonly Mock<IFinnhubStocksService> _finnhubStocksServiceMock;
    private readonly Mock<IOptions<TradingOptions>> _optionsMock;

    public StockControllerTests()
    {
      _finnhubStocksServiceMock = new Mock<IFinnhubStocksService>();
      _finnhubStocksService = _finnhubStocksServiceMock.Object;

      _loggerMock = new Mock<ILogger<StockController>>();
      _logger= _loggerMock.Object;

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
      _finnhubStocksServiceMock
          .Setup(x => x.GetStocks())
          .ReturnsAsync(new List<Dictionary<string, string>>());

      StockController controller = new StockController(
        _options,
        _finnhubStocksService,
        _logger);

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

      _finnhubStocksServiceMock
          .Setup(x => x.GetStocks())
          .ReturnsAsync(stocks);

      StockController controller = new StockController(
        _options,
        _finnhubStocksService,
        _logger);

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
      StockController controller = new StockController(
        _options,
        _finnhubStocksService,
        _logger);

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
