using EnglishVibes.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Infrastructure.Data.Configurations
{
    public class StudentConfigurations : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            //builder.ToTable("Students");
            //builder.HasOne<Group>(s => s.Group)
            //        .WithMany(g => g.Students)
            //        .HasForeignKey(s => s.GroupId);
        }
    }
}
