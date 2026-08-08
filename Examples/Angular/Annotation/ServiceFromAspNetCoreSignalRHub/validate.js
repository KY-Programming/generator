'use strict';

// Type-checks the Angular client app this example generates into - the generated hub service is checked
// against the real @microsoft/signalr and rxjs types.

const path = require('path');

require('../../../../Tests/Shared/Scripts/validate-app').run(path.join(__dirname, 'ClientApp'));
