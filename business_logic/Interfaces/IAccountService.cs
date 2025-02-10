using business_logic.DTOs;
using business_logic.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Interfaces
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
