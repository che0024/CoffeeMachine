
using Microsoft.AspNetCore.Http.HttpResults;
using static CoffeeMachine.Program;
using CoffeeMachine;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TestProject1;

public class CoffeeServiceTest
{
    private readonly Mock<TimeProvider> timeProviderMock = new();
    private readonly Mock<IWeatherService> weatherServiceMock = new();
    private readonly CoffeeService service;

    public CoffeeServiceTest()
    {
        service = new CoffeeService(timeProviderMock.Object, weatherServiceMock.Object);
    }

    [Fact]
    public void BrewCoffee_NormalConditions()
    {
        //Arrange
        var testDateTime = new DateTimeOffset(2025, 3, 1, 10, 0, 0, TimeSpan.Zero);
        timeProviderMock.Setup(t => t.GetUtcNow()).Returns(testDateTime);
        weatherServiceMock.Setup(w => w.IsAboveThirty()).Returns(false);

        //Act
        var result = service.BrewCoffee();

        //Assert
        var okResult = Assert.IsType<Ok<Coffee>>(result.GetType().GetProperty("Result")?.GetValue(result));
        Assert.Equal("Your piping hot coffee is ready", okResult.Value?.Message);
        Assert.Equal(testDateTime.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"), okResult.Value?.Prepared);
    }

    [Fact]
    public void BrewCoffee_AprilFools()
    {
        //Arrange
        var testDateTime = new DateTimeOffset(2025, 4, 1, 10, 0, 0, TimeSpan.Zero);
        timeProviderMock.Setup(t => t.GetUtcNow()).Returns(testDateTime);
        weatherServiceMock.Setup(w => w.IsAboveThirty()).Returns(false);

        //Act
        var result = service.BrewCoffee();

        //Assert
        var okResult = Assert.IsType<StatusCodeHttpResult>(result.GetType().GetProperty("Result")?.GetValue(result));
        Assert.Equal(418, okResult.StatusCode);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(15)]
    public void BrewCoffee_EveryFifthCall(int count)
    {
        //Arrange
        var testDateTime = new DateTimeOffset(2025, 5, 1, 10, 0, 0, TimeSpan.Zero);
        timeProviderMock.Setup(t => t.GetUtcNow()).Returns(testDateTime);
        weatherServiceMock.Setup(w => w.IsAboveThirty()).Returns(false);

        //Act
        for (int i = 0; i < count - 1; i++)
        {
            service.BrewCoffee();
        }

        var result = service.BrewCoffee();

        //Assert
        var okResult = Assert.IsType<StatusCodeHttpResult>(result.GetType().GetProperty("Result")?.GetValue(result));
        Assert.Equal(503, okResult.StatusCode);
    }

    [Fact]
    public void BrewCoffee_AboveThirty()
    {
        //Arrange
        var testDateTime = new DateTimeOffset(2025, 3, 1, 10, 0, 0, TimeSpan.Zero);
        timeProviderMock.Setup(t => t.GetUtcNow()).Returns(testDateTime);
        weatherServiceMock.Setup(w => w.IsAboveThirty()).Returns(true);

        //Act
        var result = service.BrewCoffee();

        //Assert
        var okResult = Assert.IsType<Ok<Coffee>>(result.GetType().GetProperty("Result")?.GetValue(result));
        Assert.Equal("Your refreshing iced coffee is ready", okResult.Value?.Message);
        Assert.Equal(testDateTime.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"), okResult.Value?.Prepared);
    }
}