using Microsoft.AspNetCore.Identity;

namespace WhiteLagoon.Domain.Entities
{
	//Como existe um IdentityUser injectado diretamente no Program.cs, nós aqui dizemos que herdamos o IdentityUser e ele já vai buscar os campos que lá estão
	public class ApplicationUser : IdentityUser
	{
		public string? Name { get; set; }
		public DateTime CreatedAt { get; set; }

	}
}
