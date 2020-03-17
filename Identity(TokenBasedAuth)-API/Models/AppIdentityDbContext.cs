using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_TokenBasedAuth__API.Models
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser,AppRole,string>
    {

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            :base(options)
        {

        }

    }
}
