using authentifi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace authentifi.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{

		}
	}
}
