// --- Archivo: Repositories/ProductRepository.cs ---

using Microsoft.EntityFrameworkCore;
using MiTienda.Application.Interfaces;
using MiTienda.Domain.Entities;
using MiTienda.Infrastructure.Persistence;

namespace MiTienda.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación concreta de IProductRepository usando Entity Framework Core.
    /// Esta clase sabe cómo hablar con una base de datos a través del DbContext.
    /// La lógica de la aplicación no sabe nada de esta clase, solo conoce la interfaz.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        // Usamos inyección de dependencias para recibir una instancia del DbContext.
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            // Nota: EF Core agrupa los cambios. Para que se guarden en la BD,
            // necesitaríamos un método "Unit of Work" que llame a _context.SaveChangesAsync().
            // Lo veremos más adelante.
        }
    }
}