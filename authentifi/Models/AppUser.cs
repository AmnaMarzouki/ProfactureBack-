using Microsoft.AspNetCore.Identity;
namespace authentifi.Models;


public class AppUser : IdentityUser
	{
	public string Name { get; set; }
	public Guid? IdAbonnement { get; set; }
	public bool? StatusAbonnement { get; set; }


}

