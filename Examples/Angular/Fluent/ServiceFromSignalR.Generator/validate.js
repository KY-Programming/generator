'use strict';

// Type-checks the Angular client app of the sibling ServiceFromSignalR project - that is where this
// generator writes its hub service and models.

const path = require('path');

require('../../../../Tests/Shared/Scripts/validate-app').run(path.join(__dirname, '..', 'ServiceFromSignalR', 'ClientApp'));
