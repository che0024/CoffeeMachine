using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace CoffeeMachine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            //
            builder.Services.AddSingleton(TimeProvider.System);
            builder.Services.AddScoped<ICoffeeService, CoffeeService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/brew-coffee", new ICoffeeService().BrewCoffee());

            app.Run();
        }

        public interface ICoffeeService
        {
            Results<Ok<Coffee>, StatusCodeHttpResult> BrewCoffee();
        }

        public class CoffeeService : ICoffeeService
        {
            private readonly TimeProvider timeProvider;
            public int counter = 0;

            public CoffeeService(TimeProvider timeProvider)
            {
                this.timeProvider = timeProvider;
            }

            public Results<Ok<Coffee>, StatusCodeHttpResult> BrewCoffee() {
                counter++;

                var dateTimeUtc = timeProvider.GetUtcNow();
                var dateTimeLocal = dateTimeUtc.ToLocalTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture);

                if (dateTimeUtc.Month.Equals(4) && dateTimeUtc.Day.Equals(1))
                {
                    return TypedResults.StatusCode(418);
                }

                if (counter % 5 == 0)
                {
                    return TypedResults.StatusCode(503);
                }

                var coffee = new Coffee()
                {
                    Message = "Your Piping hot coffee is ready",
                    Prepared = dateTimeLocal
                };

                return TypedResults.Ok(coffee);
            }
        }
    }
}
