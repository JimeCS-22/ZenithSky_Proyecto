using Microsoft.EntityFrameworkCore;
using ZenithSky_Proyecto.Common;
using ZenithSky_Proyecto.Data;
using ZenithSky_Proyecto.Services;

var builder = WebApplication.CreateBuilder(args);

// Data
builder.Services.AddDbContext<ZenithSkyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ZenithSky")));

// Business / Services
builder.Services.AddScoped<IAirportService, AirportService>();
builder.Services.AddScoped<IAirlineService, AirlineService>();
builder.Services.AddScoped<IAirplaneService, AirplaneService>();
builder.Services.AddScoped<IAirplaneTypeService, AirplaneTypeService>();

// Controllers + manejo global de errores 
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapControllers();

app.Run();