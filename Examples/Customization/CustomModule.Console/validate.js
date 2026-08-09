'use strict';

// Compiles what the custom module wrote. This is also the check that the module produced C# at all: the
// writer is the example's own, so nothing else would notice if it emitted something that does not parse.

const path = require('path');

require('../../../Tests/Shared/Scripts/validate-csharp').run(path.join(__dirname, 'Output'));
