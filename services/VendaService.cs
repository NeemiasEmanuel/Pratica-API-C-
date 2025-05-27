using loja.data;
using loja.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace loja.services
{
    public class VendaService
    {
           private readonly LojaDbContext _context;
        private readonly EstoqueService _estoqueService;
        public VendaService(LojaDbContext context, EstoqueService estoqueService)
        {
            _context = context;

            _estoqueService = estoqueService;
        }

        public async Task<IEnumerable<Venda>> GetAllVendasAsync()
        {
            return await _context.Vendas.ToListAsync();
        }

        public async Task<Venda> GetVendaByIdAsync(int id)
        {
            return await _context.Vendas.FindAsync(id);
        }

        public async Task AddVendaAsync(Venda venda)
        {
             // Validar se o cliente e o produto existem
            var clienteExists = await _context.Clientes.AnyAsync(c => c.Id == venda.ClienteId);
            var produtoExists = await _context.Produtos.AnyAsync(p => p.Id == venda.ProdutoId);


            if (!clienteExists || !produtoExists)
            {
                throw new Exception("Cliente ou Produto não encontrado.");
            }


            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Vendas.Add(venda);
                await _context.SaveChangesAsync();


                await _estoqueService.SubtractFromEstoqueAsync(venda.ProdutoId, venda.Quantidade);


                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> UpdateVendaAsync(int id, Venda venda)
        {
            var existingVenda = await _context.Vendas.FindAsync(id);
            if (existingVenda == null) return false;

            existingVenda.Cliente = venda.Cliente;
            existingVenda.Produto = venda.Produto;
            existingVenda.Quantidade = venda.Quantidade;
            existingVenda.Data = venda.Data;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteVendaAsync(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null) return false;

            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();
            return true;
        }
    



    public async Task<IEnumerable<Venda>> GetVendasPorProdutoDetalhadaAsync(int produtoId)
    {
        // Implementação para retornar vendas detalhadas por produto
        return await _context.Vendas.Where(v => v.ProdutoId == produtoId).Include(v => v.Produto).ToListAsync();
    }

   public async Task<object> GetVendasPorProdutoSumarizadaAsync(int ProdutoId)
{
    var vendasSumarizadas = await _context.Vendas
        .Where(v => v.ProdutoId == ProdutoId)
        .GroupBy(v => v.ProdutoId)
        .Select(g => new
        {
            ProdutoId = g.Key,
            NomeProduto = g.FirstOrDefault().Produto.Nome, // Supondo que você tenha uma propriedade "Nome" em Produto
            TotalQuantidade = g.Sum(v => v.Quantidade),
            TotalPreco = g.Sum(v => v.PrecoUnitario * v.Quantidade) // Calculando o total de preço cobrado
        })
        .FirstOrDefaultAsync();

    return vendasSumarizadas;
}
            public async Task<List<dynamic>> GetVendasPorClienteDetalhadaAsync(int clienteId)
        {
            return await _context.Vendas
                .Where(v => v.ClienteId == clienteId)
                .Include(v => v.Produto)
                .Select(v => new
                {
                    ProdutoNome = v.Produto.Nome,
                    v.Data,
                    v.Id,
                    v.Quantidade,
                    v.PrecoUnitario
                })
                .ToListAsync<dynamic>();
        }
      public async Task<dynamic> GetVendasPorClienteSumarizadaAsync(int clienteId)
        {
            var vendas = await _context.Vendas
                .Where(v => v.ClienteId == clienteId)
                .Include(v => v.Produto)
                .ToListAsync();


            var totalPreco = vendas.Sum(v => v.Quantidade * v.PrecoUnitario);


            var produtosQuantidade = vendas
                .GroupBy(v => v.ProdutoId)
                .Select(g => new
                {
                    ProdutoNome = g.FirstOrDefault()?.Produto?.Nome,
                    Quantidade = g.Sum(v => v.Quantidade),
                    TotalPrecoProduto = g.Sum(v => v.Quantidade * v.PrecoUnitario)
                })
                .ToList<dynamic>();


            return new
            {
                TotalPreco = totalPreco,
                ProdutosQuantidade = produtosQuantidade
            };
        }
    }
}