namespace CoffeeMachine.Services.Interface
{
    public interface ICoffeeService
    {
        Task<IResult> BrewCoffeeAsync();
    }
}
