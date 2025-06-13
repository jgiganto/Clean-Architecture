// --- Proyecto: MiTienda.Infrastructure ---
// --- Archivo: Repositories/MongoProductRepository.cs (¡NUEVO ARCHIVO!) ---

using Microsoft.Extensions.Configuration;
using MiTienda.Application.Interfaces;
using MiTienda.Domain.Entities;
using MongoDB.Driver;

namespace MiTienda.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de IProductRepository específica para MongoDB.
    /// Esta clase sabe cómo hablar con una base de datos MongoDB.
    /// </summary>
    public class MongoProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _productsCollection;

        public MongoProductRepository(IConfiguration configuration)
        {
            // Obtenemos la cadena de conexión desde la configuración (appsettings.json)
            var connectionString = configuration.GetConnectionString("MongoDbConnection");
            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase("MiTiendaMongoDb");

            _productsCollection = mongoDatabase.GetCollection<Product>("Products");
        }

        public async Task AddAsync(Product product)
        {
            await _productsCollection.InsertOneAsync(product);
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await _productsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}