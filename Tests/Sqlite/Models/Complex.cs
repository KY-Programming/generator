﻿using KY.Generator;

namespace Sqlite.Models
{
    [GenerateSqliteRepository("Output")]
    public class Complex
    {
        public string StringProperty { get; set; }

        public Simple Simple { get; set; }
    }
}
