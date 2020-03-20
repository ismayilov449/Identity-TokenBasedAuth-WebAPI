using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Identity_TokenBasedAuth__API.Domain.Services;
using Identity_TokenBasedAuth__API.Models;
using Identity_TokenBasedAuth__API.ResourceViewModel;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity_TokenBasedAuth__API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase , IActionFilter
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {

            AppUser appUser = await userService.GetUserByUserName(User.Identity.Name);

            return Ok(appUser.Adapt<UserViewModelResource>());

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserViewModelResource userViewModelResource)
        {

            var response = await userService.UpdateUser(userViewModelResource, User.Identity.Name);

            if (response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadUserPicture(IFormFile picture)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory() + "wwwroot/UserPictures", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }

            var result = new
            {
                path = "https://" + Request.Host + "/UserPicture" + fileName
            };

            var response = await userService.UploadUserPicture(result.path, User.Identity.Name);

            if (response.Success)
            {
                return Ok(path);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        //Remove password field from UserViewModelResource in UpdateUser action for Required-data annotations
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.ModelState.Remove("Password");
        }

    }

}