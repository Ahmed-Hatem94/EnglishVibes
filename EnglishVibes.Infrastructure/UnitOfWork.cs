using EnglishVibes.Data;
using EnglishVibes.Data.Interfaces;
using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using EnglishVibes.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext dbContext;
        public IGroupRepository Groups { get; private set; }
        public IStudentRepository Students { get; private set; }
        public IInstructorRepository Instructors { get; private set; }
        public IGroupWeekDaysRepository GroupWeekDays { get; private set; }

        public UnitOfWork(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
            Groups = new GroupRepository(dbContext);
            Students = new StudentRepository(dbContext);
            Instructors = new InstructorRepository(dbContext);
            GroupWeekDays = new GroupWeekDaysRepository(dbContext);
        }

        public int Complete()
        {
            return dbContext.SaveChangesAsync().Result;
        }

        public void Dispose()
        {
            dbContext.DisposeAsync();
        }
    }
}
