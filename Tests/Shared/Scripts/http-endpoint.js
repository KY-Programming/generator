'use strict';

// Serves the HTTP endpoint a watchdog test waits for, and records every request it gets. The watchdog
// polls a url until it answers with a success status, so a test that covers it needs something answering
// - and the repository cannot hold a running server any more than it can hold a database.
//
// The request log is what makes the wait observable: generated output alone does not prove that anything
// was waited for, because a generation that skipped the wait writes exactly the same files. Each request
// is appended to the log file as
//
//     2026-08-14T14:14:45.020Z GET /
//
// The readiness probes of run() go to PROBE_PATH so that a test can tell them apart from the requests the
// generation itself made.
//
// Used as a Builder preparation script, so it reports the same JSON contract a validation script does, as
// the last line of stdout:
//
//     {"state":"passed","errors":0,"validated":1}
//
// "validated" is the one endpoint that came up. A failure here means the generation would have waited for
// something that was never going to answer, which is why the Builder skips the build instead of reporting
// wrong output.
//
// Used from a project's own prepare.js:
//
//     require('../Shared/Scripts/http-endpoint').run({ port: 51987, pidFile: ..., logFile: ... });
//
// and from its cleanup.js:
//
//     require('../Shared/Scripts/http-endpoint').teardown({ pidFile: ... });
//
// The server belongs to the run, not to the machine: run() drops whatever an earlier run left behind,
// starts a fresh one detached from the script that started it - it has to outlive the preparation and
// serve the build that follows it - and teardown() ends it again. It also ends itself after
// LIFETIME_SECONDS, so a run that never got to its cleanup cannot leave a listener on the machine.

const fs = require('fs');
const http = require('http');
const path = require('path');
const { spawn } = require('child_process');

// Long enough for a build, short enough that a leftover is gone before the next run: the port has to be
// free again, and a still-running server from an earlier run would answer a test that expects silence.
const LIFETIME_SECONDS = 600;
const READY_TIMEOUT_SECONDS = 20;
const POLL_MILLISECONDS = 100;

/** What run() polls while it waits for the server - never what the generation under test requests. */
const PROBE_PATH = '/__probe';

function fail(message, code = 1) {
    console.error(message);
    console.log(JSON.stringify({ state: 'failed', errors: code, validated: 0 }));
    process.exit(0); // The JSON is the result - a non-zero exit would only duplicate it.
}

function pass(endpoints) {
    console.log(JSON.stringify({ state: 'passed', errors: 0, validated: endpoints }));
}

function sleep(milliseconds) {
    return new Promise(resolve => setTimeout(resolve, milliseconds));
}

/** Resolves with the status code, or with null when nothing answered. */
function probe(port) {
    return new Promise(resolve => {
        const request = http.get({ host: '127.0.0.1', port: port, path: PROBE_PATH, timeout: 1000 }, response => {
            response.resume();
            resolve(response.statusCode);
        });
        request.on('timeout', () => request.destroy());
        request.on('error', () => resolve(null));
    });
}

/** Ends the process the pid file names. Nothing to do when there is no file or the process is already gone. */
function kill(pidFile) {
    if (!fs.existsSync(pidFile)) {
        return false;
    }
    const pid = parseInt(fs.readFileSync(pidFile, 'utf8').trim(), 10);
    fs.rmSync(pidFile, { force: true });
    if (!Number.isInteger(pid)) {
        return false;
    }
    try {
        process.kill(pid);
        return true;
    } catch (error) {
        // ESRCH: the server ended on its own (lifetime reached, machine rebooted, ...) - nothing left to end.
        if (error.code !== 'ESRCH') {
            throw error;
        }
        return false;
    }
}

/** The requests the log holds, without the readiness probes of run(). */
function readRequests(logFile) {
    if (!fs.existsSync(logFile)) {
        return [];
    }
    return fs.readFileSync(logFile, 'utf8')
             .split('\n')
             .map(line => line.trim())
             .filter(line => line.length > 0)
             .map(line => {
                 const [timestamp, method, url] = line.split(' ');
                 return { timestamp: Date.parse(timestamp), method: method, url: url };
             })
             .filter(request => request.url !== PROBE_PATH);
}

async function run({ port, pidFile, logFile }) {
    if (!port || !pidFile || !logFile) {
        fail('run() needs a port, a pidFile and a logFile.', 2);
    }

    // A run that was killed before its cleanup would leave one behind - it is dropped rather than reused,
    // so this run cannot wait for a server somebody else started with different behaviour.
    kill(pidFile);
    if (await probe(port) !== null) {
        // Something that is not ours holds the port. Reusing it would make the test depend on whatever
        // that is, so it is reported instead.
        fail(`Port ${port} is already in use by another process.`);
    }

    fs.mkdirSync(path.dirname(pidFile), { recursive: true });
    fs.mkdirSync(path.dirname(logFile), { recursive: true });
    // The log has to hold this run's requests only - what an earlier one asked for proves nothing here.
    fs.rmSync(logFile, { force: true });

    const child = spawn(process.execPath, [__filename, '--serve', String(port), String(LIFETIME_SECONDS), logFile],
                        { detached: true, stdio: 'ignore' });
    child.unref();
    fs.writeFileSync(pidFile, `${child.pid}\n`);

    const deadline = Date.now() + READY_TIMEOUT_SECONDS * 1000;
    while (Date.now() < deadline) {
        const status = await probe(port);
        if (status !== null) {
            console.log(`Endpoint http://localhost:${port}/ answered with ${status} (pid ${child.pid})`);
            pass(1);
            return;
        }
        await sleep(POLL_MILLISECONDS);
    }

    kill(pidFile);
    fail(`The endpoint on port ${port} was not up within ${READY_TIMEOUT_SECONDS}s.`);
}

/**
 * Ends the server the run created. Called from the project's cleanup.js, which the Builder runs after
 * everything else - also after a failed build or a preparation that never got as far as starting
 * anything, so this has to work no matter how much of run() happened.
 */
function teardown({ pidFile }) {
    if (!pidFile) {
        fail('teardown() needs a pidFile.', 2);
    }
    console.log(kill(pidFile) ? 'Endpoint stopped' : 'No endpoint left to stop');
    pass(0);
}

/** The detached child: answers every request with 200 until it is killed or its lifetime is over. */
function serve(port, lifetimeSeconds, logFile) {
    const server = http.createServer((request, response) => {
        fs.appendFileSync(logFile, `${new Date().toISOString()} ${request.method} ${request.url}\n`);
        response.writeHead(200, { 'Content-Type': 'text/plain' });
        response.end('ready');
    });
    server.listen(port, '127.0.0.1');
    setTimeout(() => process.exit(0), lifetimeSeconds * 1000);
}

module.exports = { run, teardown, readRequests, PROBE_PATH };

if (require.main === module) {
    if (process.argv[2] === '--serve') {
        serve(parseInt(process.argv[3], 10), parseInt(process.argv[4], 10), process.argv[5]);
    } else {
        fail('http-endpoint.js is used from a prepare.js / cleanup.js - see the comment at the top.', 2);
    }
}
