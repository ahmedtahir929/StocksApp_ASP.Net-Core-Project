namespace ServiceContracts
{
    public interface IFinnhubService
    {
        /// <summary>
        /// Gets company profile of the related stock symbol
        /// </summary>
        /// <param name="stockSymbol">stock symbol to be passed to get company profile</param>
        /// <returns>
        /// Returns Dictionary<string, object> object consisting the json result
        /// of the related company profile
        /// </returns>
        Task<Dictionary<string, object>?> GetCompanyProfile(string? stockSymbol);

        /// <summary>
        /// Gets stock price quote of the related stock symbol
        /// </summary>
        /// <param name="stockSymbol">stock symbol to be passed to get stock price quote</param>
        /// <returns>
        /// Returns Dictionary<string, object> object consisting the json result
        /// of the related stock price quote
        /// </returns>
        Task<Dictionary<string, object>?> GetStockPriceQuote(string? stockSymbol);


        /// <summary>
        /// Fetches the list of top 25 stocks
        /// </summary>
        /// <returns>Returns the list of top 25 stocks</returns>
        Task<List<Dictionary<string, string>>?> GetStocks();


        /// <summary>
        /// Search from the available top 25 stocks
        /// </summary>
        /// <param name="stockSymbolToSearch">stock symbol to provide to search within stocks</param>
        /// <returns>Returns the related stock</returns>
        Task<Dictionary<string, object>?> SearchStocks(string? stockSymbolToSearch);
    }
}
