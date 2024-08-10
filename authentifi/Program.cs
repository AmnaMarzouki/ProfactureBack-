using authentifi.Data;
using authentifi.Models;
using authentifi.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace authentifi
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllers();

			// Add Swagger/OpenAPI
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			


			// Add Authorization
			builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
				options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
			});

			// Configure DbContext
			builder.Services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			// Add Identity
			builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
			{
				// Configure identity options if needed
			})
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();

			// Configure Email Settings and Email Service
			builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
			builder.Services.AddTransient<EmailService>();

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
            var app = builder.Build();

			// Configure middleware pipeline
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseRouting();

			// Use Authentication and Authorization
			app.UseAuthentication();
			app.UseAuthorization();
            app.UseCors("AllowAll");
            app.MapControllers();

			// Create Roles and Admin user
			using (var scope = app.Services.CreateScope())
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				await CreateRoles(roleManager);

				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
				await CreateAdminUser(userManager);
			}

			app.Run();
		}

		private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
		{
			string[] roleNames = { "Admin", "User" };
			IdentityResult roleResult;

			foreach (var roleName in roleNames)
			{
				var roleExist = await roleManager.RoleExistsAsync(roleName);
				if (!roleExist)
				{
					roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}
		}

        private static async Task CreateAdminUser(UserManager<AppUser> userManager)
        {
            var adminEmail = "admin@yourapp.com";
            var adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Admin"
                };

                var createUserResult = await userManager.CreateAsync(newAdminUser, adminPassword);
                if (createUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdminUser, "Admin");
                }
                else
                {
                    Console.WriteLine("Erreur lors de la création de l'utilisateur : " + string.Join(", ", createUserResult.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                Console.WriteLine("L'utilisateur administrateur existe déjà.");
            }
        }

    }
}
