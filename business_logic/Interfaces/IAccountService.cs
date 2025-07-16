using BLL.DTOs;
using BLL.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAccountService
    {
        Task Register(RegisterModel model);
        Task<LoginResponseDto> LoginViaEmail(LoginModelEmail model);
        Task<LoginResponseDto> LoginViaPhone(LoginModelPhone model);
        Task Logout(string refreshToken);
        Task<UserToken> RefreshTokens(UserToken tokens);
        public Task<bool> CheckEmailExistence(string email);
        Task RemoveExpiredRefreshTokens();
        Task<bool> CheckPhoneNumberExistence(string phoneNumber);
        Task ChangeEmail(EmailChangeModel model);
        Task ChangePassword(PasswordChangeModel model);
        Task ChangePhoneNumber(PhoneNumberChangeModel model);
        Task<string> GoogleLoginAsync(string googleToken);
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
