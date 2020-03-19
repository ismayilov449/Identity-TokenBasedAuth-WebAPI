using Identity_TokenBasedAuth__API.Domain.Responses;
using Identity_TokenBasedAuth__API.ResourceViewModel;
using Identity_TokenBasedAuth__API.Security.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_TokenBasedAuth__API.Domain.Services
{
    public interface IAuthenticationService
    {

        Task<BaseResponse<UserViewModelResource>> SignUp(UserViewModelResource userViewModel);

        Task<BaseResponse<AccessToken>> SignIn(SigninViewModelResource signinViewModel);

        Task<BaseResponse<AccessToken>> CreateAccessTokenByRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel);

        Task<BaseResponse<AccessToken>> RevokeRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel);

    }
}
