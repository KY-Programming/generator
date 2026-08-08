'use strict';

// The point of this example is output that opts out of strict mode, so it is checked against the shared
// non strict tsconfig - the strict one would reject exactly what the example demonstrates.

const path = require('path');

require('../../../../Tests/Shared/Scripts/validate-files').run(path.join(__dirname, 'Output'), { strict: false });
