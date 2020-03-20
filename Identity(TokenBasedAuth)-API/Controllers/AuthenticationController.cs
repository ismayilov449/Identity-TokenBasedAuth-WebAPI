using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity_TokenBasedAuth__API.Domain.Services;
using Identity_TokenBasedAuth__API.ResourceViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity_TokenBasedAuth__API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        //localhost:5001/api/authentication/isauthenticated
        [HttpGet]
        public IActionResult IsAuthenticated()
        {
            return Ok(User.Identity.IsAuthenticated);
        }


        //localhost:5000/api/authentication/SignUp/{userViewModelResource}
        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModelResource userViewModelResource)
        {
            var baseResponse = await this.authenticationService.SignUp(userViewModelResource);

            if (baseResponse.Success)
            {
                return Ok(baseResponse.Extra);
            }
            else
            {
                return BadRequest(baseResponse.Message);
            }
              
        }

        //localhost:5000/api/authentication/SignIn/{signinViewModelResource}
        [HttpPost]
        public async Task<IActionResult> SignIn(SigninViewModelResource signinViewModelResource)
        {
            var response = await this.authenticationService.SignIn(signinViewModelResource);

            if (response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }

        }

        //localhost:5000/api/authentication/TokenByRefreshToken/{refreshTokenView}
        [HttpPost]
        public async Task<IActionResult> CreateAccessTokenByRefreshToken(RefreshTokenViewModelResource refreshTokenView)
        {
            var response = await this.authenticationService.CreateAccessTokenByRefreshToken(refreshTokenView);

            if (response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        //localhost:5000/api/authentication/RevokeRefresToken/{refreshTokenView}
        [HttpDelete]
        public async Task<IActionResult> RevokeRefresToken(RefreshTokenViewModelResource refreshTokenView)
        {
            var response = await this.authenticationService.RevokeRefreshToken(refreshTokenView);

            if (response.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

    }
}