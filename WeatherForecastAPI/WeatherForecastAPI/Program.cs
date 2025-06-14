using Microsoft.EntityFrameworkCore;
using WeatherForecastAPI.Data;
using WeatherForecastAPI.Repository;
using WeatherForecastAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();

// Add EF Core (SQLite or InMemory)
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();