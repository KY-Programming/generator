namespace KY.Generator.Transfer
{
    public class TransferObject<T> : ITransferObject
    {
        public T Value { get; }

        public TransferObject(T value)
        {
            this.Value = value;
        }
    }

    public static class TransferObject
    {
        public static TransferObject<T> Create<T>(T value)
        {
            return new TransferObject<T>(value);
        }
    }
}