
using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            builder.Services
                    .AddDefaultIdentity<ApplicationUser>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 3;
                    })
                    .AddEntityFrameworkStores<ApplicationDBContext>();
            builder.Services.AddIdentityCore<Instructor>().AddEntityFrameworkStores<ApplicationDBContext>();
            builder.Services.AddIdentityCore<Student>().AddEntityFrameworkStores<ApplicationDBContext>();

            // service of check Token 
            //authentication services
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer= builder.Configuration["jwt:issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["jwt:audience"],
                    IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"]))

                };
            });


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