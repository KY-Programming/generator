﻿using KY.Generator;

namespace AnnotationsWithMultipleOutputs
{
    [GenerateAngularModel("Output\\First")]
    [GenerateWithoutHeader]
    public class FirstType
    {
        public string StringProperty { get; set; }
        public SubType SubTypeProperty { get; set; }
    }
}
