using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace loja.models
{
    public class Deposito
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Nome { get; set; }

        public ICollection<Estoque> Estoques { get; set; }
    }

    public class Estoque
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int DepositoId { get; set; }
        
        [Required]
        public int ProdutoId { get; set; }

        [Required]
        public int Quantidade { get; set; }

        public Deposito Deposito { get; set; }
        public Produto Produto { get; set; }
    }
}
