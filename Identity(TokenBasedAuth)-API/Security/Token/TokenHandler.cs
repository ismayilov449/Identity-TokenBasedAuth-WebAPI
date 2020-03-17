using Identity_TokenBasedAuth__API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Identity_TokenBasedAuth__API.Security.Token
{
    public class TokenHandler : ITokenHandler
    {
        private readonly CustomTokenOptions tokenOptions;


        public TokenHandler(IOptions<CustomTokenOptions> tokenOptions)
        {
            this.tokenOptions = tokenOptions.Value;
        }

        public AccessToken CreateAccessToken(AppUser user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(tokenOptions.AccessTokenExpiration);

            var securityKey = SignHandler.GetSecurityKey(tokenOptions.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(

                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaims(user),
                signingCredentials: signingCredentials
            );


            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            AccessToken accessToken = new AccessToken()
            {
                Token = token,
                RefreshToken = CreateRefreshToken(),
                Expiration = accessTokenExpiration
            };

            return accessToken;
        }

        public void RevokeRefreshToken(AppUser user)
        {
            throw new NotImplementedException();
        }

        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(numberByte);
                return Convert.ToBase64String(numberByte);
            }
        }

        private IEnumerable<Claim> GetClaims(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Name,$"{user.UserName}"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            return claims;



        }

    }
}
