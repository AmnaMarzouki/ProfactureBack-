using System.ComponentModel.DataAnnotations;

namespace authentifi.Models
{
	public class ForgotPasswordModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
	}
}
