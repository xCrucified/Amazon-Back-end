using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using business_logic.Interfaces;
using business_logic.DTOs.Google;

namespace Amazon_Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAuthController : Controller
    {
        private readonly IAccountService _accountsService;

        public GoogleAuthController(IAccountService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpPost("login/google")]
        public IActionResult Login([FromBody] GoogleToken model)
        {
            try
            {
                var token = _accountsService.GoogleLoginAsync(model.Token);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }

    }
}
