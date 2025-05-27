using loja.data;
using loja.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace loja.services
{
    public class EstoqueService
    {
        private readonly LojaDbContext _context;

        public EstoqueService(LojaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estoque>> GetAllEstoquesAsync()
        {
            return await _context.Estoques.ToListAsync();
        }

        public async Task<Estoque> GetEstoqueByIdAsync(int id)
        {
            return await _context.Estoques.FindAsync(id);
        }

        public async Task AddEstoqueAsync(Estoque estoque)
        {
            _context.Estoques.Add(estoque);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateEstoqueAsync(int id, Estoque estoque)
        {
            var existingEstoque = await _context.Estoques.FindAsync(id);
            if (existingEstoque == null) return false;

            existingEstoque.Produto = estoque.Produto;
            existingEstoque.Quantidade = estoque.Quantidade;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEstoqueAsync(int id)
        {
            var estoque = await _context.Estoques.FindAsync(id);
            if (estoque == null) return false;

            _context.Estoques.Remove(estoque);
            await _context.SaveChangesAsync();
            return true;
        }
 

        public async Task SubtractFromEstoqueAsync(int produtoId, int quantidade)
        {
            var estoque = await _context.Estoques.FirstOrDefaultAsync(e => e.ProdutoId == produtoId);
            if (estoque == null || estoque.Quantidade < quantidade)
            {
                throw new Exception("Estoque insuficiente.");
            }


            estoque.Quantidade -= quantidade;
            await _context.SaveChangesAsync();
        }



}
}