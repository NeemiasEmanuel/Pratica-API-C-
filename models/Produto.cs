using System.ComponentModel.DataAnnotations;

namespace loja.models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public double Preco { get; set; }

        [Required]
        public string Fornecedor { get; set; }
    }
}
