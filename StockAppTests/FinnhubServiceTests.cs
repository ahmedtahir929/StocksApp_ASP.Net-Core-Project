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
    private readonly ILogger<FinnhubStocksService> _loggerStocksService;
    private readonly ILogger<FinnhubStockPriceQuoteService> _loggerStockPriceQuoteService;
    private readonly ILogger<FinnhubSearchStocksService> _loggerSearchStocksService;
    private readonly ILogger<FinnhubCompanyProfileService> _loggerCompanyProfileService;
    private readonly IFinnhubStocksService _finnhubStocksService;
    private readonly IFinnhubStockPriceQuoteService _finnhubStockPriceQuoteService;
    private readonly IFinnhubSearchStocksService _finnhubSearchStocksService;
    private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
    private readonly IFinnhubRepository _finnhubRepository;
    private readonly Mock<ILogger<FinnhubStocksService>> _loggerStocksServiceMock;
    private readonly Mock<ILogger<FinnhubStockPriceQuoteService>> _loggerStockPriceQuoteServiceMock;
    private readonly Mock<ILogger<FinnhubSearchStocksService>> _loggerSearchStocksServiceMock;
    private readonly Mock<ILogger<FinnhubCompanyProfileService>> _loggerCompanyProfileServiceMock;
    private readonly Mock<IFinnhubRepository> _finnhubRepositoryMock;

    public FinnhubServiceTests()
    {
      // Repository
      _finnhubRepositoryMock = new Mock<IFinnhubRepository>();
      _finnhubRepository = _finnhubRepositoryMock.Object;

      // Stocks Service Logger
      _loggerStocksServiceMock = new Mock<ILogger<FinnhubStocksService>>();
      _loggerStocksService = _loggerStocksServiceMock.Object;

      // Stock Price Quote Service Logger
      _loggerStockPriceQuoteServiceMock = new Mock<ILogger<FinnhubStockPriceQuoteService>>();
      _loggerStockPriceQuoteService = _loggerStockPriceQuoteServiceMock.Object;

      // Search Stocks Service Logger
      _loggerSearchStocksServiceMock = new Mock<ILogger<FinnhubSearchStocksService>>();
      _loggerSearchStocksService = _loggerSearchStocksServiceMock.Object;

      // Company Profile Service Logger
      _loggerCompanyProfileServiceMock = new Mock<ILogger<FinnhubCompanyProfileService>>();
      _loggerCompanyProfileService = _loggerCompanyProfileServiceMock.Object;

      // Services
      _finnhubStocksService = new FinnhubStocksService(
          _finnhubRepository,
          _loggerStocksService);

      _finnhubStockPriceQuoteService = new FinnhubStockPriceQuoteService(
          _finnhubRepository,
          _loggerStockPriceQuoteService);

      _finnhubSearchStocksService = new FinnhubSearchStocksService(
          _finnhubRepository,
          _loggerSearchStocksService);

      _finnhubCompanyProfileService = new FinnhubCompanyProfileService(
          _finnhubRepository,
          _loggerCompanyProfileService);
    }

    #region GetCompanyProfile

    [Fact]
    //If the stock symbol is null or empty, the method should return null.
    public async Task GetCompanyProfile_NullOrEmptyStockSymbol_ReturnsNull()
    {
      //Arrange
      string? stockSymbol = null;

      //Act
      var result = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);

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
      var result = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);

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
      var result = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);

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
      var result = await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);

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
      var result = await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);

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
      var result = await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);

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
      var result = await _finnhubStocksService.GetStocks();
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
      var result = await _finnhubStocksService.GetStocks();
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
      var result = await _finnhubSearchStocksService.SearchStocks(stockSymbolToSearch);

      //Assert
      result.Should().BeNull();
    }

    [Fact]
    public async Task SearchStocks_InvalidStockSymbol_ReturnsNull()
    {
      //Arrange
      string stockSymbolToSearch = "XYZ";

      //Act
      var result = await _finnhubSearchStocksService.SearchStocks(stockSymbolToSearch);

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
      var result = await _finnhubSearchStocksService.SearchStocks(stockSymbolToSearch);

      //Assert
      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expectedStock);
    }

    #endregion
  }
}