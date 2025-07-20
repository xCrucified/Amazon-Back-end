using AutoMapper;
using BLL.DTOs;
using BLL.DTOs.User;
using BLL.Entities;
using BLL.Interfaces;
using BLL.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IRepository<RefreshToken> _refreshTokenRepo;
        private readonly IImageHulk _imageHulk;

        public AccountService(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IRepository<RefreshToken> refreshTokenRepo,
                              IMapper mapper,
                              IJwtService jwtService,
                              IImageHulk hulk,
                              IGoogleAuthService googleAuthService)
        {
            _googleAuthService = googleAuthService;
            _userManager = userManager;
            _signInManager = signInManager;
            _refreshTokenRepo = refreshTokenRepo;
            _mapper = mapper;
            _jwtService = jwtService;
            _imageHulk = hulk;
        }

        public async Task<LoginResponseDto> LoginViaPhone(LoginModelPhone model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                throw new HttpException("Invalid user phone number or password.", HttpStatusCode.Unauthorized);

            var refreshTokenEntity = await CreateRefreshTokenAsync(user.Id);

            return new LoginResponseDto
            {
                AccessToken = _jwtService.CreateToken(_jwtService.GetClaims(user)),
                RefreshToken = refreshTokenEntity.Token
            };
        }

        public async Task<LoginResponseDto> LoginViaEmail(LoginModelEmail model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                throw new HttpException("Invalid user email or password.", HttpStatusCode.Unauthorized);

            var refreshTokenEntity = await CreateRefreshTokenAsync(user.Id);

            return new LoginResponseDto
            {
                AccessToken = _jwtService.CreateToken(_jwtService.GetClaims(user)),
                RefreshToken = refreshTokenEntity.Token
            };
        }

        public async Task<UserToken> RefreshTokens(UserToken ut)
        {
            var refreshToken = await _refreshTokenRepo.GetItemBySpec(new RefreshTokenSpecs.ByToken(ut.RefreshToken));

            if (refreshToken == null)
                throw new HttpException(Errors.InvalidToken, HttpStatusCode.Unauthorized);

            var claims = _jwtService.GetClaimsFromExpiredToken(ut.AccessToken);
            var newAccessToken = _jwtService.CreateToken(claims);
            var newRefreshTokenString = _jwtService.CreateRefreshToken();

            refreshToken.Token = newRefreshTokenString;
            refreshToken.CreationDate = DateTime.UtcNow;

            await _refreshTokenRepo.UpdateAsync(refreshToken);
            await _refreshTokenRepo.SaveChangesAsync();

            var tokens = new UserToken()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenString
            };

            return tokens;
        }

        public async Task Register(RegisterModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
                throw new HttpException("Email is already exists.", HttpStatusCode.Conflict);

            var newUser = _mapper.Map<User>(model);

            var res = await _userManager.CreateAsync(newUser, model.Password);

            if (!res.Succeeded)
                throw new HttpException(string.Join(" ", res.Errors.Select(x => x.Description)), HttpStatusCode.BadRequest);
        }

        private async Task<RefreshToken> CreateRefreshTokenAsync(string userId)
        {
            var refreshTokenString = _jwtService.CreateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshTokenString,
                UserId = userId,
                CreationDate = DateTime.UtcNow
            };

            await _refreshTokenRepo.InsertAsync(refreshTokenEntity);
            await _refreshTokenRepo.SaveChangesAsync();

            return refreshTokenEntity;
        }

        public async Task<string> GoogleLoginAsync(string googleToken)
        {
            var googleUser = await _googleAuthService.ValidateGoogleTokenAsync(googleToken);
            if (googleUser == null)
            {
                throw new HttpException("Invalid Google Token", HttpStatusCode.Unauthorized);
            }

            var user = await _userManager.FindByEmailAsync(googleUser.Email);
            if (user == null)
            {
                user = new User
                {
                    UserName = googleUser.Email,
                    Email = googleUser.Email
                };
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    throw new HttpException($"Failed to create user: {string.Join(" ", createResult.Errors.Select(e => e.Description))}", HttpStatusCode.InternalServerError);
                }
            }

            return _jwtService.CreateToken(_jwtService.GetClaims(user));
        }

        public async Task RemoveExpiredRefreshTokens()
        {
            var lastDate = _jwtService.GetLastValidRefreshTokenDate();
            var expiredTokens = await _refreshTokenRepo.GetListBySpec(new RefreshTokenSpecs.CreatedBy(lastDate));

            foreach (var token in expiredTokens)
            {
                await _refreshTokenRepo.DeleteAsync(token);
            }
            await _refreshTokenRepo.SaveChangesAsync();
        }

        public async Task Logout(string refreshToken)
        {
            // Додатково: можливо, потрібно видалити конкретний refresh token з БД
            // var tokenToDelete = await _refreshTokenRepo.GetItemBySpec(new RefreshTokenSpecs.ByToken(refreshToken));
            // if (tokenToDelete != null)
            // {
            //     await _refreshTokenRepo.DeleteAsync(tokenToDelete);
            //     await _refreshTokenRepo.SaveChangesAsync();
            // }
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> CheckEmailExistence(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new HttpException("Email is required.", HttpStatusCode.BadRequest);
            }
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<bool> CheckPhoneNumberExistence(string phonenum)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phonenum);
            return user != null;
        }

        public async Task ChangeEmail(EmailChangeModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                throw new HttpException("User not found.", HttpStatusCode.NotFound);

            // Якщо RefreshTokens є навігаційною властивістю, переконайтеся, що вона завантажена (e.g., через Include у специфікації користувача)
            // Або завантажте їх явно, якщо вони не завантажуються автоматично.
            // Приклад явного завантаження (якщо потрібно):
            // await _userManager.GetUserManager<UserManager<User>>().GetRefreshTokens(user); // Припустимо, такий метод існує

            if (user.RefreshTokens == null || !user.RefreshTokens.Any())
                throw new HttpException("No refresh tokens available.", HttpStatusCode.BadRequest);

            var latestToken = user.RefreshTokens.OrderByDescending(x => x.CreationDate).FirstOrDefault()?.Token;

            if (string.IsNullOrEmpty(latestToken))
                throw new HttpException("Latest refresh Token is invalid.", HttpStatusCode.BadRequest);

            var res = await _userManager.ChangeEmailAsync(user, model.Email, latestToken);

            if (!res.Succeeded)
                throw new HttpException(string.Join(" ", res.Errors.Select(x => x.Description)), HttpStatusCode.BadRequest);
        }

        public async Task ChangePassword(PasswordChangeModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                throw new HttpException("User not found.", HttpStatusCode.NotFound);

            if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
                throw new HttpException("Invalid old password.", HttpStatusCode.BadRequest);

            var res = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!res.Succeeded)
                throw new HttpException(string.Join(" ", res.Errors.Select(x => x.Description)), HttpStatusCode.BadRequest);
        }

        public async Task ChangePhoneNumber(PhoneNumberChangeModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                throw new HttpException("User not found.", HttpStatusCode.NotFound);

            var res = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);

            if (!res.Succeeded)
                throw new HttpException(string.Join(" ", res.Errors.Select(x => x.Description)), HttpStatusCode.BadRequest);
        }
    }
}