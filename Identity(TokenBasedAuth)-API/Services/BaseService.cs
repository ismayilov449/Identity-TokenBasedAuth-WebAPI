using Identity_TokenBasedAuth__API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_TokenBasedAuth__API.Services
{
    public class BaseService:ControllerBase
    {

        protected UserManager<AppUser> userManager { get;}

        protected SignInManager<AppUser> signInManager { get; }

        protected RoleManager<AppRole> roleManager { get; }


        public BaseService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<AppRole> roleManager)
        {
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.userManager = userManager;

        }
    }
}
