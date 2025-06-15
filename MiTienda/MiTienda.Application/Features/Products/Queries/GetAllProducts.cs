using MediatR;
using MiTienda.Application.Interfaces;
using MiTienda.Domain.Entities;

namespace MiTienda.Application.Features.Products.Queries.GetAllProducts
{
    // Esta es la solicitud para obtener todos los productos.
    // No necesita propiedades. Espera una lista de productos como respuesta.
    public class GetAllProductsQuery : IRequest<IReadOnlyList<Product>> { }

    // Este es el manejador para la consulta.
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IReadOnlyList<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IReadOnlyList<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // La lógica es la misma, pero ahora está encapsulada en su propio handler.
            return await _productRepository.GetAllAsync();
        }
    }
}
