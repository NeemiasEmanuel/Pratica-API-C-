using loja.data;
using loja.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace loja.services
{
    public class ServiceService
    {
        private readonly LojaDbContext _context;

        public ServiceService(LojaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<Service> GetServiceByIdAsync(int id)
        {
            return await _context.Services.FindAsync(id);
        }

        public async Task AddServiceAsync(Service Service)
        {
            _context.Services.Add(Service);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateServiceAsync(int id, Service Service)
        {
            var existingService = await _context.Services.FindAsync(id);
            if (existingService == null) return false;

            existingService.Nome = Service.Nome;
            existingService.Preco = Service.Preco;
            existingService.Status = Service.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
