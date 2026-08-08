'use strict';

// Type-checks the Angular client app of the sibling ServiceFromAspNetCore project - that is where this
// generator writes its service and model.

const path = require('path');

require('../../../../Tests/Shared/Scripts/validate-app').run(path.join(__dirname, '..', 'ServiceFromAspNetCore', 'ClientApp'));
