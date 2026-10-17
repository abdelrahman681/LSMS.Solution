using LSMS.API.Errors;
using LSMS.Application.DTO.Auth;
using LSMS.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LSMS.API.Controllers
{

    public class AuthController : BaseApiController
    {
        #region Field
        private readonly IAuthService _auth;
        private readonly ILogger _logger;
        #endregion
        #region Ctor
        public AuthController(IAuthService auth,ILogger logger)
        {
            _auth = auth;
            this._logger = logger;
        }
        #endregion
        #region Method
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _auth.LoginAsync(login, cancellationToken);
                if (result is null)
                    return Unauthorized(new ErrorApiResponse(401, "Invalid email or password, or account is inactive"));
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed.");

                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion
    }
}
