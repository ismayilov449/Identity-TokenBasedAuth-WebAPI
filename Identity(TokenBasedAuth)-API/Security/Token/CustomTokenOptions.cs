using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_TokenBasedAuth__API.Security.Token
{
    public class CustomTokenOptions
    {
        public string Audience { get; set; }

        public string Issuer { get; set; }

        public int AccessTokenExpiration { get; set; }

        public int RefreshTokenExpiration { get; set; }

        public string SecurityKey { get; set; }
    }
}
