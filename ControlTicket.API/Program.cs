using ControlTicket.Application;
using ControlTicket.Infrastructure;
using ControlTicket.Infrastructure.Logging;
using ControlTicket.Infrastructure.Persistence;
using Serilog;
using Pomelo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
// Bootstrap logger — captures logs during startup before full config is ready
SerilogConfiguration.CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
    var jwtAudience = builder.Configuration.GetSection("Jwt:Audicence").Get<string>() ?? "";
    var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>(); 
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
    //var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    // Configure Serilog from appsettings.json
    builder.Host.AddSerilog();

    // Add services to the container.
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, serverVersion)
                                                                        .LogTo(Console.WriteLine, LogLevel.Warning));


   var app = builder.Build();

    // Structured HTTP request logging
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
