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
        private readonly IAccountService _accountService;
        public AccountController(IAccountService service)
        {
            this._accountService = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            await _accountService.Register(model);
            return Ok();
        }

        [HttpPost("change-email")]
        public async Task<IActionResult> ChangeEmail(EmailChangeModel model)
        {
            await _accountService.ChangeEmail(model);
            return Ok();
        }

        [HttpPost("change-phone-number")]
        public async Task<IActionResult> ChangePhoneNumber(PhoneNumberChangeModel model)
        {
            await _accountService.ChangePhoneNumber(model);
            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(PasswordChangeModel model)
        {
            await _accountService.ChangePassword(model);
            return Ok();
        }

        [HttpPost("login-via-email")]
        public async Task<IActionResult> LoginEmail(LoginModelEmail model)
        {
            return Ok(await _accountService.LoginViaEmail(model));
        }

        [HttpPost("login-via-phone")]
        public async Task<IActionResult> LoginPhone(LoginModelPhone model)
        {
            return Ok(await _accountService.LoginViaPhone(model));
        }

        [HttpPost("refreshTokens")]
        public async Task<IActionResult> RefreshTokens(UserToken tokens)
        {
            return Ok(await _accountService.RefreshTokens(tokens));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutModel model)
        {
            await _accountService.Logout(model.RefreshToken);
            return Ok();
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            bool exists = await _accountService.CheckEmailExistence(email);
            return Ok(new { exists });
        }

        [HttpGet("check-phone-number")]
        public async Task<IActionResult> CheckPhoneNumberExists([FromQuery] string phonenumber)
        {
            bool exists = await _accountService.CheckPhoneNumberExistence(phonenumber);
            return Ok(new { exists });
        }

    }
}
