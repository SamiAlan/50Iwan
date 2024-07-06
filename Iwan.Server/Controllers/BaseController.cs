using Microsoft.AspNetCore.Mvc;

namespace Iwan.Server.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class BaseController : Controller
    {

    }
}
