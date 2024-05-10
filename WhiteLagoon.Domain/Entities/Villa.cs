using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhiteLagoon.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        [Display (Name = "Price per night")]
        [Range (10, 10000)]
        public double Price { get; set; }

        public int Sqft { get; set; }

        [Range(1, 10)]
        public int Occupancy { get; set; }

        [NotMapped] //Esta propriedade serve para dizer que esta propriedade NÃO É para criar uma coluna na bd
        public IFormFile? Image { get; set; }   

        [Display(Name = "Image Url")]
        public string? ImageUrl { get; set; }           //Não queremos guardar imagens na bd. por isso vamos guardar na bd apenas o caminho

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
