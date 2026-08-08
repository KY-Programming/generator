'use strict';

// Verifies that a project generated nothing - for the tests whose whole point is that a type stays out
// of the output (e.g. a class that carries no [Generate] attribute).
//
// An empty "## Output" section alone does not prove that: a project that silently stopped generating
// looks exactly the same. This script asserts it, so the emptiness is a checked result instead of an
// absence of one.
//
// The last line of stdout is the machine readable result - everything before it is the human log:
//
//     {"state":"passed","errors":0,"validated":1}
//
// "validated" is the number of C# types the project guards - the ones that must not end up in the
// output. Counting them here keeps the number honest when types are added to the test.
//
// Used from a project's own validate.js:
//
//     require('../../Shared/Scripts/validate-no-output').run(__dirname);

const fs = require('fs');
const path = require('path');
const shared = require('./type-check');

const TYPE_DECLARATION = /^\s*(?:public|internal|protected|private)?\s*(?:abstract\s+|sealed\s+|static\s+|partial\s+)*(?:class|interface|enum|record(?:\s+struct|\s+class)?|struct)\s+([A-Za-z_][\w]*)/gm;

function countGuardedTypes(projectDirectory) {
    let count = 0;
    for (const file of shared.collectFiles(projectDirectory, '.cs')) {
        const content = fs.readFileSync(file, 'utf8');
        TYPE_DECLARATION.lastIndex = 0;
        while (TYPE_DECLARATION.exec(content) !== null) {
            count++;
        }
    }
    return count;
}

function run(target) {
    if (!target) {
        shared.fail('no project directory passed.', 2);
    }

    const projectDirectory = path.resolve(target);
    const generated = shared.collectFiles(projectDirectory, '.ts');

    if (generated.length > 0) {
        console.log(`VALIDATION FAILED: ${generated.length} file(s) were generated, but this project must generate nothing:`);
        for (const file of generated) {
            console.log(`  ${shared.display(file, projectDirectory)}`);
        }
        console.log(JSON.stringify({ state: 'failed', errors: generated.length, validated: 0 }));
        process.exit(1);
    }

    const guarded = countGuardedTypes(projectDirectory);
    if (guarded === 0) {
        // Nothing generated and nothing to generate - the test would pass without asserting anything.
        shared.fail('the project declares no type, so there is nothing being guarded.');
    }

    console.log(`No TypeScript generated for ${guarded} guarded type(s).`);
    shared.report(0, guarded);
}

module.exports = { run };

// Also runnable directly: node validate-no-output.js <projectDirectory>
if (require.main === module) {
    run(process.argv[2]);
}
