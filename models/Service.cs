using System.ComponentModel.DataAnnotations;

namespace loja.models
{
    public class Service
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Nome { get; set; }

        [Required]
        public double Preco { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}
