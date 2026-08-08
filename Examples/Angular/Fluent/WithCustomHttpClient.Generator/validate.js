'use strict';

// Type-checks the Angular client app of the sibling WithCustomHttpClient project - the generated service
// talks to the hand written http client there, so this is what proves the two still fit together.

const path = require('path');

require('../../../../Tests/Shared/Scripts/validate-app').run(path.join(__dirname, '..', 'WithCustomHttpClient', 'ClientApp'));
