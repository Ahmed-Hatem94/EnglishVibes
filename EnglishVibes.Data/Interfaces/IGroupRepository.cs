using EnglishVibes.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Data.Interfaces
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        public Task<List<Group>> GetActiveGroupAsync();
        public Task<List<Group>> GetInActiveGroupAsync();
        public Task<Group> GetByIdAsync(int id);
        public Group GetByIdIncludeWeekDaysAsync(int id);
    }
}
