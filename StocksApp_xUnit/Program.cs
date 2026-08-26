using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Rotativa.AspNetCore;
using Serilog;
using ServiceContracts;
using Services;
using StocksApp_xUnit.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterType<FinnhubService>().As<IFinnhubService>().InstancePerLifetimeScope();
    container.RegisterType<StocksService>().As<IStocksService>().InstancePerLifetimeScope();
    container.RegisterType<FinnhubRepository>().As<IFinnhubRepository>().InstancePerLifetimeScope();
    container.RegisterType<StocksRepository>().As<IStocksRepository>().InstancePerLifetimeScope();
});
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);
});

builder.Services.AddHttpLogging();
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddLogging();

if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<Entities.StockMarketDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
}

builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.Configure<TradingApiOptions>(builder.Configuration.GetSection("TradingApiOptions"));

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

if (!builder.Environment.IsEnvironment("Test")) 
{ 
    RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

app.UseSerilogRequestLogging();
app.UseHttpLogging();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { } // Added for integration testing purposes