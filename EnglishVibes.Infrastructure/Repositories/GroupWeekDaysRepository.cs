using EnglishVibes.Data.Interfaces;
using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Infrastructure.Repositories
{
    public class GroupWeekDaysRepository : GenericRepository<GroupWeekDays>, IGroupWeekDaysRepository
    {
        private readonly DbSet<GroupWeekDays> groupWeekDays;
        public GroupWeekDaysRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            groupWeekDays = dbContext.Set<GroupWeekDays>();
        }
    }
}
