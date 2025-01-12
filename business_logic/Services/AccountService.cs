using AutoMapper;
using business_logic.DTOs;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Specifications;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public AccountService(UserManager<User> userManager,
                                SignInManager<User> signInManager,
                                IRepository<RefreshToken> refreshTokenR,
                                IMapper mapper,
                                IJwtService jwtService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.refreshTokenR = refreshTokenR;
            this.mapper = mapper;
            this.jwtService = jwtService;
        }

        public async Task<LoginResponseDto> Login(LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
                throw new HttpException("Invalid user login or password.", HttpStatusCode.BadRequest);

            return new LoginResponseDto
            {
                AccessToken = jwtService.CreateToken(jwtService.GetClaims(user)),
                RefreshToken = CreateRefreshToken(user.Id).Token
            };

        }
        

        public async Task<UserToken> RefreshTokens(UserToken ut)
        {
            var refrestToken = await refreshTokenR.GetItemBySpec(new RefreshTokenSpecs.ByToken(ut.RefreshToken));

            if (refrestToken == null) throw new HttpException(Errors.InvalidToken, HttpStatusCode.BadRequest);

            var claims = jwtService.GetClaimsFromExpiredToken(ut.AccessToken);
            var newAccessToken = jwtService.CreateToken(claims);
            var newRefreshToken = jwtService.CreateRefreshToken();

            refrestToken.Token = newRefreshToken;

            refreshTokenR.Update(refrestToken);
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

    }
}
