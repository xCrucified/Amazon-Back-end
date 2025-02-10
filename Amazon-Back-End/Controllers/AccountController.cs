using business_logic.DTOs;
using business_logic.DTOs.User;
using business_logic.Interfaces;
using business_logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Amazon_Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService service)
        {
            this.accountService = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            await accountService.Register(model);
            return Ok();
        }

        [HttpPost("login-via-email")]
        public async Task<IActionResult> LoginEmail([FromForm] LoginModelEmail model)
        {
            return Ok(await accountService.LoginViaEmail(model));
        }

        [HttpPost("login-via-phone")]
        public async Task<IActionResult> LoginPhone([FromForm] LoginModelPhone model)
        {
            return Ok(await accountService.LoginViaPhone(model));
        }

        [HttpPost("refreshTokens")]
        public async Task<IActionResult> RefreshTokens(UserToken tokens)
        {
            return Ok(await accountService.RefreshTokens(tokens));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutModel model)
        {
            await accountService.Logout(model.RefreshToken);
            return Ok();
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            bool exists = await accountService.CheckEmailExistence(email);
            return Ok(new { exists });
        }

        [HttpGet("check-phone-number")]
        public async Task<IActionResult> CheckPhoneNumberExists([FromQuery] string phonenumber)
        {
            bool exists = await accountService.CheckPhoneNumberExistence(phonenumber);
            return Ok(new { exists });
        }

    }
}
