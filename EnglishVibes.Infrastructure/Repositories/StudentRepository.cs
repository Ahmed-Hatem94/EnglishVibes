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
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly DbSet<Student> students;
        public StudentRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            students = dbContext.Set<Student>();
        }
    }
}
