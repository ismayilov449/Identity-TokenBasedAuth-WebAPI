using Identity_TokenBasedAuth__API.Domain.Responses;
using Identity_TokenBasedAuth__API.Domain.Services;
using Identity_TokenBasedAuth__API.Models;
using Identity_TokenBasedAuth__API.ResourceViewModel;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity_TokenBasedAuth__API.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager) 
            : base(userManager, signInManager, roleManager)
        {

        }

        public async Task<Tuple<AppUser, IList<Claim>>> GetUserByRefreshToken(string refreshToken)
        {
            Claim claimRefreshToken = new Claim("refreshToken", refreshToken);

            var users = await userManager.GetUsersForClaimAsync(claimRefreshToken);

            if (users.Any())
            {
                var user = users.First();

                IList<Claim> userClaims = await userManager.GetClaimsAsync(user);

                string refreshTokenEndDate = userClaims.First(c => c.Type == "refreshTokenEndDate").Value;

                if (DateTime.Parse(refreshTokenEndDate) > DateTime.Now)
                {
                    return new Tuple<AppUser, IList<Claim>>(user, userClaims);
                }
                else
                {
                    return new Tuple<AppUser, IList<Claim>>(null, null);
                }

            }

            return new Tuple<AppUser, IList<Claim>>(null, null);
        }

        public async Task<AppUser> GetUserByUserName(string userName)
        {

            return await userManager.FindByNameAsync(userName);

        }

        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            var result = await GetUserByRefreshToken(refreshToken);

            if(result.Item1 == null) return false;

            IdentityResult response = await userManager.RemoveClaimsAsync(result.Item1, result.Item2);

            if (response.Succeeded)
            {
                return true;
            }

            return false;

        }

        public async Task<BaseResponse<UserViewModelResource>> UpdateUser(UserViewModelResource userViewModel, string userName)
        {
            var user = await userManager.FindByNameAsync(userName); 

            if(userManager.Users.Count(u=> u.PhoneNumber == userViewModel.PhoneNumber) >= 1)
            {
                return new BaseResponse<UserViewModelResource>("This number was used by another user!");
            }
            else
            {
                user.PhoneNumber = userViewModel.PhoneNumber;
            }

            user.BirthDay = userViewModel.BirthDay;
            user.City = userViewModel.City;
            user.Gender = (int)userViewModel.Gender;
            user.Email = userViewModel.Email;
            user.Picture = userViewModel.Picture;
            user.UserName = userViewModel.UserName;


            IdentityResult result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new BaseResponse<UserViewModelResource>(user.Adapt<UserViewModelResource>());
            }
            else
            {
                return new BaseResponse<UserViewModelResource>(result.Errors.FirstOrDefault().Description);
            }
        
        }

        public async Task<BaseResponse<AppUser>> UploadUserPicture(string picturePath, string userName)
        {
            var user = await userManager.FindByNameAsync(userName);

            user.Picture = picturePath;

            IdentityResult identityResult = new IdentityResult();

            identityResult = await userManager.UpdateAsync(user);

            if (identityResult.Succeeded)
            {
                return new BaseResponse<AppUser>(user);
            }
            else
            {
                return new BaseResponse<AppUser>(identityResult.Errors.First().Description);
            }

        }
    }
}
