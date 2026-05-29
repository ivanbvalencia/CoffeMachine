using CoffeeMachine.Infrastructure.Interface;

namespace CoffeeMachine.Infrastructure.Implementation
{
    public class RequestCounter : IRequestCounter
    {
        private int _counter;
        public int IncrementAndGet()
        {
            return Interlocked.Increment(ref _counter);
        }
    }
}
