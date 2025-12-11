using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Ardalis.GuardClauses;
using Web.Infrastructure.Data;
using System.Reflection;
using FluentValidation;
using Web.Application.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("JAAppDb");
Guard.Against.Null(connectionString, message: "Connection string 'JAAppDb' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(configuration =>
{
    configuration.UseNpgsql(connectionString);
});

builder.Services.AddAutoMapper(configuration => 
    configuration.AddMaps(Assembly.GetExecutingAssembly()));

builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<ApplicationDbContextInitialiser>();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api", (options) =>
    {
        options.Title = "Just another app API";
        options.Layout = ScalarLayout.Modern;
        options.HideClientButton = true;
    });
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
