// --- Proyecto: MiTienda.Infrastructure ---
// --- Capa: Frameworks y Drivers (Detalles de Implementación) ---
// Este proyecto implementa las interfaces definidas en la capa de Application.
// Contiene el "CÓMO" se hacen las cosas (ej: cómo se accede a la base de datos).

// --- Archivo: Persistence/ApplicationDbContext.cs ---

using Microsoft.EntityFrameworkCore;
using MiTienda.Domain.Entities;
using System.Reflection;

namespace MiTienda.Infrastructure.Persistence
{
    /// <summary>
    /// Representa la sesión con la base de datos. Hereda de DbContext de EF Core.
    /// Es el puente principal entre nuestras entidades de dominio y la base de datos.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Declaramos un DbSet por cada entidad de dominio que queremos persistir.
        // EF Core usará esto para crear las tablas en la base de datos.
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Aquí podríamos configurar el modelo de datos con más detalle usando Fluent API.
            // Por ejemplo, definir longitudes máximas para las cadenas, índices, etc.
            // builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}