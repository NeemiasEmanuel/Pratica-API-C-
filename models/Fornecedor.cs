using System.ComponentModel.DataAnnotations;

namespace loja.models
{
    public class Fornecedor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Cnpj { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Endereco { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Telefone { get; set; }
    }
}
