using System;

namespace KY.Generator.Extensions
{
    public static class TimeSpanExtension
    {
        public static string Format(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalSeconds >= 1)
            {
                return $"{timeSpan.TotalSeconds:0.#} sec";
            }
            if (timeSpan.TotalMilliseconds >= 1)
            {
                return $"{timeSpan.TotalMilliseconds:0} ms";
            }
            return ">1 ms";
        }
    }
}
