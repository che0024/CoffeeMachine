namespace CoffeeMachine
{
    public interface IWeatherService 
    {
        public bool IsAboveThirty(DateTimeOffset dateTime);
    }

    public class WeatherService : IWeatherService
    {
        public bool IsAboveThirty(DateTimeOffset dateTime)
        {
            var apiUrl = $"api.openweathermap.org/data/2.5/weather?q=Melbourne&units=metric&APPID=e6a69c0059eee75f1465f88c4821b3d0";
            // Simulate a weather check
            return true; // or false based on actual weather data
        }
    }
}
