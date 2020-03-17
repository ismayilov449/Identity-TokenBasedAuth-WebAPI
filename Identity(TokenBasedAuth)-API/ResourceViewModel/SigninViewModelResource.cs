using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_TokenBasedAuth__API.ResourceViewModel
{
    public class SigninViewModelResource
    {
        [Required(ErrorMessage = "Invalid email address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Invalid password")]
        [MinLength(4, ErrorMessage = "Password must be more than 4 characters")]
        public string Password { get; set; }

    }
}
