using Identity_TokenBasedAuth__API.Domain.Responses;
using Identity_TokenBasedAuth__API.Domain.Services;
using Identity_TokenBasedAuth__API.Models;
using Identity_TokenBasedAuth__API.ResourceViewModel;
using Identity_TokenBasedAuth__API.Security.Token;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity_TokenBasedAuth__API.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {

        private readonly ITokenHandler tokenHandler;
        private readonly CustomTokenOptions tokenOptions;
        private readonly IUserService userService;

        public AuthenticationService(IUserService userService, ITokenHandler tokenHandler, IOptions<CustomTokenOptions> options, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager) : base(userManager, signInManager, roleManager)
        {
            this.tokenHandler = tokenHandler;
            this.tokenOptions = options.Value;
            this.userService = userService;
        }

        public async Task<BaseResponse<AccessToken>> CreateAccessTokenByRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel)
        {

            var userClaims = await userService.GetUserByRefreshToken(refreshTokenViewModel.RefreshToken);

            if (userClaims.Item1 != null)
            {
                AccessToken accessToken = tokenHandler.CreateAccessToken(userClaims.Item1);

                Claim refreshTokenClaim = new Claim("refreshToken", accessToken.RefreshToken);
                Claim refreshTokenEndDate = new Claim("refreshTokenEndDate", DateTime.Now.AddMinutes(tokenOptions.RefreshTokenExpiration).ToString());

                await userManager.ReplaceClaimAsync(userClaims.Item1, userClaims.Item2[0], refreshTokenClaim);
                await userManager.ReplaceClaimAsync(userClaims.Item1, userClaims.Item2[1], refreshTokenEndDate);

                return new BaseResponse<AccessToken>(accessToken);

            }
            else
            {
                return new BaseResponse<AccessToken>("Invalid user");
            }



        }

        public async Task<BaseResponse<AccessToken>> RevokeRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel)
        {

            bool result = await userService.RevokeRefreshToken(refreshTokenViewModel.RefreshToken);
            if (result)
            {
                return new BaseResponse<AccessToken>(new AccessToken());
            }
            else
            {
                return new BaseResponse<AccessToken>("!");
            }

        }

        public async Task<BaseResponse<AccessToken>> SignIn(SigninViewModelResource signinViewModel)
        {

            var user = await this.userManager.FindByEmailAsync(signinViewModel.Email);

            if (user != null)
            {
                bool isUser = await this.userManager.CheckPasswordAsync(user, signinViewModel.Password);

                if (isUser)
                {
                    AccessToken accessToken = tokenHandler.CreateAccessToken(user);

                    Claim refreshTokenClaim = new Claim("refreshToken", accessToken.RefreshToken);
                    Claim refreshTokenEndDateClaim = new Claim("refreshTokenEndDate", DateTime.Now.AddMinutes(tokenOptions.RefreshTokenExpiration).ToString());

                    List<Claim> refreshClaimList = this.userManager.GetClaimsAsync(user).Result.Where(c => c.Type.Contains("refreshToken")).ToList();

                    if (refreshClaimList.Any())
                    {
                        await userManager.ReplaceClaimAsync(user, refreshClaimList[0], refreshTokenClaim);
                        await userManager.ReplaceClaimAsync(user, refreshClaimList[1], refreshTokenEndDateClaim);
                    }
                    else
                    {
                        await userManager.AddClaimsAsync(user, new List<Claim> { refreshTokenClaim, refreshTokenEndDateClaim });
                    }

                    return new BaseResponse<AccessToken>(accessToken);
                }
                return new BaseResponse<AccessToken>("Email or password is invalid");
            }

            return new BaseResponse<AccessToken>("Email or password is invalid");

        }

        public async Task<BaseResponse<UserViewModelResource>> SignUp(UserViewModelResource userViewModel)
        {

            AppUser appUser = new AppUser { UserName = userViewModel.UserName, Email = userViewModel.Email };

            IdentityResult identityResult = await this.userManager.CreateAsync(appUser,userViewModel.Password);

            if (identityResult.Succeeded)
            {
                return new BaseResponse<UserViewModelResource>(appUser.Adapt<UserViewModelResource>());
            }
            else
            {
                return new BaseResponse<UserViewModelResource>(identityResult.Errors.First().Description);
            }

        }
    }
}
