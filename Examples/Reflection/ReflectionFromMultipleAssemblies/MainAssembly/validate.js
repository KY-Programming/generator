'use strict';

// The models of both assemblies are written into MainAssembly/Output, so one type check covers them all.
// They are loose models without a tsconfig of their own and use the shared strict one.

const path = require('path');

require('../../../../Tests/Shared/Scripts/validate-files').run(path.join(__dirname, 'Output'));
