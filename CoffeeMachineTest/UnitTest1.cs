using CoffeeMachine.Infrastructure.Interface;
using CoffeeMachine.Model;
using CoffeeMachine.Services.Implementation;
using CoffeeMachine.Services.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CoffeeMachineTest
{
    public class CoffeeServiceTests
    {
        private static (CoffeeService service, Mock<IRequestCounter> counter, Mock<IWeatherService> weather) BuildService()
        {
            var counter = new Mock<IRequestCounter>();
            var weather = new Mock<IWeatherService>();
            var service = new CoffeeService(counter.Object, weather.Object);
            return (service, counter, weather);
        }

        [Fact]
        public async Task Should_Return_418_On_April_Fools_Day()
        {
            var (service, _, _) = BuildService();
            var result = await service.BrewCoffeeAsync();
            Assert.IsType<StatusCodeHttpResult>(result);
        }

        [Fact]
        public async Task Should_Return_200_On_Normal_Day()
        {
            var (service, counter, weather) = BuildService();
            counter.Setup(x => x.IncrementAndGet()).Returns(1);
            weather.Setup(x => x.GetCurrentTemperatureAsync()).ReturnsAsync(25);

            var result = await service.BrewCoffeeAsync();
            Assert.IsType<Ok<CoffeeResponse>>(result);
        }

        [Fact]
        public async Task Should_Return_Iced_Coffee_Message_When_Temperature_Above_30()
        {
            var (service, counter, weather) = BuildService();
            counter.Setup(x => x.IncrementAndGet()).Returns(1);
            weather.Setup(x => x.GetCurrentTemperatureAsync()).ReturnsAsync(35);

            var result = await service.BrewCoffeeAsync();
            var okResult = Assert.IsType<Ok<CoffeeResponse>>(result);
            var response = okResult.Value;
            Assert.Equal("Your refreshing iced coffee is ready", response?.Message);
        }

        [Fact]
        public async Task Should_Return_Hot_Coffee_Message_When_Temperature_Below_Or_Equal_30()
        {
            var (service, counter, weather) = BuildService();
            counter.Setup(x => x.IncrementAndGet()).Returns(1);
            weather.Setup(x => x.GetCurrentTemperatureAsync()).ReturnsAsync(30);

            var result = await service.BrewCoffeeAsync();
            var okResult = Assert.IsType<Ok<CoffeeResponse>>(result);
            var response = okResult.Value;
            Assert.Equal("Your piping hot coffee is ready", response?.Message);
        }

        [Fact]
        public async Task Should_Return_503_On_Every_Fifth_Request()
        {
            var (service, counter, _) = BuildService();
            counter.Setup(x => x.IncrementAndGet()).Returns(5);

            var result = await service.BrewCoffeeAsync();
            Assert.Equal(503, ((StatusCodeHttpResult)result).StatusCode);
        }
    }
}