using AutoMapper;
using Elfie.Serialization;
using EnglishVibes.API.DTO;
using EnglishVibes.Data.Models;
using EnglishVibes.Data.Interfaces;
using EnglishVibes.Infrastructure.Data;
using EnglishVibes.Service.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EnglishVibes.Data;

namespace EnglishVibes.API.Controllers
{

    public class GroupController : BaseAPIController
    {
        //private readonly IGenericRepository<Group> groupRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationDBContext context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public GroupController(
            //IGenericRepository<Group> groupRepository,
            IUnitOfWork unitOfWork,
            //ApplicationDBContext _context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            //this.groupRepository = groupRepository;
            this.unitOfWork = unitOfWork;
            //context = _context;
            _mapper = mapper;
            this.userManager = userManager;
        }
        // what should i do :- 

        //1-  Action to return inactive group (level , student in this group [ number , names]) 
        [HttpGet("inactive")]
        public async Task<ActionResult<IEnumerable<InActiveGroupDto>>> GetInActiveGroup()
        {
            List<Group> inactiveGroups = (List<Group>)await unitOfWork.Groups.FindAllAsync(g => !g.ActiveStatus, new[] { "Students" });
            //List <Group> inactiveGroups = await context.Groups
            //                                  .Where(s => !s.ActiveStatus)
            //                                  .Include(g => g.Students)
            //                                  .ToListAsync();

            var map = _mapper.Map<IEnumerable<Group>, IEnumerable<InActiveGroupDto>>(inactiveGroups);

            return Ok(map);
        }

        // 2-  Action to return Active group(level , student in this group , [number, names])
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ActiveGroupDto>>> GetActiveGroup()
        {
            List<Group> activeGroups = (List<Group>)await unitOfWork.Groups.FindAllAsync(g => g.ActiveStatus, new[] { "Students", "Instructor", "GroupWeekDays" });
            //var activeGroups = await context.Groups
            //                                .Where(s => s.ActiveStatus)
            //                                .Include(g => g.Students)
            //                                .Include(g => g.Instructor)
            //                                .Include(g => g.GroupWeekDays)
            //                                .ToListAsync();
            var map = _mapper.Map<IEnumerable<Group>, IEnumerable<ActiveGroupDto>>(activeGroups);

            return Ok(map);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDto>> GetGroupById(int id)
        {
            var group = await unitOfWork.Groups.FindAsync(g => g.Id == id, new[] { "Students", "Instructor" });
            //var group = context.Groups.Include(g => g.Students).Include(g => g.Instructor).FirstOrDefault(n => n.Id == id);
            //   var map = _mapper.Map<IReadOnlyList<Group>, IReadOnlyList<ActiveGroupDto>>(ActiveGroups);
            GroupDto groupDto = new GroupDto()
            {
                Id = group.Id,
                Level = group.Level,
                StudyPlan = group.StudyPlan,
                ActiveStatus = group.ActiveStatus,


                //  Students = group.Students.Select(g => g.Id).ToList()

            };
            Instructor instructor;
            if (group.ActiveStatus)
            {
                //groupDto.Instructors.Add(group.Instructor.UserName);
                instructor = new Instructor()
                {
                    Id = group.Instructor.Id,
                    UserName = group.Instructor.UserName
                };
                groupDto.Instructors.Add(instructor);
            }
            else
            {
                //foreach (var instructor in context.Instructors)
                //{
                //    groupDto.Instructors.Add(instructor.UserName);

                //}
                var instructors = await unitOfWork.Instructors.GetAllAsync();
                foreach (Instructor inst in instructors)
                {
                    instructor = new Instructor()
                    {
                        Id = inst.Id,
                        UserName = inst.UserName
                    };
                    groupDto.Instructors.Add(instructor);
                }
            }

            foreach (Student s in group.Students)
            {
                groupDto.Students.Add(s.UserName);

            }

            return Ok(groupDto);
        }



        //3-  Action Complete Group-Data [httpput] (startdate,instructor,timeslot) 
        [HttpPut("{id}")]
        public async Task<ActionResult> CompleteGroupData(int id, CompleteGroupDataDTO groupDataDTO)
        {
            var group = await unitOfWork.Groups.FindAsync(x => x.Id == id, new[] { "GroupWeekDays" });
            //var group = context.Groups.Include(g => g.GroupWeekDays)
            //                    .FirstOrDefault(g => g.Id == id); // we will take group id from form
            if (group.ActiveStatus)
            {
                return BadRequest();
            }
            group.StartDate = groupDataDTO.StartDate;
            group.InstructorId = groupDataDTO.InstructorId;
            group.TimeSlot = groupDataDTO.TimeSlot;
            group.ActiveStatus = true;
            unitOfWork.Complete();


            //if (group.GroupWeekDays.Count != 0)
            //{
            //    group.GroupWeekDays.Clear();
            //};

            IEnumerable<GroupWeekDays> groupWeekDays = new List<GroupWeekDays>()
            {
                new GroupWeekDays {GroupId = id, WeekDay = (DayOfWeek)groupDataDTO.Day1 },
                new GroupWeekDays {GroupId = id, WeekDay = (DayOfWeek)groupDataDTO.Day2 }
            };
            await unitOfWork.GroupWeekDays.AddRangeAsync(groupWeekDays);

            unitOfWork.Complete();

            return Ok();
        }

    }
}
