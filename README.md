# 📈 StocksApp

A Stock Trading web application built with **ASP.NET Core MVC** that allows users to view live Microsoft stock prices, place buy and sell orders, and export order history as PDF reports. The application follows a layered architecture and was developed using **Test-Driven Development (TDD)** with **xUnit**.

## 🚀 Features

- Live Microsoft stock price retrieval using the Finnhub API
- Buy and Sell stock order management
- Full CRUD operations for stock orders
- Server-side validation using Data Annotations
- Entity Framework Core with SQL Server (LocalDB)
- Database configuration using Fluent API
- PDF export of buy and sell orders using Rotativa
- Dependency Injection for loose coupling
- Unit testing with xUnit
- Clean MVC architecture

## 🛠️ Technologies Used

### Backend
- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server (LocalDB)
- LINQ
- Dependency Injection

### Frontend
- Razor Views
- HTML5
- CSS3
- Bootstrap
- jQuery

### Testing
- xUnit
- FluentAssertions

### External Libraries
- Rotativa.AspNetCore
- Finnhub API

## 🏗️ Architecture

The application follows the **ASP.NET Core MVC** architecture.

```
Presentation Layer
        │
        ▼
Controllers
        │
        ▼
Service Layer
        │
        ▼
Entity Framework Core
        │
        ▼
SQL Server Database
```

The service layer separates business logic from controllers, improving maintainability and testability.

## 📊 Database

The application uses **Microsoft SQL Server LocalDB** with **Entity Framework Core**.

Features include:

- Code First Approach
- EF Core Migrations
- Fluent API configurations
- Data Validation
- Relationships between entities

## 🌐 Finnhub API

The application integrates with the Finnhub REST API to fetch live stock market data.

Example endpoint:

```
GET https://finnhub.io/api/v1/quote
```

API Key configuration:

```json
"Finnhub": {
  "Token": "YOUR_API_KEY"
}
```

## 📄 PDF Export

Order history can be downloaded as a PDF document using **Rotativa.AspNetCore**, generating printable reports directly from Razor Views.

## ✅ Testing

The project follows **Test-Driven Development (TDD)**.

Tests include:

- Service layer unit tests
- CRUD operation testing
- Validation testing
- Sorting and filtering tests
- Exception handling tests

Frameworks:

- xUnit
- FluentAssertions

Run tests:

```bash
dotnet test
```

## ⚙️ Getting Started

### Prerequisites

- Visual Studio 2022
- .NET 8 SDK (or the version used by the project)
- SQL Server LocalDB
- Finnhub API Key

### Installation

Clone the repository.

```bash
git clone https://github.com/yourusername/StocksApp.git
```

Navigate to the project directory.

```bash
cd StocksApp
```

Restore NuGet packages.

```bash
dotnet restore
```

Update the database.

```bash
dotnet ef database update
```

Run the application.

```bash
dotnet run
```

Or simply open the solution in **Visual Studio** and press **F5**.

## 📸 Screenshots

Add screenshots of:

- Home Page
- Stock Quote Page
- Buy Order Form
- Sell Order Form
- Orders Table
- PDF Export

## 📚 Learning Outcomes

This project helped strengthen practical knowledge of:

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Dependency Injection
- Repository and Service Layer concepts
- REST API Integration
- Fluent API
- Unit Testing with xUnit
- Test-Driven Development (TDD)
- PDF Generation in ASP.NET Core
- Clean Application Architecture

## 📄 License

This project is for educational and portfolio purposes.
