using business_logic.DTOs;
using business_logic.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Interfaces
{
    public interface IAccountService
    {
        Task Register(RegisterModel model);
        Task<LoginResponseDto> LoginViaEmail(LoginModelEmail model);
        Task<LoginResponseDto> LoginViaPhone(LoginModelPhone model);
        ChallengeResult LoginViaGoogle();
        Task<object> HandleGoogleResponse();
        object GetUserProfile(ClaimsPrincipal user);
        Task Logout(string refreshToken);
        Task<UserToken> RefreshTokens(UserToken tokens);
        public Task<bool> CheckEmailExistence(string email);
        Task RemoveExpiredRefreshTokens();
        Task<bool> CheckPhoneNumberExistence(string phoneNumber);
        Task ChangeEmail(EmailChangeModel model);
        Task ChangePassword(PasswordChangeModel model);
        Task ChangePhoneNumber(PhoneNumberChangeModel model);
    }
    public class ResetToken
    {
        public string Token { get; set; }
    }
    public class ResetPasswordModel
    {
        public string? NewPassword { get; set; }
        public string Token { get; set; }
    }
}
