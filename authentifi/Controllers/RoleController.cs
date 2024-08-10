using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using authentifi.Models;
using authentifi.Data;
using Microsoft.Data.SqlClient;
using authentifi.DTO;

namespace authentifi.Controllers
{
	[Route("api/RoleController")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
		public IActionResult GetRoles()
		{
			var roles = _roleManager.Roles.ToList();
			return Ok(roles);
		}

		[HttpPost]
		public async Task<IActionResult> CreateRole([FromBody] string roleName)
		{
			if (string.IsNullOrEmpty(roleName))
				return BadRequest("Role name should not be empty.");

			var roleExist = await _roleManager.RoleExistsAsync(roleName);
			if (roleExist)
				return BadRequest("Role already exists.");

			var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
			if (result.Succeeded)
				return Ok();

			return BadRequest(result.Errors);
		}

		[HttpDelete("{roleName}")]
		public async Task<IActionResult> DeleteRole(string roleName)
		{
			var role = await _roleManager.FindByNameAsync(roleName);
			if (role == null)
				return NotFound("Role not found.");

			var result = await _roleManager.DeleteAsync(role);
			if (result.Succeeded)
				return Ok();

			return BadRequest(result.Errors);
		}

		[HttpPut("{roleName}")]
		public async Task<IActionResult> UpdateRole(string roleName, [FromBody] string newRoleName)
		{
			var role = await _roleManager.FindByNameAsync(roleName);
			if (role == null)
				return NotFound("Role not found.");

			role.Name = newRoleName;
			var result = await _roleManager.UpdateAsync(role);
			if (result.Succeeded)
				return Ok();

			return BadRequest(result.Errors);
		}

        [HttpGet]
        [Route("api/users/byrole")]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            var users = new List<UserDto>();

            // Chaîne de connexion à la base de données
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Requête SQL
            string query = @"
        SELECT u.Id, u.UserName, u.Email, r.Name AS RoleName
        FROM AspNetUsers u
        INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
        INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
        WHERE r.Name = @RoleName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Ajout du paramètre SQL pour éviter les injections SQL
                    command.Parameters.AddWithValue("@RoleName", roleName);

                    connection.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new UserDto
                            {
                                Id = reader["Id"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                Email = reader["Email"].ToString(),
                                RoleName = reader["RoleName"].ToString()
                            });
                        }
                    }
                }
            }

            return Ok(users);
        }

    }
}
