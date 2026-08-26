using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StocksApp_xUnit.Controllers;

namespace StocksApp_xUnit.Filters.ActionFilters
{
  public class ValidateModelStateActionFilter : IAsyncActionFilter
  {
    private readonly ILogger<ValidateModelStateActionFilter> _logger;

    public ValidateModelStateActionFilter(ILogger<ValidateModelStateActionFilter> logger)
    {
      _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      //TO DO: before logic
      _logger.LogInformation("{ActionFilterName}.{MethodName}() invoked - before", nameof(ValidateModelStateActionFilter), nameof(OnActionExecutionAsync));

      if (context.Controller is TradeController tradeController &&
        !tradeController.ModelState.IsValid)
      {
        context.Result = new RedirectToActionResult(
        nameof(TradeController.Index),
        "Trade",
        null);
      }
      else
        await next();

      //TO DO: after logic
      _logger.LogInformation("{ActionFilterName}.{MethodName}() invoked - after", nameof(ValidateModelStateActionFilter), nameof(OnActionExecutionAsync));
    }
  }
}
