'use strict';

// This example writes loose models instead of generating into a client app, so there is no tsconfig of
// its own - the shared strict one is used.

const path = require('path');

require('../../../Tests/Shared/Scripts/validate-files').run(path.join(__dirname, 'Output'));
