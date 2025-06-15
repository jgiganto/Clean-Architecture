// --- Proyecto: MiTienda.Api ---
// --- Archivo: Program.cs (Versión Final con MediatR) ---

using MediatR; // <-- Asegúrate de que este using está presente
using MiTienda.Application.Interfaces;
using MiTienda.Domain.Entities;
using MiTienda.Infrastructure.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);

// --- SECCIÓN DE CONFIGURACIÓN DE MONGODB ---
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));


// --- SECCIÓN DE INYECCIÓN DE DEPENDENCIAS (DI) ---

// Añadir la configuración para poder leer appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// 1. Registrar la implementación del Repositorio
builder.Services.AddSingleton<IProductRepository, MongoProductRepository>();

// 2. ¡Registrar MediatR!
// Esta única línea escanea todo el ensamblado de 'Application' en busca de
// cualquier clase que implemente las interfaces de MediatR (IRequestHandler, etc.)
// y las registra automáticamente en el contenedor de DI.
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(MiTienda.Application.Features.Products.Commands.CreateProduct.CreateProductCommandHandler).Assembly));

// ¡Ya no necesitamos registrar los handlers uno por uno!
// builder.Services.AddScoped<CreateProductCommandHandler>(); // <-- Esta línea se elimina o comenta.


// --- FIN DE LA SECCIÓN DE DI ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // --- Añadir datos de prueba (usando MediatR para ser consistentes) ---
    using (var scope = app.Services.CreateScope())
    {
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

        // Comprobamos si ya hay datos para no insertarlos cada vez que arranca.
        if (!(await productRepository.GetAllAsync()).Any())
        {
            await mediator.Send(new MiTienda.Application.Features.Products.Commands.CreateProduct.CreateProductCommand { Name = "Laptop Pro (Mongo)", Price = 1250.50m, Stock = 50 });
            await mediator.Send(new MiTienda.Application.Features.Products.Commands.CreateProduct.CreateProductCommand { Name = "Mouse Inalámbrico (Mongo)", Price = 30.00m, Stock = 200 });
            await mediator.Send(new MiTienda.Application.Features.Products.Commands.CreateProduct.CreateProductCommand { Name = "Teclado Mecánico RGB (Mongo)", Price = 155.75m, Stock = 100 });
        }
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
