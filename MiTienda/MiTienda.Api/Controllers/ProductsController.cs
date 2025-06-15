// --- PASO 2: DEJAR EL CONTROLLER COMPLETAMENTE LIMPIO ---
// --- Proyecto: MiTienda.Api ---
// --- Archivo: Controllers/ProductsController.cs (VERSIÓN FINAL) ---

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiTienda.Application.Features.Products.Commands.CreateProduct;
using MiTienda.Application.Features.Products.Queries.GetAllProducts; // <-- Añadir using
using MiTienda.Application.Features.Products.Queries.GetProductById; // <-- Añadir using
using MiTienda.Domain.Entities; // <-- Este using sigue siendo necesario para los tipos de retorno

namespace MiTienda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator; // ¡Nuestra ÚNICA dependencia!

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtiene una lista de todos los productos.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            // Creamos y enviamos la consulta a MediatR.
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        /// <summary>
        /// Obtiene un producto por su ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            // Creamos y enviamos la consulta con su ID.
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            var productId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = productId }, command);
        }
    }
}