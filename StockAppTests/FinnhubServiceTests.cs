using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace StockAppTests
{
  public class FinnhubServiceTests
  {
    private readonly ILogger<FinnhubService> _logger;
    private readonly IFinnhubService _finnhubService;
    private readonly IFinnhubRepository _finnhubRepository;
    private readonly Mock<ILogger<FinnhubService>> _loggerMock;
    private readonly Mock<IFinnhubRepository> _finnhubRepositoryMock;

    public FinnhubServiceTests()
    {
      _finnhubRepositoryMock = new Mock<IFinnhubRepository>();
      _finnhubRepository = _finnhubRepositoryMock.Object;

      _loggerMock = new Mock<ILogger<FinnhubService>>();
      _logger = _loggerMock.Object;

      _finnhubService = new FinnhubService(_finnhubRepository, _logger);
    }

    #region GetCompanyProfile

    [Fact]
    //If the stock symbol is null or empty, the method should return null.
    public async Task GetCompanyProfile_NullOrEmptyStockSymbol_ReturnsNull()
    {
      //Arrange
      string? stockSymbol = null;

      //Act
      var result = await _finnhubService.GetCompanyProfile(stockSymbol);

      //Assert
      result.Should().BeNull();
    }

    [Fact]
    //If the stock symbol is invalid, the method should return null.
    public async Task GetCompanyProfile_InvalidStockSymbol_ReturnsNull()
    {
      //Arrange
      string stockSymbol = "XYZ";

      //Act
      var result = await _finnhubService.GetCompanyProfile(stockSymbol);

      //Assert
      result.Should().BeNull();
    }

    [Fact]
    //If the stock symbol is valid, the method should return the company profile.
    public async Task GetCompanyProfile_ValidStockSymbol_ReturnsCompanyProfile()
    {
      //Arrange
      string stockSymbol = "AAPL";
      var expectedCompanyProfile = new Dictionary<string, object>
            {
                { "name", "Apple Inc." },
                { "ticker", "AAPL" }
            };
      _finnhubRepositoryMock
          .Setup(repo => repo.GetCompanyProfile(It.IsAny<string>()))
          .ReturnsAsync(expectedCompanyProfile);

      //Act
      var result = await _finnhubService.GetCompanyProfile(stockSymbol);

      //Assert
      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expectedCompanyProfile);
    }

    #endregion

    #region GetStockPriceQuote

    [Fact]
    //If the stock symbol is null or empty, the method should return null.
    public async Task GetStockPriceQuote_NullOrEmptyStockSymbol_ReturnsNull()
    {
      //Arrange
      string? stockSymbol = null;

      //Act
      var result = await _finnhubService.GetStockPriceQuote(stockSymbol);

      //Assert
      result.Should().BeNull();
    }

    [Fact]
    //If the stock symbol is invalid, the method should return null.
    public async Task GetStockPriceQuote_InvalidStockSymbol_ReturnsNull()
    {
      //Arrange
      string stockSymbol = "XYZ";

      //Act
      var result = await _finnhubService.GetStockPriceQuote(stockSymbol);

      //Assert
      result.Should().BeNull();
    }

    [Fact]
    //If the stock symbol is valid, the method should return the stock price quote.
    public async Task GetStockPriceQuote_ValidStockSymbol_ReturnsStockPriceQuote()
    {
      //Arrange
      string stockSymbol = "AAPL";
      var expectedStockPriceQuote = new Dictionary<string, object>
            {
                { "price", 150.00 },
                { "symbol", "AAPL" }
            };
      _finnhubRepositoryMock
          .Setup(repo => repo.GetStockPriceQuote(It.IsAny<string>()))
          .ReturnsAsync(expectedStockPriceQuote);

      //Act
      var result = await _finnhubService.GetStockPriceQuote(stockSymbol);

      //Assert
      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expectedStockPriceQuote);
    }


    #endregion

    #region GetStocks

    [Fact]
    //If the repository returns null, the method should return an empty list.
    public async Task GetStocks_RepositoryReturnsNull_ReturnsEmptyList()
    {
      //Arrange
      _finnhubRepositoryMock.Setup(repo => repo.GetStocks())
          .ReturnsAsync((List<Dictionary<string, string>>?)null);
      //Act
      var result = await _finnhubService.GetStocks();
      //Assert
      result.Should().NotBeNull();
      result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetStocks_RepositoryReturnsStocks_ToBeSuccessful()
    {
      //Arrange
      var expectedStocks = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "symbol", "AAPL" }, { "name", "Apple Inc." } },
                new Dictionary<string, string> { { "symbol", "MSFT" }, { "name", "Microsoft Corporation" } }
            };
      _finnhubRepositoryMock
          .Setup(repo => repo.GetStocks())
          .ReturnsAsync(expectedStocks);
      //Act
      var result = await _finnhubService.GetStocks();
      //Assert
      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expectedStocks);
    }

    #endregion

    #region SearchStocks

    [Fact]
    public async Task SearchStocks_NullOrEmptyStockSymbol_ReturnsNull()
    {
      //Arrange
      string? stockSymbolToSearch = null;

      //Act
      var result = await _finnhubService.SearchStocks(stockSymbolToSearch);

      //Assert
      result.Should().BeNull();
    }

    [Fact]
    public async Task SearchStocks_InvalidStockSymbol_ReturnsNull()
    {
      //Arrange
      string stockSymbolToSearch = "XYZ";

      //Act
      var result = await _finnhubService.SearchStocks(stockSymbolToSearch);

      //Assert
      result.Should().BeNull();
    }

    [Fact]
    public async Task SearchStocks_ValidStockSymbol_ReturnsStock()
    {
      //Arrange
      string stockSymbolToSearch = "AAPL";
      var expectedStock = new Dictionary<string, object>
            {
                { "symbol", "AAPL" },
                { "name", "Apple Inc." }
            };
      _finnhubRepositoryMock
          .Setup(repo => repo.SearchStocks(It.IsAny<string>()))
          .ReturnsAsync(expectedStock);

      //Act
      var result = await _finnhubService.SearchStocks(stockSymbolToSearch);

      //Assert
      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expectedStock);
    }

    #endregion
  }
}