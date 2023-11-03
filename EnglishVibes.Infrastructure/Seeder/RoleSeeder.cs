using EnglishVibes.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Infrastructure.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            var rolesCount = await roleManager.Roles.CountAsync();
            if (rolesCount <= 0)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = "admin"
                });
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = "instructor"
                });
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = "student"
                });
            }
        }
    }
}
