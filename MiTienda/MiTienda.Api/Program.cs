// --- Proyecto: MiTienda.Api ---
// --- Archivo: Program.cs (MODIFICADO) ---

using Microsoft.EntityFrameworkCore;
using MiTienda.Application.Interfaces;
using MiTienda.Domain.Entities;
using MiTienda.Infrastructure.Persistence;
using MiTienda.Infrastructure.Repositories;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson; 
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// --- SECCIÓN DE CONFIGURACIÓN DE MONGODB ---
// ¡LA CORRECCIÓN ESTÁ AQUÍ!
// Le decimos al driver de MongoDB cómo tratar los Guids globalmente.
// Esto evita contaminar nuestro Dominio con atributos de persistencia.
// Usamos la representación estándar (un tipo binario) que es más eficiente.
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

// --- SECCIÓN DE INYECCIÓN DE DEPENDENCIAS (DI) ---

// Añadir la configuración para poder leer appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

/*
// --- IMPLEMENTACIÓN ANTIGUA (EF CORE IN-MEMORY) ---
// Comentamos o eliminamos estas líneas.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("MiTiendaDb"));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
*/


// --- ¡EL CAMBIO! NUEVA IMPLEMENTACIÓN (MONGODB) ---
// Simplemente cambiamos la clase que implementa la interfaz.
// El resto de la aplicación (Controladores, Casos de Uso) no se entera de este cambio.
builder.Services.AddSingleton<IProductRepository, MongoProductRepository>();
// Usamos AddSingleton aquí porque el driver de Mongo gestiona el pooling de conexiones eficientemente.


// --- FIN DE LA SECCIÓN DE DI ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // --- Añadir datos de prueba para MongoDB ---
    var productRepository = app.Services.GetRequiredService<IProductRepository>();
    if (!(await productRepository.GetAllAsync()).Any())
    {
        await productRepository.AddAsync(new Product { Id = Guid.NewGuid(), Name = "Laptop Pro (Mongo)", Price = 1250.50m, Stock = 50 });
        await productRepository.AddAsync(new Product { Id = Guid.NewGuid(), Name = "Mouse Inalámbrico (Mongo)", Price = 30.00m, Stock = 200 });
        await productRepository.AddAsync(new Product { Id = Guid.NewGuid(), Name = "Teclado Mecánico RGB (Mongo)", Price = 155.75m, Stock = 100 });
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();