using business_logic.DTOs;
using business_logic.DTOs.User;
using business_logic.Interfaces;
using business_logic.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
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
        public async Task<IActionResult> Register(RegisterModel model)
        {
            await accountService.Register(model);
            return Ok();
        }

        [HttpPost("change-email")]
        public async Task<IActionResult> ChangeEmail(EmailChangeModel model)
        {
            await accountService.ChangeEmail(model);
            return Ok();
        }

        [HttpPost("change-phone-number")]
        public async Task<IActionResult> ChangePhoneNumber(PhoneNumberChangeModel model)
        {
            await accountService.ChangePhoneNumber(model);
            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(PasswordChangeModel model)
        {
            await accountService.ChangePassword(model);
            return Ok();
        }

        [HttpPost("login-via-email")]
        public async Task<IActionResult> LoginEmail(LoginModelEmail model)
        {
            return Ok(await accountService.LoginViaEmail(model));
        }

        [HttpPost("login-via-phone")]
        public async Task<IActionResult> LoginPhone(LoginModelPhone model)
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
