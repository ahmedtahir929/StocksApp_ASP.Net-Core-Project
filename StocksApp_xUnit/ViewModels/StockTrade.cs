namespace StocksApp_xUnit.ViewModels
{
    public class StockTrade
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public string? Logo { get; set; }
        public double Price { get; set; }
        public uint Quantity { get; set; }
    }
}
