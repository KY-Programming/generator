'use strict';

// Puts docker back into the state prepare.js found it in - see Tests/Shared/Scripts/mssql-container.js.
// Runs after everything else, also after a failed build.

const path = require('path');

require('../../../Tests/Shared/Scripts/mssql-container').teardown({
    compose: path.join(__dirname, '..', 'Shared', 'docker-compose.yml')
});
