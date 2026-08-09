'use strict';

// Validates GenerateNever. Unlike every other project this one must NOT build: the generator has to
// abort with an error that names the file the forbidden type would have been written to.
//
// The last line of stdout is the machine readable result - the Builder reads that, not the exit code:
//
//     {"state":"passed","errors":0,"validated":1}
//
// "validated" is the one output file this project guards - never-generated-model.ts.

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

const GUARDED = path.join('Output', 'never-generated-model.ts');
const ABORT_MESSAGE = 'is decorated with GenerateNeverAttribute and must never be generated';

function fail(message) {
    console.log(`VALIDATION FAILED: ${message}`);
    console.log(JSON.stringify({ state: 'failed', errors: 1, validated: 0 }));
    process.exit(1);
}

function main() {
    fs.rmSync(path.join(__dirname, 'Output'), { recursive: true, force: true });

    const build = spawnSync('dotnet', ['build', path.join(__dirname, 'NeverAnnotations.csproj'), '--no-incremental'],
                            { cwd: __dirname, encoding: 'utf8' });
    const log = `${build.stdout || ''}${build.stderr || ''}`;
    console.log(log.trimEnd());

    if (build.status === 0) {
        fail('the build succeeded, but GenerateNever should have aborted it.');
    }
    if (!log.includes(ABORT_MESSAGE)) {
        fail('the build failed, but not with the GenerateNever error.');
    }
    // The generator reports a native path, so the separator is matched the way it is written.
    if (!log.includes(GUARDED) && !log.includes(GUARDED.split(path.sep).join('/'))) {
        fail('the GenerateNever error does not name the generated file.');
    }
    if (fs.existsSync(path.join(__dirname, GUARDED))) {
        fail('the forbidden file was written anyway.');
    }

    console.log('VALIDATION PASSED');
    console.log(JSON.stringify({ state: 'passed', errors: 0, validated: 1 }));
}

main();
