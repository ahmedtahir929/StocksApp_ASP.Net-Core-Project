# 📈 StocksApp

A stock trading simulator built with **ASP.NET Core MVC (.NET 10)**. It lets a user browse a curated list of stocks, view live price quotes and company profiles pulled from the **Finnhub API**, place simulated buy/sell orders, review order history, and export that history as a **PDF report**.

The codebase is split into small, independent class libraries (Clean/Onion‑style layering) and was built following **Test‑Driven Development** with **xUnit**.

---

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Tech Stack](#tech-stack)
- [How It Works](#how-it-works)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Running Tests](#running-tests)
- [Roadmap](#roadmap)
- [License](#license)

---

## Features

- 🔎 **Explore stocks** — browse a configurable list of top stocks (symbol + company name) with an interactive detail panel per symbol.
- 💹 **Live quotes** — real‑time price, and company profile (name, ticker, logo) fetched from Finnhub.
- 🛒 **Buy / Sell orders** — place buy and sell orders against a stock, with server‑side validation via Data Annotations and custom validators.
- 📜 **Order history** — view all buy and sell orders in sortable tables, rendered as partial views/ViewComponents.
- 🧾 **PDF export** — download the full order history as a printable PDF using Rotativa.
- 🗄️ **Persistence** — orders are stored in SQL Server via EF Core, with Fluent API constraints (e.g. quantity/price range checks) enforced at the database level.
- 🧪 **Unit‑tested service layer** — business logic is covered by xUnit tests written TDD‑style.
- 🧩 **Dependency Injection via Autofac** — services and repositories are registered against their contracts, not their concrete types.

---

## Architecture

The solution is organized as a set of layered class libraries around a single ASP.NET Core MVC web project, so each concern can be developed, tested, and swapped independently.

```
┌─────────────────────────────┐
│   StocksApp_xUnit (Web)     │  Controllers, Views, ViewComponents,
│   ASP.NET Core MVC          │  ViewModels, Program.cs (composition root)
└───────────────┬─────────────┘
                │ depends on
                ▼
┌─────────────────────────────┐
│   Services / ServiceContracts│  Business logic (IStocksService,
│                              │  IFinnhubService) + DTOs + validators
└───────────────┬─────────────┘
                │ depends on
                ▼
┌─────────────────────────────┐
│ Repositories / RepositoryContracts │  Data access abstractions
│                              │  (IStocksRepository, IFinnhubRepository)
└───────────────┬─────────────┘
                │ depends on
                ▼
┌─────────────────────────────┐
│   Entities (EF Core)        │  BuyOrder, SellOrder, StockMarketDbContext
└───────────────┬─────────────┘
                │
                ▼
       SQL Server (LocalDB)

┌─────────────────────────────┐
│   Options                   │  Strongly-typed configuration
│                              │  (TradingOptions, TradingApiOptions)
└─────────────────────────────┘

┌─────────────────────────────┐
│   StockAppTests (xUnit)     │  Tests the Services layer in isolation
└─────────────────────────────┘
```

**Key architectural decisions:**

- **Contracts-first layering** — `ServiceContracts` and `RepositoryContracts` define interfaces that `Services` and `Repositories` implement, so controllers depend only on abstractions (`IStocksService`, `IFinnhubService`).
- **Autofac as the DI container** — registered in `Program.cs` via `AutofacServiceProviderFactory`, mapping each service/repository interface to its implementation with per‑lifetime‑scope instancing.
- **Strongly-typed configuration** — `TradingOptions` (default order quantity, top‑25 stock list) and `TradingApiOptions` (Finnhub API key) are bound from `appsettings.json` via the Options pattern, rather than read as raw strings.
- **Two independent HTTP integrations feeding one service layer** — `FinnhubRepository` talks to the external Finnhub API while `StocksRepository`/EF Core talks to SQL Server; both are consumed through `Services` so the controllers never call either directly.
- **Fluent API constraints at the database layer** — `StockMarketDbContext.OnModelCreating` enforces column types and check constraints (e.g. quantity between 1–100,000) so invalid data can't reach the database even if service-layer validation is bypassed.

---

## Project Structure

| Project | Responsibility |
|---|---|
| `StocksApp_xUnit` | The ASP.NET Core MVC web app — controllers (`StockController`, `TradeController`), Razor views, ViewComponents, ViewModels, and the app's composition root (`Program.cs`). |
| `Services` | Business logic implementations: `StocksService` (order creation/retrieval) and `FinnhubService` (stock data orchestration). |
| `ServiceContracts` | Interfaces (`IStocksService`, `IFinnhubService`), request/response DTOs, and custom validation attributes. |
| `Repositories` | Data access implementations: `StocksRepository` (EF Core) and `FinnhubRepository` (Finnhub HTTP client). |
| `RepositoryContracts` | Interfaces for the repository layer (`IStocksRepository`, `IFinnhubRepository`). |
| `Entities` | EF Core domain models (`BuyOrder`, `SellOrder`) and the `StockMarketDbContext`, plus migrations. |
| `Options` | Strongly-typed configuration classes bound to `appsettings.json` sections. |
| `StockAppTests` | xUnit tests for the `Services` layer. |

---

## Tech Stack

**Backend**
- ASP.NET Core MVC (.NET 10)
- C#
- Entity Framework Core (Code First + Migrations)
- SQL Server (LocalDB)
- Autofac (Dependency Injection)

**Frontend**
- Razor Views, ViewComponents
- HTML5, CSS3, Bootstrap, jQuery

**Testing**
- xUnit

**External Integrations**
- [Finnhub API](https://finnhub.io/) — live stock prices and company profiles
- Rotativa.AspNetCore — server-side HTML‑to‑PDF rendering

---

## How It Works

1. **Explore (`/Stock/Explore`)** — `StockController` reads the configured `Top25PopularStocks` list, calls `IFinnhubService.GetStocks()`, and filters/orders the results into a symbol/name list. Selecting a symbol loads a `SelectedStock` ViewComponent via `/Stock/GetStockDetails/{symbol}`.
2. **Trade (`/` or `/Trade/Index/{symbol}`)** — `TradeController` fetches the company profile and live price quote for a symbol (defaulting to `MSFT`) from `IFinnhubService`, and pre-fills a buy/sell form.
3. **Placing an order** — `POST /Trade/BuyOrder` or `POST /Trade/SellOrder` validate the submitted form (Data Annotations + custom validators); on success, `IStocksService` persists the order via EF Core and redirects to `/Trade/Orders`.
4. **Order history (`/Trade/Orders`)** — buy and sell orders are loaded through `IStocksService` and rendered via `BuyTable`/`SellTable` ViewComponents.
5. **PDF export (`/Trade/OrdersPDF`)** — the same order data is rendered through Rotativa as a landscape PDF for download.

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (installed with Visual Studio, or standalone)
- Visual Studio 2022 (or later) — optional, but recommended for LocalDB tooling
- A free [Finnhub API key](https://finnhub.io/register)

### Installation

```bash
# 1. Clone the repository
git clone https://github.com/ahmedtahir929/StocksApp_ASP.Net-Core-Project.git
cd StocksApp_ASP.Net-Core-Project

# 2. Restore dependencies
dotnet restore

# 3. Add your Finnhub API key (see Configuration below)

# 4. Apply EF Core migrations to create the database
dotnet ef database update --project Entities --startup-project StocksApp_xUnit

# 5. Run the app
dotnet run --project StocksApp_xUnit
```

Or open `StocksApp_xUnit.slnx` in Visual Studio and press **F5**.

The app listens on the URL(s) printed in the console (e.g. `https://localhost:xxxx`) and defaults to the Trade page for `MSFT`.

---

## Configuration

Configuration lives in `StocksApp_xUnit/appsettings.json` (and `appsettings.Development.json`).

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OrdersDatabase;Integrated Security=True;..."
  },
  "TradingOptions": {
    "DefaultOrderQuantity": 100,
    "Top25PopularStocks": "AAPL,MSFT,AMZN,TSLA,GOOGL,GOOG,NVDA,BRK.B,META,UNH,JNJ,JPM,V,PG,XOM,HD,CVX,MA,BAC,ABBV,PFE,AVGO,COST,DIS,KO"
  },
  "TradingApi": {
    "ApiKey": "YOUR_FINNHUB_API_KEY"
  }
}
```

> ⚠️ **Don't commit real API keys.** For local development, prefer [.NET User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) — the project already has a `UserSecretsId` configured:
> ```bash
> dotnet user-secrets set "TradingApi:ApiKey" "YOUR_FINNHUB_API_KEY" --project StocksApp_xUnit
> ```

| Setting | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server LocalDB connection string for the `OrdersDatabase`. |
| `TradingOptions:DefaultOrderQuantity` | Quantity pre-filled on the buy/sell form. |
| `TradingOptions:Top25PopularStocks` | Comma-separated symbols shown on the Explore page. |
| `TradingApi:ApiKey` | Your Finnhub API token, bound to `TradingApiOptions`. |

---

## Running Tests

Unit tests target the `Services` layer and use xUnit.

```bash
dotnet test StockAppTests
```

Tests cover buy/sell order creation, retrieval, and validation behavior of `StocksService`.

---

## Roadmap

Ideas for extending the project:

- [ ] Add authentication/authorization so orders are scoped per user
- [ ] Add integration tests for controllers and the Finnhub repository (with a mocked `HttpClient`)
- [ ] Cache Finnhub responses to reduce API calls and handle rate limits gracefully
- [ ] Add sorting/filtering on the order history tables
- [ ] Containerize with Docker Compose (app + SQL Server)

---

## License

This project is for educational and portfolio purposes.
