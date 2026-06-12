using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Security.Identity;

namespace Shopping.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
       

    }
}
    