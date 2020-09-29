namespace WebApiController.Models
{
    public class IgnoreMe<T>
    {
        public T Value { get; }

        public IgnoreMe(T value)
        {
            this.Value = value;
        }
    }

    public class IgnoreMe2<T>
    {
        public T Value { get; }

        public IgnoreMe2(T value)
        {
            this.Value = value;
        }
    }

    public class IgnoreMe3<T>
    {
        public T Value { get; }

        public IgnoreMe3(T value)
        {
            this.Value = value;
        }
    }
}