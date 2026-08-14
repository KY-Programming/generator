'use strict';

// Ends the endpoint prepare.js started. Runs after everything else, also after a failed build - the port
// has to be free again for the next run.

const path = require('path');

require('../Shared/Scripts/http-endpoint').teardown({
    pidFile: path.join(__dirname, 'obj', 'watchdog-endpoint.pid')
});
