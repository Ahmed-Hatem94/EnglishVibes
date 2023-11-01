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
    public class InstructorController : BaseAPIController
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
            return instructorList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorScheduleDTO>> GetInstructorSchedule(Guid id)
        {
            var instructor = await context.Instructors
                .Include(i => i.Groups)
                .FirstOrDefaultAsync(i => i.Id == id);
            InstructorScheduleDTO instructorSchedule = new InstructorScheduleDTO()
            {
                UserName = instructor.UserName,
                Email = instructor.Email,
                PhoneNumber = instructor.PhoneNumber,
                Groups = instructor.Groups
            };
            return instructorSchedule;
        }

        [HttpPost]
        public async Task<ActionResult<Instructor>> AddInstructor(InstructorDTO instructorDTO)
        {
            if (ModelState.IsValid)
            {
                var newInstructor = new Instructor()
                {
                    UserName = instructorDTO.UserName,
                    Email = instructorDTO.Email,
                    PhoneNumber = instructorDTO.PhoneNumber                    
                };
                IdentityResult result = await userManager.CreateAsync(newInstructor);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Instructor Added" });
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

        [HttpDelete]
        public async Task<ActionResult<Instructor>> RemoveInstructor(Guid id)
        {
            var instructor = await context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            context.Instructors.Remove(instructor);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
