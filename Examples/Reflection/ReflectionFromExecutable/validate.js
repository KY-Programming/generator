'use strict';

// These examples write loose models rather than generating into a client app, so there is no tsconfig of
// their own - the generated files are type-checked against the shared strict one.

const path = require('path');

require('../../../Tests/Shared/Scripts/validate-files').run(path.join(__dirname, 'Output'));
