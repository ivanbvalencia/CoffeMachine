using CoffeeMachine.Infrastructure.Interface;
using CoffeeMachine.Model;
using CoffeeMachine.Services.Interface;

namespace CoffeeMachine.Services.Implementation
{
    public class CoffeeService : ICoffeeService
    {
        private readonly IRequestCounter _counter;
        private readonly IWeatherService _weatherService;

        public CoffeeService(IRequestCounter counter, IWeatherService weatherService)
        {
            _counter = counter;
            _weatherService = weatherService;
        }
        public async Task<IResult>  BrewCoffeeAsync()
        {
            var today = DateTime.Now;
            // Requirement #3
            if (today.Month == 4 && today.Day == 1)
            {
                return Results.StatusCode(418);
            }
            // Requirement #2
            var requestCount = _counter.IncrementAndGet();
            if (requestCount % 5 == 0)
            {
                return Results.StatusCode(503);
            }
            var message = "Your piping hot coffee is ready";

            // Extra credit
            var temperature = await _weatherService.GetCurrentTemperatureAsync();
            if (temperature > 30)
            {
                message = "Your refreshing iced coffee is ready";
            }
            var response = new CoffeeResponse
            {
                Message = message,
                Prepared = DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")
            };
            return Results.Ok(response);


        }
    }
}
