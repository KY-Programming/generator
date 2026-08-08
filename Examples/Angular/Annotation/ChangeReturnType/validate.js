'use strict';

// Type-checks the Angular client app this example generates into. The generated service imports the
// custom model through the "@my-lib/models" path mapping of ClientApp/tsconfig.json, so this is what
// proves that [GenerateImport] and [GenerateMethod] produced TypeScript that actually compiles.

const path = require('path');

require('../../../../Tests/Shared/Scripts/validate-app').run(path.join(__dirname, 'ClientApp'));
