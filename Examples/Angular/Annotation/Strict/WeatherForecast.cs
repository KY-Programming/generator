using System;
using System.Collections.Generic;

namespace Strict
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public List<string> StringList { get; set; }
        public string[] StringArray { get; set; }
        public Dictionary<string,string> StringDictionary { get; set; }
        public object Object { get; set; }
        public SubType SubType { get; set; }
        public DateTime? NullableDate { get; set; }
    }

    public class SubType
    { }
}
