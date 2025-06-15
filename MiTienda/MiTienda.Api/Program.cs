// --- Proyecto: MiTienda.Api ---
// --- Archivo: Program.cs (Modificado para usar Serilog) ---

using MediatR;
using MiTienda.Application.Common.Behaviors; // Aseg�rate de tener este using para el LoggingBehavior
using MiTienda.Application.Interfaces;
using MiTienda.Infrastructure.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Serilog; // <-- A�ADIR ESTE USING

// --- PASO 1: CONFIGURAR SERILOG ---
// Creamos un logger de "bootstrap" para poder registrar cualquier error que ocurra DURANTE el inicio de la aplicaci�n.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Iniciando la aplicaci�n MiTienda...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // --- PASO 2: USAR SERILOG COMO EL LOGGER DE LA APLICACI�N ---
    // Limpiamos los proveedores de logging por defecto y le decimos a la aplicaci�n que use Serilog.
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration) // Lee la configuraci�n desde appsettings.json (opcional)
        .ReadFrom.Services(services) // Permite inyectar servicios en los Sinks
        .Enrich.FromLogContext() // A�ade contexto a los logs
        .WriteTo.Console( // Le decimos que escriba en la consola
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")); // �NUESTRA PLANTILLA PERSONALIZADA!


    // --- SECCI�N DE CONFIGURACI�N DE MONGODB ---
    BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));


    // --- SECCI�N DE INYECCI�N DE DEPENDENCIAS (DI) ---
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    builder.Services.AddSingleton<IProductRepository, MongoProductRepository>();

    // Registrar el Pipeline Behavior de MediatR
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

    // Registrar MediatR
    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(MiTienda.Application.Features.Products.Commands.CreateProduct.CreateProductCommandHandler).Assembly));


    // --- FIN DE LA SECCI�N DE DI ---

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Usar el request logging de Serilog (opcional pero muy �til)
    // Esto crea logs autom�ticos para cada petici�n HTTP que llega.
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        // ... (Tu c�digo para a�adir datos de prueba)
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicaci�n ha fallado al iniciar.");
}
finally
{
    Log.CloseAndFlush();
}
