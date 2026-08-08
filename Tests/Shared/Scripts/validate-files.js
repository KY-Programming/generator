'use strict';

// Type-checks a folder of generated .ts files - for the examples that write loose models instead of
// generating into a client app of their own, so there is no tsconfig to check them against.
//
// The files are checked against Tests/Shared/tsconfig.strict.json, or tsconfig.non-strict.json when
// strict is false, for output that was generated with GenerateNonStrict. Unlike the v10 tests the mode
// is passed in rather than derived: an example demonstrates one setting on purpose.
//
// The last line of stdout is the machine readable result - everything before it is the human log:
//
//     {"state":"passed","errors":0,"validated":3}
//
// Used from an example's own validate.js:
//
//     require('../../../../Tests/Shared/Scripts/validate-files').run(path.join(__dirname, 'Output'));
//     require('../../../../Tests/Shared/Scripts/validate-files').run(dir, { strict: false });

const fs = require('fs');
const path = require('path');
const shared = require('./type-check');

function run(target, { strict = true } = {}) {
    if (!target) {
        shared.fail('no directory passed.', 2);
    }

    const directory = path.resolve(target);
    if (!fs.existsSync(directory)) {
        shared.fail(`${directory} does not exist.`, 2);
    }

    const files = shared.collectFiles(directory, '.ts').filter(file => !file.endsWith('.d.ts')).sort();
    if (files.length === 0) {
        // Nothing to type-check is a failure, not a pass - an example that quietly stopped generating
        // would otherwise stay green forever.
        shared.fail(`no .ts files found in ${directory}.`);
    }

    console.log(`${path.basename(directory)}: ${files.length} generated file(s)`);
    shared.report(shared.typeCheck(files, strict, directory) || 0, files.length);
}

module.exports = { run };

// Also runnable directly: node validate-files.js <directory> [--non-strict]
if (require.main === module) {
    const args = process.argv.slice(2);
    run(args.find(argument => !argument.startsWith('--')), { strict: !args.includes('--non-strict') });
}
