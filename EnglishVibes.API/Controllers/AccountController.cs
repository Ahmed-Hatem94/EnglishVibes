using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using EnglishVibes.Service.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishVibes.API.Controllers
{
  
    public class AccountController : BaseAPIController
    {
        private readonly ApplicationDBContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController
            (ApplicationDBContext _context,
                UserManager<ApplicationUser> _userManager,
                SignInManager<ApplicationUser> _signInManager)
        {
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAll()
        //{
        //    return await context.Users.ToListAsync();
        //}

        // POST: api/Account/register
        [HttpPost("register")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<Student>> RegisterStudent(RegisterStudentDTO studentDTO)
        {
            //if (_context.Students == null)
            //{
            //    return Problem("Entity set 'ApplicationDBContext.Students'  is null.");
            //}
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
                    await signInManager.SignInAsync(newStudent, false);
                    return Ok(new { message = "Thank You For your registration. We will contact you shortly" });
                }
                else
                {
                    //ModelState.AddModelError("", result.Errors.FirstOrDefault().Description);
                    return Problem(result.Errors.FirstOrDefault().Description);
                }
            }
            return BadRequest(ModelState);
            //_context.Students.Add(student);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // POST: api/Account/login
        [HttpPost("login")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<Student>> Login(UserLoginDTO userLoginDTO)
        {
            //if (_context.Students == null)
            //{
            //    return Problem("Entity set 'ApplicationDBContext.Students'  is null.");
            //}
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = await userManager.FindByNameAsync(userLoginDTO.UserName);
                if (appUser != null)
                {
                    bool found = await userManager.CheckPasswordAsync(appUser, userLoginDTO.Password);
                    if (found)
                    {
                        await signInManager.SignInAsync(appUser, userLoginDTO.RememberMe);
                        //return Redirect("");
                        return Ok(new { message = "Logged In Successfully" });
                    }
                    else
                        return Problem("Incorrect UserName Or Password");
                }
                else
                {
                    return Problem("Incorrect UserName Or Password");
                }
            }
            return BadRequest(ModelState);
        }

        // POST: api/Account/logout
        [HttpGet("Logout")]
        public async Task<ActionResult<Student>> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
}
