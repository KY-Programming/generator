﻿using System.Collections.Generic;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator.Tests.Models
{
    internal class Write1Command : IConfigurationCommand
    {
        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            Write1Configuration configuration = (Write1Configuration)configurationBase;
            return configuration != null;
        }
    }
}