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
    public class WaitingListController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public WaitingListController(ApplicationDBContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaitingListStudentDTO>>> GetWaitingList()
        {
            var inactiveStudents = await context.Students.Where(s => !s.ActiveStatus).ToListAsync();
            List<WaitingListStudentDTO> waitingList = new List<WaitingListStudentDTO>();
            foreach (Student student in inactiveStudents)
            {
                WaitingListStudentDTO waitingListStudent = new WaitingListStudentDTO()
                {
                    UserName = student.UserName,
                    Email = student.Email,
                    PhoneNumber = student.PhoneNumber,
                    SelectedStudyPlan = student.StudyPlan
                };
                waitingList.Add(waitingListStudent);
            }
            return waitingList.ToList();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> CompleteStudentData(Guid id, CompleteStudentDataDTO studentData)
        {
            //if (id != student.Id)
            //{
            //    return BadRequest();
            //}
            if (ModelState.IsValid)
            {
                //var student = await userManager.FindByIdAsync(id);
                var student = await context.Students.FindAsync(id);
                if (student != null)
                {
                    student.CurrentLevel = studentData.Level;
                    student.PayedAmount = studentData.PayedAmount;
                    student.ActiveStatus = true;
                    await context.SaveChangesAsync();
                    if (student.StudyPlan == "private")
                    {
                        Group newGroup = new Group()
                        {
                            Level = studentData.Level,
                            ActiveStatus = true
                        };
                        await context.Groups.AddAsync(newGroup);
                        await context.SaveChangesAsync();
                        var createdGroup = await context.Groups.OrderBy(g => g.Id).LastOrDefaultAsync();
                        student.GroupId = createdGroup.Id;
                    }
                    else
                    {
                        var matchingGroup = await context.Groups
                                .SingleOrDefaultAsync(g =>
                                !g.ActiveStatus &&
                                g.Level == studentData.Level &&
                                g.Students.Count < 4);
                        if (matchingGroup == null)
                        {
                            Group newGroup = new Group()
                            {
                                Level = studentData.Level,
                                ActiveStatus = false
                            };
                            await context.Groups.AddAsync(newGroup);
                            await context.SaveChangesAsync();
                            var createdGroup = await context.Groups.OrderBy(g => g.Id).LastOrDefaultAsync();
                            student.GroupId = createdGroup.Id;
                        }
                        else
                        {
                            student.GroupId = matchingGroup.Id;
                            if (matchingGroup.Students.Count > 3)
                            {
                                matchingGroup.ActiveStatus = true;
                            }
                            await context.SaveChangesAsync();
                        }
                    }
                    return Ok();
                }
                else
                    return BadRequest();
            }
            return BadRequest(ModelState);
        }
    }
}
