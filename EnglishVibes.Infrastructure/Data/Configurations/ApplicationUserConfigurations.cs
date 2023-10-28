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
    public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            //builder.HasMany(u => u.UserRoles).WithOne(x => x.).HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany<ApplicationUserRole>(au => au.UserRoles)
                    .WithOne(aur => aur.ApplicationUser)
                    .HasForeignKey(au => au.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);

            //tell database to use this column as Discriminator
            //builder.HasDiscriminator<string>("UserType");
        }
    }
}
