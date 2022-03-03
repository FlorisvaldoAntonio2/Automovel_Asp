using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automovel.Models
{
    public class AutomovelModel
    {
        [Key]
        public int IdAutomovel { get; set; }

        [Required(ErrorMessage = "Nome invalido"), MaxLength(50)]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "cor invalido"), MaxLength(20)]
        public string Cor { get; set; }

        [Required(ErrorMessage = "Numero de portas invalida")]
        public int Portas { get; set; }

        [Required(ErrorMessage = "Ano invalida"), MinLength(4) , MaxLength(4)]
        public string Ano { get; set;}

        [Required(ErrorMessage = "Quilometragem invalida")]
        public double Quilometragem { get; set; }
        public string Foto { get; set; }

        [ForeignKey("Montadora")]
        public int IdMontadora { get; set; }

        public MontadoraModel Montadora { get; set; }
    }
}
