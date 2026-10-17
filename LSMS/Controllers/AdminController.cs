using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LSMS.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseApiController
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new
            {
                message = "Admin access granted."
            });
        }
    }
}
