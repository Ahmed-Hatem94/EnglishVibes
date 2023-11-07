using EnglishVibes.Data.Models;
using EnglishVibes.Data.Interfaces;
using EnglishVibes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Infrastructure.Repositories
{
    public class GroupRepository : GenericRepository<Group>, IGroupRepository
    {
        private readonly DbSet<Group> groups;
        public GroupRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            groups = dbContext.Set<Group>();
        }

        public async Task<List<Group>> GetActiveGroupAsync()
        {
            return await groups.Where(s => s.ActiveStatus)
                                            .Include(g => g.Students)
                                            .Include(g => g.Instructor)
                                            .Include(g => g.GroupWeekDays)
                                            .ToListAsync();
        }

        public async Task<List<Group>> GetInActiveGroupAsync()
        {
            return await groups.Where(s => !s.ActiveStatus)
                                .Include(g => g.Students)
                                .ToListAsync();
        }

        public async Task<Group> GetByIdAsync(int id)
        {
            return await groups.Include(g => g.Students)
                                .Include(g => g.Instructor)
                                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public Group GetByIdIncludeWeekDaysAsync(int id)
        {
            return groups.Include(g => g.GroupWeekDays)
                                .FirstOrDefault(g => g.Id == id);
        }

    }
}
