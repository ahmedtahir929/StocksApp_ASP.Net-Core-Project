using Moq;
using AutoFixture;
using FluentAssertions;
using Rotativa.AspNetCore;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using StocksApp_xUnit.Controllers;
using StocksApp_xUnit.Options;
using StocksApp_xUnit.ViewModels;

namespace StockAppTests
{
    public class TradeControllerTests
    {
        private readonly IFixture _fixture;
        private readonly IStocksService _stocksService;
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _options;

        private readonly Mock<IStocksService> _stocksServiceMock;
        private readonly Mock<IFinnhubService> _finnhubServiceMock;
        private readonly Mock<IOptions<TradingOptions>> _optionsMock;

        public TradeControllerTests()
        {
            _fixture = new Fixture();

            _stocksServiceMock = new Mock<IStocksService>();
            _stocksService = _stocksServiceMock.Object;

            _finnhubServiceMock = new Mock<IFinnhubService>();
            _finnhubService = _finnhubServiceMock.Object;

            _optionsMock = new Mock<IOptions<TradingOptions>>();
            _optionsMock
                .Setup(o => o.Value)
                .Returns(new TradingOptions
                {
                    DefaultOrderQuantity = 10
                });

            _options = _optionsMock.Object;
        }

        #region Index

        [Fact]
        //If stock symbol is null or empty, the controller should return the Index view with a default StockTrade model.
        public async Task Index_ShouldReturnIndexViewWithDefaultStockTrade_WhenSymbolIsNullOrEmpty()
        {
            // Arrange
            TradeController tradeController = new TradeController(_options, _finnhubService, _stocksService);

            // Act
            IActionResult result = await tradeController.Index(null);

            // Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;

            var model = viewResult.Model.Should().BeAssignableTo<StockTrade>().Subject;

            viewResult.ViewData.Model.Should().BeOfType<StockTrade>();

            model.StockSymbol.Should().Be("MSFT");
        }

        [Fact]
        //If stock symbol is valid, the controller should return the Index view with a StockTrade model.
        public async Task Index_ShouldReturnIndexViewWithStockTrade_WhenSymbolIsValid()
        {
            // Arrange
            TradeController tradeController = new TradeController(_options, _finnhubService, _stocksService);

            // Act
            IActionResult result = await tradeController.Index("AAPL");

            // Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.ViewData.Model.Should().BeOfType<StockTrade>();
        }

        #endregion

        #region BuyOrder

        [Fact]
        //If model state is invalid, the controller should return the Index view with a StockTrade model.
        public async Task BuyOrder_ShouldReturnIndexView_WhenModelStateIsInvalid()
        {
            // Arrange
            TradeController controller = new TradeController(
                _options,
                _finnhubService,
                _stocksService);

            controller.ModelState.AddModelError("Quantity", "Quantity is required");

            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(x => x.StockSymbol, "AAPL")
                .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();

            _stocksServiceMock
                .Setup(s => s.CreateBuyOrder(It.IsAny<BuyOrderRequest>()))
                .ReturnsAsync(buyOrderResponse);

            // Act
            IActionResult result = await controller.BuyOrder(buyOrderRequest);

            // Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;

            viewResult.ViewName.Should().Be("Index");
            viewResult.Model.Should().BeAssignableTo<StockTrade>();
        }

        [Fact]
        //If model state is valid, the controller should redirect to the Orders action.
        public async Task BuyOrder_ShouldRedirectToOrders_WhenModelStateIsValid()
        {
            // Arrange
            TradeController controller = new TradeController(
                _options,
                _finnhubService,
                _stocksService);

            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(x => x.StockSymbol, "AAPL")
                .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();

            _stocksServiceMock
                .Setup(s => s.CreateBuyOrder(It.IsAny<BuyOrderRequest>()))
                .ReturnsAsync(buyOrderResponse);

            // Act
            IActionResult result = await controller.BuyOrder(buyOrderRequest);

            // Assert
            RedirectToActionResult redirectResult =
                result.Should().BeOfType<RedirectToActionResult>().Subject;

            redirectResult.ActionName.Should().Be("Orders");
            redirectResult.ControllerName.Should().Be("Trade");
        }

        #endregion

        #region SellOrder

