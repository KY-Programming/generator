namespace WebApiControllerWithRoute.Models
{
    public class IgnoreMe<T>
    {
        public T Value { get; }

        public IgnoreMe(T value)
        {
            this.Value = value;
        }
    }
}