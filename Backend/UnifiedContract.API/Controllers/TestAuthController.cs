using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UnifiedContract.API.Controllers
{
    [ApiController]
    [Route("api/test-auth")]
    public class TestAuthController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get() => Ok("Authenticated!");
    }
} 