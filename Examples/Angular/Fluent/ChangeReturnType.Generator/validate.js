'use strict';

// Type-checks the Angular client app of the sibling ChangeReturnType project - that is where this
// generator writes, so that is what proves the changed return type produced compilable TypeScript.

const path = require('path');

require('../../../../Tests/Shared/Scripts/validate-app').run(path.join(__dirname, '..', 'ChangeReturnType', 'ClientApp'));
