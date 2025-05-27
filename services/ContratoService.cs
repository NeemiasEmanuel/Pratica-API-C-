using loja.data;
using loja.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace loja.services
{
    public class ContratoService
    {
        private readonly LojaDbContext _context;

        public ContratoService(LojaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contrato>> GetAllContratosAsync()
        {
            return await _context.Contratos.Include(c => c.Cliente).Include(c => c.Servico).ToListAsync();
        }

        public async Task<Contrato> GetContratoByIdAsync(int id)
        {
            return await _context.Contratos.Include(c => c.Cliente).Include(c => c.Servico).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddContratoAsync(Contrato contrato)
        {
            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateContratoAsync(int id, Contrato contrato)
        {
            var existingContrato = await _context.Contratos.FindAsync(id);
            if (existingContrato == null) return false;

            existingContrato.ClienteId = contrato.ClienteId;
            existingContrato.ServicoId = contrato.ServicoId;
            existingContrato.PrecoCobrado = contrato.PrecoCobrado;
            existingContrato.DataContratacao = contrato.DataContratacao;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContratoAsync(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null) return false;

            _context.Contratos.Remove(contrato);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Service>> GetServicosByClienteIdAsync(int clienteId)
        {
            return await _context.Contratos
                .Where(c => c.ClienteId == clienteId)
                .Include(c => c.Servico)
                .Select(c => c.Servico)
                .ToListAsync();
        }
    }
}
