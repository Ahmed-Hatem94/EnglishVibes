using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Infrastructure.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
   
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupWeekDays> GroupWeekDays { get; set; }

      

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<Instructor>();
            builder.Ignore<Student>();
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);

        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            configurationBuilder.Properties<TimeOnly>()
            .HaveConversion<TimeOnlyConverter, TimeOnlyComparer>();
        }
    }
}