        [Fact]
        //If model state is invalid, the controller should return the Index view with a StockTrade model.
        public async Task SellOrder_ShouldReturnIndexView_WhenModelStateIsInvalid()
        {
            // Arrange
            TradeController controller = new TradeController(
                _options,
                _finnhubService,
                _stocksService);

            controller.ModelState.AddModelError("Quantity", "Quantity is required");

            SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(x => x.StockSymbol, "AAPL")
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            SellOrderResponse sellOrderResponse = sellOrder.ToSellOrderResponse();

            _stocksServiceMock
                .Setup(s => s.CreateSellOrder(It.IsAny<SellOrderRequest>()))
                .ReturnsAsync(sellOrderResponse);

            // Act
            IActionResult result = await controller.SellOrder(sellOrderRequest);

            // Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;

            viewResult.ViewName.Should().Be("Index");
            viewResult.Model.Should().BeAssignableTo<StockTrade>();
        }

        [Fact]
        //If model state is valid, the controller should redirect to the Orders action.
        public async Task SellOrder_ShouldRedirectToOrders_WhenModelStateIsValid()
        {
            // Arrange
            TradeController controller = new TradeController(
                _options,
                _finnhubService,
                _stocksService);

            SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(x => x.StockSymbol, "AAPL")
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            SellOrderResponse sellOrderResponse = sellOrder.ToSellOrderResponse();

            _stocksServiceMock
                .Setup(s => s.CreateSellOrder(It.IsAny<SellOrderRequest>()))
                .ReturnsAsync(sellOrderResponse);

            // Act
            IActionResult result = await controller.SellOrder(sellOrderRequest);

            // Assert
            RedirectToActionResult redirectResult =
                result.Should().BeOfType<RedirectToActionResult>().Subject;

            redirectResult.ActionName.Should().Be("Orders");
            redirectResult.ControllerName.Should().Be("Trade");
        }

        #endregion

        #region Orders

        [Fact]
        public async Task Orders_ShouldReturnOrdersViewWithOrdersViewModel()
        {
            // Arrange
            List<BuyOrderResponse> buyOrders = _fixture
                .CreateMany<BuyOrderResponse>(3)
                .ToList();

            List<SellOrderResponse> sellOrders = _fixture
                .CreateMany<SellOrderResponse>(3)
                .ToList();

            _stocksServiceMock.Setup(temp => temp.GetAllBuyOrders()).ReturnsAsync(buyOrders);
            _stocksServiceMock.Setup(temp => temp.GetAllSellOrders()).ReturnsAsync(sellOrders);

            TradeController controller = new TradeController(
                _options,
                _finnhubService,
                _stocksService);

            // Act
            IActionResult result = await controller.Orders();

            // Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;

            Orders model = viewResult.Model.Should().BeAssignableTo<Orders>().Subject;

            model.BuyOrders.Should().BeEquivalentTo(buyOrders);
            model.SellOrders.Should().BeEquivalentTo(sellOrders);
        }

        #endregion

        #region OrdersPDF

        [Fact]
        public async Task OrdersPDF_ShouldReturnViewAsPdfWithOrdersModel()
        {
            // Arrange
            List<BuyOrderResponse> buyOrders = _fixture
                .CreateMany<BuyOrderResponse>(3)
                .ToList();

            List<SellOrderResponse> sellOrders = _fixture
                .CreateMany<SellOrderResponse>(2)
                .ToList();

            _stocksServiceMock.Setup(temp => temp.GetAllBuyOrders()).ReturnsAsync(buyOrders);
            _stocksServiceMock.Setup(temp => temp.GetAllSellOrders()).ReturnsAsync(sellOrders);

            TradeController controller = new TradeController(
                _options,
                _finnhubService,
                _stocksService);

            // Act
            IActionResult result = await controller.OrdersPDF();

            // Assert
            ViewAsPdf pdfResult = result.Should().BeOfType<ViewAsPdf>().Subject;

            pdfResult.ViewName.Should().Be("OrdersPDF");

            Orders model = pdfResult.Model.Should()
                .BeAssignableTo<Orders>()
                .Subject;

            model.BuyOrders.Should().BeEquivalentTo(buyOrders);
            model.SellOrders.Should().BeEquivalentTo(sellOrders);

            pdfResult.PageOrientation.Should()
                .Be(Rotativa.AspNetCore.Options.Orientation.Landscape);

            pdfResult.PageMargins.Left.Should().Be(10);
            pdfResult.PageMargins.Right.Should().Be(10);
            pdfResult.PageMargins.Top.Should().Be(20);
            pdfResult.PageMargins.Bottom.Should().Be(20);
        }

        #endregion

    }
}