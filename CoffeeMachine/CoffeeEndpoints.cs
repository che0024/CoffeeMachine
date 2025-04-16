using Microsoft.AspNetCore.Http.HttpResults;

namespace CoffeeMachine;

public static class CoffeeEndpoints
{
    public static void MapCoffeeEndpoints(this WebApplication app)
    {
        app.MapGet("/brew-coffee", BrewCoffee);
    }

    private static Results<Ok<Coffee>, StatusCodeHttpResult> BrewCoffee(ICoffeeService coffeeService)
    {
        return coffeeService.BrewCoffee();
    }
}
