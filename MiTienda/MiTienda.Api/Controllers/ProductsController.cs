// --- Archivo: Controllers/ProductsController.cs ---

using Microsoft.AspNetCore.Mvc;
using MiTienda.Application.Interfaces;
using MiTienda.Domain.Entities;

namespace MiTienda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // La ruta será /api/products
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        // ¡Magia! El controlador no sabe que existe 'ProductRepository' o EF Core.
        // Solo pide la interfaz 'IProductRepository' que definimos en la capa de Application.
        // El sistema de DI se encarga de inyectar la implementación correcta que registramos en Program.cs.
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Obtiene una lista de todos los productos.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }
    }
}