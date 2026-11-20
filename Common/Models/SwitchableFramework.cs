namespace KY.Generator.Models
{
    public enum SwitchableFramework
    {
        None,

        [FrameworkName("net462")]
        Net4,

        [FrameworkName("net8.0")]
        Net8,

        [FrameworkName("net9.0")]
        Net9,

        [FrameworkName("net10.0")]
        Net10,

        [FrameworkName("net11.0")]
        Net11
    }
}
