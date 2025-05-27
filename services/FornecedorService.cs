using loja.data;
using loja.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace loja.services
{
    public class FornecedorService
    {
        private readonly LojaDbContext _context;

        public FornecedorService(LojaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Fornecedor>> GetAllFornecedoresAsync()
        {
            return await _context.Fornecedores.ToListAsync();
        }

        public async Task<Fornecedor> GetFornecedorByIdAsync(int id)
        {
            return await _context.Fornecedores.FindAsync(id);
        }

        public async Task AddFornecedorAsync(Fornecedor fornecedor)
        {
            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateFornecedorAsync(int id, Fornecedor fornecedor)
        {
            var existingFornecedor = await _context.Fornecedores.FindAsync(id);
            if (existingFornecedor == null) return false;

            existingFornecedor.Nome = fornecedor.Nome;
            existingFornecedor.Cnpj = fornecedor.Cnpj;
            existingFornecedor.Email = fornecedor.Email;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFornecedorAsync(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null) return false;

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
