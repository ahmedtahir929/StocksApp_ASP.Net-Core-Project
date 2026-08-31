namespace ServiceContracts
{
  public interface IFinnhubStocksService
  {

    /// <summary>
    /// Fetches the list of top 25 stocks
    /// </summary>
    /// <returns>Returns the list of top 25 stocks</returns>
    Task<List<Dictionary<string, string>>?> GetStocks();
  }
}