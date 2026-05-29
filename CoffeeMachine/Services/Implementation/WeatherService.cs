using CoffeeMachine.Model;
using CoffeeMachine.Services.Interface;

namespace CoffeeMachine.Services.Implementation
{
    public class WeatherService: IWeatherService

    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<double?> GetCurrentTemperatureAsync()
        {
            var apiKey = _configuration["Weather:ApiKey"];
            var city = _configuration["Weather:City"];
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units = metric";
            
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;
            var json = await response.Content.ReadFromJsonAsync<WeatherResponse>();
            return json?.Main?.Temp;
        }

    }
}
