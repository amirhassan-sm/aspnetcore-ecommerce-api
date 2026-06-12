using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleManagerController : ControllerBase
    {
        [HttpGet]
     
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
