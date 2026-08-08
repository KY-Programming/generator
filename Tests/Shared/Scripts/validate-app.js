'use strict';

// Type-checks the TypeScript of an example's client app or library - the step that catches generated
// output which verifies fine against its hash but does not actually compile.
//
// Unlike the other validations this one uses the app's own tsc, node_modules and tsconfig rather than
// the shared ones in Tests. That is deliberate: an example must compile against the toolchain it ships,
// not against a pinned central version.
//
//   npmRoot   the folder holding package.json and node_modules - tsc runs from here
//   tsconfig  relative to npmRoot, defaults to tsconfig.app.json. A library keeps its config one level
//             down (e.g. projects/test/tsconfig.lib.json) while node_modules stays at the root.
//
// The last line of stdout is the machine readable result - everything before it is the human log:
//
//     {"state":"passed","errors":0,"validated":11}
//
// "validated" is the number of .ts files under the tsconfig's src folder that were type-checked.
//
// Used from an example's own validate.js:
//
//     require('../../../../Tests/Shared/Scripts/validate-app').run(path.join(__dirname, 'ClientApp'));

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');
const shared = require('./type-check');
const { ensurePackages } = require('./ensure-packages');

const DIAGNOSTIC = /error TS\d+/;

function run(npmRoot, tsconfig = 'tsconfig.app.json') {
    if (!npmRoot) {
        shared.fail('no npm root passed.', 2);
    }

    const root = path.resolve(npmRoot);
    const configPath = path.join(root, tsconfig);
    if (!fs.existsSync(configPath)) {
        shared.fail(`${configPath} not found.`, 2);
    }

    const failure = ensurePackages(root);
    if (failure !== null) {
        shared.fail(failure);
    }

    // Run tsc through node directly - no shell, no dependency on the .bin shims.
    const tsc = path.join(root, 'node_modules', 'typescript', 'bin', 'tsc');
    if (!fs.existsSync(tsc)) {
        shared.fail(`typescript is not installed in ${root}${path.sep}node_modules.`);
    }

    // The sources sit next to the tsconfig, which is not necessarily the npm root.
    const sources = path.join(path.dirname(configPath), 'src');
    const files = shared.collectFiles(sources, '.ts').filter(file => !file.endsWith('.d.ts'));
    if (files.length === 0) {
        shared.fail(`no .ts files found in ${sources}.`);
    }

    console.log(`${path.basename(root)}: ${files.length} file(s) against ${tsconfig}`);
    const result = spawnSync(process.execPath, [tsc, '-p', tsconfig, '--noEmit'], { cwd: root, encoding: 'utf8' });
    const output = `${result.stdout || ''}${result.stderr || ''}`;
    if (output.trim().length > 0) {
        console.log(output.trimEnd());
    }

    const errors = output.split(/\r?\n/).filter(line => DIAGNOSTIC.test(line)).length;
    // tsc can fail without a single diagnostic line (bad config, crash) - that is still a failure.
    shared.report(result.status === 0 && errors === 0 ? 0 : Math.max(errors, 1), files.length);
}

module.exports = { run };

// Also runnable directly: node validate-app.js <npmRoot> [tsconfig]
if (require.main === module) {
    run(process.argv[2], process.argv[3]);
}
