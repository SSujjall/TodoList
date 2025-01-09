using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Todo.Application.DTOs;
using Todo.Application.Interface.IServices;
using Todo.Domain.Entities;

namespace Todo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public UserController(IUserService userService, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpPut("Update-Password")]
        public async Task<IActionResult> UpdateUserPassword(UpdatePasswordDTO updatePasswordDto)
        {
            var errors = new List<string>();

            try
            {
                var userId = User.FindFirst("userId")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User Id Not Found In Token" });
                }

                updatePasswordDto.Id = userId;

                var res = await _userService.UpdateUserPassword(updatePasswordDto, errors);

                if (errors.Count > 0)
                {
                    return BadRequest(new { errors });
                }

                return Ok(new { message = res });
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("Update-Details")]
        public async Task<IActionResult> UpdateUserDetail(UpdateUserDTO updateUserDto)
        {
            var errors = new List<string>();

            try
            {
                var userId = User.FindFirstValue("userId");

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID could not be found.");
                }

                updateUserDto.Id = userId;

                var res = await _userService.UpdateUserDetail(updateUserDto, errors);

                if(errors.Count >= 1)
                {
                    return BadRequest(new { errors });
                }

                return Ok(new { message = res });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUser()
        {
            var errors = new List<string>();
            try
            {
                var userId = User.FindFirst("userId")?.Value;

                var response = await _userService.DeleteUser(userId, errors);

                if (errors.Count > 0)
                {
                    return BadRequest(new { errors });
                }

                return Ok(new { message = response });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
    }
}
