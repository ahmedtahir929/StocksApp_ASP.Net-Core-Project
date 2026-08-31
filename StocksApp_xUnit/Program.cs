using Autofac;
using Autofac.Extensions.DependencyInjection;
using Repositories;
using RepositoryContracts;
using Rotativa.AspNetCore;
using Serilog;
using ServiceContracts;
using Services;
using StocksApp_xUnit.Middleware;
using StocksApp_xUnit.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
  container.RegisterType<FinnhubCompanyProfileService>().As<IFinnhubCompanyProfileService>().InstancePerLifetimeScope();
  container.RegisterType<FinnhubSearchStocksService>().As<IFinnhubSearchStocksService>().InstancePerLifetimeScope();
  container.RegisterType<FinnhubStockPriceQuoteService>().As<IFinnhubStockPriceQuoteService>().InstancePerLifetimeScope();
  container.RegisterType<FinnhubStocksService>().As<IFinnhubStocksService>().InstancePerLifetimeScope();
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

//Configure services
builder.Services.ConfigureServices(builder.Environment, builder.Configuration);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}
else
{
  app.UseExceptionHandler("/Home/Error");
  app.UseExceptionHandlingMiddleware();
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