using System;
using System.Threading.Tasks;

namespace DynamicFilterControls
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
    public delegate Task EventHandlerAsync(object sender, EventArgs arg);
    public delegate Task EventHandlerAsync<T>(object sender, T arg);
}