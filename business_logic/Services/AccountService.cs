using AutoMapper;
using business_logic.DTOs;
using business_logic.DTOs.User;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IImageHulk imageHulk;

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
            if(model.AvatarPicture != null)
            {
                var imageName = await imageHulk.Save(model.AvatarPicture);
                //productimageR.Insert(imageProduct);
                //productimageR.Save();
            }

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

        
    }
}
