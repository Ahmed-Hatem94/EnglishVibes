using AutoMapper;
using Elfie.Serialization;
using EnglishVibes.API.DTO;
using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using EnglishVibes.Service.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EnglishVibes.API.Controllers
{

    public class GroupController : BaseAPIController
    {

        private readonly ApplicationDBContext context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public GroupController(
            ApplicationDBContext _context,
            IMapper mapper,
            UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            _mapper = mapper;
            userManager = _userManager;
        }
        // what should i do :- 

        //1-  Action to return inactive group (level , student in this group [ number , names]) 
        [HttpGet("inactive")]
        public async Task<ActionResult<IEnumerable<InActiveGroupDto>>> GetInActiveGroup()
        {

            List<Group> inactiveGroups = await context.Groups
                                              .Where(s => !s.ActiveStatus)
                                              .Include(g => g.Students)
                                              .ToListAsync();

            var map = _mapper.Map<IEnumerable<Group>, IEnumerable<InActiveGroupDto>>(inactiveGroups);

            return Ok(map);
        }

        // 2-  Action to return Active group(level , student in this group , [number, names])
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ActiveGroupDto>>> GetActiveGroup()
        {
            var ActiveGroups = await context.Groups
                                            .Where(s => s.ActiveStatus)
                                            .Include(g => g.Students)
                                            .Include(g => g.Instructor)
                                            .Include(g => g.GroupWeekDays)
                                            .ToListAsync();
            var map = _mapper.Map<IEnumerable<Group>, IEnumerable<ActiveGroupDto>>(ActiveGroups);

            return Ok(map);
        }

        [HttpGet("{id}")]
        public ActionResult<GroupDto> GetGroupById(int id)
        {
            var groups = context.Groups.Include(g => g.Students).Include(g => g.Instructor).FirstOrDefault(n => n.Id == id);
            //   var map = _mapper.Map<IReadOnlyList<Group>, IReadOnlyList<ActiveGroupDto>>(ActiveGroups);
            GroupDto group = new GroupDto()
            {
                Id = groups.Id,
                Level = groups.Level,
                StudyPlan = groups.StudyPlan,
                ActiveStatus = groups.ActiveStatus,


                //  Students = groups.Students.Select(g => g.Id).ToList()

            };
            Instructor instructor;
            if (groups.ActiveStatus)
            {
                //group.Instructors.Add(groups.Instructor.UserName);
                instructor = new Instructor()
                {
                    Id = groups.Instructor.Id,
                    UserName = groups.Instructor.UserName
                };
                group.Instructors.Add(instructor);
            }
            else
            {
                //foreach (var instructor in context.Instructors)
                //{
                //    group.Instructors.Add(instructor.UserName);

                //}
                foreach (Instructor inst in context.Instructors)
                {
                    instructor = new Instructor()
                    {
                        Id = inst.Id,
                        UserName = inst.UserName
                    };
                    group.Instructors.Add(instructor);
                }
            }

            foreach (Student s in groups.Students)
            {
                group.Students.Add(s.UserName);

            }

            return Ok(group);
        }



        //3-  Action Complete Group-Data [httpput] (startdate,instructor,timeslot) 
        [HttpPost("{id}")]
        public async Task<ActionResult> CompleteGroupData(int id, DateTime StartDate, Guid instructorId, TimeSpan TimeSlot, DayOfWeek d1, DayOfWeek d2)
        {
            var group = await context.Groups.FindAsync(id); // we will take group id from form
            group.StartDate = StartDate;
            group.InstructorId = instructorId;
            group.TimeSlot = TimeSlot;
            group.GroupWeekDays.Add(new GroupWeekDays { GroupId = id, WeekDay = d1 });
            group.GroupWeekDays.Add(new GroupWeekDays { GroupId = id, WeekDay = d2 });
            context.Groups.Update(group);
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
