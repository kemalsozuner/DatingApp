using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;

        public AuthController(IAuthRepository repo,IConfiguration config)
        {
            this.repo = repo;
            this.config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userDTO) {
            /*  [ApiController] kullanılmadığında [FromBody] ve ModelState.IsValid kullanılmalı
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            */

            string userName = userDTO.UserName.ToLower();
            if (await repo.UserExists(userName))
                return new BadRequestObjectResult("User Name already exists");

            var user = new User { Username = userName};
            var createdUser = await repo.Register(user,userDTO.Password);
            return StatusCode(201);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO user){
            var currentUser = await repo.Login(user.UserName,user.Password);
            if (currentUser == null)
                return Unauthorized();
            
            var claims = new [] {             
             new System.Security.Claims.Claim(ClaimTypes.Name,currentUser.Username),
             new System.Security.Claims.Claim(ClaimTypes.NameIdentifier,currentUser.Id.ToString())
            };  

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return Ok (
                new { token = tokenHandler.WriteToken(token)}
            );
        } 
    }
}