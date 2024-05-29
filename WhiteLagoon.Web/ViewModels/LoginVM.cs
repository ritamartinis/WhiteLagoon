using System.ComponentModel.DataAnnotations;

namespace WhiteLagoon.Web.ViewModels
{
	public class LoginVM
	{
		[Required]
		public string Email { get; set; } = null!;
		[Required]
		//Como o atributo é uma string, e para meter ****, metemos esta propriedade
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;
		public bool RememberMe { get; set; }
		public string? RedirectUrl { get; set; }

	}
}
