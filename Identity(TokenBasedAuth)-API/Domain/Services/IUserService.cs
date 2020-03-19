using Identity_TokenBasedAuth__API.Domain.Responses;
using Identity_TokenBasedAuth__API.Models;
using Identity_TokenBasedAuth__API.ResourceViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity_TokenBasedAuth__API.Domain.Services
{
    public interface IUserService
    {

        Task<BaseResponse<UserViewModelResource>> UpdateUser(UserViewModelResource userViewModel, string userName);

        Task<AppUser> GetUserByUserName(string userName);

        Task<BaseResponse<AppUser>> UploadUserPicture(string picturePath, string userName);

        Task<Tuple<AppUser, IList<Claim>>> GetUserByRefreshToken(string refreshToken);

        Task<bool> RevokeRefreshToken(string refreshToken);

    }
}
