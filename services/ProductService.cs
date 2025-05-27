using loja.data;
using loja.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace loja.services
{
    public class ProductService
    {
        private readonly LojaDbContext _context;

        public ProductService(LojaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> GetAllProductsAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto> GetProductByIdAsync(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task AddProductAsync(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateProductAsync(int id, Produto produto)
        {
            var existingProduct = await _context.Produtos.FindAsync(id);
            if (existingProduct == null) return false;

            existingProduct.Nome = produto.Nome;
            existingProduct.Preco = produto.Preco;
            existingProduct.Fornecedor = produto.Fornecedor;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Produtos.FindAsync(id);
            if (product == null) return false;

            _context.Produtos.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
