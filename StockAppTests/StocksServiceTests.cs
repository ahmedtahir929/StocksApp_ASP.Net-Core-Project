using AutoFixture;
using Castle.Core.Logging;
using Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryContracts;
using Serilog;
using ServiceContracts.DTO;
using Services;

namespace StockAppTests
{
  public class StocksServiceTests
  {
    private readonly IFixture _fixture;
    private readonly IStocksService _stockService;
    private readonly IStocksRepository _stocksRepository;
    private readonly ILogger<StocksService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly Mock<IDiagnosticContext> _diagnosticContextMock;
    private readonly Mock<ILogger<StocksService>> _loggerMock;
    private readonly Mock<IStocksRepository> _stocksRepositoryMock;

    public StocksServiceTests()
    {
      _fixture = new Fixture();
      
      _loggerMock = new Mock<ILogger<StocksService>>();
      _logger = _loggerMock.Object;

      _diagnosticContextMock = new Mock<IDiagnosticContext>();
      _diagnosticContext = _diagnosticContextMock.Object;
      
      _stocksRepositoryMock = new Mock<IStocksRepository>();
      _stocksRepository = _stocksRepositoryMock.Object;
      
      _stockService = new StocksService(_stocksRepository, _logger, _diagnosticContext);
    }

    #region CreateBuyOrder
    [Fact]
    public async Task CreateBuyOrder_NullBuyOrder_ToBeArgumentNullException()
    {
      //Arrage
      BuyOrderRequest? buyOrderRequest = null;

      //Act
      Func<Task> action = async () => await _stockService.CreateBuyOrder(buyOrderRequest);

      //Assert
      await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateBuyOrder_MinQuantity_ToBeArgumentException()
    {
      //Arrange
      BuyOrderRequest buyOrderRequest =
          _fixture.Build<BuyOrderRequest>()
          .With(x => x.Quantity, 0u)
          .Create();

      //Act
      Func<Task> action = async () => await _stockService.CreateBuyOrder(buyOrderRequest);

      //Assert
      await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateBuyOrder_MaxQuantity_ToBeArgumentException()
    {
      //Arrange
      BuyOrderRequest buyOrderRequest =
          _fixture.Build<BuyOrderRequest>()
          .With(x => x.Quantity, 100001u)
          .Create();

      //Act
      Func<Task> action = async () => await _stockService.CreateBuyOrder(buyOrderRequest);

      //Assert
      await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateBuyOrder_NullSymbol_ToBeArgumentException()
    {
      //Arrange
      BuyOrderRequest buyOrderRequest =
          _fixture.Build<BuyOrderRequest>()
          .With(x => x.StockSymbol, String.Empty)
          .Create();

      //Act
      Func<Task> action = async () => await _stockService.CreateBuyOrder(buyOrderRequest);

      //Assert
      await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateBuyOrder_OlderDate_ToBeArgumentException()
    {
      //Arrange
      BuyOrderRequest buyOrderRequest =
          _fixture.Build<BuyOrderRequest>()
          .With(x => x.DateAndTimeOfOrder, DateTime.Parse("1999-12-31"))
          .Create();

      //Act
      Func<Task> action = async () => await _stockService.CreateBuyOrder(buyOrderRequest);

      //Assert
      await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateBuyOrder_ValidBuyOrder_ToBeSuccessful()
    {
      //Arrange
      BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
          .With(x => x.Quantity, 1000u)
          .With(x => x.DateAndTimeOfOrder, DateTime.UtcNow)
          .Create();

      BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

      _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
          .ReturnsAsync(buyOrder);

      BuyOrderResponse buyOrderResponseExpected = buyOrder.ToBuyOrderResponse();

      //Act
      BuyOrderResponse actualBuyOrderResponseFromCreate = await _stockService.CreateBuyOrder(buyOrderRequest);

      buyOrderResponseExpected.BuyOrderID = actualBuyOrderResponseFromCreate.BuyOrderID;

      //Assert
      actualBuyOrderResponseFromCreate.Should().NotBeNull();
      actualBuyOrderResponseFromCreate.Should().BeEquivalentTo(buyOrderResponseExpected);
    }
    #endregion

    #region CreateSellOrder
    [Fact]
    public async Task CreateSellOrder_NullSellOrder_ToBeArgumentNullException()
    {
      //Arrage
      SellOrderRequest? sellOrderRequest = null;

      //Act
      Func<Task> action = async () => await _stockService.CreateSellOrder(sellOrderRequest);

      //Assert
      await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateSellOrder_MinQuantity_ToBeArgumentException()
    {
      //Arrange
      SellOrderRequest sellOrderRequest =
          _fixture.Build<SellOrderRequest>()
          .With(x => x.Quantity, 0u)
          .Create();

      //Act
      Func<Task> action = async () => await _stockService.CreateSellOrder(sellOrderRequest);

      //Assert
      await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateSellOrder_MaxQuantity_ToBeArgumentException()
    {
      //Arrange
      SellOrderRequest sellOrderRequest =
          _fixture.Build<SellOrderRequest>()
          .With(x => x.Quantity, 100001u)
          .Create();

      //Act
      Func<Task> action = async () => await _stockService.CreateSellOrder(sellOrderRequest);

      //Assert
      await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateSellOrder_NullSymbol_ToBeArgumentException()
    {
      //Arrange
      SellOrderRequest sellOrderRequest =
          _fixture.Build<SellOrderRequest>()
          .With(x => x.StockSymbol, string.Empty)
          .Create();

      //Act
      Func<Task> action = async () => await _stockService.CreateSellOrder(sellOrderRequest);

      //Assert
      await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateSellOrder_OlderDate_ToBeArgumentException()
    {
      //Arrange
      SellOrderRequest sellOrderRequest =
          _fixture.Build<SellOrderRequest>()
          .With(x => x.DateAndTimeOfOrder, DateTime.Parse("1999-12-31"))
          .Create();

      //Act
      Func<Task> action = async () => await _stockService.CreateSellOrder(sellOrderRequest);

      //Assert
      await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateSellOrder_ValidSellOrder_ToBeSuccessful()
    {
      //Arrange
      SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
          .With(x => x.Quantity, 1000u)
          .With(x => x.DateAndTimeOfOrder, DateTime.UtcNow)
          .Create();

      SellOrder sellOrder = sellOrderRequest.ToSellOrder();

      _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
          .ReturnsAsync(sellOrder);

      SellOrderResponse sellOrderResponseExpected = sellOrder.ToSellOrderResponse();

      //Act
      SellOrderResponse actualSellOrderResponseFromCreate = await _stockService.CreateSellOrder(sellOrderRequest);

      sellOrderResponseExpected.SellOrderID = actualSellOrderResponseFromCreate.SellOrderID;

      //Assert
      actualSellOrderResponseFromCreate.Should().NotBeNull();
      actualSellOrderResponseFromCreate.Should().BeEquivalentTo(sellOrderResponseExpected);
    }

    #endregion

    #region GetAllBuyOrders
    [Fact]
    public async Task GetAllBuyOrders_EmptyList_ToBeEmpty()
    {
      //Act
      List<BuyOrderResponse> buyOrdersFromGet = await _stockService.GetAllBuyOrders();

      //Assert
      buyOrdersFromGet.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllBuyOrders_ValidBuyOrders_ToBeSuccessful()
    {
      //Arrange
      List<BuyOrder> buyOrders = _fixture.Create<List<BuyOrder>>();

      _stocksRepositoryMock
          .Setup(temp => temp.GetAllBuyOrders())
          .ReturnsAsync(buyOrders);

      List<BuyOrderResponse> expectedBuyOrders = buyOrders.Select(x => x.ToBuyOrderResponse()).ToList();

      //Act
      List<BuyOrderResponse> actualBuyOrdersFromGet = await _stockService.GetAllBuyOrders();

      //Assert
      actualBuyOrdersFromGet.Should().BeEquivalentTo(expectedBuyOrders);
    }
    #endregion

    #region GetAllSellOrders
    [Fact]
    public async Task GetAllSellOrders_EmptyList_ToBeEmpty()
    {
      //Act
      List<SellOrderResponse> sellOrdersFromGet = await _stockService.GetAllSellOrders();

      //Assert
      sellOrdersFromGet.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllSellOrders_ValidSellOrders_ToBeSuccessful()
    {
      //Arrange
      List<SellOrder> sellOrders = _fixture.Create<List<SellOrder>>();

      _stocksRepositoryMock
          .Setup(temp => temp.GetAllSellOrders())
          .ReturnsAsync(sellOrders);

      List<SellOrderResponse> expectedSellOrders = sellOrders.Select(x => x.ToSellOrderResponse()).ToList();

      //Act
      List<SellOrderResponse> actualSellOrdersFromGet = await _stockService.GetAllSellOrders();

      //Assert
      actualSellOrdersFromGet.Should().BeEquivalentTo(expectedSellOrders);
    }
    #endregion
  }
}