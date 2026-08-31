namespace ServiceContracts
{
  public interface IFinnhubSearchStocksService
  {
    /// <summary>
    /// Search from the available top 25 stocks
    /// </summary>
    /// <param name="stockSymbolToSearch">stock symbol to provide to search within stocks</param>
    /// <returns>Returns the related stock</returns>
    Task<Dictionary<string, object>?> SearchStocks(string? stockSymbolToSearch);
  }
}