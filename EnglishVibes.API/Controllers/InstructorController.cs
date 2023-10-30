using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using EnglishVibes.Service.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishVibes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public InstructorController(ApplicationDBContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstructorDTO>>> GetAll()
        {
            var instructors = await context.Instructors.ToListAsync();
            List<InstructorDTO> instructorList = new List<InstructorDTO>();
            foreach (Instructor instructor in instructors)
            {
                InstructorDTO instructorDTO = new InstructorDTO()
                {
                    UserName = instructor.UserName,
                    Email = instructor.Email,
                    PhoneNumber = instructor.PhoneNumber,
                    NoOfGroups = instructor.Groups == null ? 0 : instructor.Groups.Count
                };
                instructorList.Add(instructorDTO);
            }
            return instructorList.ToList();
        }
    }
}
