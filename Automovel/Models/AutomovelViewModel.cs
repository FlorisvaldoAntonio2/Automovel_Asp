using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automovel.Models
{
    public class AutomovelViewModel
    {
        [Required]
        public int IdAutomovel { get; set; }
        [Required]
        public string Modelo { get; set; }
        [Required]
        public string Cor { get; set; }
        [Required]
        public int Portas { get; set; }
        [Required]
        public string Ano { get; set;}
        [Required]
        public double Quilometragem { get; set; }
        
        public IFormFile Foto { get; set; }
        public int IdMontadora { get; set; }
    }
}
