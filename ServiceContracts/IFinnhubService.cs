namespace ServiceContracts
{
    public interface IFinnHubService
    {
        /// <summary>
        /// Gets company profile of the related stock symbol
        /// </summary>
        /// <param name="stockSymbol">stock symbol to be passed to get company profile</param>
        /// <returns>
        /// Returns Dictionary<string, object> object consisting the json result
        /// of the related company profile
        /// </returns>
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

        /// <summary>
        /// Gets stock price quote of the related stock symbol
        /// </summary>
        /// <param name="stockSymbol">stock symbol to be passed to get stock price quote</param>
        /// <returns>
        /// Returns Dictionary<string, object> object consisting the json result
        /// of the related stock price quote
        /// </returns>
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}
