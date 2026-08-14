'use strict';

// Starts the endpoint the generation waits for. Runs before the build, so the wait has something to poll.

const path = require('path');

require('../Shared/Scripts/http-endpoint').run({
    port: 51987,
    pidFile: path.join(__dirname, 'obj', 'watchdog-endpoint.pid'),
    logFile: path.join(__dirname, 'obj', 'watchdog-endpoint.log')
});
