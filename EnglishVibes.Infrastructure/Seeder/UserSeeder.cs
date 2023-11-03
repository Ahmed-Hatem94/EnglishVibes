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
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var userCount = await userManager.Users.CountAsync();
            if (userCount <= 0)
            {
                // Adding Default Admin
                var defaultAdmin = new ApplicationUser()
                {
                    UserName = "aaa",
                    FirstName = "aaaaa",
                    LastName = "aaaaa",
                    Email = "a@a.com",
                    PhoneNumber = "123456",
                    Age = 20
                };
                await userManager.CreateAsync(defaultAdmin,"M123_m");
                await userManager.AddToRolesAsync(defaultAdmin, new List<string>() { "admin", "instructor" });

                // Adding 3 Default Instructors
                string letters = "bcd";
                ApplicationUser defaultInstructor;
                for (int i = 0; i < letters.Length; i++)
                {
                    defaultInstructor = new Instructor()
                    {
                        UserName = new string(letters[i], 3),
                        FirstName = new string(letters[i], 5),
                        LastName = new string(letters[i], 5),
                        Email = $"{letters[i]}@{letters[i]}.com",
                        PhoneNumber = "123456",
                        Age = 20
                    };
                    await userManager.CreateAsync(defaultInstructor, "M123_m");
                    await userManager.AddToRoleAsync(defaultInstructor, "instructor");
                }

                // Adding 5 Default Students
                letters = "vwxyz";
                ApplicationUser defaultStudent;
                bool flag = true;
                for (int i = 0; i < letters.Length; i++)
                {
                    defaultStudent = new Student()
                    {
                        UserName = new string(letters[i], 3),
                        FirstName = new string(letters[i], 5),
                        LastName = new string(letters[i], 5),
                        Email = $"{letters[i]}@{letters[i]}.com",
                        PhoneNumber = "123456",
                        Age = 20,
                        ActiveStatus = false,
                        StudyPlan = flag ? "group" : "private"
                    };
                    await userManager.CreateAsync(defaultStudent, "M123_m");
                    await userManager.AddToRoleAsync(defaultStudent, "student");
                    flag = !flag;
                }
            }
            
        }
    }
}
