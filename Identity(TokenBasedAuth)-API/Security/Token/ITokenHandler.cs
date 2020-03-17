using Identity_TokenBasedAuth__API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_TokenBasedAuth__API.Security.Token
{
    public interface ITokenHandler
    {
        AccessToken CreateAccessToken(AppUser user);

        void RevokeRefreshToken(AppUser user);
    }
}
