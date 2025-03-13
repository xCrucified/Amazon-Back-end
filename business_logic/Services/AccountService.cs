using AutoMapper;
using business_logic.DTOs;
using business_logic.DTOs;
using business_logic.DTOs.User;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Specifications;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;  
using System.Threading.Tasks;

namespace business_logic.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;
        private readonly IJwtService jwtService;
        private readonly IRepository<RefreshToken> refreshTokenR;
        private readonly IImageHulk imageHulk;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<User> userManager,
                                SignInManager<User> signInManager,
                                IRepository<RefreshToken> refreshTokenR,
                                IMapper mapper,
                                IJwtService jwtService,
                                IImageHulk hulk)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.refreshTokenR = refreshTokenR;
            this.mapper = mapper;
            this.jwtService = jwtService;
            this.imageHulk = hulk;
        }

        public async Task<LoginResponseDto> LoginViaPhone(LoginModelPhone model)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
                throw new HttpException("Invalid user phone number or password.", HttpStatusCode.BadRequest);

            return new LoginResponseDto
            {
                AccessToken = jwtService.CreateToken(jwtService.GetClaims(user)),
                RefreshToken = CreateRefreshToken(user.Id).Token
            };
        }
        public async Task<LoginResponseDto> LoginViaEmail(LoginModelEmail model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
                throw new HttpException("Invalid user email or password.", HttpStatusCode.BadRequest);

            return new LoginResponseDto
            {
                AccessToken = jwtService.CreateToken(jwtService.GetClaims(user)),
                RefreshToken = CreateRefreshToken(user.Id).Token
            };
        }
        
        public async Task<UserToken> RefreshTokens(UserToken ut)
        {
            var refreshToken = await refreshTokenR.GetItemBySpec(new RefreshTokenSpecs.ByToken(ut.RefreshToken));

            if (refreshToken == null) throw new HttpException(Errors.InvalidToken, HttpStatusCode.BadRequest);

            var claims = jwtService.GetClaimsFromExpiredToken(ut.AccessToken);
            var newAccessToken = jwtService.CreateToken(claims);
            var newRefreshToken = jwtService.CreateRefreshToken();

            refreshToken.Token = newRefreshToken;

            refreshTokenR.Update(refreshToken);
            refreshTokenR.Save();

            var tokens = new UserToken()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return tokens;
        }



        public async Task Register(RegisterModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
                throw new HttpException("Email is already exists.", HttpStatusCode.BadRequest);

            if (DateTime.Now.Year - model.Birthdate.Year <= 14)
                throw new HttpException("Users aged younger than 14 are forbidden from  using this site", HttpStatusCode.BadRequest);
            
            var NewUser = mapper.Map<User>(model);
            
            var res = await userManager.CreateAsync(NewUser, model.Password);

            if (!res.Succeeded)
                throw new HttpException(string.Join(" ", res.Errors.Select(x => x.Description)), HttpStatusCode.BadRequest);
        }

        private RefreshToken CreateRefreshToken(string userId)
        {
            var refeshToken = jwtService.CreateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refeshToken,
                UserId = userId,
                CreationDate = DateTime.UtcNow
            };

            refreshTokenR.Insert(refreshTokenEntity);
            refreshTokenR.Save();

            return refreshTokenEntity;
        }


        public async Task RemoveExpiredRefreshTokens()
        {
            var lastDate = jwtService.GetLastValidRefreshTokenDate();
            var expiredTokens = await refreshTokenR.GetListBySpec(new RefreshTokenSpecs.CreatedBy(lastDate));

            foreach (var i in expiredTokens)
            {
                refreshTokenR.Delete(i);
            }
            refreshTokenR.Save();
        }

        public async Task Logout(string refreshToken) => await signInManager.SignOutAsync();
        public async Task<bool> CheckEmailExistence(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new HttpException("Email is required.", HttpStatusCode.BadRequest);
            }
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<bool> CheckPhoneNumberExistence(string phonenum)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phonenum);
            return user != null;
        }

        public async Task ChangeEmail(EmailChangeModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
                throw new HttpException("User not found.", HttpStatusCode.NotFound);

            if (user.RefreshTokens == null || !user.RefreshTokens.Any())
                throw new HttpException("No refresh tokens available.", HttpStatusCode.BadRequest);

            var latestToken = user.RefreshTokens.OrderBy(x => x.CreationDate).LastOrDefault()?.Token;

            if (string.IsNullOrEmpty(latestToken))
                throw new HttpException("Latest refresh token is invalid.", HttpStatusCode.BadRequest);

            var res = await userManager.ChangeEmailAsync(user, model.Email, latestToken);

            if (!res.Succeeded)
                throw new HttpException(string.Join(" ", res.Errors.Select(x => x.Description)), HttpStatusCode.BadRequest);

        }

        public async Task ChangePassword(PasswordChangeModel model)
        {
            var user = userManager.FindByIdAsync(model.Id);
            
            if (user == null)
                throw new HttpException("User not found.", HttpStatusCode.NotFound);
            
            if(!userManager.CheckPasswordAsync(user.Result, model.OldPassword).Result)
                throw new HttpException("Invalid old password.", HttpStatusCode.BadRequest);
            
            var res = userManager.ChangePasswordAsync(user.Result, model.OldPassword, model.NewPassword);

            if (!res.Result.Succeeded)
                throw new HttpException(string.Join(" ", res.Result.Errors.Select(x => x.Description)), HttpStatusCode.BadRequest);
        }

        public async Task ChangePhoneNumber(PhoneNumberChangeModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
                throw new HttpException("User not found.", HttpStatusCode.NotFound);

            var res = await userManager.SetPhoneNumberAsync(user, model.PhoneNumber);

            if (!res.Succeeded)
                throw new HttpException(string.Join(" ", res.Errors.Select(x => x.Description)), HttpStatusCode.BadRequest);
        }

        public ChallengeResult LoginViaGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = "/api/auth/google-response" };
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        }
        public async Task<object> HandleGoogleResponse()
        {
            var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return new { Message = "Google authentication failed." };

            var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims
                .Select(c => new { c.Type, c.Value });

            return new
            {
                Message = "Google Authentication successful",
                User = claims
            };
        }

        public object GetUserProfile(ClaimsPrincipal user)
        {
            return new
            {
                Name = user.FindFirst(ClaimTypes.Name)?.Value,
                Email = user.FindFirst(ClaimTypes.Email)?.Value
            };
        }
    }
}
