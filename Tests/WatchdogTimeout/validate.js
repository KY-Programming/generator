'use strict';

// Validates the failing wait. Unlike most projects this one must NOT build: nothing listens on the url,
// so the wait can only time out - and a generation whose wait failed must not write anything and must
// report the failure instead of a green build.
//
// The build is run from here, because a project the Builder builds itself is expected to succeed.
//
// The last line of stdout is the machine readable result - the Builder reads that, not the exit code:
//
//     {"state":"passed","errors":0,"validated":1}
//
// "validated" is the one output file this project guards - model.ts, which the read/write chain behind the
// wait would produce if the failed wait did not stop it.

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

const GUARDED = path.join('Output', 'model.ts');
const URL = 'http://localhost:51988/';

function fail(message) {
    console.log(`VALIDATION FAILED: ${message}`);
    console.log(JSON.stringify({ state: 'failed', errors: 1, validated: 0 }));
    process.exit(1);
}

function main() {
    fs.rmSync(path.join(__dirname, 'Output'), { recursive: true, force: true });

    const build = spawnSync('dotnet', ['build', path.join(__dirname, 'WatchdogTimeout.csproj'), '--no-incremental'],
                            { cwd: __dirname, encoding: 'utf8' });
    const log = `${build.stdout || ''}${build.stderr || ''}`;
    console.log(log.trimEnd());

    if (!log.includes(URL)) {
        fail(`the generation never waited for ${URL}.`);
    }
    if (fs.existsSync(path.join(__dirname, GUARDED))) {
        fail('the failed wait did not stop the generation - the output was written anyway.');
    }
    if (build.status === 0) {
        fail('the build succeeded, but the wait it depends on timed out.');
    }

    console.log('VALIDATION PASSED');
    console.log(JSON.stringify({ state: 'passed', errors: 0, validated: 1 }));
}

main();
