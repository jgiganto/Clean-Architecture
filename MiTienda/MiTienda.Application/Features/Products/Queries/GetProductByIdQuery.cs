// --- Archivo: Features/Products/Queries/GetProductByIdQuery.cs ---
using MediatR;
using MiTienda.Application.Interfaces;
using MiTienda.Domain.Entities;

namespace MiTienda.Application.Features.Products.Queries.GetProductById
{
    // Esta es la solicitud para obtener un producto por su ID.
    // Necesita una propiedad para llevar el ID. Espera un único producto (que puede ser null).
    public class GetProductByIdQuery : IRequest<Product?>
    {
        public Guid Id { get; set; }
    }

    // Este es el manejador para la consulta.ssssss
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetByIdAsync(request.Id);
        }
    }
}