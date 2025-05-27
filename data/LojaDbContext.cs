using loja.models;
using Microsoft.EntityFrameworkCore;

namespace loja.data
{
    public class LojaDbContext : DbContext
    {
        public LojaDbContext(DbContextOptions<LojaDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Deposito> Depositos { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
    }
}
