using loja.data;
using loja.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace loja.services
{
    public class ClientService
    {
        private readonly LojaDbContext _context;

        public ClientService(LojaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientsAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente> GetClientByIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task AddClientAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateClientAsync(int id, Cliente cliente)
        {
            var existingClient = await _context.Clientes.FindAsync(id);
            if (existingClient == null) return false;

            existingClient.Nome = cliente.Nome;
            existingClient.Cpf = cliente.Cpf;
            existingClient.Email = cliente.Email;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var client = await _context.Clientes.FindAsync(id);
            if (client == null) return false;

            _context.Clientes.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
