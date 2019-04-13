using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InzBackCore.Domain;
using InzBackInfrastructure.DTO;
using InzBackInfrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cors;

namespace InzBackApi.Controllers
{
    [Route("[controller]")]
    [EnableCors("AllowClientOrigin")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        public AuthController(IAuthService authService, IConfiguration config)
        {
            _config = config;
            _authService = authService;
        }
        
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserToRegister userToRegister)   
            //przeszedl walidacej
        {
            userToRegister.Username = userToRegister.Username.ToLower(); 
            if (await _authService.UserExist(userToRegister.Username))
                return BadRequest("Username alredy exist");

            var userToCreate = new User
            {
                Username = userToRegister.Username
            };

            var createdUser = _authService.Register(userToCreate, userToRegister.Password);    

          
            return StatusCode(201);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto) 
        {
            var userFromRepo = await _authService.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);  

            if(userFromRepo == null)
                return Unauthorized();  

            var claims = new[]  
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),   
                new Claim(ClaimTypes.Name, userFromRepo.Username),   
                new Claim(ClaimTypes.Role, userFromRepo.Role)   
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));   
            

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);    

            var tokenDescriptor = new SecurityTokenDescriptor   
            {
                Subject = new ClaimsIdentity(claims),   
                Expires = DateTime.Now.AddMinutes(30),  
                SigningCredentials = creds  
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);  

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)  
            });
         
        }

        [HttpGet]
        [Route("GetUsersAccounts")]
        public async Task<IActionResult> GetUsersAccounts()   
        {

            var usersAccounts = await _authService.GetAsyncAllUsersAccounts();
            return Json(usersAccounts);
        }

        [HttpGet("{userId}")]
        [Route("GetOneUserAccount/{userId}")]
        public async Task<IActionResult> GetOneUserAccount(int userId)   
        {

            var userAccount = await _authService.GetOneUserAccount(userId);
            return Json(userAccount);
        }

        [HttpDelete("{userId}")]
        [Route("RemoveOneUserAccount/{userId}")]
        public async Task<IActionResult> RemoveOneUserAccount(int userId)   
        {
            await _authService.RemoveUserAccount(userId);
            return NoContent();
        }

        [HttpPut("{userId}")]
        [Route("PutUserAccountUserName/{userId}")]
        public async Task<IActionResult> PutUserAccountUserName(int userId, [FromBody]PutUserAccountUserNameDto userAccount)    
        {

           if (!ModelState.IsValid) return BadRequest(ModelState);     
            
       
            var userToUpdate = new User 
            {
                Username = userAccount.Username
            };
            var createdAccount = await _authService.UpdateAsyncUserAccounUserName(userId, userToUpdate);
            
            return NoContent(); 

        }
        [HttpPut("{userId}")]
        [Route("PutUserAccountPassword/{userId}")]
        public async Task<IActionResult> PutUserAccountPassword(int userId, [FromBody]PutUserAccountPasswordDto userAccount)    
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);     
            if(userAccount.Password1 != userAccount.Password2)
            {
                throw new Exception("the password is not the same repeated");
            }
            var createdAccount = await _authService.UpdateAsyncUserAccounPassword(userId, userAccount.Password1);

            return NoContent(); 

        }

        [HttpPut("{userId}")]
        [Route("PutUserRole/{userId}")]
        public async Task<IActionResult> PutUserRole(int userId, [FromBody]PutUserRole userAccount)   
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);      
            var createdAccount = await _authService.UpdateAcynsUserRole(userId, userAccount.Role);
            return NoContent(); 

        }


    }
}