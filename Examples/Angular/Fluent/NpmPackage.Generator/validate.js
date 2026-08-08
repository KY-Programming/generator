'use strict';

// Type-checks the generated Angular library. node_modules sits at the root of the scaffolded package
// while the library keeps its own tsconfig one level down, so both are passed explicitly.

const path = require('path');

require('../../../../Tests/Shared/Scripts/validate-app').run(path.join(__dirname, '..', 'NpmPackage', 'package'), 'projects/test/tsconfig.lib.json');
