using System;
using System.ComponentModel.DataAnnotations;

namespace loja.models
{
    public class Contrato
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ServicoId { get; set; }

        [Required]
        public decimal PrecoCobrado { get; set; }

        [Required]
        public DateTime DataContratacao { get; set; }

        public Cliente Cliente { get; set; }
        public Service Servico { get; set; }
    }
}
