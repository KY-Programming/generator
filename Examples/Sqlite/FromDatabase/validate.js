'use strict';

// Compiles the model generated from the SQLite schema with the project it belongs to - see
// Tests/Shared/Scripts/validate-csharp.js for why the build alone does not cover it.

const path = require('path');

require('../../../Tests/Shared/Scripts/validate-csharp').run(path.join(__dirname, 'Output'));
