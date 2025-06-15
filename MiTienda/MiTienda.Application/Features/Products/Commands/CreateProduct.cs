// --- Proyecto: MiTienda.Application ---
// --- Archivo: Features/Products/Commands/CreateProduct/CreateProductCommandHandler.cs (MODIFICADO) ---
// Aquí es donde van las REGLAS DE APLICACIÓN.

using MiTienda.Application.Interfaces;
using MiTienda.Domain.Entities;
using MediatR;

namespace MiTienda.Application.Features.Products.Commands.CreateProduct
{
    /// <summary>
    /// Ahora el comando implementa IRequest<TResponse>.
    /// IRequest<Guid> significa: "Soy una solicitud que espera una respuesta de tipo Guid".
    /// </summary>
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
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
                Name = request.Name,
                Price = request.Price, // ¡La validación se ejecuta aquí!
                Stock = request.Stock
            };

            await _productRepository.AddAsync(product);

            return product.Id;
        }
    }
}