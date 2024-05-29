using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WhiteLagoon.Web.ViewModels
{
	public class RegisterVM
	{
		[Required]
		public string Email { get; set; } = null!;
		[Required]
		//Como o atributo é uma string, e para meter ****, metemos esta propriedade
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;

		[Required]
		//Como o atributo é uma string, e para meter ****, metemos esta propriedade
		[DataType(DataType.Password)]
		//propriedade para comparar as palavras-passes
		[Compare(nameof(Password))]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; } = null!;

		[Required]
		public string Name { get; set; } = null!;

		[Display(Name = "Phone Number")]
		public string? PhoneNumber { get; set; } 
		public string? RedirectUrl { get; set; }

		public string? Role { get; set; }
		[ValidateNever]
        public IEnumerable<SelectListItem>? RoleList { get; set; }	//tipo de utilizador

    }
}
