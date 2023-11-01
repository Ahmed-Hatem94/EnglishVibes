using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using EnglishVibes.Service.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using System.Text;

namespace EnglishVibes.API.Controllers
{
  
    public class AccountController : BaseAPIController
    {
        private readonly ApplicationDBContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        public AccountController
            (ApplicationDBContext _context,
                UserManager<ApplicationUser> _userManager ,
                IConfiguration _config)
        {
            config = _config;
            userManager = _userManager;
          
        }

  
        // POST: api/Account/register
        [HttpPost("register")]
        public async Task<ActionResult<Student>> RegisterStudent(RegisterStudentDTO studentDTO)
        {

            if (ModelState.IsValid)
            {
                var newStudent = new Student()
                {
                    UserName = studentDTO.UserName,
                    Email = studentDTO.Email,
                    PasswordHash = studentDTO.Password,
                    PhoneNumber = studentDTO.PhoneNumber,
                    StudyPlan = studentDTO.StudyPlan,
                    ActiveStatus = false
                };
                IdentityResult result = await userManager.CreateAsync(newStudent, studentDTO.Password);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Thank You For your registration. We will contact you shortly" });
                }
                else
                {  
                    return Problem(result.Errors.FirstOrDefault().Description);
                }
            }
            return BadRequest(ModelState);
            
        }
    
        // POST: api/Account/login
        [HttpPost("login")]
        public async Task<ActionResult<Student>> Login(UserLoginDTO userLoginDTO)
        {
            if (ModelState.IsValid)
            {
                
                ApplicationUser appUser = await userManager.FindByNameAsync(userLoginDTO.UserName);
                
                if (appUser != null)
                {
                    bool found = await userManager.CheckPasswordAsync(appUser, userLoginDTO.Password);
                    if(found ==true)
                    {
                        // generate token
                        //1-create claims Name and Role 
                        List <Claim> claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, appUser.UserName));
                        //claims.Add(new Claim(ClaimTypes.NameIdentifier, appUser.Id));
                        if (appUser.Id != null)
                         {
                            claims.Add(new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()));
                        }
                        else
                        {
                            // Handle the case when appUser.Id is null, such as logging an error or taking appropriate action.
                        }
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        //var roles = await userManager.GetRolesAsync(appUser);
                        //if(roles != null)
                        // {
                        //     foreach(var itemRole in roles) 
                        //     {
                        //         claims.Add(new Claim(ClaimTypes.Role, itemRole));
                        //     }
                        // }
                        //2- create security key
                        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:key"]));
                        //3- signing credentials
                        SigningCredentials credentials= new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

                        //4- create Json token
                        JwtSecurityToken myToken = new JwtSecurityToken(
                            issuer: config["jwt:issuer"],
                            audience: config["jwt:audience"],
                            claims: claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: credentials
                            );
                        return Ok(
                            new
                            {
                                token= new JwtSecurityTokenHandler().WriteToken(myToken),
                                expiration=myToken.ValidTo
                            });
                    }
                }
                return Unauthorized("UserName or Password is Incorrect");
               
            }
            return BadRequest(ModelState);
        }

        // POST: api/Account/logout
        //[HttpGet("Logout")]
        //public async Task<ActionResult<Student>> Logout()
        //{
        //    await signInManager.SignOutAsync();
        //    return Ok();
        //}
    }
}
