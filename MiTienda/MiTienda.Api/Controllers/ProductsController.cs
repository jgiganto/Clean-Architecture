// --- Archivo: Controllers/ProductsController.cs ---

using Microsoft.AspNetCore.Mvc;
using MiTienda.Application.Features.Products.Commands.CreateProduct;
using MiTienda.Application.Interfaces;
using MiTienda.Domain.Entities;

namespace MiTienda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // La ruta será /api/products
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly CreateProductCommandHandler _createProductHandler;

        // ¡Magia! El controlador no sabe que existe 'ProductRepository' o EF Core.
        // Solo pide la interfaz 'IProductRepository' que definimos en la capa de Application.
        // El sistema de DI se encarga de inyectar la implementación correcta que registramos en Program.cs.
        public ProductsController(IProductRepository productRepository, CreateProductCommandHandler createProductHandler)
        {
            _productRepository = productRepository;
            _createProductHandler = createProductHandler;
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

        /// <summary>
        /// Obtiene un producto por su ID.
        /// ESTE ES EL MÉTODO QUE FALTABA.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(); // Devuelve un 404 si no se encuentra
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            // Inyectaríamos el CreateProductCommandHandler en el constructor del controlador.
            var handler = new CreateProductCommandHandler(_productRepository); // Simplificado

            var productId = await handler.Handle(command);

            return CreatedAtAction(nameof(GetProductById), new { id = productId }, null);
        }
    }
}