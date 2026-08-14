'use strict';

// Guards the wait. The generated file alone proves nothing: a generation that skipped the wait entirely
// writes exactly the same output, so what is checked here is the endpoint's request log - the generation
// has to have polled the url, and it has to have done so before it wrote its files.
//
// Afterwards the generated TypeScript is type-checked as in every other project, so a green run here means
// the wait did not cost the generation anything.
//
// The last line of stdout is the machine readable result:
//
//     {"state":"passed","errors":0,"validated":1}

const fs = require('fs');
const path = require('path');
const endpoint = require('../Shared/Scripts/http-endpoint');

const logFile = path.join(__dirname, 'obj', 'watchdog-endpoint.log');
const outputDirectory = path.join(__dirname, 'Output');

// File timestamps and the log timestamps come from two processes, so the order is compared with a little
// slack instead of exactly - what this rules out is a generation that ran long before the wait, not one
// that overlapped it by a few milliseconds.
const ORDER_TOLERANCE_MILLISECONDS = 1000;

function fail(reason) {
    console.log(`VALIDATION FAILED: ${reason}`);
    console.log(JSON.stringify({ state: 'failed', errors: 1, validated: 0 }));
    process.exit(1);
}

const requests = endpoint.readRequests(logFile);
if (requests.length === 0) {
    fail(fs.existsSync(logFile)
             ? 'the generation never requested the url it was told to wait for.'
             : `the endpoint wrote no log - ${path.relative(__dirname, logFile)} does not exist.`);
}

const generated = fs.existsSync(outputDirectory)
    ? fs.readdirSync(outputDirectory).filter(file => file.endsWith('.ts')).map(file => path.join(outputDirectory, file))
    : [];
if (generated.length === 0) {
    fail('the wait happened, but nothing was generated after it.');
}

const firstRequest = Math.min(...requests.map(request => request.timestamp));
const firstGenerated = Math.min(...generated.map(file => fs.statSync(file).mtimeMs));
if (firstRequest > firstGenerated + ORDER_TOLERANCE_MILLISECONDS) {
    fail(`the generation wrote its output before it waited (first request ${new Date(firstRequest).toISOString()}, first file ${new Date(firstGenerated).toISOString()}).`);
}

console.log(`watchdog: ${requests.length} request(s) to the endpoint, ${generated.length} file(s) generated after them`);

require('../Shared/Scripts/validate-generated').run(__dirname);
