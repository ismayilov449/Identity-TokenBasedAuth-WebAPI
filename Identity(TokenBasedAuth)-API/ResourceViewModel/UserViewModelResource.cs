using Identity_TokenBasedAuth__API.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_TokenBasedAuth__API.ResourceViewModel
{
    public class UserViewModelResource
    {

        [Required(ErrorMessage = "Invalid username")]
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Invalid email address")]
        public string  Email { get; set; }

        [Required(ErrorMessage = "Invalid password")]
        public string Password { get; set; }

        public DateTime? BirthDay { get; set; }

        public string Picture { get; set; }

        public string City { get; set; }

        public Gender Gender { get; set; }
         
    }
}
