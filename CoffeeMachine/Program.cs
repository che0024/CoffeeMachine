using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace CoffeeMachine;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        //
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddScoped<IWeatherService, WeatherService>();
        builder.Services.AddScoped<ICoffeeService, CoffeeService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapCoffeeEndpoints();

        app.Run();
    }
}

public interface ICoffeeService
{
    Results<Ok<Coffee>, StatusCodeHttpResult> BrewCoffee();
}

public class CoffeeService : ICoffeeService
{
    private readonly TimeProvider _timeProvider;
    private readonly IWeatherService _weatherService;
    private int _counter = 0;

    public CoffeeService(TimeProvider timeProvider, IWeatherService weatherService)
    {
        _timeProvider = timeProvider;
        _weatherService = weatherService;
    }

    public Results<Ok<Coffee>, StatusCodeHttpResult> BrewCoffee()
    {
        _counter++;

        var dateTimeUtc = _timeProvider.GetUtcNow();
        var dateTimeLocal = dateTimeUtc.ToLocalTime();

        if (dateTimeUtc.Month.Equals(4) && dateTimeUtc.Day.Equals(1))
        {
            return TypedResults.StatusCode(418);
        }

        if (_counter % 5 == 0)
        {
            return TypedResults.StatusCode(503);
        }

        var coffee = new Coffee()
        {
            Message = _weatherService.IsAboveThirty() ? "Your refreshing iced coffee is ready" : "Your piping hot coffee is ready",
            Prepared = dateTimeLocal.ToString("s", System.Globalization.CultureInfo.InvariantCulture)
        };

        return TypedResults.Ok(coffee);
    }
}

