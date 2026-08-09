'use strict';

// The example writes both languages from the same JSON document, so both are checked here: the
// TypeScript model is type-checked against the shared strict tsconfig, the C# models are compiled with
// the project they were generated for. The build alone does not cover the C# - the generated files are
// deleted before it and written after the compiler ran.

const path = require('path');

const OUTPUT = path.join(__dirname, 'Output');
const typescript = require('../../../Tests/Shared/Scripts/validate-files').check(OUTPUT);
const csharp = require('../../../Tests/Shared/Scripts/validate-csharp').check(OUTPUT);

require('../../../Tests/Shared/Scripts/type-check')
    .report(typescript.errors + csharp.errors, typescript.validated + csharp.validated);
