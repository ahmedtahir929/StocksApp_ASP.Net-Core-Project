namespace RepositoryContracts
{
    public interface IFinnhubRepository
    {
        /// <summary>
        /// Retrieves the company profile for the specified stock symbol.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol of the company.</param>
        /// <returns>
        /// A dictionary containing the company profile data if found; otherwise, <c>null</c>.
        /// </returns>
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

        /// <summary>
        /// Retrieves the latest stock price quote for the specified stock symbol.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol.</param>
        /// <returns>
        /// A dictionary containing the stock price quote if successful; otherwise, <c>null</c>.
        /// </returns>
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);

        /// <summary>
        /// Retrieves the list of available stocks configured in the application settings.
        /// </summary>
        /// <returns>
        /// A list of stock details if successful; otherwise, <c>null</c>.
        /// </returns>
        Task<List<Dictionary<string, string>>?> GetStocks();

        /// <summary>
        /// Searches for a stock that matches the specified stock symbol.
        /// </summary>
        /// <param name="stockSymbolToSearch">The stock symbol to search for.</param>
        /// <returns>
        /// A dictionary containing the matching stock information if found; otherwise, <c>null</c>.
        /// </returns>
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
    }
}