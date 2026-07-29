using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO for buy order response, which is returned as response object for requests
    /// </summary>
    public class BuyOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder {  get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double? TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not BuyOrderResponse other)
                return false;

            return BuyOrderID == other.BuyOrderID &&
                   StockName == other.StockName &&
                   StockSymbol == other.StockSymbol &&
                   Quantity == other.Quantity &&
                   DateAndTimeOfOrder == other.DateAndTimeOfOrder &&
                   Price == other.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                BuyOrderID,
                StockName,
                StockSymbol,
                Quantity,
                DateAndTimeOfOrder,
                Price
            );
        }
    }

    public static class BuyOrderExtensions
    {
        /// <summary>
        /// Extension method to convert BuyOrder domain model object to BuyOrderResponse
        /// </summary>
        /// <param name="buyOrder">Takes current BuyOrder object to convert</param>
        /// <returns>Returns a BuyOrderResponse object</returns>
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                Price = buyOrder.Price,
                TradeAmount = buyOrder.Price * buyOrder.Quantity
            };
        }
    }
}
