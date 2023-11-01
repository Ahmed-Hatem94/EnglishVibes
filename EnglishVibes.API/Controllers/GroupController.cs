using AutoMapper;
using EnglishVibes.API.DTO;
using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using EnglishVibes.Service.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InActiveGroupDto>>> GetInActiveGroup()
        {
            var inactiveGroups = await context.Groups.Where(s => !s.ActiveStatus).ToListAsync();
            var map = _mapper.Map<IReadOnlyList<Group>, IReadOnlyList<InActiveGroupDto>>(inactiveGroups);

            return Ok(map.ToList());
        }

        // 2-  Action to return Active group(level , student in this group , [number, names])
        [HttpGet("Represent")]
        public async Task<ActionResult<IEnumerable<ActiveGroupDto>>> GetActiveGroup()
        {
            var ActiveGroups = await context.Groups.Where(s => s.ActiveStatus).ToListAsync();
            var map = _mapper.Map<IReadOnlyList<Group>, IReadOnlyList<ActiveGroupDto>>(ActiveGroups);

            return Ok(map.ToList());
        }



        //3-  Action Complete Group-Data [httpput] (startdate,instructor,timeslot) 
        [HttpPost("assign")]
        public async Task<ActionResult> CompleteGroupData(DateTime StartDate, Instructor instructor, DateTime TimeSlot)
        {
            var group = await context.Groups.FirstOrDefaultAsync(); // we will take group id from form
            group.StartDate = StartDate;
            group.InstructorId = instructor.Id;
            group.TimeSlot = TimeSlot;

            context.Groups.Update(group);

            // context.SaveChangesAsync();
            return Ok();
        }

    }
}
