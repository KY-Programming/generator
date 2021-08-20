namespace KY.Generator
{
    public interface IGlobalOptionsReader
    {
        void Read(object key, OptionsSet entry);
    }
}