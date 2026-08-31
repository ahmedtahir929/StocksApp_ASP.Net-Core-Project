using Microsoft.AspNetCore.Mvc;

namespace StocksApp_xUnit.Controllers
{
  [Route("[controller]")]
  public class HomeController : Controller
  {
    [Route("[action]")]
    public IActionResult Error()
    {
      return View();
    }
  }
}
