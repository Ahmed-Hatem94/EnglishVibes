
using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnglishVibes.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Connection To SQL Server
            builder.Services.AddDbContext<ApplicationDBContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DBCS"));
            });

            // Register Identity Manager
            //builder.Services.AddIdentity<ApplicationIdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDBContext>();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDBContext>();

            builder.Services.AddIdentityCore<Instructor>().AddEntityFrameworkStores<ApplicationDBContext>();
            builder.Services.AddIdentityCore<Student>().AddEntityFrameworkStores<ApplicationDBContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // not required in .Net 7
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}