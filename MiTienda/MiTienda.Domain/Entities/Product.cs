// --- Proyecto: MiTienda.Domain ---
// --- Archivo: Entities/Product.cs (MODIFICADO) ---
// Aquí es donde van las REGLAS DE DOMINIO (invariantes).

namespace MiTienda.Domain.Entities
{
    public class Product
    {
        private decimal _price; // Campo privado para almacenar el precio

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Propiedad pública para el precio.
        /// La lógica de validación está encapsulada aquí dentro.
        /// </summary>
        public decimal Price
        {
            get => _price;
            set
            {
                // REGLA DE DOMINIO: El precio no puede ser negativo o cero.
                // Esta regla protege a la entidad de estar en un estado inválido.
                if (value <= 0)
                {
                    // Lanzamos una excepción específica del dominio.
                    throw new ArgumentOutOfRangeException(nameof(Price), "El precio debe ser un valor positivo.");
                }
                _price = value;
            }
        }

        public int Stock { get; set; }

        // Podríamos usar un constructor para forzar la creación de una entidad válida desde el principio.
        public Product(Guid id, string name, decimal price, int stock)
        {
            Id = id;
            Name = name;
            Price = price; // La validación del setter se ejecutará aquí.
            Stock = stock;
        }

        // Constructor sin parámetros necesario para algunos frameworks como EF Core
        public Product() { }
    }
}