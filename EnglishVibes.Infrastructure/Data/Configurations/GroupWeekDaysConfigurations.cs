using EnglishVibes.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Infrastructure.Data.Configurations
{
    public class GroupWeekDaysConfigurations : IEntityTypeConfiguration<GroupWeekDays>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<GroupWeekDays> builder)
        {
            builder.HasKey(gwd => new
            {
                gwd.GroupId,
                gwd.WeekDay
            });
        }
    }
}
