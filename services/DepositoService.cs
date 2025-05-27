using loja.data;
using loja.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace loja.services
{
    public class DepositoService
    {
        private readonly LojaDbContext _context;

        public DepositoService(LojaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Deposito>> GetAllDepositosAsync()
        {
            return await _context.Depositos.ToListAsync();
        }

        public async Task<Deposito> GetDepositoByIdAsync(int id)
        {
            return await _context.Depositos.FindAsync(id);
        }

        public async Task AddDepositoAsync(Deposito deposito)
        {
            _context.Depositos.Add(deposito);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateDepositoAsync(int id, Deposito deposito)
        {
            var existingDeposito = await _context.Depositos.FindAsync(id);
            if (existingDeposito == null) return false;

            existingDeposito.Nome = deposito.Nome;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDepositoAsync(int id)
        {
            var deposito = await _context.Depositos.FindAsync(id);
            if (deposito == null) return false;

            _context.Depositos.Remove(deposito);
            await _context.SaveChangesAsync();
            return true;
        }
      public async Task<List<object>> GetProdutosNoDepositoSumarizadaAsync(int depositoId)
{
    var produtosQuantidades = await _context.Estoques
        .Where(e => e.DepositoId == depositoId)
        .Include(e => e.Produto)  // Inclui o produto relacionado
        .Select(e => new
        {
            Produto = e.Produto,
            Quantidade = e.Quantidade
        })
        .ToListAsync();

    return produtosQuantidades.Select(pq => new
    {
        Produto = pq.Produto,
        Quantidade = pq.Quantidade
    }).ToList<object>();
}


public async Task<object> GetQuantidadeProdutoNoDepositoAsync(int produtoId)
{
    var quantidadeTotal = await _context.Estoques
        .Where(e => e.ProdutoId == produtoId)
        .SumAsync(e => e.Quantidade);

    var produto = await _context.Produtos.FindAsync(produtoId);

    if (produto == null)
    {
        return null; // Ou Results.NotFound(), dependendo do tratamento desejado
    }

    return new
    {
        Produto = new
        {
            produto.Id,
            produto.Nome,
            produto.Preco,
            produto.Fornecedor
        },
        Quantidade = quantidadeTotal
    };
}
    }
}

