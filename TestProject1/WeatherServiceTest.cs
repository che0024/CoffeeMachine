
using Microsoft.AspNetCore.Http.HttpResults;
using static CoffeeMachine.Program;
using CoffeeMachine;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TestProject1;

public class WeatherServiceTest
{
    private readonly WeatherService service;

    public WeatherServiceTest()
    {
        service = new WeatherService();
    }

    [Fact]
    public void IsAboveThirty()
    {
        //Arrange
        var testDateTime = new DateTimeOffset(2025, 1, 1, 10, 0, 0, TimeSpan.Zero);

        //Act
        var result = service.IsAboveThirty(testDateTime);

        //Assert
        var okResult = Assert.IsType<bool>(result);
        Assert.True(okResult);
    }

    [Fact]
    public void IsBelowThirty()
    {
        //Arrange
        var testDateTime = new DateTimeOffset(2024, 6, 1, 10, 0, 0, TimeSpan.Zero);

        //Act
        var result = service.IsAboveThirty(testDateTime);

        //Assert
        var okResult = Assert.IsType<Ok<Coffee>>(result.GetType().GetProperty("Result")?.GetValue(result));
        Assert.Equal("Your piping hot coffee is ready", okResult.Value?.Message);
        Assert.Equal(testDateTime.ToLocalTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture), okResult.Value?.Prepared);
    }
}