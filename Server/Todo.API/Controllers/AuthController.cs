using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Todo.Application.DTOs;
using Todo.Application.Helpers;
using Todo.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ExceptionServices;


namespace Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<User> userManager, IConfiguration configuration1)
        {
            _userManager = userManager;
            _configuration = configuration1;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllAvailableUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(new Response(users, null, HttpStatusCode.OK));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
            {
                return NotFound(new Response(null, new List<string> { "Invalid Login Credentials." }, HttpStatusCode.NotFound));
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new Response(null, new List<string> { "Invalid Password." }, HttpStatusCode.Unauthorized));
            }

            var authClaims = new List<Claim>
            {
                new Claim("userName", user.UserName), // User name claim
                new Claim("userId", user.Id), // Custom claim key for UserId
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new Response(
                data: new
                {
                    Token = jwtToken,
                    Expiration = token.ValidTo,
                    UserId = user.Id
                },
                errors: null,
                statusCode: HttpStatusCode.OK
            ));
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.Username);

            if (userExists != null)
            {
                return BadRequest(new Response(null, new List<string> { "User already exists." }, HttpStatusCode.BadRequest));
            }

            var user = new User
            {
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.Username,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
            };

            var createUser = await _userManager.CreateAsync(user, registerDto.Password);

            if (!createUser.Succeeded)
            {
                return BadRequest(new Response(null, new List<string> { "Failed to create user." }, HttpStatusCode.BadRequest));
            }

            return Ok(new Response("User Created Succesfully.", null, HttpStatusCode.OK));
        }
    }
}