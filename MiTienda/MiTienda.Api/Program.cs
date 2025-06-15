// --- Proyecto: MiTienda.Api ---
// --- Archivo: Program.cs (Modificado para usar Serilog) ---

using MediatR;
using MiTienda.Application.Common.Behaviors; // Asegúrate de tener este using para el LoggingBehavior
using MiTienda.Application.Interfaces;
using MiTienda.Infrastructure.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Serilog; // <-- AÑADIR ESTE USING

// --- PASO 1: CONFIGURAR SERILOG ---
// Creamos un logger de "bootstrap" para poder registrar cualquier error que ocurra DURANTE el inicio de la aplicación.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Iniciando la aplicación MiTienda...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // --- PASO 2: USAR SERILOG COMO EL LOGGER DE LA APLICACIÓN ---
    // Limpiamos los proveedores de logging por defecto y le decimos a la aplicación que use Serilog.
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration) // Lee la configuración desde appsettings.json (opcional)
        .ReadFrom.Services(services) // Permite inyectar servicios en los Sinks
        .Enrich.FromLogContext() // Añade contexto a los logs
        .WriteTo.Console( // Le decimos que escriba en la consola
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")); // ¡NUESTRA PLANTILLA PERSONALIZADA!


    // --- SECCIÓN DE CONFIGURACIÓN DE MONGODB ---
    BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));


    // --- SECCIÓN DE INYECCIÓN DE DEPENDENCIAS (DI) ---
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    builder.Services.AddSingleton<IProductRepository, MongoProductRepository>();

    // Registrar el Pipeline Behavior de MediatR
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

    // Registrar MediatR
    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(MiTienda.Application.Features.Products.Commands.CreateProduct.CreateProductCommandHandler).Assembly));


    // --- FIN DE LA SECCIÓN DE DI ---

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Usar el request logging de Serilog (opcional pero muy útil)
    // Esto crea logs automáticos para cada petición HTTP que llega.
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        // ... (Tu código para añadir datos de prueba)
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación ha fallado al iniciar.");
}
finally
{
    Log.CloseAndFlush();
}
