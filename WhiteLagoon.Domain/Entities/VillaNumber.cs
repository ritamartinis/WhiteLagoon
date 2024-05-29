using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhiteLagoon.Domain.Entities
{
    public class VillaNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        //sempre que eu quiser que a chave primária seja algo != de id ou o nome da classe, tenho obrigatoria// de colocar [Key] para saber
        //Em relação do BD Generated, isto é para não ser AI e para a base de dados não controlar
        //Somos nós que decidimos que número de quarto é que colocamos
        [Display(Name = "Villa Number")]
        public int Villa_Number { get; set; }       //nr do quarto

        [ForeignKey("Villa")]   //Chave estrangeira é a Villa, ou seja, o tipo de quarto. Esta é a ligação ao Villa.cs
        [Display(Name = "Villa")]
        //Para criar uma chave estrangeira, precisamos destes dois: int villa id e villa villa
        public int VillaId { get; set; }
        //este é só para navegação entre as duas tabelas, para não nos dar erro quando preenchemos o formulário, precisamos:
        [ValidateNever]
        public Villa Villa { get; set; } = null!;       //esta propriedade serve para ele navegar entre as duas tabelas

        [Display(Name = "Special Details")]
        public string? SpecialDetails { get; set; }
    }
}
