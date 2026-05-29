namespace CoffeeMachine.Services.Interface
{
    public interface IWeatherService
    {
        Task<double?> GetCurrentTemperatureAsync();
    }
}
