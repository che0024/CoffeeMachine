
using Microsoft.AspNetCore.Http.HttpResults;
using static CoffeeMachine.Program;
using CoffeeMachine;


namespace TestProject1
{
    public class UnitTest1
    {
        private readonly Mock<TimeProvider> timeProviderMock = new();
        [Fact]
        public void Test1()
        {
            //Arrange
            var testDateTime = DateTimeOffset.UtcNow;
            timeProviderMock.Setup(t => t.GetUtcNow()).Returns(testDateTime);

            var coffeeService = new CoffeeService(timeProviderMock.Object);

            //Act
            var result = coffeeService.BrewCoffee();

            //Assert
            var okResult = Assert.IsType<Ok<Coffee>>(result);
            Assert.Equal("Your Piping hot coffee is ready", okResult.Value?.Message);
            Assert.Equal(testDateTime.ToLocalTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture), okResult.Value?.Prepared);
        }
    }
}