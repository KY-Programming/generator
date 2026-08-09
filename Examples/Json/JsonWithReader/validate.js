'use strict';

// Only the TypeScript side is checked here - the generated C# models sit inside the project and are
// compiled by the build itself, so a broken one fails the build before this script ever runs.

const path = require('path');

require('../../../Tests/Shared/Scripts/validate-files').run(path.join(__dirname, 'Output'));
