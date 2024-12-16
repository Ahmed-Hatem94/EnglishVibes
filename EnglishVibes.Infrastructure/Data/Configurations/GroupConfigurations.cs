using EnglishVibes.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishVibes.Infrastructure.Data.Configurations
{
    public class GroupConfigurations : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            //builder.HasOne<Instructor>(i => i.Instructor)
            //        .WithMany(g => g.Groups)
            //        .HasForeignKey(i => i.InstructorId);

            builder.HasOne<Instructor>(g => g.Instructor)
                    .WithMany(i => i.Groups)
                    .HasForeignKey(g => g.InstructorId);

            builder.HasMany<Student>(g => g.Students)
                    .WithOne(s => s.Group)
                    .HasForeignKey(g => g.GroupId);

        }
    }
}
