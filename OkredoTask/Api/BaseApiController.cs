using Microsoft.AspNetCore.Mvc;

namespace OkredoTask.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : Controller
    {
    }
}