'use strict';

// Type-checks the Angular client app this project generates into - all twenty generated services and
// their models are checked against the real @angular/common/http and rxjs types.

const path = require('path');

require('../Shared/Scripts/validate-app').run(path.join(__dirname, 'ClientApp'));
