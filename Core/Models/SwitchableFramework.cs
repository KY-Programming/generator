namespace KY.Generator.Models
{
    public enum SwitchableFramework
    {
        None,

        [FrameworkName("net461")]
        Net4,

        [FrameworkName("netcoreapp2.0")]
        NetCore2,

        [FrameworkName("netcoreapp3.0")]
        NetCore3,

        [FrameworkName("net5.0")]
        Net5
    }
}