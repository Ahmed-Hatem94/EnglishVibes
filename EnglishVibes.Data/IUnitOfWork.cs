using EnglishVibes.Data.Interfaces;
using EnglishVibes.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IGroupRepository Groups { get; }
        IStudentRepository Students { get; }
        IInstructorRepository Instructors { get; }
        IGroupWeekDaysRepository GroupWeekDays { get; }

        int Complete();
    }
}
