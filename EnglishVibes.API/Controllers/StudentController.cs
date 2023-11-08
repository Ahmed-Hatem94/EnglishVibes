using EnglishVibes.Data;
using EnglishVibes.Data.Consts;
using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using EnglishVibes.Service.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishVibes.API.Controllers
{
    
    public class StudentController : BaseAPIController
    {
        private readonly IUnitOfWork unitOfWork;
        //private readonly ApplicationDBContext context;
        private readonly UserManager<Student> userManager;

        public StudentController(
            IUnitOfWork unitOfWork,
            //ApplicationDBContext _context,
            UserManager<Student> _userManager)
        {
            this.unitOfWork = unitOfWork;
            //context = _context;
            userManager = _userManager;
        }

        [HttpGet("waitinglist")]
        public async Task<ActionResult<IEnumerable<WaitingListStudentDTO>>> GetWaitingList()
        {
            var inactiveStudents = await unitOfWork.Students.FindAllAsync(s => !s.ActiveStatus);
            //var inactiveStudents = await context.Students.Where(s => !s.ActiveStatus).ToListAsync();
            List<WaitingListStudentDTO> waitingList = new List<WaitingListStudentDTO>();
            foreach (Student student in inactiveStudents)
            {
                WaitingListStudentDTO waitingListStudent = new WaitingListStudentDTO()
                {
                    Id = student.Id,
                    UserName = student.UserName,
                    Email = student.Email,
                    PhoneNumber = student.PhoneNumber,
                    SelectedStudyPlan = student.StudyPlan
                };
                waitingList.Add(waitingListStudent);
            }
            return waitingList.ToList();
        }

        [HttpGet("active/all")]
        public async Task<ActionResult<IEnumerable<ActiveStudentDTO>>> GetActiveStudents()
        {
            var activeStudents = await unitOfWork.Students.FindAllAsync(s => s.ActiveStatus);
            //var activeStudents = await context.Students.Where(s => s.ActiveStatus).ToListAsync();
            List<ActiveStudentDTO> activeStudentList = new List<ActiveStudentDTO>();
            foreach (Student student in activeStudents)
            {
                ActiveStudentDTO activeStudent = new ActiveStudentDTO()
                {
                    Id = student.Id,
                    UserName = student.UserName,
                    Email = student.Email,
                    PhoneNumber = student.PhoneNumber,
                    SelectedStudyPlan = student.StudyPlan,
                    CurrentLevel = student.CurrentLevel,
                    GroupId = (int)student.GroupId,
                    PayedAmount = (decimal)student.PayedAmount,
                    ActiveStatus = student.ActiveStatus
                };
                activeStudentList.Add(activeStudent);
            }
            return activeStudentList.ToList();
        }

        [HttpGet("active/{id}")]
        public async Task<ActionResult<ActiveStudentDTO>> GetActiveStudent(Guid id)
        {
            var activeStudent = await unitOfWork.Students.FindAsync(s => s.ActiveStatus && s.Id == id);
            //var activeStudent = await context.Students.FirstOrDefaultAsync(s => s.ActiveStatus && s.Id == id);
            if (activeStudent != null)
            {
                ActiveStudentDTO activeStudentDTO = new ActiveStudentDTO()
                {
                    Id = activeStudent.Id,
                    UserName = activeStudent.UserName,
                    Email = activeStudent.Email,
                    PhoneNumber = activeStudent.PhoneNumber,
                    SelectedStudyPlan = activeStudent.StudyPlan,
                    CurrentLevel = activeStudent.CurrentLevel,
                    GroupId = (int)activeStudent.GroupId,
                    PayedAmount = (decimal)activeStudent.PayedAmount,
                    ActiveStatus = activeStudent.ActiveStatus
                };
                return activeStudentDTO;
            }
            return BadRequest();
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
                var student = await userManager.FindByIdAsync(id.ToString());
                //var student = await context.Students.FindAsync(id);
                if (student != null && !student.ActiveStatus)
                {
                    student.CurrentLevel = studentData.Level;
                    student.PayedAmount = studentData.PayedAmount;
                    student.ActiveStatus = true;
                    await userManager.UpdateAsync(student);
                    //await context.SaveChangesAsync();
                    if (student.StudyPlan == "private")
                    {
                        Group newGroup = new Group()
                        {
                            Level = studentData.Level,
                            ActiveStatus = false,
                            StudyPlan = student.StudyPlan
                        };
                        await unitOfWork.Groups.AddAsync(newGroup);
                        unitOfWork.Complete();
                        //await context.Groups.AddAsync(newGroup);
                        //await context.SaveChangesAsync();
                        var createdGroup = await unitOfWork.Groups.FindAsync(g => true,
                                                                                    null,
                                                                                    g => g.Id,
                                                                                    OrderBy.Descending);
                        //var createdGroup = await context.Groups.OrderBy(g => g.Id).LastOrDefaultAsync();
                        student.GroupId = createdGroup.Id;
                        unitOfWork.Complete();
                    }
                    else
                    {
                        var matchingGroup = await unitOfWork.Groups
                                                            .FindAsync(g => g.StudyPlan == "group" &&
                                                                        g.Level == studentData.Level &&
                                                                        g.Students.Count < 4,
                                                                        new[] { "Students" });
                        //var matchingGroup = await context.Groups.Include(g => g.Students)
                        //        .SingleOrDefaultAsync(g =>
                        //        g.StudyPlan == "group" &&
                        //        g.Level == studentData.Level &&
                        //        g.Students.Count < 4);
                        if (matchingGroup == null)
                        {
                            Group newGroup = new Group()
                            {
                                Level = studentData.Level,
                                ActiveStatus = false,
                                StudyPlan = student.StudyPlan
                            };
                            await unitOfWork.Groups.AddAsync(newGroup);
                            unitOfWork.Complete();
                            //await context.Groups.AddAsync(newGroup);
                            //await context.SaveChangesAsync();
                            var createdGroup = await unitOfWork.Groups.FindAsync(g => true,
                                                                                    null,
                                                                                    g => g.Id, 
                                                                                    OrderBy.Descending);
                            //var createdGroup = await context.Groups.OrderBy(g => g.Id).LastOrDefaultAsync();
                            student.GroupId = createdGroup.Id;
                            
                        }
                        else
                        {
                            student.GroupId = matchingGroup.Id;
                            //await context.SaveChangesAsync();
                        }
                        unitOfWork.Complete();
                    }
                    return Ok(new { message = "success"});
                }
                else
                    return BadRequest();
            }
            return BadRequest(ModelState);
        }
    }
}
