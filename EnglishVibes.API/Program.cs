
using EnglishVibes.Data.Models;
using EnglishVibes.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EnglishVibes.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // This method gets called by the runtime. Use this method to add services to the container.

            #region ConfigureServices

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
           builder.Services.AddAutoMapper(typeof(Group));    
            #endregion

            var app = builder.Build();
            // new feature
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _dbContext = services.GetRequiredService<ApplicationDBContext>(); // Ask CLR For Creating Object Form ApplicationDBContext Class


            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {

                await _dbContext.Database.MigrateAsync();       // Update Database 
                //  await ApplicationDBContext.SeedAsync(_dbContext);         //  await ApplicationDBContext.SeedAsync(_dbContext);    // Data Seeding
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();

                logger.LogError(ex, "an error Has occured during apply the migration ");
            }

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



            // Configure the HTTP request pipeline.
            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

            #region Configure

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // not required in .Net 7
            app.UseAuthorization();


            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}