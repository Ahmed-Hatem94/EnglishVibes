
using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EnglishVibes.Infrastructure.Seeder;
using AutoMapper;
using EnglishVibes.API.DTO;
using EnglishVibes.API.Helpers;
using EnglishVibes.Data.Interfaces;
using EnglishVibes.Infrastructure.Repositories;
using EnglishVibes.Data;
using EnglishVibes.Infrastructure;

namespace EnglishVibes.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {            
            var builder = WebApplication.CreateBuilder(args);


            #region Register/Configure Services

            // Add services to the container.
            // This method gets called by the runtime. Use this method to add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register DbContext Service
            // Configure Connection To SQL Server
            builder.Services.AddDbContext<ApplicationDBContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DBCS"));
            });


            // Register Identity Manager Service
            builder.Services
                    .AddDefaultIdentity<ApplicationUser>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 3;
                        options.User.RequireUniqueEmail = true;
                    })
                    .AddRoles<IdentityRole<Guid>>() // Don't forget the generic datatype Guid
                    .AddEntityFrameworkStores<ApplicationDBContext>();
            builder.Services.AddIdentityCore<Instructor>().AddEntityFrameworkStores<ApplicationDBContext>();
            builder.Services.AddIdentityCore<Student>().AddEntityFrameworkStores<ApplicationDBContext>();
            builder.Services.AddAutoMapper(typeof(InActiveGroupMappingProfile));
            builder.Services.AddAutoMapper(typeof(ActiveGroupMappingProfile));



            // Register Authentication services / Token-checking
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
                    ValidIssuer = builder.Configuration["jwt:issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["jwt:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"]))

                };
            });


            // CORS Policy Name To Allow Cross-domain Requests
            string corsPolicyName = "";
            // CORS Service To Allow Cross-domain Requests
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName,
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });

            #endregion

            #region Register/Configure Custom Services

            //builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            #endregion

            var app = builder.Build();

            #region Auto Database Creation & Data Seeding
            using (var scope = app.Services.CreateScope())
            {
                // Ask CLR For Creating Object From UserManager, RoleManager, and ApplicationDBContext Classes
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();

                try
                {
                    await _dbContext.Database.MigrateAsync();            // Update Database
                    await RoleSeeder.SeedAsync(roleManager);
                    await UserSeeder.SeedAsync(userManager);
                    //await ApplicationDBContext.SeedAsync(_dbContext);  // Data Seeding
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();

                    logger.LogError(ex, "an error Has occured during apply the migration");
                }
            }
            #endregion


            #region Configure Middleware

            // Configure the HTTP request pipeline.
            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // not required in .Net 7
            app.UseAuthorization();

            // Allow Cross-domain Requests
            app.UseCors(corsPolicyName);

            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}