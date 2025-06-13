// --- Proyecto: MiTienda.Application ---
// --- Capa: Casos de Uso (Lógica de Aplicación) ---
// Este proyecto orquesta el flujo de datos y contiene la lógica de negocio específica.
// Define las interfaces que la capa de Infraestructura deberá implementar.

using MiTienda.Domain.Entities;

namespace MiTienda.Application.Interfaces
{
    /// <summary>
    /// Define el contrato para las operaciones de datos relacionadas con los productos.
    /// Esta interfaz vive en la capa de Aplicación porque son los casos de uso
    /// (la lógica de la aplicación) quienes dictan qué operaciones de datos se necesitan.
    /// La capa de Infraestructura PROVEERÁ una implementación de este contrato.
    /// Esto es el Principio de Inversión de Dependencias (la 'D' de SOLID) en acción.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Obtiene un producto por su identificador único.
        /// </summary>
        /// <param name="id">El Guid del producto.</param>
        /// <returns>El producto encontrado o null si no existe.</returns>
        Task<Product?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene una lista de todos los productos.
        /// </summary>
        /// <returns>Una colección de solo lectura con todos los productos.</returns>
        Task<IReadOnlyList<Product>> GetAllAsync();

        /// <summary>
        /// Agrega un nuevo producto a la persistencia.
        /// </summary>
        /// <param name="product">La entidad del producto a agregar.</param>
        /// <returns>Tarea que se completa cuando la operación finaliza.</returns>
        Task AddAsync(Product product);

        // Podríamos agregar más métodos como UpdateAsync, DeleteAsync, etc.
    }
}
