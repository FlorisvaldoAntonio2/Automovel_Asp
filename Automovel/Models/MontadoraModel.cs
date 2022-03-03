using System.ComponentModel.DataAnnotations;

namespace Automovel.Models
{
    public class MontadoraModel
    {
        [Key]
        public int IdMontadora { get; set; }


        [Required(ErrorMessage = "Informe o nome"), MaxLength(50)]
        public string Name { get; set; }
    }
}
