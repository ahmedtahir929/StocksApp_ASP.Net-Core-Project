using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Rotativa.AspNetCore;
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

builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Entities.StockMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.Configure<TradingApiOptions>(builder.Configuration.GetSection("TradingApi"));

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
