// --- Proyecto: MiTienda.Domain ---
// --- Capa: Entidades (El núcleo) ---
// Este proyecto contiene las reglas de negocio más fundamentales y de más alto nivel.
// No depende de NINGÚN otro proyecto en la solución. Es completamente puro.

namespace MiTienda.Domain.Entities
{
    /// <summary>
    /// Representa un producto en nuestro catálogo.
    /// Esta es una entidad de dominio. Observe que es un POCO (Plain Old CLR Object).
    /// No tiene referencias a Entity Framework, JSON.NET, o cualquier otra librería externa.
    /// Solo contiene datos y, potencialmente, lógica de negocio pura (ej: un método para cambiar el precio).
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del producto.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descripción detallada del producto.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Precio del producto. Usamos decimal para mayor precisión monetaria.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Cantidad de unidades disponibles en stock.
        /// </summary>
        public int Stock { get; set; }
    }
}
