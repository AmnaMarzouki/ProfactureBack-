using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using authentifi.Models;

namespace authentifi.Controllers
{
	[Route("api/RoleController")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<AppUser> _userManager;

		public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
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
	}
}
