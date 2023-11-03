using EnglishVibes.API.DTO;
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
        [HttpPost("register/student")]
        public async Task<ActionResult<Student>> RegisterStudent(RegisterStudentDTO studentDTO)
        {

            if (ModelState.IsValid)
            {
                var newStudent = new Student()
                {
                    FirstName = studentDTO.FirstName,
                    LastName = studentDTO.LastName,
                    Age = 20,
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
                    return Ok(new { message = "success" });
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }


        [HttpPost("register/admin")]
        public async Task<ActionResult<Student>> RegisterAdmin(RegisterAdminDTO adminDTO)
        {

            if (ModelState.IsValid)
            {
                var newAdmin = new ApplicationUser()
                {
                    UserName = adminDTO.UserName,
                    Email = adminDTO.Email,
                    PasswordHash = adminDTO.Password,
                    PhoneNumber = adminDTO.PhoneNumber
                };
                IdentityResult result = await userManager.CreateAsync(newAdmin, adminDTO.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "admin");
                    return Ok(new { message = "Admin Registered" });
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
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
                ApplicationUser appUser = await userManager.FindByEmailAsync(userLoginDTO.Email);
                if (appUser != null)
                {
                    bool found = await userManager.CheckPasswordAsync(appUser, userLoginDTO.Password);
                    if(found)
                    {
                        var roles = await userManager.GetRolesAsync(appUser);
                        // generate token
                        //1-create claims Name and Role 
                        List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, appUser.UserName)
                        };
                        if(roles != null)
                        {
                            foreach (var itemRole in roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, itemRole));
                            }
                        }
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
                                message = "success",
                                role = roles,
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
