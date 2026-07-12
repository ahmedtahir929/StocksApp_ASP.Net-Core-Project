using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO for sell order response which will be used as return object for requests
    /// </summary>
    public class SellOrderResponse
    {
        public Guid SellOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double? TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not SellOrderResponse other)
                return false;

            return SellOrderID == other.SellOrderID &&
                   StockName == other.StockName &&
                   StockSymbol == other.StockSymbol &&
                   Quantity == other.Quantity &&
                   DateAndTimeOfOrder == other.DateAndTimeOfOrder &&
                   Price == other.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                SellOrderID,
                StockName,
                StockSymbol,
                Quantity,
                DateAndTimeOfOrder,
                Price
            );
        }
    }

    public static class SellOrderExtensions
    {
        /// <summary>
        /// Extension method to convert domain model object SellOrder to SellOrderResponse
        /// </summary>
        /// <param name="sellOrder">Takes current sellOrder object to convert</param>
        /// <returns>Returns SellOrderResponse object</returns>
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                Price = sellOrder.Price,
            };
        }
    }
}
