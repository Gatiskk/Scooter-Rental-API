using Microsoft.EntityFrameworkCore;
using ScooterRental.Core.Interfaces;
using ScooterRental.Core.Models;
using ScooterRental.Core.Models.Validations;
using ScooterRental.Core.Services;
using ScooterRental.Data.Data;
using ScooterRental.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<IEntityService<Scooter>, EntityService<Scooter>>();
builder.Services.AddScoped<IRentedScooterService, RentedScooterService>();
builder.Services.AddScoped<IScooterValidator, ScooterPriceValidator>();
builder.Services.AddScoped<IScooterValidator, ScooterIdValidator>();

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

app.Run();
