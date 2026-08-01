'use strict';

// Type-checks the generated v10 TypeScript and reports the outcome to KY.Generator.Builder.
//
// The last line of stdout is the machine readable result - everything before it is the human log:
//
//     {"state":"passed","errors":0,"validated":174}
//
//   state      "passed" when tsc reported no error, "failed" otherwise
//   errors     number of tsc diagnostics
//   validated  number of files that were type-checked - the Builder stores this as the output marker
//
// The exit code is only a convenience for running this by hand (0 = passed); the Builder reads the JSON.
// A run that dies before printing the JSON counts as failed, which is why the result is printed last.
//
// Usage: node validate-typescript.js --strict|--all   (from the project directory)

const path = require('path');
const { spawnSync } = require('child_process');
const { buildFileList, report, parseMode, OUTPUT_FILE, V10_ROOT } = require('./build-file-list');

const TSC = path.join(V10_ROOT, 'node_modules', 'typescript', 'bin', 'tsc');
const DIAGNOSTIC = /error TS\d+/;

function main() {
    const mode = parseMode(process.argv);
    if (mode === null) {
        console.error('Usage: node validate-typescript.js --strict|--all');
        process.exit(2);
    }

    const projectDirectory = process.cwd();
    const list = buildFileList(mode, projectDirectory);
    report(list, mode);

    // Run tsc through node directly - no shell, no dependency on the .bin shims.
    const tsc = spawnSync(process.execPath, [TSC, '-p', OUTPUT_FILE], { cwd: projectDirectory, encoding: 'utf8' });
    const output = `${tsc.stdout || ''}${tsc.stderr || ''}`;
    if (output.trim().length > 0) {
        console.log(output.trimEnd());
    }

    const errors = output.split(/\r?\n/).filter(line => DIAGNOSTIC.test(line)).length;
    // tsc can fail without a single diagnostic line (bad config, crash) - that is still a failure.
    const passed = errors === 0 && tsc.status === 0;
    if (!passed && errors === 0) {
        console.log(`VALIDATION FAILED: tsc exited with ${tsc.status} without reporting a diagnostic.`);
    } else {
        console.log(passed ? 'VALIDATION PASSED' : `VALIDATION FAILED: ${errors} error(s).`);
    }

    console.log(JSON.stringify({
        state: passed ? 'passed' : 'failed',
        errors: passed ? 0 : Math.max(errors, 1),
        validated: list.included.length,
    }));
    process.exit(passed ? 0 : 1);
}

main();
