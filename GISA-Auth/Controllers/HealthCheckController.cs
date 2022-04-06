using GISA_Auth.Auth;
using GISA_Auth.Auth.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GISA_Auth.Controllers
{
    [ApiController]
    [Route("healthcheck")]
    public class HealthCheckController : ControllerBase
    {
        public HealthCheckController() { }

        [HttpGet(Name = "GetHealthCheck")]
        public async Task<IActionResult> Get()
        {
            return Ok(new Response { Status = StatusCategory.Success, Message = "I'm Groot" });
        }
    }
}