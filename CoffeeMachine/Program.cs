using CoffeeMachine.Infrastructure.Implementation;
using CoffeeMachine.Infrastructure.Interface;
using CoffeeMachine.Services.Implementation;
using CoffeeMachine.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRequestCounter, RequestCounter>();
builder.Services.AddScoped<ICoffeeService, CoffeeService>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.MapGet("/brew-coffee", async (ICoffeeService service) =>
{
    return await service.BrewCoffeeAsync();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
