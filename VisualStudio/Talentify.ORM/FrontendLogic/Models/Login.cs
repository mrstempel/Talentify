using System.ComponentModel.DataAnnotations;

namespace Talentify.ORM.FrontendLogic.Models
{
	public class Login
	{
		[Required]
		[Display(Name = "E-Mail")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Passwort")]
		public string Password { get; set; }

		public string ReturnUrl { get; set; }
	}
}
