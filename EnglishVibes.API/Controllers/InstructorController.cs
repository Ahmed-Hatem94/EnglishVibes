using EnglishVibes.Data;
using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using EnglishVibes.Service.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishVibes.API.Controllers
{
    public class InstructorController : BaseAPIController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationDBContext context;
        private readonly UserManager<Instructor> userManager;

        public InstructorController(
            IUnitOfWork unitOfWork,
            ApplicationDBContext _context,
            UserManager<Instructor> _userManager)
        {
            this.unitOfWork = unitOfWork;
            context = _context;
            userManager = _userManager;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<InstructorDTO>>> GetAll()
        {
            var instructors = await userManager.Users.ToListAsync();
            //var instructors = await context.Instructors.ToListAsync();
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

        [HttpGet("schedule/{id}")]
        public async Task<ActionResult<InstructorScheduleDTO>> GetInstructorSchedule(Guid id)
        {
            var instructor = await unitOfWork.Instructors.FindAsync(i => i.Id == id, new[] { "Groups" });
            //var instructor = await context.Instructors
            //    .Include(i => i.Groups)
            //    .FirstOrDefaultAsync(i => i.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }
            else
            { 
                InstructorScheduleDTO instructorSchedule = new InstructorScheduleDTO()
                {
                    UserName = instructor.UserName,
                    Email = instructor.Email,
                    PhoneNumber = instructor.PhoneNumber,
                    GroupIds = instructor.Groups.Select(g => g.Id).ToList()
                };
                return instructorSchedule;
            }
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
                    await userManager.AddToRoleAsync(newInstructor, "instructor");
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
            await userManager.DeleteAsync(instructor);
            //context.Instructors.Remove(instructor);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
