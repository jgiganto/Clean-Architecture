// --- Proyecto: MiTienda.Application ---
// --- Archivo: Features/Products/Commands/CreateProduct/CreateProductCommandHandler.cs (MODIFICADO) ---
// Aquí es donde van las REGLAS DE APLICACIÓN.

using MiTienda.Application.Interfaces;
using MiTienda.Domain.Entities;

namespace MiTienda.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    public class CreateProductCommandHandler
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Guid> Handle(CreateProductCommand command)
        {
            // REGLA DE APLICACIÓN: Comprobar si ya existe un producto con el mismo nombre.
            // Esta regla necesita dependencias externas (el repositorio), por eso va aquí.
            // var productExists = await _productRepository.GetByNameAsync(command.Name);
            // if (productExists != null)
            // {
            //     throw new Exception($"Ya existe un producto con el nombre '{command.Name}'.");
            // }

            // Creamos la entidad. Si el precio en el 'command' es <= 0,
            // el setter de la entidad Product lanzará la excepción que definimos.
            // El caso de uso no necesita repetir la validación del precio.
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Price = command.Price, // ¡La validación se ejecuta aquí!
                Stock = command.Stock
            };

            await _productRepository.AddAsync(product);

            return product.Id;
        }
    }
}