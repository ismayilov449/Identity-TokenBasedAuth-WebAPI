using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_TokenBasedAuth__API.Security.Token
{
    public class AccessToken
    {

        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public string RefreshToken { get; set; }
    }
}
