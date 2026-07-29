using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;

namespace StockAppTests
{
    public class StockServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IStocksService _stockService;

        public StockServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _stockService = new StocksService();
        }

        #region CreateBuyOrder
        [Fact]
        public async Task CreateBuyOrder_NullBuyOrder()
        {
            //Arrage
            BuyOrderRequest? buyOrderRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            
                //Act
                _stockService.CreateBuyOrder(buyOrderRequest)
            );
        }

        [Fact]
        public async Task CreateBuyOrder_MinQuantity()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "abc",
                StockSymbol = "123",
                Quantity = 0,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 121.23
            };

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stockService.CreateBuyOrder(buyOrderRequest));
        }

        [Fact]
        public async Task CreateBuyOrder_MaxQuantity()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "abc",
                StockSymbol = "123",
                Quantity = 100001,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 121.23
            };

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stockService.CreateBuyOrder(buyOrderRequest));
        }

        [Fact]
        public async Task CreateBuyOrder_NullSymbol()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "abc",
                StockSymbol = null,
                Quantity = 100001,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 121.23
            };

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stockService.CreateBuyOrder(buyOrderRequest));
        }

        [Fact]
        public async Task CreateBuyOrder_OlderDate()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "abc",
                StockSymbol = null,
                Quantity = 100001,
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
                Price = 121.23
            };

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stockService.CreateBuyOrder(buyOrderRequest));
        }

        [Fact]
        public async Task CreateBuyOrder_ValidBuyOrder()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "abc",
                StockSymbol = "123",
                Quantity = 1000,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 121.23
            };

            //Act
            BuyOrderResponse buy_order_response_from_create = await _stockService.CreateBuyOrder(buyOrderRequest);
            List<BuyOrderResponse> buy_order_response_from_get = await _stockService.GetAllBuyOrders();

            Assert.Contains(buy_order_response_from_get, order => 
            order.BuyOrderID == buy_order_response_from_create.BuyOrderID);
        }
        #endregion

        #region CreateSellOrder
        [Fact]
        public async Task CreateSellOrder_NullBuyOrder()
        {
            //Arrage
            SellOrderRequest? sellOrderRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>

                //Act
                _stockService.CreateSellOrder(sellOrderRequest)
            );
        }

        [Fact]
        public async Task CreateSellOrder_MinQuantity()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = "abc",
                StockSymbol = "123",
                Quantity = 0,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 121.23
            };

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stockService.CreateSellOrder(sellOrderRequest));
        }

        [Fact]
        public async Task CreateSellOrder_MaxQuantity()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = "abc",
                StockSymbol = "123",
                Quantity = 100001,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 121.23
            };

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stockService.CreateSellOrder(sellOrderRequest));
        }

        [Fact]
        public async Task CreateSellOrder_NullSymbol()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = "abc",
                StockSymbol = null,
                Quantity = 100001,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 121.23
            };

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stockService.CreateSellOrder(sellOrderRequest));
        }

        [Fact]
        public async Task CreateSellOrder_OlderDate()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = "abc",
                StockSymbol = null,
                Quantity = 100001,
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
                Price = 121.23
            };

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentException>(() => _stockService.CreateSellOrder(sellOrderRequest));
        }

        [Fact]
        public async Task CreateBuyOrder_ValidSellOrder()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = "abc",
                StockSymbol = "123",
                Quantity = 1000,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 121.23
            };

            //Act
            SellOrderResponse sell_order_response_from_create = await _stockService.CreateSellOrder(sellOrderRequest);
            List <SellOrderResponse> sell_order_response_from_get = await _stockService.GetAllSellOrders();

            Assert.Contains(sell_order_response_from_get, order =>
            order.SellOrderID == sell_order_response_from_create.SellOrderID);
        }
        #endregion

        #region GetAllBuyOrders
        [Fact]
        public async Task GetAllBuyOrders_EmptyList()
        {
            //Act
            List<BuyOrderResponse> buy_orders_from_get = await _stockService.GetAllBuyOrders();

            //Assert
            Assert.Empty(buy_orders_from_get);
        }

        [Fact]
        public async Task GetAllBuyOrders_ValidBuyOrders()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest1 = new BuyOrderRequest()
            {
                StockName = "abc",
                StockSymbol = "123",
                Quantity = 20,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 101.63
            };
            BuyOrderRequest buyOrderRequest2 = new BuyOrderRequest()
            {
                StockName = "xyz",
                StockSymbol = "456",
                Quantity = 100,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 110.27
            };
            BuyOrderRequest buyOrderRequest3 = new BuyOrderRequest()
            {
                StockName = "lmn",
                StockSymbol = "789",
                Quantity = 10,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 126.23
            };

            BuyOrderResponse buy_order_response1 = await _stockService.CreateBuyOrder(buyOrderRequest1);
            BuyOrderResponse buy_order_response2 = await _stockService.CreateBuyOrder(buyOrderRequest2);
            BuyOrderResponse buy_order_response3 = await _stockService.CreateBuyOrder(buyOrderRequest3);

            List<BuyOrderResponse> buy_orders = new List<BuyOrderResponse>();

            buy_orders.Add(buy_order_response1);
            buy_orders.Add(buy_order_response2);
            buy_orders.Add(buy_order_response3);

            //Act
            List<BuyOrderResponse> buy_orders_from_get = await _stockService.GetAllBuyOrders();

            //Assert
            foreach (BuyOrderResponse orderResponse in buy_orders)
            {
                Assert.Contains(orderResponse, buy_orders_from_get);
            }
        }
        #endregion

        #region GetAllSellOrders
        [Fact]
        public async Task GetAllSellOrders_EmptyList()
        {
            //Act
            List<SellOrderResponse> sell_orders_from_get = await _stockService.GetAllSellOrders();

            //Assert
            Assert.Empty(sell_orders_from_get);
        }

        [Fact]
        public async Task GetAllSellOrders_ValidBuyOrders()
        {
            //Arrange
            SellOrderRequest sellOrderRequest1 = new SellOrderRequest()
            {
                StockName = "abc",
                StockSymbol = "123",
                Quantity = 20,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 101.63
            };
            SellOrderRequest sellOrderRequest2 = new SellOrderRequest()
            {
                StockName = "xyz",
                StockSymbol = "456",
                Quantity = 100,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 110.27
            };
            SellOrderRequest sellOrderRequest3 = new SellOrderRequest()
            {
                StockName = "lmn",
                StockSymbol = "789",
                Quantity = 10,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 126.23
            };

            SellOrderResponse sell_order_response1 = await _stockService.CreateSellOrder(sellOrderRequest1);
            SellOrderResponse sell_order_response2 = await _stockService.CreateSellOrder(sellOrderRequest2);
            SellOrderResponse sell_order_response3 = await _stockService.CreateSellOrder(sellOrderRequest3);

            List<SellOrderResponse> sell_orders = new List<SellOrderResponse>();

            sell_orders.Add(sell_order_response1);
            sell_orders.Add(sell_order_response2);
            sell_orders.Add(sell_order_response3);

            //Act
            List<SellOrderResponse> sell_orders_from_get = await _stockService.GetAllSellOrders();

            //Assert
            foreach (SellOrderResponse orderResponse in sell_orders)
            {
                Assert.Contains(orderResponse, sell_orders_from_get);
            }
        }
        #endregion
    }
}