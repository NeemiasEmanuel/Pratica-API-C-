using System;
using System.ComponentModel.DataAnnotations;

namespace loja.models
{
    public class Venda
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public string NumeroNotaFiscal { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public double PrecoUnitario { get; set; }

        public Produto Produto { get; set; }
        public Cliente Cliente { get; set; }
    }
}
