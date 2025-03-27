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
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IJwtService jwtService;
        private readonly IMapper mapper;
        private readonly IRepository<RefreshToken> refreshTokenR;
        private readonly IImageHulk imageHulk;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<User> userManager,
                                SignInManager<User> signInManager,
                                IRepository<RefreshToken> refreshTokenR,
                                IMapper mapper,
                                IJwtService jwtService,
                                IImageHulk hulk,
                                IGoogleAuthService googleAuthService)
        {
            this._googleAuthService = googleAuthService;
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

        public async Task<string> GoogleLoginAsync(string googleToken)
        {
            var googleUser = await _googleAuthService.ValidateGoogleTokenAsync(googleToken);
            if (googleUser == null)
            {
                throw new Exception("Invalid Google Token");
            }

            var user = await userManager.FindByEmailAsync(googleUser.Email);
            if (user == null)
            {
                user = new User
                {
                    UserName = googleUser.Email,
                    Email = googleUser.Email
                };
                await userManager.CreateAsync(user);
            }

            return jwtService.CreateToken(jwtService.GetClaims(user));
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
                throw new HttpException("Latest refresh Token is invalid.", HttpStatusCode.BadRequest);

            var res = await userManager.ChangeEmailAsync(user, model.Email, latestToken);

            if (!res.Succeeded)
                throw new HttpException(string.Join(" ", res.Errors.Select(x => x.Description)), HttpStatusCode.BadRequest);

        }

        public async Task ChangePassword(PasswordChangeModel model)
        {
            var user = userManager.FindByIdAsync(model.Id);

            if (user == null)
                throw new HttpException("User not found.", HttpStatusCode.NotFound);

            if (!userManager.CheckPasswordAsync(user.Result, model.OldPassword).Result)
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

        public Task GoogleLoginAsync(object token)
        {
            throw new NotImplementedException();
        }
    }
}
