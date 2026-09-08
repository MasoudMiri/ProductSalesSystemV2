using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interfaces;
using MyProject.Application.Mappings;
using MyProject.Application.Services;
using MyProject.Persistence.Context;
using MyProject.Persistence.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ====== تنظیمات Serilog ======
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ====== AutoMapper ======
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ====== DbContext ======
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ====== Dependency Injection ======
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Application Starting Up");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application Start-up Failed");
}
finally
{
    Log.CloseAndFlush();
}