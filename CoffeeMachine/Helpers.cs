using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoffeeMachine
{
    public interface IWeatherService 
    {
        public bool IsAboveThirty();
    }

    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherService (IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public bool IsAboveThirty()
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q=Melbourne&units=metric&APPID=e6a69c0059eee75f1465f88c4821b3d0";

                var task = client.GetStringAsync(apiUrl);
                task.Wait();
                var response = task.Result;                

                var result = JObject.Parse(response);

                decimal temperature = 0;

                if (result["main"] != null && result["main"]["temp"] != null)
                {
                    temperature = decimal.Parse(result["main"]["temp"].ToString());
                }

                if (temperature > 30)
                    return true;

                return false; 
            }
        }
    }
}
